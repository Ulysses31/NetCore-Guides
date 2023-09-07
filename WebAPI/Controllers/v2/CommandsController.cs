using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers.v2
{
  /// <summary>
  /// CommandsController (async)
  /// </summary>
  [ApiController]
  [ApiVersion("2.0")]
  [Route("api/v{version:apiVersion}/[controller]")]
  [Consumes("application/json", "application/xml")]
  [Produces("application/json", "application/xml")]
  public class CommandsController : ControllerBase
  {
    private readonly CommandContext _context;

    /// <summary>
    /// CommandsController constructor
    /// </summary>
    public CommandsController(CommandContext context) => this._context = context;

    /// <summary>
    /// Get all commands async
    /// </summary>
    /// <returns>Task IEnumerable of CommandDto</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///	GET /api/v2/commands
    ///
    /// </remarks>
    /// <response code="200">Http OK</response>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    /// <response code="406">Not Acceptable</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(typeof(Task<ActionResult<IEnumerable<CommandDto>>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<CommandDto>>> GetCommands()
    {
      return await Task.FromResult(_context.CommandItems);
    }

    /// <summary>
    /// Get command by id async
    /// </summary>
    /// <param name="id">int</param>
    /// <remarks>
    /// Sample request:
    ///
    ///	GET /api/v2/commands/{id}
    ///
    /// </remarks>
    /// <returns>Task ActionResult CommandDto></returns>
    /// <response code="200">Http OK</response>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    /// <response code="406">Not Acceptable</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("{id}")]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(typeof(Task<ActionResult<CommandDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<CommandDto>> GetCommandById(int id)
    {
      var commandItem = await _context.CommandItems.FindAsync(id);

      if (commandItem == null) return NotFound();

      return await Task.FromResult(commandItem);
    }

    /// <summary>
    /// AddCommand async
    /// </summary>
    /// <param name="commandItem">CommandDto</param>
    /// <returns>Task ActionResult  of CommandDto></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///	POST /api/v2/commands
    ///	{
    ///		"howTo": "string",
    ///		"platform": "string",
    ///		"commandLine": "string"
    ///	}
    ///
    /// </remarks>
    /// <response code="200">Http OK</response>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    /// <response code="406">Not Acceptable</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(typeof(Task<ActionResult<CommandDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<CommandDto>> AddCommand(CommandDto commandItem)
    {
      if (commandItem == null) return BadRequest();

      await _context.CommandItems.AddAsync(commandItem);
      await _context.SaveChangesAsync();

      return CreatedAtAction(
        "GetCommandById",
        new CommandDto() { Id = commandItem.Id },
        commandItem
      );
    }

    /// <summary>
    /// UpdateCommand async
    /// </summary>
    /// <param name="id">int</param>
    /// <param name="commandItem">CommandDto</param>
    /// <returns>No Content</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///	PUT /api/v2/commands/{id}
    ///	{
    ///		"id": 0,
    ///		"howTo": "string",
    ///		"platform": "string",
    ///		"commandLine": "string"
    ///	}
    ///
    /// </remarks>
    /// <response code="200">Http OK</response>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    /// <response code="406">Not Acceptable</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPut("{id}")]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<CommandDto>> UpdateCommand(int id, CommandDto commandItem)
    {
      if ((commandItem == null) || (id != commandItem.Id)) return BadRequest();

      _context.Entry(commandItem).State = EntityState.Modified;
      await _context.SaveChangesAsync();

      return await Task.FromResult(NoContent());
    }

    /// <summary>
    /// DeleteCommand async
    /// </summary>
    /// <param name="id">int</param>
    /// <returns>Task ActionResult of CommandDto</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///	DELETE /api/v2/commands/{id}
    ///
    /// </remarks>
    /// <response code="200">Http OK</response>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    /// <response code="406">Not Acceptable</response>
    /// <response code="500">Internal Server Error</response>
    [HttpDelete("{id}")]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(typeof(Task<ActionResult<CommandDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<CommandDto>> DeleteCommand(int id)
    {
      var commandItem = await _context.CommandItems.FindAsync(id);

      if (commandItem == null) return NotFound();

      _context.CommandItems.Remove(commandItem);
      await _context.SaveChangesAsync();

      return await Task.FromResult(commandItem);
    }

  }
}