using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllLightsController : MonoBehaviour
{
    [HideInInspector] public AllLightsController instance = null;
    [HideInInspector] public List<Junction> junctions = new List<Junction>();

    private bool reported = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (!reported) {
            foreach (var junction in junctions)
                Debug.Log("(" + junction.XLocus + "," + junction.YLocus + ")");
            reported = true;
        }
    }

    public void switchNS(int x, int y)
    {
        var junction = findJunction(x, y);

        junction.Controller.goGreenNorthSouth(true);
    }

    public void switchEW(int x, int y)
    {

    }

    private Junction findJunction(int x, int y)
    {
        foreach (var junction in junctions)
            if (x == junction.XLocus && y == junction.YLocus) return junction;

        throw new System.ArgumentOutOfRangeException("loci", "Loci do not match junction");
    }
}
