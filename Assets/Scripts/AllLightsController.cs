using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllLightsController : MonoBehaviour
{
    [HideInInspector] public AllLightsController instance = null;
    public EnvironmentManager environmentManager;
    public GameObject junctionController;
    [HideInInspector] public List<Junction> junctions = new List<Junction>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    void Start()
    {
        setupJunctionControllers();
        Debug.Log(reportHighestX());
        Debug.Log(reportHighestY());
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

    public int reportHighestX() => junctions.Max(x => x.XLocus);

    public int reportHighestY() => junctions.Max(x => x.YLocus);

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
