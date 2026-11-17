using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text;
using GameOfLife.Application.Interfaces;
using GameOfLife.Domain.Entities;
using GameOfLife.Domain.Models;
using GameOfLife.Infrastructure.Entities;
using GameOfLife.Infrastructure.Persistence;

namespace GameOfLife.Infrastructure.Repositories;

public class SimulationRepository : ISimulationRepository
{
    private readonly AppDbContext _context;

    public SimulationRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public GameGrid GetGrid()
    {
        var session = GetOrCreateSession();
        var cells = DeserializeCells(session.SerializedCells);
        var grid = new GameGrid(cells);

        typeof(GameGrid)
        .GetProperty(nameof(GameGrid.Generation))!
        .SetValue(grid, session.Generation);

        return grid;
    }

    public void SaveGrid(GameGrid grid)
    {
        var session = GetOrCreateSession();
        session.Generation = grid.Generation;
        session.SerializedCells = SerializeCells(grid.Cells);
        _context.SaveChanges();
    }

    public RuleSet GetRules()
    {
        var session = GetOrCreateSession();
        var birth = ParseRules(session.BirthRules);
        var survival = ParseRules(session.SurvivalRules);
        return new RuleSet(birth, survival);
    }

    public void SaveRules(RuleSet rules)
    {
        var session = GetOrCreateSession();
        session.BirthRules = string.Join(",", rules.BirthRules);
        session.SurvivalRules = string.Join(",", rules.SurvivalRules);
        _context.SaveChanges();
    }

    public void AddMetrics(StabilityMetrics metrics)
    {
        var record = new MetricRecordEntity
        {
            Generation = metrics.Generation,
            LiveCount = metrics.LiveCount,
            PopulationDelta = metrics.PopulationDelta,
            IsExtinct = metrics.IsExtinct,
            IsStable = metrics.IsStable
        };

        _context.MetricsHistory.Add(record);
        _context.SaveChanges();
    }

    public IReadOnlyList<StabilityMetrics> GetMetricsHistory()
    {
        return _context.MetricsHistory
            .OrderBy(m => m.Generation)
            .Select(m => new StabilityMetrics
            {
                Generation = m.Generation,
                LiveCount = m.LiveCount,
                PopulationDelta = m.PopulationDelta
            })
            .ToList();
    }

    public void ClearHistory()
    {
        _context.MetricsHistory.RemoveRange(_context.MetricsHistory);
        _context.SaveChanges();
    }

    private SimulationSessionEntity GetOrCreateSession()
    {
        var session = _context.Sessions.FirstOrDefault();
        if (session == null)
        {
            var emptyGrid = new bool[GameGrid.Size, GameGrid.Size];
            session = new SimulationSessionEntity
            {
                Generation = 0,
                SerializedCells = SerializeCells(emptyGrid),
                BirthRules = "3",
                SurvivalRules = "2,3"
            };
            _context.Sessions.Add(session);
            _context.SaveChanges();
        }
        return session;
    }

    private static string SerializeCells(bool[,] cells)
    {
        var sb = new StringBuilder(GameGrid.Size * GameGrid.Size);
        for (int x = 0; x < GameGrid.Size; x++)
        {
            for (int y = 0; y < GameGrid.Size; y++)
            {
                sb.Append(cells[x, y] ? '1' : '0');
            }
        }
        return sb.ToString();
    }

    private static bool[,] DeserializeCells(string serialized)
    {
        var cells = new bool[GameGrid.Size, GameGrid.Size];
        if (string.IsNullOrEmpty(serialized) || serialized.Length != GameGrid.Size * GameGrid.Size)
        {
            return cells;
        }

        int index = 0;
        for (int x = 0; x < GameGrid.Size; x++)
        {
            for (int y = 0; y < GameGrid.Size; y++)
            {
                cells[x, y] = serialized[index] == '1';
                index++;
            }
        }
        return cells;
    }

    private static IEnumerable<int> ParseRules(string rulesString)
    {
        if (string.IsNullOrWhiteSpace(rulesString))
            return Enumerable.Empty<int>();

        return rulesString
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse);
    }
}
