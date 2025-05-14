using Microsoft.AspNetCore.Http;

namespace MatchMaker.Domain.DTOs;
public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string? Title { get; set; }
    public string Message { get; set; } = null!;
    public int? StatusCode { get; set; }
    public T? Data { get; set; }

    public static Result<T> Success(T? data, string message = "") => new Result<T> { IsSuccess = true, Data = data, Message = message };
    public static Result<T> Failure(string message, string? title = null,  int? statusCode = null) => new Result<T> { IsSuccess = false, Title = title, Message = message, StatusCode = statusCode };

    public int GetStatusCodeOrDefault(int defaultCode = StatusCodes.Status400BadRequest) => StatusCode ?? defaultCode;
}

