using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Domain.Models;

public class StabilityMetrics
{
    public int Generation { get; set; }
    public int LiveCount { get; set; }
    public int PopulationDelta { get; set; }
    public bool IsExtinct => LiveCount == 0;
    public bool IsStable => PopulationDelta == 0;
}
