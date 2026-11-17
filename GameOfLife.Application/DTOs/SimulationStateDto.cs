using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameOfLife.Domain.Models;

namespace GameOfLife.Application.DTOs;

public class SimulationStateDto
{
    public int Generation { get; set; }
    public string ActiveRules { get; set; } = string.Empty;
    public StabilityMetrics CurrentMetrics { get; set; } = new();
    public bool[][] Grid { get; set; } = Array.Empty<bool[]>();
}
