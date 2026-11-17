using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameOfLife.Application.DTOs;
using GameOfLife.Domain.Models;

namespace GameOfLife.Application.Interfaces;

public interface ISimulationService
{
    SimulationStateDto GetCurrentState();
    SimulationStateDto SetRules(RuleSetDto rulesDto);
    SimulationStateDto SetCell(CellCoordinateDto cellDto);
    SimulationStateDto Randomize(double density);
    SimulationStateDto Clear();
    SimulationStateDto Step();
    SimulationStateDto RunSteps(int steps);
    IReadOnlyList<StabilityMetrics> GetHistory();
}