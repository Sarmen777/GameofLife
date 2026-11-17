using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Application.DTOs;

public class CellCoordinateDto
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsAlive { get; set; }
}
