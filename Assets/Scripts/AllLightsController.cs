using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllLightsController : MonoBehaviour
{
    [HideInInspector] public AllLightsController instance = null;
    //[HideInInspector] public List<Junction> junctions = new List<Junction>();
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

    void Update()
    {
        if (!reported) {
            //foreach (var junction in junctions)
            //    Debug.Log("(" + junction.XLocus + "," + junction.YLocus + ")");
            reported = true;

            //Debug.Log(reportHighestX() + " by " + reportHighestY());
            //switchEW(0, 0);
            //junctions[0].Controller.goGreenNorthSouth(false);
            //var controller = junctions[0].Controller.gameObject.GetComponent<JunctionController>();
            //controller.goGreenNorthSouth(false);
            //Debug.Log(environmentManager.junctionControllers.Count);
            //environmentManager.junctionControllers[0].Object.GetComponent<JunctionController>().goGreenNorthSouth(false);
            //environmentManager.junctionControllers[0].Controller.goGreenNorthSouth(false);
            //junctions[0].Controller.goGreenNorthSouth(false);
            switchEW(0, 1);
            Debug.Log(checkAlignment(0, 0));
            Debug.Log(checkAlignment(0, 1));
        }
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

        //foreach (var junction in junctions)
        //    if (junction.XLocus > xValue) xValue = junction.XLocus;

        return xValue;
    }

    public int reportHighestY()
    {
        int yValue = 0;

        //foreach (var junction in junctions)
        //    if (junction.YLocus > yValue) yValue = junction.YLocus;

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
