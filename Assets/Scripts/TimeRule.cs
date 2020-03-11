using UnityEngine;

internal class TimeRuleNS : Rule
{
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