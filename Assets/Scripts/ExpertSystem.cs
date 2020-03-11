using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpertSystem : MonoBehaviour
{
    public EnvironmentManager environment;
    public AllLightsController lightsController;

    private List<Rule> ruleBase = new List<Rule>();

    private void Awake()
    {
        environment.setupEnvironment();
        lightsController.setupJunctionControllers();
        ruleBase = RulesBuilder.buildRules(lightsController.Junctions);
    }

    private void Start()
    {
        environment.prepareCarManager();
    }
}
