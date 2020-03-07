using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class DispatcherAgent : Agent
{
    public AllLightsController lightsController;
    public MetricsWatcher watcher;

    private float lastMeanJourneyTime = 1000f;

    public override void InitializeAgent() => base.InitializeAgent();

    public override void AgentAction(float[] vectorAction)
    {
        //base.AgentAction(vectorAction);
        for (int iii = 0; iii < vectorAction.Length; iii++) {
            if (vectorAction[iii] > 0.5f)
                lightsController.junctions[iii].Controller.setGreenAlignment(true);
            else lightsController.junctions[iii].Controller.setGreenAlignment(false);
        }
    }

    public override void CollectObservations()
    {
        //base.CollectObservations();
        float currentMeanJourneyTime = watcher.reportJourneyTimeMean();
        if (currentMeanJourneyTime < lastMeanJourneyTime) AddReward(1f);
        else AddReward(-1f);
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
}
