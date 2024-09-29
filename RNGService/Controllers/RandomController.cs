using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using RNGService.Models;
using RNGService.Services;

namespace RNGService.Controllers;

[ApiController]
[Route("")]
public class RandomController : ControllerBase
{
    private readonly ILogger<RandomController> _logger;
    private readonly IRandomGenerator _generator;

    public RandomController(ILogger<RandomController> logger, IRandomGenerator generator)
    {
        _logger = logger;
        _generator = generator;
    }

    [HttpGet(Name = "GetRandom")]
    public ActionResult<RandomResult> GetRandom(int len = 32)
    {
        try {
            var bytes = _generator.GetBytes(len);
            return Ok(
                new RandomResult {
                    Random = bytes
                }
            );
        } catch (Exception e) {
            return BadRequest(e.Message);
        } 
        
    }
}
