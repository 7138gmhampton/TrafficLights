public struct Junction
{
    public int XLocus { get; }
    public int YLocus { get; }
    public JunctionController Controller { get; }

    public Junction(int xLocus, int yLocus, JunctionController controller)
    {
        XLocus = xLocus;
        YLocus = yLocus;
        Controller = controller;
    }
}