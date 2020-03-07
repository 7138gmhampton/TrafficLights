using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class DispatcherAgent : Agent
{
    public AllLightsController lightsController;
    public MetricsWatcher watcher;
    public TextMesh rewardDisplay;
    public TextMesh timeDisplay;

    private float lastMeanJourneyTime = 1000f;

    public override void InitializeAgent() => base.InitializeAgent();

    public override void AgentAction(float[] vectorAction)
    {
        //base.AgentAction(vectorAction);
        for (int iii = 0; iii < vectorAction.Length; iii++) {
            if ((int)vectorAction[iii] == 1)
                lightsController.junctions[iii].Controller.setGreenAlignment(true);
            else lightsController.junctions[iii].Controller.setGreenAlignment(false);
        }

        rewardDisplay.text = this.GetCumulativeReward().ToString("0.00");
        timeDisplay.text = watcher.reportJourneyTimeMean().ToString("0.00s");
    }

    public override void CollectObservations()
    {
        //base.CollectObservations();
        float currentMeanJourneyTime = watcher.reportJourneyTimeMean();
        if (currentMeanJourneyTime < lastMeanJourneyTime) AddReward(1f);
        else if (currentMeanJourneyTime > lastMeanJourneyTime) AddReward(-1f);
        lastMeanJourneyTime = currentMeanJourneyTime;

        AddVectorObs(currentMeanJourneyTime);

        var waitTimes = watcher.getWaitTimes();
        var queueTimes = new List<float>();
        
        for (int y = 0; y < waitTimes.GetLength(0); ++y)
            for (int x = 0; x < waitTimes.GetLength(1); ++x) {
                AddVectorObs(waitTimes[y, x].Item1);
                AddVectorObs(waitTimes[y, x].Item2);
                AddVectorObs(waitTimes[y, x].Item3);
                AddVectorObs(waitTimes[y, x].Item4);
            }

    }

    public override float[] Heuristic()
    {
        //return base.Heuristic();
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

    private void unacceptableWait()
    {
        AddReward(-1f);
    }

    private void finishCar()
    {
        AddReward(0.1f);
    }
}
