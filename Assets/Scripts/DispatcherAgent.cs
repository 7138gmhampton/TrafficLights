using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class DispatcherAgent : Agent
{
    public AllLightsController lightsController;

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

    public override void CollectObservations() => base.CollectObservations();
}
