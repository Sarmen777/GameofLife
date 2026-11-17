using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Domain.Entities;

public class RuleSet
{
    public HashSet<int> BirthRules { get; }
    public HashSet<int> SurvivalRules { get; }

    public RuleSet(IEnumerable<int> birthRules, IEnumerable<int> survivalRules)
    {
        if (birthRules == null)
            throw new ArgumentNullException(nameof(birthRules));
        if (survivalRules == null)
            throw new ArgumentNullException(nameof(survivalRules));

        BirthRules = ValidateAndCreateSet(birthRules, nameof(birthRules));
        SurvivalRules = ValidateAndCreateSet(survivalRules, nameof(survivalRules));
    }

    private static HashSet<int> ValidateAndCreateSet(IEnumerable<int> rules, string paramName)
    {
        var set = new HashSet<int>();

        foreach (int rule in rules)
        {
            if (rule < 0 || rule > 8)
            {
                throw new ArgumentOutOfRangeException(
                    paramName,
                    $"Invalid rule value: {rule}. Neighbor count must be between 0 and 8.");
            }

            set.Add(rule);
        }

        return set;
    }

    public override string ToString()
    {
        var b = string.Concat(BirthRules.OrderBy(x => x));
        var s = string.Concat(SurvivalRules.OrderBy(x => x));
        return $"B{b}/S{s}";
    }
}
