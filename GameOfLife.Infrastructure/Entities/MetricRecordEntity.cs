using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Infrastructure.Entities;

public class MetricRecordEntity
{
    public int Id { get; set; }
    public int Generation { get; set; }
    public int LiveCount { get; set; }
    public int PopulationDelta { get; set; }
    public bool IsExtinct { get; set; }
    public bool IsStable { get; set; }
}
