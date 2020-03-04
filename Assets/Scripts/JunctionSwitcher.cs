using UnityEngine;

public struct JunctionSwitcher
{
    public int XLocus { get; }
    public int YLocus { get; }
    public GameObject Object { get; }
    public JunctionController Controller { get; }

    public JunctionSwitcher(int xLocus, int yLocus, GameObject gameObject, JunctionController controller)
    {
        XLocus = xLocus;
        YLocus = yLocus;
        Object = gameObject;
        Controller = controller;
    }
}