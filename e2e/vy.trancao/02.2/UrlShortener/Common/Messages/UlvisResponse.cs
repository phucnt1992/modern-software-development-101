namespace Common.Messages;

public class UlvisResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
}
