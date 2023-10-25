using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseLibrary.Dtos
{
  public class PostDto : BaseEntityDto
  {
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int BlogId { get; set; }
    public virtual BlogDto Blog { get; set; }
  }
}
