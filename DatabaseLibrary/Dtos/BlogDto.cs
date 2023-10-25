using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseLibrary.Dtos
{
  public class BlogDto : BaseEntityDto
  {
    public int BlogId { get; set; }
    public string Url { get; set; }

    public virtual List<PostDto> Posts => new();
  }
}
