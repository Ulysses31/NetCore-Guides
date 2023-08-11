using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseLibrary.MongoDbDatabase.Models
{
  public interface IBaseEntity
  {
    Guid Id { get; set; }
  }
}