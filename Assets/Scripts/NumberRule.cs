using System;
using UnityEngine;
using Object = UnityEngine.Object;

internal class NumberRuleNS : Rule
{
    //private Func<int, int, Tuple<int, int, int, int>> dataMethod;

    public NumberRuleNS(int x, int y, int priority)
    {
        xPosition = x;
        yPosition = y;
        this.priority = priority;
        //dataMethod = Object.FindObjectOfType<AllLightsController>().reportQueueNumbers;
    }

    //public Func<int, int, Tuple<int, int, int, int>> DataMethod { get { return dataMethod; } }

    public override void fire() => throw new System.NotImplementedException();
    public override bool match(params object[] args)
    {
        //throw new System.NotImplementedException();
        int northwardQueue = (int)args[0];
        int eastwardQueue = (int)args[0];
        int southwardQueue = (int)args[0];
        int westwardQueue = (int)args[0];

        if (northwardQueue + southwardQueue > eastwardQueue + westwardQueue) return true;
        else return false;
    }
}

internal class NumberRuleEW : Rule
{
    public NumberRuleEW(int x, int y, int priority)
    {
        xPosition = x;
        yPosition = y;
        this.priority = priority;
    }

    public override void fire() => throw new System.NotImplementedException();
    public override bool match(params object[] args) => throw new System.NotImplementedException();
}