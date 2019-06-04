using System.Drawing;

namespace Alturos.ImageAnnotation.Model
{
    public class DragPoint
    {
        public Point Point { get; private set; }
        public DragPointType Type { get; private set; }

        public DragPoint(Point point, DragPointType type)
        {
            this.Point = point;
            this.Type = type;
        }
    }
}
