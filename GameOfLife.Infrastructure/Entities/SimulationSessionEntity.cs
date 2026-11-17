using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Infrastructure.Entities;

public class SimulationSessionEntity
{
    public int Id { get; set; }
    public int Generation { get; set; }
    public string SerializedCells { get; set; } = string.Empty;
    public string BirthRules { get; set; } = "3";
    public string SurvivalRules { get; set; } = "2,3";
}
