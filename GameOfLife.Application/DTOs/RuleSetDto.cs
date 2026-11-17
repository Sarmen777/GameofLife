using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Application.DTOs;

public class RuleSetDto
{
    public List<int> BirthRules { get; set; } = new() { 3 };
    public List<int> SurvivalRules { get; set; } = new() { 2, 3 };
}
