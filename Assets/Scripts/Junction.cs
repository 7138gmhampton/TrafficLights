using UnityEngine;

public struct Junction
{
    public int XLocus { get; }
    public int YLocus { get; }
    public GameObject Object { get; }
    public JunctionController Controller { get; }

    public Junction(int xLocus, int yLocus, GameObject gameObject, JunctionController controller)
    {
        XLocus = xLocus;
        YLocus = yLocus;
        Object = gameObject;
        Controller = controller;
    }
}