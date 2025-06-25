using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands.Responses;

public class ResponseCommand<T>
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorType { get; set; }
    public T? Data { get; set; }
}

