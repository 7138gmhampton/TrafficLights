using System;
using UnityEngine;
using Object = UnityEngine.Object;

internal class NumberRuleNS : Rule
{
    private Func<int, int, Tuple<int, int, int, int>> dataMethod;

    public NumberRuleNS(int x, int y, int priority)
    {
        xPosition = x;
        yPosition = y;
        this.priority = priority;
        dataMethod = Object.FindObjectOfType<AllLightsController>().reportQueueNumbers;
    }

    public Func<int, int, Tuple<int, int, int, int>> DataMethod { get { return dataMethod; } }

    public override void fire() => throw new System.NotImplementedException();
    public override bool match(params object[] args) => throw new System.NotImplementedException();
}

internal class NumberRuleEW : Rule
{
    public override void fire() => throw new System.NotImplementedException();
    public override bool match(params object[] args) => throw new System.NotImplementedException();
}