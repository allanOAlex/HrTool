namespace GSG.Shared;

public class ResponseBody<T>
{
    public int ReponseCode { get; set; } = 200;
    public bool Success { get; set; } = true;
    public string Message { get; set; }
    public T Body { get; set; }
}

public class ReponseBody : ResponseBody<object>
{
}