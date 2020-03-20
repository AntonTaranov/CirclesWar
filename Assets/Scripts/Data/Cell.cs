using System;
namespace CirclesWar.Data
{
    public class Cell
    {
        readonly float x;
        readonly float y;
        readonly float width;
        readonly float height;

        public Cell(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public bool IsInside(CircleData circle)
        {
            var circlePosition = circle.GetPosition();
            var circleRadius = circle.GetRadius();
            if (x > circlePosition.x + circleRadius) return false;
            if (x + width < circlePosition.x - circleRadius) return false;
            if (y > circlePosition.y + circleRadius) return false;
            if (y + height < circlePosition.y - circleRadius) return false;
            return true;
        }
    }
}
