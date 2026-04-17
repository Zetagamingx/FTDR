public class GridCell
{
    public bool isOccupied;
    public bool isBuildable;

    public GridCell(bool buildable)
    {
        isOccupied = false;
        isBuildable = buildable;
    }
}