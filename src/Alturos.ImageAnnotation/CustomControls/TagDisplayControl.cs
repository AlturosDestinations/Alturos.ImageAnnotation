using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.CustomControls
{
    public partial class TagDisplayControl : UserControl
    {
        private const int _labelSpacing = 5;
        private const int _labelPadding = 2;

        private int _labelOffsetX;
        private int _labelOffsetY;

        public string[] Tags { get; set; }

        public TagDisplayControl()
        {
            this.InitializeComponent();
        }

        public void SetTags(params string[] tags)
        {
            this.Tags = tags;
            this.Invalidate();
        }

        private void TagDisplayControl_Paint(object sender, PaintEventArgs e)
        {
            var size = this.Size;

            this._labelOffsetX = 0;
            this._labelOffsetY = 0;

            if (this.Tags == null)
            {
                return;
            }

            foreach (var tag in this.Tags)
            {
                var rectangle = this.GetRectangle(e.Graphics, tag, size);
                e.Graphics.FillRectangle(Brushes.SkyBlue, rectangle);
                e.Graphics.DrawString(tag, SystemFonts.DefaultFont, Brushes.DarkBlue, new PointF(rectangle.X + _labelPadding, rectangle.Y + _labelPadding));
            }
        }

        private Rectangle GetRectangle(Graphics graphics, string tag, Size size)
        {
            var tagSize = graphics.MeasureString(tag, SystemFonts.DefaultFont);
            tagSize = new SizeF(tagSize.Width + _labelPadding * 2, tagSize.Height + _labelPadding * 2);

            if (this._labelOffsetX + tagSize.Width > size.Width)
            {
                this._labelOffsetX = 0;
                this._labelOffsetY += (int)tagSize.Height + _labelSpacing;
            }

            var rect = new Rectangle(this._labelOffsetX, this._labelOffsetY, (int)tagSize.Width, (int)tagSize.Height);

            this._labelOffsetX += (int)tagSize.Width + _labelSpacing;

            return rect;
        }
    }
}
