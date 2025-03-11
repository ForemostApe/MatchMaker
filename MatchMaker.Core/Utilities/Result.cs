using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.Domain.DTOs;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = null!;
    public T? Data { get; set; }
    public static Result<T> Success(T data, string message = "") => new Result<T> { IsSuccess = true, Data = data, Message = message };
    public static Result<T> Failure(string message) => new Result<T> { IsSuccess = false, Message = message };

}
