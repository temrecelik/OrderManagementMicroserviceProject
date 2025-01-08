namespace Shared.Core.Utilities.Results.Abstract;

//Temel voidler için başlangıç
public interface IResult
{
    bool Success { get; }
    string? Message { get; }
}