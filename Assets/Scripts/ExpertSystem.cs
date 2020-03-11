using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpertSystem : MonoBehaviour
{
    public EnvironmentManager environment;

    private void Awake()
    {
        environment.setupEnvironment();
    }

    private void Start()
    {
        environment.prepareCarManager();
    }
}
