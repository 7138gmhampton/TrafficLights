﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllLightsController : MonoBehaviour
{
    [HideInInspector] public AllLightsController instance = null;
    public EnvironmentManager environmentManager;
    public GameObject junctionController;
    [HideInInspector] public List<JunctionSwitcher> junctions = new List<JunctionSwitcher>();

    public List<JunctionSwitcher> Junctions { get { return junctions; } }

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    public void switchNS(int x, int y) => findJunction(x, y).Controller.setGreenAlignment(true);

    public void switchEW(int x, int y) => findJunction(x, y).Controller.setGreenAlignment(false);

    public int reportHighestX() => junctions.Max(x => x.XLocus);

    public int reportHighestY() => junctions.Max(x => x.YLocus);

    public bool checkAlignment(int x, int y) => findJunction(x, y).Controller.northSouthAlign;

    public bool checkAlignment(int index) => junctions[index].Controller.northSouthAlign;

    public Tuple<float, float, float, float> reportWaitTimes(int x, int y)
    {
        var junction = findJunction(x, y);

        float northTime = junction.Controller.reportTotalWaitTimeInQueue(Direction.NORTH);
        float eastTime = junction.Controller.reportTotalWaitTimeInQueue(Direction.EAST);
        float southTime = junction.Controller.reportTotalWaitTimeInQueue(Direction.SOUTH);
        float westTime = junction.Controller.reportTotalWaitTimeInQueue(Direction.WEST);

        return new Tuple<float, float, float, float>(northTime, eastTime, southTime, westTime);
    }

    public Tuple<int,int,int,int> reportQueueNumbers(int x, int y)
    {
        var junction = findJunction(x, y);

        int northwardQueue = junction.Controller.countQueue(Direction.NORTH);
        int eastwardQueue = junction.Controller.countQueue(Direction.EAST);
        int southwardQueue = junction.Controller.countQueue(Direction.SOUTH);
        int westwardQueue = junction.Controller.countQueue(Direction.WEST);

        return new Tuple<int, int, int, int>(northwardQueue, eastwardQueue, southwardQueue, westwardQueue);
    }

    private JunctionSwitcher findJunction(int x, int y) =>
        junctions.Single(a => a.XLocus == x && a.YLocus == y);

    public void setupJunctionControllers()
    {
        foreach (var junction in environmentManager.junctions)
            junctions.Add(createJunction(junction.transform.position));
    }

    private JunctionSwitcher createJunction(Vector2 locus)
    {
        var controller = Instantiate(junctionController, locus, Quaternion.identity);

        var junction = new JunctionSwitcher(
            ((int)locus.x) / 6,
            ((int)locus.y) / 6,
            controller,
            controller.GetComponent<JunctionController>());

        return junction;
    }
}
