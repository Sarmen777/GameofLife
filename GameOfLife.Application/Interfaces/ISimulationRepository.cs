using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameOfLife.Domain.Entities;
using GameOfLife.Domain.Models;

namespace GameOfLife.Application.Interfaces;

public interface ISimulationRepository
{
    GameGrid GetGrid();
    void SaveGrid(GameGrid grid);
    RuleSet GetRules();
    void SaveRules(RuleSet rules);
    void AddMetrics(StabilityMetrics metrics);
    IReadOnlyList<StabilityMetrics> GetMetricsHistory();
    void ClearHistory();
}
