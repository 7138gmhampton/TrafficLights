﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

    private void Update()
    {
        var triggeredRules = new List<Rule>();

        foreach (var rule in ruleBase) {
            //var junction = lightsController.findJunction(rule.XPosition, rule.YPosition);
            //var waitTimes = lightsController.reportWaitTimes(rule.XPosition, rule.YPosition);
            //if (rule.match(waitTimes.Item1, waitTimes.Item2, waitTimes.Item3, waitTimes.Item4))
            //    triggeredRules.Add(rule);
            if (rule.GetType() == typeof(TimeRuleEW) || rule.GetType() == typeof(TimeRuleNS)) {
                var waitTimes = lightsController.reportWaitTimes(rule.XPosition, rule.YPosition);
                if (rule.match(waitTimes.Item1, waitTimes.Item2, waitTimes.Item3, waitTimes.Item4))
                    triggeredRules.Add(rule);
            }
            if (rule.GetType() == typeof(NumberRuleEW) || rule.GetType() == typeof(NumberRuleNS)) {
                var queueLengths = lightsController.reportQueueNumbers(rule.XPosition, rule.YPosition);
                if (rule.match(queueLengths.Item1, queueLengths.Item2, queueLengths.Item3, queueLengths.Item4))
                    triggeredRules.Add(rule);
            }
        }

        int lowestPriority = 1000;
        foreach (var rule in triggeredRules)
            if (rule.Priority < lowestPriority) lowestPriority = rule.Priority;

        foreach (var rule in triggeredRules)
            if (rule.Priority != lowestPriority) triggeredRules.Remove(rule);

        if (triggeredRules.Count > 0)
            triggeredRules[Random.Range(0, triggeredRules.Count)].fire();
    }
}
