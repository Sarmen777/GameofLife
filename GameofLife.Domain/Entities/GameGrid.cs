using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Domain.Entities;

public class GameGrid
{
    public const int Size = 100;
    public bool[,] Cells { get; private set; }
    public int Generation { get; private set; }

    public GameGrid(bool[,]? initialCells = null)
    {
        if (initialCells != null)
        {
            if (initialCells.GetLength(0) != Size || initialCells.GetLength(1) != Size)
            {
                throw new ArgumentException($"Grid size must be exactly {Size}x{Size}.");
            }
            Cells = (bool[,])initialCells.Clone();
        }
        else
        {
            Cells = new bool[Size, Size];
        }

        Generation = 0;
    }

    public void SetCell(int x, int y, bool isAlive)
    {
        if (x < 0 || x >= Size)
            throw new ArgumentOutOfRangeException(nameof(x), $"Coordinate x must be between 0 and {Size - 1}.");
        if (y < 0 || y >= Size)
            throw new ArgumentOutOfRangeException(nameof(y), $"Coordinate y must be between 0 and {Size - 1}.");

        Cells[x, y] = isAlive;
    }

    public void Randomize(double density = 0.25)
    {
        if (density < 0.0 || density > 1.0)
            throw new ArgumentOutOfRangeException(nameof(density), "Density must be between 0.0 and 1.0.");

        var random = new Random();

        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                Cells[x, y] = random.NextDouble() < density;
            }
        }

        Generation = 0;
    }

    public void Clear()
    {
        Array.Clear(Cells, 0, Cells.Length);
        Generation = 0;
    }

    public int CountNeighbors(int x, int y)
    {
        int count = 0;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int nx = (x + dx + Size) % Size;
                int ny = (y + dy + Size) % Size;

                if (Cells[nx, ny])
                {
                    count++;
                }
            }
        }

        return count;
    }

    public void Step(RuleSet rules)
    {
        if (rules == null)
            throw new ArgumentNullException(nameof(rules));

        var nextCells = new bool[Size, Size];

        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                int neighbors = CountNeighbors(x, y);
                bool isAlive = Cells[x, y];

                if (isAlive)
                {
                    if (rules.SurvivalRules.Contains(neighbors))
                    {
                        nextCells[x, y] = true;
                    }
                }
                else
                {
                    if (rules.BirthRules.Contains(neighbors))
                    {
                        nextCells[x, y] = true;
                    }
                }
            }
        }

        Cells = nextCells;
        Generation++;
    }

    public int GetLiveCount()
    {
        int count = 0;
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                if (Cells[x, y]) count++;
            }
        }
        return count;
    }
}
