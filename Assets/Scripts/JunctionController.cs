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
    private TrafficLight westControl;
    private GameObject eastLight;
    private TrafficLight eastControl;
    private GameObject northLight;
    private TrafficLight northControl;
    private GameObject southLight;
    private TrafficLight southControl;

    private void Awake()
    {
        createTrafficLights();
    }

    void Start()
    {
        westControl = westLight.GetComponent<TrafficLight>();
        eastControl = eastLight.GetComponent<TrafficLight>();
        northControl = northLight.GetComponent<TrafficLight>();
        southControl = southLight.GetComponent<TrafficLight>();

        goGreenNorthSouth(true);
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

    private void goGreenNorthSouth(bool northSouth)
    {
        northControl.goGreen(northSouth);
        southControl.goGreen(northSouth);
        eastControl.goGreen(!northSouth);
        westControl.goGreen(!northSouth);
    }
}
