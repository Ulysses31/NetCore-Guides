using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
  /// <summary>
  /// Command dto
  /// </summary>
  public class CommandDto
  {
    /// <summary>
    /// Id
    /// </summary>
    /// <value>integer</value>
    public int Id { get; set; }

    /// <summary>
    /// HowTo
    /// </summary>
    /// <value>string</value>
    public string? HowTo { get; set; }

    /// <summary>
    /// Platform
    /// </summary>
    /// <value>string</value>
    public string? Platform { get; set; }

    /// <summary>
    /// CommandLine
    /// </summary>
    /// <value>string</value>
    public string? CommandLine { get; set; }
  }
}