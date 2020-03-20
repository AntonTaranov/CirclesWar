using System.Collections.Generic;
namespace CirclesWar.Data
{
    public class Field2D
    {
        readonly Cell[] cells;

        public Field2D(float x, float y, float width, float height, int numRows, int numColumns)
        {
            cells = new Cell[numRows * numColumns];
            var cellWidth = width / numColumns;
            var cellHeight = height / numRows;
            for (int column = 0; column < numColumns; column++)
            {
                for(int row = 0; row < numRows; row++)
                {
                    var cell = new Cell(x + cellWidth * column, y + cellHeight * row,
                                        cellWidth, cellHeight);
                    cells[column + row * numColumns] = cell;
                }
            }
        }

        public Pair<CircleData>[] FindOverlaps(List<CircleData> circles)
        {
            var overlaps = new List<Pair<CircleData>>();

            foreach(Cell cell in cells)
            {
                var circlesInCell = new List<CircleData>();
                foreach(CircleData circle in circles)
                {
                    if (cell.IsInside(circle))
                    {
                        circlesInCell.Add(circle);
                    }
                }
                for (int i = 0; i < circlesInCell.Count; i++)
                {
                    for (int j = i + 1; j < circlesInCell.Count; j++)
                    {
                        var circleOne = circlesInCell[i];
                        var circleTwo = circlesInCell[j];
                        var sqrDistance = (circleOne.GetPosition() - circleTwo.GetPosition()).sqrMagnitude;
                        var sqrMinDistance = (circleOne.GetRadius() + circleTwo.GetRadius()) * (circleOne.GetRadius() + circleTwo.GetRadius());
                        if (sqrDistance < sqrMinDistance)
                        {
                            bool alreadyInResult = false;
                            foreach(Pair<CircleData> pair in overlaps)
                            {
                                if (pair.first == circleOne && pair.second == circleTwo 
                                || pair.first == circleTwo && pair.second == circleOne)
                                {
                                    alreadyInResult = true;
                                }
                            }
                            if (!alreadyInResult)
                            {
                                overlaps.Add(new Pair<CircleData>(circleOne, circleTwo));
                            }
                        }
                    }
                }
            }

            return overlaps.ToArray();
        }
       
    }
}
