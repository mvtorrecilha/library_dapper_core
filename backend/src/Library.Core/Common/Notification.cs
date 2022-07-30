namespace Library.Core.Common;

public class Notification
{
    public string Code { get; }
    public string Message { get; }
    public object Value { get; }

    public Notification(string code, string message, object value)
    {
        this.Code = code;
        this.Message = message;
        this.Value = value;
    }
}
