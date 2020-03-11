using System.Collections.Generic;

internal static class RulesBuilder
{
    public static List<Rule> buildRules(List<JunctionSwitcher> junctions)
    {
        var builtRules = new List<Rule>();

        foreach (var junction in junctions) {
            builtRules.Add(new TimeRuleNS(junction.XLocus, junction.YLocus, 0));
            builtRules.Add(new TimeRuleEW(junction.XLocus, junction.YLocus, 0));
        }

        return builtRules;
    }
}