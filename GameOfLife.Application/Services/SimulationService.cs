using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameOfLife.Application.DTOs;
using GameOfLife.Application.Interfaces;
using GameOfLife.Domain.Entities;
using GameOfLife.Domain.Models;

namespace GameOfLife.Application.Services;

public class SimulationService : ISimulationService
{
    private readonly ISimulationRepository _repository;

    public SimulationService(ISimulationRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public SimulationStateDto GetCurrentState()
    {
        var grid = _repository.GetGrid();
        var rules = _repository.GetRules();
        var history = _repository.GetMetricsHistory();

        var latestMetrics = history.LastOrDefault() ?? new StabilityMetrics
        {
            Generation = grid.Generation,
            LiveCount = grid.GetLiveCount(),
            PopulationDelta = 0
        };

        return MapToStateDto(grid, rules, latestMetrics);
    }

    public SimulationStateDto SetRules(RuleSetDto rulesDto)
    {
        if (rulesDto == null)
            throw new ArgumentNullException(nameof(rulesDto));

        var newRules = new RuleSet(rulesDto.BirthRules, rulesDto.SurvivalRules);
        _repository.SaveRules(newRules);

        return GetCurrentState();
    }

    public SimulationStateDto SetCell(CellCoordinateDto cellDto)
    {
        if (cellDto == null)
            throw new ArgumentNullException(nameof(cellDto));

        var grid = _repository.GetGrid();
        grid.SetCell(cellDto.X, cellDto.Y, cellDto.IsAlive);
        _repository.SaveGrid(grid);

        return GetCurrentState();
    }

    public SimulationStateDto Randomize(double density)
    {
        var grid = _repository.GetGrid();
        grid.Randomize(density);
        _repository.SaveGrid(grid);
        _repository.ClearHistory();

        var initialMetrics = new StabilityMetrics
        {
            Generation = 0,
            LiveCount = grid.GetLiveCount(),
            PopulationDelta = 0
        };
        _repository.AddMetrics(initialMetrics);

        return GetCurrentState();
    }

    public SimulationStateDto Clear()
    {
        var grid = _repository.GetGrid();
        grid.Clear();
        _repository.SaveGrid(grid);
        _repository.ClearHistory();

        return GetCurrentState();
    }

    public SimulationStateDto Step()
    {
        return RunSteps(1);
    }

    public SimulationStateDto RunSteps(int steps)
    {
        if (steps <= 0)
            throw new ArgumentOutOfRangeException(nameof(steps), "Steps count must be greater than zero.");

        var grid = _repository.GetGrid();
        var rules = _repository.GetRules();

        StabilityMetrics latestMetrics = new();

        for (int i = 0; i < steps; i++)
        {
            int previousLiveCount = grid.GetLiveCount();
            grid.Step(rules);
            int currentLiveCount = grid.GetLiveCount();

            latestMetrics = new StabilityMetrics
            {
                Generation = grid.Generation,
                LiveCount = currentLiveCount,
                PopulationDelta = currentLiveCount - previousLiveCount
            };

            _repository.AddMetrics(latestMetrics);
        }

        _repository.SaveGrid(grid);

        return MapToStateDto(grid, rules, latestMetrics);
    }

    public IReadOnlyList<StabilityMetrics> GetHistory()
    {
        return _repository.GetMetricsHistory();
    }

    private static SimulationStateDto MapToStateDto(GameGrid grid, RuleSet rules, StabilityMetrics metrics)
    {
        var jaggedGrid = new bool[GameGrid.Size][];
        for (int x = 0; x < GameGrid.Size; x++)
        {
            jaggedGrid[x] = new bool[GameGrid.Size];
            for (int y = 0; y < GameGrid.Size; y++)
            {
                jaggedGrid[x][y] = grid.Cells[x, y];
            }
        }

        return new SimulationStateDto
        {
            Generation = grid.Generation,
            ActiveRules = rules.ToString(),
            CurrentMetrics = metrics,
            Grid = jaggedGrid
        };
    }
}
