using System;
using System.Collections.Generic;

namespace Library.Core.Models;

public class Course : BaseEntity
{
    public string Name { get; set; }
    public List<Guid> CategoriesOfBooksIds { get; set; }
}
