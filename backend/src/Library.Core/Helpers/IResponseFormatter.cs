using Microsoft.AspNetCore.Mvc;

namespace Library.Core.Helpers;

public interface IResponseFormatter
{
    ActionResult Format();
    ActionResult Format<T>(T body = null) where T : class;
}
