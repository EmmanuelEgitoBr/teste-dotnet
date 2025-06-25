namespace Questao5.Application.Queries.Responses;

public class ResponseQuery<T>
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorType { get; set; }
    public T? Data { get; set; }
}
