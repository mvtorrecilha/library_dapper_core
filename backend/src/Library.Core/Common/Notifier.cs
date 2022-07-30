using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Library.Core.Common;

public class Notifier : INotifier
{
    public bool HasError => Errors != null && Errors.Any();
    public HttpStatusCode StatusCode { get; private set; }
    public List<Notification> Warnings { get; private set; }
    public List<Notification> Errors { get; private set; }

    public Notifier()
    {
        Warnings = new List<Notification>();
        Errors = new List<Notification>();
    }

    public void SetStatuCode(HttpStatusCode statusCode)
    {
        this.StatusCode = statusCode;
    }

    public void AddWarning(string code, string message, object value)
    {
        if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(message))
            Warnings.Add(new Notification(code, message, value));
    }

    public void AddError(string code, string message, object value)
    {
        if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(message))
            Errors.Add(new Notification(code, message, value));
    }
}
