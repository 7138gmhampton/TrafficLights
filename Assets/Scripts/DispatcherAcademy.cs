using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class DispatcherAcademy : Academy
{
    public CarManager carManager;
    public MetricsWatcher metricsWatcher;

    public override void AcademyReset()
    {
        //base.AcademyReset();
        carManager.resetCars();
        metricsWatcher.resetMetrics();
    }
}
