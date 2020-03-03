﻿using UnityEngine;

public class JunctionController : MonoBehaviour
{
    public GameObject trafficLightNorth;
    public GameObject trafficLightEast;
    public GameObject trafficLightSouth;
    public GameObject trafficLightWest;
    [HideInInspector] public bool northSouthAlign;

    private GameObject westLight;
    private TrafficLight westControl;
    private GameObject eastLight;
    private TrafficLight eastControl;
    private GameObject northLight;
    private TrafficLight northControl;
    private GameObject southLight;
    private TrafficLight southControl;

    private void Awake() => createTrafficLights();

    private void Start()
    {
        westControl = westLight.GetComponent<TrafficLight>();
        eastControl = eastLight.GetComponent<TrafficLight>();
        northControl = northLight.GetComponent<TrafficLight>();
        southControl = southLight.GetComponent<TrafficLight>();

        goGreenNorthSouth(true);
    }

    private void createTrafficLights()
    {
        var origin = transform.position;

        westLight = createLight(trafficLightWest, origin, new Vector2(0, 0));
        northLight = createLight(trafficLightNorth, origin, new Vector2(1, 0));
        eastLight = createLight(trafficLightEast, origin, new Vector2(1, -1));
        southLight = createLight(trafficLightSouth, origin, new Vector2(0, -1));
    }

    private GameObject createLight(GameObject lightObject, Vector3 origin, Vector2 offset)
    {
        var light = Instantiate(lightObject, origin + (Vector3)offset, Quaternion.identity);
        light.transform.parent = gameObject.transform;

        return light;
    }

    public void goGreenNorthSouth(bool northSouth)
    {
        northSouthAlign = northSouth;

        northControl.setLightMode(northSouth);
        southControl.setLightMode(northSouth);
        eastControl.setLightMode(!northSouth);
        westControl.setLightMode(!northSouth);
    }
}
