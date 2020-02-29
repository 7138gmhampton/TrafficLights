using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JunctionController : MonoBehaviour
{
    public GameObject trafficLightNorth;
    public GameObject trafficLightEast;
    public GameObject trafficLightSouth;
    public GameObject trafficLightWest;

    //private List<GameObject> trafficLights = new List<GameObject>();
    private GameObject westLight;
    private GameObject eastLight;
    private GameObject northLight;
    private GameObject southLight;

    private void Awake()
    {
        createTrafficLights();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void createTrafficLights()
    {
        //var lightOffsets = new List<Tuple<Vector3, GameObject, GameObject>>()
        //{
        //    new Tuple<Vector3, GameObject>(new Vector3(0,0,0), trafficLightWest, westLight),
        //    new Tuple<Vector3, GameObject>(new Vector3(1,0,0), trafficLightNorth, northLight),
        //    new Tuple<Vector3, GameObject>(new Vector3(1,1,0), trafficLightEast, eastLight),
        //    new Tuple<Vector3, GameObject>(new Vector3(0,1,0), trafficLightSouth, )
        //};
        var origin = transform.position;

        westLight = Instantiate(trafficLightWest, origin, Quaternion.identity);
        northLight = Instantiate(trafficLightNorth, origin + new Vector3(1, 0, 0), Quaternion.identity);
        eastLight = Instantiate(trafficLightEast, origin + new Vector3(1, -1, 0), Quaternion.identity);
        southLight = Instantiate(trafficLightSouth, origin + new Vector3(0, -1, 0), Quaternion.identity);
    }
}
