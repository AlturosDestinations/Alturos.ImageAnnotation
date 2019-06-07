using Alturos.ImageAnnotation.Helper;
using Alturos.ImageAnnotation.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Alturos.ImageAnnotation.CustomControls
{
    public partial class AnnotationDrawControl : UserControl
    {
        enum KeyboardOperation { Move, Resize };
        enum ScaleOperation { Regular, Inverse };

        public event Action<AnnotationImage> ImageEdited;

        public bool AutoplaceAnnotations { get; set; }
        public bool ShowLabels { get; private set; }

        private readonly int _mouseDragElementSize = 10;
        private readonly int _maxMouseDistanceToDragPoint = 10;
        private readonly Size _minSize = new Size(30, 30);
        private readonly Size _minSizeForEdgeAnchors = new Size(50, 50);

        private bool _mouseOver;
        private Point _mousePosition = new Point(0, 0);
        private RectangleF _selectedObjectRect;
        private AnnotationBoundingBox[] _cachedBoundingBoxes;
        private AnnotationBoundingBox _selectedBoundingBox;
        private AnnotationBoundingBox _draggedBoundingBox;
        private DragPoint _dragPoint;
        private AnnotationImage _annotationImage;
        private List<ObjectClass> _objectClasses;
        private double _grabOffsetX;
        private double _grabOffsetY;
        private bool _createBoundingBox;
        private Point _creationPoint;
        private bool _changedImageViaKey;

        public AnnotationDrawControl()
        {
            this.InitializeComponent();
        }

        public void Reset()
        {
            this.SetImage(null);
            this._cachedBoundingBoxes = null;
        }

        public void SetObjectClasses(List<ObjectClass> objectClasses)
        {
            this._objectClasses = objectClasses;

            this.legendsChart.Legends[0].CustomItems.Clear();

            for (var i = 0; i < objectClasses.Count; i++)
            {
                var legendItem = new LegendItem
                {
                    Name = $"{objectClasses[i].Id} - {objectClasses[i].Name}",
                    Color = DrawHelper.GetColorCode(i)
                };

                this.legendsChart.Legends[0].CustomItems.Add(legendItem);
            }
        }

        public void SetLabelsVisible(bool showLabels)
        {
            this.ShowLabels = showLabels;
            this.legendsChart.Visible = !showLabels;

            this.pictureBox1.Invalidate();
        }

        private void CacheLastBoundingBoxes()
        {
            if (this._annotationImage?.BoundingBoxes != null)
            {
                //Create a new copy of the object
                var items = this._annotationImage.BoundingBoxes.Select(o => new AnnotationBoundingBox
                {
                    CenterX = o.CenterX,
                    CenterY = o.CenterY,
                    Width = o.Width,
                    Height = o.Height,
                    ObjectIndex = o.ObjectIndex
                }).ToArray();

                this._cachedBoundingBoxes = items;
            }
        }

        public void SetImage(AnnotationImage image)
        {
            this.CacheLastBoundingBoxes();

            this._annotationImage = image;
            var oldImage = this.pictureBox1.Image;

            if (image == null)
            {
                this.pictureBox1.Image = null;
            }
            else
            {
                this.pictureBox1.Image = DrawHelper.DrawBoxes(image);
            }

            if (oldImage != null)
            {
                oldImage.Dispose();
            }
        }

        public void ApplyCachedBoundingBox()
        {
            if (!this.AutoplaceAnnotations)
            {
                return;
            }

            if (this._cachedBoundingBoxes != null && _cachedBoundingBoxes.Length > 0)
            {
                if (this._annotationImage.BoundingBoxes == null)
                {
                    this._annotationImage.BoundingBoxes = new List<AnnotationBoundingBox>();
                }

                if (!this._annotationImage.BoundingBoxes.Any())
                {
                    this._annotationImage.BoundingBoxes.AddRange(this._cachedBoundingBoxes);
                    this.ImageEdited?.Invoke(this._annotationImage);
                }
            }
        }

        private CanvasInfo GetCanvasInformation()
        {
            var imageWidth = this.pictureBox1.Image.Width;
            var imageHeight = this.pictureBox1.Image.Height;
            var canvasWidth = this.pictureBox1.Width;
            var canvasHeight = this.pictureBox1.Height;

            var imageRatio = imageWidth / (float)imageHeight; // image W:H ratio
            var containerRatio = canvasWidth / (float)canvasHeight; // container W:H ratio

            var canvasInfo = new CanvasInfo();
            if (imageRatio >= containerRatio)
            {
                //Horizontal image
                var scaleFactor = canvasWidth / (float)imageWidth;
                canvasInfo.ScaledHeight = imageHeight * scaleFactor;
                canvasInfo.ScaledWidth = imageWidth * scaleFactor;
                canvasInfo.OffsetY = (canvasHeight - canvasInfo.ScaledHeight) / 2;
            }
            else
            {
                //Vertical image
                var scaleFactor = canvasHeight / (float)imageHeight;
                canvasInfo.ScaledHeight = imageHeight * scaleFactor;
                canvasInfo.ScaledWidth = imageWidth * scaleFactor;
                canvasInfo.OffsetX = (canvasWidth - canvasInfo.ScaledWidth) / 2;
            }

            return canvasInfo;
        }

        private float PointDistance(PointF pt1, Point pt2)
        {
            float dx = pt1.X - pt2.X;
            float dy = pt1.Y - pt2.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private bool PointInRectangle(PointF pt, Rectangle rect, double tolerance = 0)
        {
            return pt.X >= rect.X - tolerance && pt.X <= rect.X + rect.Width + tolerance && pt.Y >= rect.Y - tolerance && pt.Y <= rect.Y + rect.Height + tolerance;
        }

        private DragPoint[] GetDragPoints(Rectangle rectangle, int drawOffset)
        {
            var dragPoints = new List<DragPoint>();

            var topLeftPoint = new Point(rectangle.X - drawOffset, rectangle.Y - drawOffset);
            var topRightPoint = new Point(rectangle.X + rectangle.Width + drawOffset, rectangle.Y - drawOffset);
            var bottomLeftPoint = new Point(rectangle.X - drawOffset, rectangle.Y + rectangle.Height + drawOffset);
            var bottomRightPoint = new Point(rectangle.X + rectangle.Width + drawOffset, rectangle.Y + rectangle.Height + drawOffset);
            var deletePoint = new Point(rectangle.X + rectangle.Width - 15 - drawOffset, rectangle.Y - drawOffset);

            dragPoints.Add(new DragPoint(topLeftPoint, DragPointType.Resize, Cursors.SizeNWSE, 315));
            dragPoints.Add(new DragPoint(topRightPoint, DragPointType.Resize, Cursors.SizeNESW, 225));
            dragPoints.Add(new DragPoint(bottomLeftPoint, DragPointType.Resize, Cursors.SizeNESW, 45));
            dragPoints.Add(new DragPoint(bottomRightPoint, DragPointType.Resize, Cursors.SizeNWSE, 135));
            dragPoints.Add(new DragPoint(deletePoint, DragPointType.Delete, Cursors.Hand, 0));

            if (rectangle.Width > this._minSizeForEdgeAnchors.Width)
            {
                var topPoint = new Point(rectangle.X + rectangle.Width / 2, rectangle.Y - drawOffset);
                var bottomPoint = new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height + drawOffset);

                dragPoints.Add(new DragPoint(topPoint, DragPointType.Resize, Cursors.SizeNS, 270));
                dragPoints.Add(new DragPoint(bottomPoint, DragPointType.Resize, Cursors.SizeNS, 90));
            }

            if (rectangle.Height > this._minSizeForEdgeAnchors.Height)
            {
                var leftPoint = new Point(rectangle.X - drawOffset, rectangle.Y + rectangle.Height / 2);
                var rightPoint = new Point(rectangle.X + rectangle.Width + drawOffset, rectangle.Y + rectangle.Height / 2);

                dragPoints.Add(new DragPoint(leftPoint, DragPointType.Resize, Cursors.SizeWE, 0));
                dragPoints.Add(new DragPoint(rightPoint, DragPointType.Resize, Cursors.SizeWE, 180));
            }

            return dragPoints.ToArray();
        }

        private Rectangle GetRectangle(AnnotationBoundingBox boundingBox)
        {
            var canvasInfo = this.GetCanvasInformation();

            var width = boundingBox.Width * canvasInfo.ScaledWidth;
            var height = (boundingBox.Height * canvasInfo.ScaledHeight);
            var x = (boundingBox.CenterX * canvasInfo.ScaledWidth) - (width / 2) + canvasInfo.OffsetX;
            var y = (boundingBox.CenterY * canvasInfo.ScaledHeight) - (height / 2) + canvasInfo.OffsetY;

            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }

        private PointF GetOppositeAnchor(RectangleF rect, PointF anchor)
        {
            var center = new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

            var closestAnchorX = rect.X + (float)Math.Round((anchor.X / rect.Width) * 2) * rect.Width / 2;
            var closestAnchorY = rect.Y + (float)Math.Round((anchor.Y / rect.Height) * 2) * rect.Height / 2;

            var oppositeAnchorX = center.X + (center.X - closestAnchorX);
            var oppositeAnchorY = center.Y + (center.Y - closestAnchorY);

            return new PointF(oppositeAnchorX, oppositeAnchorY);
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (this.pictureBox1.Image == null)
            {
                return;
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.High;

            var drawOffset = this._mouseDragElementSize / 2;

            if (this._mouseOver)
            {
                e.Graphics.DrawLine(Pens.Blue, new Point(this._mousePosition.X, this.pictureBox1.Top), new Point(this._mousePosition.X, this.pictureBox1.Bottom));
                e.Graphics.DrawLine(Pens.Blue, new Point(this.pictureBox1.Left, this._mousePosition.Y), new Point(this.pictureBox1.Right, this._mousePosition.Y));
            }

            var boundingBoxes = this._annotationImage?.BoundingBoxes;
            if (boundingBoxes != null)
            {
                foreach (var boundingBox in boundingBoxes)
                {
                    var rectangle = this.GetRectangle(boundingBox);
                    var brush = DrawHelper.GetColorCode(boundingBox.ObjectIndex);
                    var objectClass = this._objectClasses.FirstOrDefault(o => o.Id == boundingBox.ObjectIndex);

                    using (var pen = new Pen(brush, 2))
                    {
                        e.Graphics.DrawRectangle(pen, rectangle);
                    }
                    if (this._selectedBoundingBox == boundingBox)
                    {
                        e.Graphics.DrawRectangle(Pens.Yellow, Rectangle.Inflate(rectangle, 2, 2));
                    }

                    this.DrawLabel(e.Graphics, rectangle.X, rectangle.Y, objectClass);

                    var biggerRectangle = Rectangle.Inflate(rectangle, 20, 20);
                    if (biggerRectangle.Contains(this._mousePosition))
                    {
                        System.Windows.Forms.Cursor.Current = Cursors.SizeAll;

                        var dragPoints = this.GetDragPoints(rectangle, drawOffset);
                        foreach (var dragPoint in dragPoints)
                        {
                            var dragElementBrush = Brushes.LightPink;

                            if (this.PointDistance(this._mousePosition, new Point(dragPoint.Point.X, dragPoint.Point.Y)) < this._maxMouseDistanceToDragPoint)
                            {
                                System.Windows.Forms.Cursor.Current = dragPoint.Cursor;
                                dragElementBrush = Brushes.Yellow;
                            }

                            if (dragPoint.Type == DragPointType.Resize)
                            {
                                // Make horizontal/vertical anchors smaller
                                var size = (dragPoint.Angle % 90 == 0) ? 6.5f : 10;

                                var points = new PointF[]
                                {
                                    new PointF(
                                        dragPoint.Point.X + (float)Math.Cos(MathHelper.Deg2Rad * (dragPoint.Angle - 90)) * size,
                                        dragPoint.Point.Y - (float)Math.Sin(MathHelper.Deg2Rad * (dragPoint.Angle - 90)) * size),
                                    new PointF(
                                        dragPoint.Point.X + (float)Math.Cos(MathHelper.Deg2Rad * (dragPoint.Angle + 180)) * size,
                                        dragPoint.Point.Y - (float)Math.Sin(MathHelper.Deg2Rad * (dragPoint.Angle + 180)) * size),
                                    new PointF(
                                        dragPoint.Point.X + (float)Math.Cos(MathHelper.Deg2Rad * (dragPoint.Angle + 90)) * size,
                                        dragPoint.Point.Y - (float)Math.Sin(MathHelper.Deg2Rad * (dragPoint.Angle + 90)) * size),
                                };

                                e.Graphics.FillPolygon(dragElementBrush, points);
                            }
                            else if (dragPoint.Type == DragPointType.Delete)
                            {
                                e.Graphics.FillEllipse(Brushes.Red, dragPoint.Point.X - this._mouseDragElementSize / 3, dragPoint.Point.Y - this._mouseDragElementSize / 3, this._mouseDragElementSize * 1.5f, this._mouseDragElementSize * 1.5f);

                                using (var pen = new Pen(Brushes.White, 4))
                                {
                                    var centerX = dragPoint.Point.X + this._mouseDragElementSize / 2;
                                    var centerY = dragPoint.Point.Y + this._mouseDragElementSize / 2;
                                    e.Graphics.DrawLine(pen, new Point(centerX - 4, centerY - 4), new Point(centerX + 4, centerY + 4));
                                    e.Graphics.DrawLine(pen, new Point(centerX + 4, centerY - 4), new Point(centerX - 4, centerY + 4));
                                }
                            }
                            else
                            {
                                e.Graphics.FillEllipse(dragElementBrush, dragPoint.Point.X, dragPoint.Point.Y, this._mouseDragElementSize, this._mouseDragElementSize);
                            }
                        }
                    }
                }
            }

            if (this._createBoundingBox)
            {
                var point1 = this._creationPoint;
                var point2 = this._mousePosition;

                var topLeftCorner = new Point((int)Math.Min(point1.X, point2.X), (int)Math.Min(point1.Y, point2.Y));
                var bottomRightCorner = new Point((int)Math.Max(point1.X, point2.X), (int)Math.Max(point1.Y, point2.Y));

                var allowed = true;
                if (bottomRightCorner.X - topLeftCorner.X < this._minSize.Width || bottomRightCorner.Y - topLeftCorner.Y < this._minSize.Height)
                {
                    allowed = false;
                }

                using (var pen = new Pen(allowed ? Color.FromArgb(255, 255, 255, 0) : Color.FromArgb(255, 255, 191, 0)))
                {
                    e.Graphics.DrawRectangle(pen,
                        new Rectangle(topLeftCorner, new Size(bottomRightCorner.X - topLeftCorner.X, bottomRightCorner.Y - topLeftCorner.Y)));

                    if (!allowed)
                    {
                        e.Graphics.DrawLine(pen, topLeftCorner, bottomRightCorner);
                        e.Graphics.DrawLine(pen, new PointF(topLeftCorner.X, bottomRightCorner.Y), new PointF(bottomRightCorner.X, topLeftCorner.Y));
                    }
                }
            }
        }

        private void DrawLabel(Graphics graphics, float x, float y, ObjectClass objectClass)
        {
            if (objectClass == null || !this.ShowLabels)
            {
                return;
            }

            using (var brush = new SolidBrush(DrawHelper.GetColorCode(objectClass.Id)))
            using (var bgBrush = new SolidBrush(Color.FromArgb(128, 255, 255, 255)))
            using (var font = new Font("Arial", 12))
            {
                var text = $"{objectClass.Id} {objectClass.Name}";
                var point = new PointF(x + 4, y + 4);
                var size = graphics.MeasureString(text, font);

                graphics.FillRectangle(bgBrush, point.X, point.Y, size.Width, size.Height);
                graphics.DrawString(text, font, brush, point);
            }
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            var startDrag = false;
            this._createBoundingBox = false;

            var boundingBoxes = this._annotationImage?.BoundingBoxes;
            if (boundingBoxes != null)
            {
                var drawOffset = this._mouseDragElementSize / 2;
                var canvasInfo = this.GetCanvasInformation();

                foreach (var boundingBox in boundingBoxes)
                {
                    this._draggedBoundingBox = null;
                    this._dragPoint = null;

                    var width = boundingBox.Width * canvasInfo.ScaledWidth;
                    var height = (boundingBox.Height * canvasInfo.ScaledHeight);
                    var x = (boundingBox.CenterX * canvasInfo.ScaledWidth) - (width / 2) + canvasInfo.OffsetX;
                    var y = (boundingBox.CenterY * canvasInfo.ScaledHeight) - (height / 2) + canvasInfo.OffsetY;

                    var rectangle = new Rectangle((int)x, (int)y, (int)width, (int)height);

                    if (this.PointInRectangle(this._mousePosition, rectangle, 15))
                    {
                        startDrag = true;

                        this._grabOffsetX = (this._mousePosition.X - rectangle.X) / canvasInfo.ScaledWidth;
                        this._grabOffsetY = (this._mousePosition.Y - rectangle.Y) / canvasInfo.ScaledHeight;
                    }

                    var dragPoints = this.GetDragPoints(rectangle, drawOffset);
                    foreach (var dragPoint in dragPoints)
                    {
                        if (this.PointDistance(this._mousePosition, new Point(dragPoint.Point.X, dragPoint.Point.Y)) < this._maxMouseDistanceToDragPoint)
                        {
                            this._dragPoint = dragPoint;

                            startDrag = true;

                            this._grabOffsetX = (this._dragPoint.Point.X - rectangle.X) / canvasInfo.ScaledWidth;
                            this._grabOffsetY = (this._dragPoint.Point.Y - rectangle.Y) / canvasInfo.ScaledHeight;

                            break;
                        }
                    }

                    if (startDrag)
                    {
                        this._selectedBoundingBox = boundingBox;
                        this._draggedBoundingBox = boundingBox;
                        this._selectedObjectRect = new RectangleF(rectangle.X / (float)canvasInfo.ScaledWidth, rectangle.Y / (float)canvasInfo.ScaledHeight,
                            rectangle.Width / (float)canvasInfo.ScaledWidth, rectangle.Height / (float)canvasInfo.ScaledHeight);

                        break;
                    }
                }
            }

            if (!startDrag)
            {
                this._createBoundingBox = true;
                this._creationPoint = e.Location;
            }

            this._mousePosition = e.Location;
            this.pictureBox1.Invalidate();
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (this._createBoundingBox)
            {
                this._createBoundingBox = false;
                if (Math.Abs(this._creationPoint.X - e.Location.X) >= this._minSize.Width &&
                    Math.Abs(this._creationPoint.Y - e.Location.Y) >= this._minSize.Height) {
                    this.CreateBoundingBox(this._creationPoint, e.Location);
                }

                return;
            }

            if (this._dragPoint?.Type == DragPointType.Delete)
            {
                this._annotationImage?.BoundingBoxes?.Remove(this._draggedBoundingBox);
            }

            this._draggedBoundingBox = null;

            this._mousePosition = new Point(0, 0);
            this.pictureBox1.Invalidate();

            this.ImageEdited?.Invoke(this._annotationImage);
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this._draggedBoundingBox != null)
            {
                var canvasInfo = this.GetCanvasInformation();

                var x = (e.X - canvasInfo.OffsetX) / canvasInfo.ScaledWidth;
                var y = (e.Y - canvasInfo.OffsetY) / canvasInfo.ScaledHeight;

                var centerX = x + (this._draggedBoundingBox.Width / 2) - this._grabOffsetX;
                var centerY = y + (this._draggedBoundingBox.Height / 2) - this._grabOffsetY;

                centerX = centerX.Clamp(this._draggedBoundingBox.Width / 2, 1 - this._draggedBoundingBox.Width / 2);
                centerY = centerY.Clamp(this._draggedBoundingBox.Height / 2, 1 - this._draggedBoundingBox.Height / 2);

                if (this._dragPoint != null)
                {
                    var cachedCenter = new PointF(this._selectedObjectRect.X + this._selectedObjectRect.Width / 2, this._selectedObjectRect.Y + this._selectedObjectRect.Height / 2);

                    var anchor = this.GetOppositeAnchor(
                        this._selectedObjectRect,
                        new PointF((float)this._grabOffsetX, (float)this._grabOffsetY));

                    var xSign = Math.Sign((this._selectedObjectRect.X + this._grabOffsetX) - anchor.X);
                    var ySign = Math.Sign((this._selectedObjectRect.Y + this._grabOffsetY) - anchor.Y);

                    double width = this._selectedObjectRect.Width;
                    double height = this._selectedObjectRect.Height;

                    if (this._dragPoint.Angle != 90 && this._dragPoint.Angle != 270)
                    {
                        width = Math.Max(this._minSize.Width / canvasInfo.ScaledWidth, xSign * ((x - anchor.X) + (canvasInfo.OffsetX / canvasInfo.ScaledWidth)));
                    }
                    if (this._dragPoint.Angle != 0 && this._dragPoint.Angle != 180)
                    {
                        height = Math.Max(this._minSize.Height / canvasInfo.ScaledHeight, ySign * ((y - anchor.Y) + (canvasInfo.OffsetY / canvasInfo.ScaledHeight)));
                    }

                    centerX = -(canvasInfo.OffsetX / canvasInfo.ScaledWidth) + cachedCenter.X + (width - this._selectedObjectRect.Width) / 2 * xSign;
                    centerY = -(canvasInfo.OffsetY / canvasInfo.ScaledHeight) + cachedCenter.Y + (height - this._selectedObjectRect.Height) / 2 * ySign;

                    this._draggedBoundingBox.Width = (float)width;
                    this._draggedBoundingBox.Height = (float)height;
                }

                this._draggedBoundingBox.CenterX = (float)centerX;
                this._draggedBoundingBox.CenterY = (float)centerY;
            }

            this._mousePosition = e.Location;
            this.pictureBox1.Invalidate();
        }

        private void PictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.CreateBoundingBox(new PointF(e.X, e.Y), new PointF(e.X + 150, e.Y + 100));
        }

        private void CreateBoundingBox(PointF point1, PointF point2)
        {
            var canvasInfo = this.GetCanvasInformation();

            var topLeftCorner = new PointF(Math.Min(point1.X, point2.X), Math.Min(point1.Y, point2.Y));
            var bottomRightCorner = new PointF(Math.Max(point1.X, point2.X), Math.Max(point1.Y, point2.Y));

            var width = (bottomRightCorner.X - topLeftCorner.X) / canvasInfo.ScaledWidth;
            var height = (bottomRightCorner.Y - topLeftCorner.Y) / canvasInfo.ScaledHeight;

            var x = (topLeftCorner.X - canvasInfo.OffsetX + (width * canvasInfo.ScaledWidth / 2)) / canvasInfo.ScaledWidth;
            var y = (topLeftCorner.Y - canvasInfo.OffsetY + (height * canvasInfo.ScaledHeight / 2)) / canvasInfo.ScaledHeight;

            if (this._annotationImage.BoundingBoxes == null)
            {
                this._annotationImage.BoundingBoxes = new List<AnnotationBoundingBox>();
            }

            var newBoundingBox = new AnnotationBoundingBox
            {
                CenterX = (float)x,
                CenterY = (float)y,
                Width = (float)width,
                Height = (float)height
            };
            this._selectedBoundingBox = newBoundingBox;

            this._annotationImage.BoundingBoxes.Add(newBoundingBox);
            this._dragPoint = null;
            this.ImageEdited?.Invoke(this._annotationImage);
        }

        private void PictureBox1_MouseEnter(object sender, EventArgs e)
        {
            this._mouseOver = true;
        }

        private void PictureBox1_MouseLeave(object sender, EventArgs e)
        {
            this._mouseOver = false;
            this.pictureBox1.Invalidate();
        }

        private void ClearAnnotationsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this._annotationImage.BoundingBoxes = null;
            this.ImageEdited?.Invoke(this._annotationImage);
        }

        #region Delegate callbacks

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            var currentBoundingBox = this._selectedBoundingBox;

            if (currentBoundingBox == null)
            {
                return;
            }

            // Select object class
            var index = -1;
            if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
            {
                index = (int)e.KeyCode - (int)Keys.D0;
            }
            else if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
            {
                index = (int)e.KeyCode - (int)Keys.NumPad0;
            }
            else if (e.KeyCode == Keys.Right)
            {
                index = currentBoundingBox.ObjectIndex + 1;
            }
            else if (e.KeyCode == Keys.Left)
            {
                index = currentBoundingBox.ObjectIndex - 1;
            }

            if (index >= 0 && index < this._objectClasses.Count)
            {
                this._changedImageViaKey = true;
                currentBoundingBox.ObjectIndex = index;
            }

            // Move Bounding Box
            var speed = e.Shift ? 1 : 10;
            var canvasInfo = this.GetCanvasInformation();
            
            if (e.KeyCode == Keys.W)
            {
                this._changedImageViaKey = true;

                this.MoveOrResize(currentBoundingBox, new PointF(0, -speed), canvasInfo,
                    e.Control ? KeyboardOperation.Resize : KeyboardOperation.Move,
                    e.Alt ? ScaleOperation.Inverse : ScaleOperation.Regular);
            }
            if (e.KeyCode == Keys.A)
            {
                this._changedImageViaKey = true;

                this.MoveOrResize(currentBoundingBox, new PointF(-speed, 0), canvasInfo,
                    e.Control ? KeyboardOperation.Resize : KeyboardOperation.Move,
                    e.Alt ? ScaleOperation.Inverse : ScaleOperation.Regular);
            }
            if (e.KeyCode == Keys.S)
            {
                this._changedImageViaKey = true;

                this.MoveOrResize(currentBoundingBox, new PointF(0, speed), canvasInfo,
                    e.Control ? KeyboardOperation.Resize : KeyboardOperation.Move,
                    e.Alt ? ScaleOperation.Inverse : ScaleOperation.Regular);
            }
            if (e.KeyCode == Keys.D)
            {
                this._changedImageViaKey = true;

                this.MoveOrResize(currentBoundingBox, new PointF(speed, 0), canvasInfo,
                    e.Control ? KeyboardOperation.Resize : KeyboardOperation.Move,
                    e.Alt ? ScaleOperation.Inverse : ScaleOperation.Regular);
            }

            this.pictureBox1.Invalidate();
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (this._changedImageViaKey)
            {
                this._changedImageViaKey = false;
                this.ImageEdited?.Invoke(this._annotationImage);
            }
        }

        private void MoveOrResize(
            AnnotationBoundingBox boundingBox,
            PointF translation,
            CanvasInfo canvasInfo,
            KeyboardOperation keyboardOperation,
            ScaleOperation scaleOperation)
        {
            if (keyboardOperation == KeyboardOperation.Move)
            {
                boundingBox.CenterX += translation.X / (float)canvasInfo.ScaledWidth;
                boundingBox.CenterY += translation.Y / (float)canvasInfo.ScaledWidth;

                boundingBox.CenterX = boundingBox.CenterX.Clamp(boundingBox.Width / 2, 1 - boundingBox.Width / 2);
                boundingBox.CenterY = boundingBox.CenterY.Clamp(boundingBox.Height / 2, 1 - boundingBox.Height / 2);
            }
            else
            {
                var inverseFac = (scaleOperation == ScaleOperation.Inverse ? -1 : 1);

                var newCenterX = boundingBox.CenterX + translation.X / (float)canvasInfo.ScaledWidth / 2;
                var newWidth = boundingBox.Width + translation.X / (float)canvasInfo.ScaledWidth * inverseFac;

                if (newCenterX - newWidth / 2 >= 0 && newCenterX + newWidth / 2 <= 1 && newWidth * canvasInfo.ScaledWidth > this._minSize.Width)
                {
                    boundingBox.CenterX = newCenterX;
                    boundingBox.Width = newWidth;
                }

                var newCenterY = boundingBox.CenterY + translation.Y / (float)canvasInfo.ScaledHeight / 2;
                var newHeight = boundingBox.Height + translation.Y / (float)canvasInfo.ScaledHeight * inverseFac;

                if (newCenterY - newHeight / 2 >= 0 && newCenterY + newHeight / 2 <= 1 && newHeight * canvasInfo.ScaledHeight > this._minSize.Height)
                {
                    boundingBox.CenterY += translation.Y / (float)canvasInfo.ScaledHeight / 2;
                    boundingBox.Height += translation.Y / (float)canvasInfo.ScaledHeight * inverseFac;
                }
            }
        }

        #endregion
    }
}
