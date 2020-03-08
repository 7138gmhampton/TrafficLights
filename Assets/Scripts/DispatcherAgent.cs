using System.Collections.Generic;
using UnityEngine;
using MLAgents;

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

    public override void AgentAction(float[] vectorAction)
    {
        for (int iii = 0; iii < vectorAction.Length; iii++) 
            lightsController.junctions[iii].Controller.setGreenAlignment((int)vectorAction[iii] == 1);

        rewardDisplay.text = GetCumulativeReward().ToString("0.00");
        timeDisplay.text = watcher.reportJourneyTimeMean().ToString("0.00s");
    }

    public override void CollectObservations()
    {
        float currentMeanJourneyTime = watcher.reportJourneyTimeMean();
        lastMeanJourneyTime = currentMeanJourneyTime;

        //AddVectorObs(currentMeanJourneyTime);

        var waitTimes = watcher.getWaitTimes();
        
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
        //Debug.Log("Reset Agent");
        carManager.resetCars();
        watcher.resetMetrics();
        carsPassed = 0;
    }

    private void unacceptableWait() => AddReward(-1f);

    private void finishCar()
    {
        AddReward(0.1f);
        ++carsPassed;
        Debug.Log(carsPassed);

        if (carsPassed > carsPerEpisode) Done();
    }
}
