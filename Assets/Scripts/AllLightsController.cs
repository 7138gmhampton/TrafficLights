using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllLightsController : MonoBehaviour
{
    [HideInInspector] public AllLightsController instance = null;
    public EnvironmentManager environmentManager;
    public GameObject junctionController;
    [HideInInspector] public List<Junction> junctions = new List<Junction>();

    private bool reported = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    void Start()
    {
        setupJunctionControllers();
    }

    public void switchNS(int x, int y)
    {
        var junction = findJunction(x, y);

        junction.Controller.goGreenNorthSouth(true);
    }

    public void switchEW(int x, int y)
    {
        var junction = findJunction(x, y);

        junction.Controller.goGreenNorthSouth(false);
    }

    public int reportHighestX()
    {
        int xValue = 0;

        foreach (var junction in junctions)
            if (junction.XLocus > xValue) xValue = junction.XLocus;

        return xValue;
    }

    public int reportHighestY()
    {
        int yValue = 0;

        foreach (var junction in junctions)
            if (junction.YLocus > yValue) yValue = junction.YLocus;

        return yValue;
    }

    public bool checkAlignment(int x, int y)
    {
        var junction = findJunction(x, y);

        return junction.Controller.northSouthAlign;
    }

    private Junction findJunction(int x, int y)
    {
        foreach (var junction in junctions)
            if (x == junction.XLocus && y == junction.YLocus) return junction;

        throw new System.ArgumentOutOfRangeException("loci", "Loci do not match junction");
    }

    private void setupJunctionControllers()
    {
        foreach (var junction in environmentManager.junctions)
            junctions.Add(createJunction(junction.transform.position));
    }

    private Junction createJunction(Vector2 locus)
    {
        var controller = Instantiate(junctionController, locus, Quaternion.identity);

        var junction = new Junction(
            ((int)locus.x) / 6,
            ((int)locus.y) / 6,
            controller,
            controller.GetComponent<JunctionController>());

        return junction;
    }
}
