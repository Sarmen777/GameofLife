using GameOfLife.Application.DTOs;
using GameOfLife.Application.Interfaces;
using GameOfLife.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameOfLife.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SimulationController : ControllerBase
{
    private readonly ISimulationService _simulationService;

    public SimulationController(ISimulationService simulationService)
    {
        _simulationService = simulationService;
    }

    [HttpGet("state")]
    public ActionResult<SimulationStateDto> GetState()
    {
        return Ok(_simulationService.GetCurrentState());
    }

    [HttpPost("rules")]
    public ActionResult<SimulationStateDto> SetRules([FromBody] RuleSetDto rulesDto)
    {
        try
        {
            return Ok(_simulationService.SetRules(rulesDto));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("cell")]
    public ActionResult<SimulationStateDto> SetCell([FromBody] CellCoordinateDto cellDto)
    {
        try
        {
            return Ok(_simulationService.SetCell(cellDto));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("randomize")]
    public ActionResult<SimulationStateDto> Randomize([FromQuery] double density = 0.25)
    {
        try
        {
            return Ok(_simulationService.Randomize(density));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("clear")]
    public ActionResult<SimulationStateDto> Clear()
    {
        return Ok(_simulationService.Clear());
    }

    [HttpPost("step")]
    public ActionResult<SimulationStateDto> Step()
    {
        return Ok(_simulationService.Step());
    }

    [HttpPost("run")]
    public ActionResult<SimulationStateDto> RunSteps([FromQuery] int steps = 10)
    {
        try
        {
            return Ok(_simulationService.RunSteps(steps));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("history")]
    public ActionResult<IReadOnlyList<StabilityMetrics>> GetHistory()
    {
        return Ok(_simulationService.GetHistory());
    }
}