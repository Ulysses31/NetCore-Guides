using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models
{
  /// <summary>
  /// CommandContext
  /// </summary>  
  public class CommandContext : DbContext
  {
    /// <summary>
    /// CommandContext
    /// </summary>
    /// <param name="options"></param>
    /// <returns>DbContextOptions of CommandContext</returns>
    public CommandContext(DbContextOptions<CommandContext> options) : base(options)
    {
    }

    /// <summary>
    /// CommandItems
    /// </summary>
    /// <value>DbSet of CommandDto</value>
    public DbSet<CommandDto>? CommandItems { get; set; }
  }
}