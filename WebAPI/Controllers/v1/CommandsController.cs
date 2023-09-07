using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers.v1
{
  /// <summary>
  /// CommandsController
  /// </summary>
  [ApiController]
  [ApiVersion("1.0")]
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
    /// Get all commands
    /// </summary>
    /// <returns>IEnumerable of CommandDto</returns>
    /// <response code="200">Http OK</response>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    /// <response code="406">Not Acceptable</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(IEnumerable<CommandDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult<IEnumerable<CommandDto>> GetCommands()
    {
      return Ok(_context.CommandItems);
    }

    /// <summary>
    /// Get command by id
    /// </summary>
    /// <param name="id">int</param>
    /// <returns>IEnumerable of CommandDto</returns>
    /// <response code="200">Http OK</response>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    /// <response code="406">Not Acceptable</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("{id}")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(CommandDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult<CommandDto> GetCommandById(int id)
    {
      var commandItem = _context.CommandItems.Find(id);

      if (commandItem == null) return NotFound();

      return Ok(commandItem);
    }

    /// <summary>
    /// AddCommand
    /// </summary>
    /// <param name="commandItem">CommandDto</param>
    /// <returns>CommandDto</returns>
    /// <response code="200">Http OK</response>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    /// <response code="406">Not Acceptable</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(CommandDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult<CommandDto> AddCommand(CommandDto commandItem)
    {
      if (commandItem == null) return BadRequest();

      _context.CommandItems.Add(commandItem);
      _context.SaveChanges();

      return CreatedAtAction(
        "GetCommandById",
        new CommandDto() { Id = commandItem.Id },
        commandItem
      );
    }

    /// <summary>
    /// UpdateCommand
    /// </summary>
    /// <param name="id">int</param>
    /// <param name="commandItem">CommandDto</param>
    /// <returns>CommandDto</returns>
    /// <response code="200">Http OK</response>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    /// <response code="406">Not Acceptable</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPut("{id}")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult<CommandDto> UpdateCommand(int id, CommandDto commandItem)
    {
      if ((commandItem == null) || (id != commandItem.Id)) return BadRequest();

      _context.Entry(commandItem).State = EntityState.Modified;
      _context.SaveChanges();

      return NoContent();
    }


    /// <summary>
    /// DeleteCommand
    /// </summary>
    /// <param name="id">int</param>
    /// <returns>CommandDto</returns>
    /// <response code="200">Http OK</response>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    /// <response code="406">Not Acceptable</response>
    /// <response code="500">Internal Server Error</response>
    [HttpDelete("{id}")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(CommandDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult<CommandDto> DeleteCommand(int id)
    {
      var commandItem = _context.CommandItems.Find(id);

      if (commandItem == null) return NotFound();

      _context.CommandItems.Remove(commandItem);
      _context.SaveChanges();

      return Ok(commandItem);
    }

  }
}