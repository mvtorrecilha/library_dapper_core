using Dapper.Contrib.Extensions;
using System;

namespace Library.Core.Models;

public class BaseEntity
{
    [ExplicitKey]
    public Guid Id { get; set; } = Guid.NewGuid();
}
