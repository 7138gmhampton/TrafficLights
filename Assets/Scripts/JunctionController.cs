using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class JunctionController : MonoBehaviour
{
    public GameObject trafficLightNorth;
    public GameObject trafficLightEast;
    public GameObject trafficLightSouth;
    public GameObject trafficLightWest;
    public LayerMask blockingLayer;
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

    public int countQueue(Direction direction)
    {
        var controllerPosition = (Vector2)transform.position;

        List<Collider2D> cars = getQueueOfCars(controllerPosition, direction);

        return cars.Count;
    }

    public float reportTotalWaitTimeInQueue(Direction direction)
    {
        var controllerPosition = transform.position;
        var cars = getQueueOfCars(controllerPosition, direction);

        float totalWaitTime = cars.Sum(x => x.gameObject.GetComponent<Car>().TimeSinceLastMove);

        return totalWaitTime;
    }

    private List<Collider2D> getQueueOfCars(Vector2 controllerPosition, Direction direction)
    {
        var offsetA = getOffSetA(direction);
        var offsetB = getOffSetB(direction);
        var colliders = Physics2D.OverlapAreaAll(controllerPosition + offsetA,
                    controllerPosition + offsetB, blockingLayer);

        List<Collider2D> cars = new List<Collider2D>();

        foreach (var collider in colliders)
            if (collider.tag == "Vehicle")
                cars.Add(collider);

        return cars;
    }

    private Vector2 getOffSetA(Direction direction)
    {
        switch (direction) {
            case Direction.EAST: return new Vector2(-4.5f, 0.5f);
            case Direction.NORTH: return new Vector2(-0.5f, -1.5f);
            case Direction.SOUTH: return new Vector2(0.5f, 4.5f);
            case Direction.WEST: return new Vector2(1.5f, -0.5f);
            default: throw new ArgumentOutOfRangeException("direction");
        }
    }

    private Vector2 getOffSetB(Direction direction)
    {
        switch (direction) {
            case Direction.EAST: return new Vector2(-0.5f, -0.5f);
            case Direction.NORTH: return new Vector2(0.5f, -5.5f);
            case Direction.SOUTH: return new Vector2(1.5f, 0.5f);
            case Direction.WEST: return new Vector2(5.5f, -1.5f);
            default: throw new ArgumentOutOfRangeException("direction");
        }
    }
}
