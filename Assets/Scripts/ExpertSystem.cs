using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpertSystem : MonoBehaviour
{
    public EnvironmentManager environment;
    public AllLightsController lightsController;

    private void Awake()
    {
        environment.setupEnvironment();
        lightsController.setupJunctionControllers();
    }

    private void Start()
    {
        environment.prepareCarManager();
    }
}
