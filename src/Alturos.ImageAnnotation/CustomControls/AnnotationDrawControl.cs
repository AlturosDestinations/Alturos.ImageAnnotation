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
        public event Action<AnnotationImage> ImageEdited;

        public bool AutoplaceAnnotations { get; set; }
        public bool ShowLabels { get; private set; }

        private readonly int _mouseDragElementSize = 10;
        private readonly int _maxMouseDistanceToDragPoint = 10;

        private bool _mouseOver;
        private Point _mousePosition = new Point(0, 0);
        private RectangleF _selectedObjectRect;
        private AnnotationBoundingBox[] _cachedBoundingBoxes;
        private AnnotationBoundingBox _selectedBoundingBox;
        private DragPoint _dragPoint;
        private AnnotationImage _annotationImage;
        private List<ObjectClass> _objectClasses;
        private double _grabOffsetX;
        private double _grabOffsetY;
        private PointF _cachedCenter;

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
            var p1 = new Point(rectangle.X - drawOffset, rectangle.Y - drawOffset);
            var p2 = new Point(p1.X + rectangle.Width, p1.Y);
            var p3 = new Point(p1.X, p1.Y + rectangle.Height);
            var p4 = new Point(p1.X + rectangle.Width, p1.Y + rectangle.Height);
            var p5 = new Point(p1.X + rectangle.Width - 15, p1.Y + 15);

            return new DragPoint[]
            {
                new DragPoint(p1, DragPointType.Resize),
                new DragPoint(p2, DragPointType.Resize),
                new DragPoint(p3, DragPointType.Resize),
                new DragPoint(p4, DragPointType.Resize),
                new DragPoint(p5, DragPointType.Delete)
            };
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

        private PointF GetOppositeAnchor(RectangleF rect, PointF center, PointF anchor)
        {
            float x;
            if (rect.X + anchor.X > center.X)
            {
                x = rect.X;
            }
            else
            {
                x = rect.X + rect.Width;
            }

            float y;
            if (rect.Y + anchor.Y > center.Y)
            {
                y = rect.Y;
            }
            else
            {
                y = rect.Y + rect.Height;
            }

            return new PointF(x, y);
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

            System.Windows.Forms.Cursor.Current = Cursors.Default;

            if (this._mouseOver)
            {
                e.Graphics.DrawLine(Pens.Blue, new Point(this._mousePosition.X, this.pictureBox1.Top), new Point(this._mousePosition.X, this.pictureBox1.Bottom));
                e.Graphics.DrawLine(Pens.Blue, new Point(this.pictureBox1.Left, this._mousePosition.Y), new Point(this.pictureBox1.Right, this._mousePosition.Y));
            }

            if (this._annotationImage?.BoundingBoxes == null)
            {
                return;
            }

            var boundingBoxes = this._annotationImage?.BoundingBoxes;
            if (boundingBoxes == null)
            {
                return;
            }

            foreach (var boundingBox in boundingBoxes)
            {
                var rectangle = this.GetRectangle(boundingBox);
                var brush = DrawHelper.GetColorCode(boundingBox.ObjectIndex);
                var objectClass = this._objectClasses.FirstOrDefault(o => o.Id == boundingBox.ObjectIndex);

                e.Graphics.DrawRectangle(new Pen(brush, 2), rectangle);
                this.DrawLabel(e.Graphics, rectangle.X, rectangle.Y, objectClass);

                var biggerRectangle = Rectangle.Inflate(rectangle, 20, 20);
                if (biggerRectangle.Contains(this._mousePosition))
                {
                    var dragPoints = this.GetDragPoints(rectangle, drawOffset);
                    foreach (var dragPoint in dragPoints)
                    {
                        var dragElementBrush = Brushes.LightPink;
                        if (this.PointDistance(this._mousePosition, new Point(dragPoint.Point.X, dragPoint.Point.Y)) < this._maxMouseDistanceToDragPoint)
                        {
                            System.Windows.Forms.Cursor.Current = Cursors.Hand;
                            dragElementBrush = Brushes.Yellow;
                        }

                        if (dragPoint.Type == DragPointType.Resize)
                        {
                            var points = new Point[]
                            {
                                new Point(dragPoint.Point.X, dragPoint.Point.Y),
                                new Point(dragPoint.Point.X + 15 , dragPoint.Point.Y),
                                new Point(dragPoint.Point.X, dragPoint.Point.Y + 15),
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
            if (this._annotationImage?.BoundingBoxes == null)
            {
                return;
            }

            var drawOffset = this._mouseDragElementSize / 2;
            var canvasInfo = this.GetCanvasInformation();

            var boundingBoxes = this._annotationImage?.BoundingBoxes;
            if (boundingBoxes == null)
            {
                return;
            }

            foreach (var boundingBox in boundingBoxes)
            {
                this._selectedBoundingBox = null;
                this._dragPoint = null;

                var width = boundingBox.Width * canvasInfo.ScaledWidth;
                var height = (boundingBox.Height * canvasInfo.ScaledHeight);
                var x = (boundingBox.CenterX * canvasInfo.ScaledWidth) - (width / 2) + canvasInfo.OffsetX;
                var y = (boundingBox.CenterY * canvasInfo.ScaledHeight) - (height / 2) + canvasInfo.OffsetY;

                var rectangle = new Rectangle((int)x, (int)y, (int)width, (int)height);

                var startDrag = false;
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

                        this._grabOffsetX = (this._dragPoint.Point.X - rectangle.X) / canvasInfo.ScaledWidth;
                        this._grabOffsetY = (this._dragPoint.Point.Y - rectangle.Y) / canvasInfo.ScaledHeight;

                        break;
                    }
                }

                if (startDrag)
                {
                    this._cachedCenter = Point.Empty;

                    this._selectedBoundingBox = boundingBox;
                    this._selectedObjectRect = new RectangleF(rectangle.X / (float)canvasInfo.ScaledWidth, rectangle.Y / (float)canvasInfo.ScaledHeight,
                        rectangle.Width / (float)canvasInfo.ScaledWidth, rectangle.Height / (float)canvasInfo.ScaledHeight);

                    this._cachedCenter = new PointF(this._selectedObjectRect.X + this._selectedObjectRect.Width / 2, this._selectedObjectRect.Y + this._selectedObjectRect.Height / 2);

                    break;
                }
            }

            this._mousePosition = e.Location;
            this.pictureBox1.Invalidate();
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (this._dragPoint?.Type == DragPointType.Delete)
            {
                this._annotationImage?.BoundingBoxes?.Remove(this._selectedBoundingBox);
            }

            this._selectedBoundingBox = null;

            this._mousePosition = new Point(0, 0);
            this.pictureBox1.Invalidate();

            this.ImageEdited?.Invoke(this._annotationImage);
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this._selectedBoundingBox != null)
            {
                var canvasInfo = this.GetCanvasInformation();

                var x = (e.X - canvasInfo.OffsetX) / canvasInfo.ScaledWidth;
                var y = (e.Y - canvasInfo.OffsetY) / canvasInfo.ScaledHeight;

                var centerX = x + (this._selectedBoundingBox.Width / 2) - this._grabOffsetX;
                var centerY = y + (this._selectedBoundingBox.Height / 2) - this._grabOffsetY;

                centerX = centerX.Clamp(this._selectedBoundingBox.Width / 2, 1 - this._selectedBoundingBox.Width / 2);
                centerY = centerY.Clamp(this._selectedBoundingBox.Height / 2, 1 - this._selectedBoundingBox.Height / 2);

                if (this._dragPoint != null)
                {
                    var anchor = this.GetOppositeAnchor(
                        this._selectedObjectRect,
                        new PointF(this._cachedCenter.X, this._cachedCenter.Y),
                        new PointF((float)this._grabOffsetX, (float)this._grabOffsetY));

                    var xSign = Math.Sign((this._selectedObjectRect.X + this._grabOffsetX) - anchor.X);
                    var ySign = Math.Sign((this._selectedObjectRect.Y + this._grabOffsetY) - anchor.Y);

                    var width = Math.Max(30 / canvasInfo.ScaledWidth, xSign * (x - anchor.X));
                    var height = Math.Max(30 / canvasInfo.ScaledHeight, ySign * (y - anchor.Y));

                    centerX = this._cachedCenter.X + (width - this._selectedObjectRect.Width) / 2 * xSign;
                    centerY = this._cachedCenter.Y + (height - this._selectedObjectRect.Height) / 2 * ySign;

                    this._selectedBoundingBox.Width = (float)width;
                    this._selectedBoundingBox.Height = (float)height;
                }

                this._selectedBoundingBox.CenterX = (float)centerX;
                this._selectedBoundingBox.CenterY = (float)centerY;
            }

            this._mousePosition = e.Location;
            this.pictureBox1.Invalidate();
        }

        private void PictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var canvasInfo = this.GetCanvasInformation();
            var width = 0.04;
            var height = 0.06;

            var x = (e.X - canvasInfo.OffsetX + (width * canvasInfo.ScaledWidth / 2)) / canvasInfo.ScaledWidth;
            var y = (e.Y - canvasInfo.OffsetY + (height * canvasInfo.ScaledHeight / 2)) / canvasInfo.ScaledHeight;

            if (this._annotationImage.BoundingBoxes == null)
            {
                this._annotationImage.BoundingBoxes = new List<AnnotationBoundingBox>();
            }

            var newBoundingBox = new AnnotationBoundingBox
            {
                CenterX = (float)x,
                CenterY = (float)y,
                Height = (float)height,
                Width = (float)width
            };

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
            AnnotationBoundingBox currentBoundingBox = null;

            var boundingBoxes = this._annotationImage?.BoundingBoxes;
            if (boundingBoxes != null)
            {
                foreach (var boundingBox in boundingBoxes)
                {
                    var rectangle = this.GetRectangle(boundingBox);

                    var biggerRectangle = Rectangle.Inflate(rectangle, 20, 20);
                    if (biggerRectangle.Contains(this._mousePosition))
                    {
                        currentBoundingBox = boundingBox;
                        break;
                    }
                }
            }

            if (currentBoundingBox == null)
            {
                return;
            }

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

            if (index < 0 || index >= this._objectClasses.Count)
            {
                return;
            }

            currentBoundingBox.ObjectIndex = index;
            this.pictureBox1.Invalidate();

            this.ImageEdited?.Invoke(this._annotationImage);
        }

        #endregion
    }
}
