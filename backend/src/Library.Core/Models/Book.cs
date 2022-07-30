using Dapper.Contrib.Extensions;
using System;

namespace Library.Core.Models;

[Table("Book")]
public class Book : BaseEntity 
{        
    public string Title { get; set; }
    public string Author { get; set; }
    public int Pages { get; set; }
    public string Publisher { get; set; }
    public Guid BookCategoryId { get; set; }
}
