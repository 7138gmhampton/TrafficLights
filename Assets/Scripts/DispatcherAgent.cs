﻿using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System;

public class DispatcherAgent : Agent
{
    public AllLightsController lightsController;
    public CarManager carManager;
    public MetricsWatcher watcher;
    public TextMesh rewardDisplay;
    public TextMesh timeDisplay;
    public int carsPerEpisode;

    private float lastMeanJourneyTime = 1000f;
    private int carsPassed = 0;
    private Tuple<float,float,float,float>[,] lastWaitTimes;

    public override void AgentAction(float[] vectorAction)
    {
        for (int iii = 0; iii < vectorAction.Length; iii++) {
            //int action = Mathf.RoundToInt(vectorAction[iii]);
            //if (action == 1) changeLights(iii, true);
            //else if (action == 2) changeLights(iii, false);
            int index = iii / 2;
            int action = Mathf.RoundToInt(vectorAction[iii]);
            if (iii % 2 == 0 && Mathf.RoundToInt(vectorAction[iii + 1]) == 0) {
                changeLights(index, true);
            }
            else if (iii % 2 == 1 && Mathf.RoundToInt(vectorAction[iii - 1]) == 0)
                changeLights(index, false);
        }
            //lightsController.junctions[iii].Controller.setGreenAlignment((int)vectorAction[iii] == 1);
            //changeLights(iii, Mathf.RoundToInt(vectorAction[iii]) == 1);

        if (lastWaitTimes != null) {
            var waitTimes = watcher.getWaitTimes();
            for (int y = 0; y < waitTimes.GetLength(0); ++y)
                for (int x = 0; x < waitTimes.GetLength(1); ++x)
                    checkWaitTime(waitTimes[y, x], lastWaitTimes[y, x]);
        }

        if (rewardDisplay != null)
            rewardDisplay.text = GetCumulativeReward().ToString("0.00");
        if (timeDisplay != null)
            timeDisplay.text = watcher.reportJourneyTimeMean().ToString("0.00s");
        lastWaitTimes = watcher.getWaitTimes();
    }

    public override void CollectObservations()
    {
        float currentMeanJourneyTime = watcher.reportJourneyTimeMean();
        lastMeanJourneyTime = currentMeanJourneyTime;

        var waitTimes = watcher.getWaitTimes();
        
        for (int y = 0; y < waitTimes.GetLength(0); ++y)
            for (int x = 0; x < waitTimes.GetLength(1); ++x) {
                AddVectorObs(waitTimes[y, x].Item1);
                AddVectorObs(waitTimes[y, x].Item2);
                AddVectorObs(waitTimes[y, x].Item3);
                AddVectorObs(waitTimes[y, x].Item4);
            }

        for (int iii = 0; iii < lightsController.Junctions.Count; ++iii)
            AddVectorObs(lightsController.checkAlignment(iii));

        //if (lastWaitTimes != null)
        //    for (int y = 0; y < waitTimes.GetLength(0); ++y)
        //        for (int x = 0; x < waitTimes.GetLength(1); ++x)
        //            checkWaitTime(waitTimes[y, x], lastWaitTimes[y, x]);

        //lastWaitTimes = waitTimes;
    }

    public override float[] Heuristic()
    {
        var alignments = new List<float>();

        foreach (var junction in lightsController.junctions) {
            if (junction.Controller.northSouthAlign)
                alignments.Add(1f);
            else alignments.Add(2f);
        }

        switch (Input.inputString) {
            case "0": alignments[0] = alignments[0] == 1f ? 2f : 1f; break;
            case "1": alignments[1] = alignments[1] == 1f ? 2f : 1f; break;
            case "2": alignments[2] = alignments[2] == 1f ? 2f : 1f; break;
            case "3": alignments[3] = alignments[3] == 1f ? 2f : 1f; break;
        }

        return alignments.ToArray();
    }

    public override void AgentReset()
    {
        carManager.resetCars();
        watcher.resetMetrics();
        carsPassed = 0;
    }

    private void unacceptableWait() => AddReward(-1f);

    private void finishCar()
    {
        //AddReward(0.1f);

        if (++carsPassed > carsPerEpisode) Done();
    }

    private void checkWaitTime(Tuple<float, float, float, float> current,
        Tuple<float, float, float, float> previous)
    {
        float sumNorthSouthTimePrevious = previous.Item1 + previous.Item3;
        float sumEastWestTimePrevious = previous.Item2 + previous.Item4;
        float sumNorthSouthTimeCurrent = current.Item1 + current.Item3;
        float sumEastWestTimeCurrent = current.Item2 + current.Item4;

        //Debug.Log("Checking");
        //Debug.Log("NS: " + sumNorthSouthTimePrevious + " -> " + sumNorthSouthTimeCurrent);
        //if (sumNorthSouthTimePrevious > 1f && sumNorthSouthTimeCurrent < 1f) AddReward(1f);
        //if (sumEastWestTimePrevious > 1f && sumEastWestTimeCurrent < 1f) AddReward(1f);
        if (sumNorthSouthTimeCurrent < sumNorthSouthTimePrevious) {
            //float reward = (sumNorthSouthTimePrevious - sumNorthSouthTimeCurrent > 1f) ? 1f : sumNorthSouthTimePrevious - sumNorthSouthTimeCurrent;
            AddReward(0.1f);
        }
        if (sumEastWestTimeCurrent < sumEastWestTimePrevious) {
            //float reward = (sumEastWestTimePrevious - sumEastWestTimeCurrent > 1f) ? 1f : sumEastWestTimePrevious - sumEastWestTimeCurrent;
            AddReward(0.1f);
        }
    }

    private void clearQueue()
    {
        Debug.Log("Reward for clearing queue");
        AddReward(1f);
    }

    private void changeLights(int index, bool nextAlignment)
    {
        bool previousAlignment = lightsController.checkAlignment(index);

        if (nextAlignment != previousAlignment) {
            lightsController.Junctions[index].Controller.setGreenAlignment(nextAlignment);
            AddReward(-0.01f);
        }
    }
}
