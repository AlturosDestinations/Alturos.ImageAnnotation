using System.Drawing;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.Model
{
    public class DragPoint
    {
        public Point Point { get; private set; }
        public DragPointType Type { get; private set; }
        public Cursor Cursor { get; private set; }
        public double Angle { get; private set; }

        public DragPoint(Point point, DragPointType type, Cursor cursor, double angle)
        {
            this.Point = point;
            this.Type = type;
            this.Cursor = cursor;
            this.Angle = angle;
        }
    }
}
