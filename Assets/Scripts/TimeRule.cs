using System;
using UnityEngine;
using Object = UnityEngine.Object;

internal class TimeRuleNS : Rule
{
    private Func<int, int, Tuple<float, float, float, float>> dataMethod;

    public Func<int, int, Tuple<float, float, float, float>> DataMethod { get { return dataMethod; } }

    public TimeRuleNS(int x, int y, int priority)
    {
        xPosition = x;
        yPosition = y;
        this.priority = priority;
        dataMethod = Object.FindObjectOfType<AllLightsController>().reportWaitTimes;
    }

    public override void fire()
    {
        //throw new System.NotImplementedException();
        Object.FindObjectOfType<AllLightsController>().switchNS(xPosition, yPosition);
    }

    public override bool match(params object[] args)
    {
        //throw new System.NotImplementedException();
        float northTime = (float)args[0];
        float eastTime = (float)args[1];
        float southTime = (float)args[2];
        float westTime = (float)args[3];

        if ((northTime + southTime) - (eastTime + westTime) > 1f)
            return true;
        else return false;
    }
}

internal class TimeRuleEW : Rule
{
    public TimeRuleEW(int x, int y, int priority)
    {
        xPosition = x;
        yPosition = y;
        this.priority = priority;
    }

    public override void fire()
    {
        //throw new System.NotImplementedException();
        Object.FindObjectOfType<AllLightsController>().switchEW(xPosition, yPosition);
    }

    public override bool match(params object[] args)
    {
        //throw new System.NotImplementedException();
        float northTime = (float)args[0];
        float eastTime = (float)args[1];
        float southTime = (float)args[2];
        float westTime = (float)args[3];
        
        if ((eastTime + westTime) - (northTime + southTime) > 1f)
            return true;
        else return false;
    }
}