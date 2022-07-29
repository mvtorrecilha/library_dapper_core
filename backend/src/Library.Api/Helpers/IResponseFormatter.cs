using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Helpers
{
    public interface IResponseFormatter
    {
        ActionResult Format();
        ActionResult Format<T>(T body = null) where T : class;
    }
}
