using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseLibrary.MsSqlDatabase.Models
{
  public class Blog : BaseEntity
  {
    public int BlogId { get; set; }
    public string Url { get; set; }

    public virtual List<Post> Posts { get; } = new();
  }
}