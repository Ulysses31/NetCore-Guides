using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseLibrary.Dtos
{
  public class BaseEntityDto
  {
    /// <summary>
    /// Who created the record.
    /// </summary>
    public String CreatedBy { get; set; }

    /// <summary>
    /// Date and time the record was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }
  }
}
