using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hexagon.Dtos;

public class ApiResult<T>
{
    public T Result { get; set; }

    public bool Success { get; set; }
    public List<Error> Errors { get; set; } = new List<Error>();

#if DEBUG
    public Exception Exception { get; set; }
#endif

    public new static ApiResult<T> Ok()
    {
        return new ApiResult<T>()
        {
            Success = true,
        };
    }

    public new static ApiResult<T> Ok(T result)
    {
        return new ApiResult<T>()
        {
            Result = result,
            Success = true,
        };
    }

    public static ApiResult<T> Error(Exception exception)
    {
        var msg = exception.Message;
        if (exception.InnerException != null)
        {
            msg = exception.InnerException.Message;
        }

        var res = new ApiResult<T>()
        {
            Success = false,
            Errors = new List<Error>()
            {
                new Error()
                {
                    Message = msg
                }
            }
        };
#if DEBUG
        res.Exception = exception;
#endif
        return res;
    }

    public static ApiResult<T> Error(ModelStateDictionary modelState)
    {
        var res = new ApiResult<T>()
        {
            Success = false,
            Errors = new List<Error>() { },
        };
        res.Errors = modelState.Keys.SelectMany(key =>
        {
            return modelState[key].Errors.Select(e => new Error()
            {
                Message = e.ErrorMessage,
                Property = key
            });
        }).ToList();
        return res;
    }

    public static ApiResult<T> Error(string error, string property = null)
    {
        return new ApiResult<T>()
        {
            Success = false,
            Errors = new List<Error>()
            {
                new Error()
                {
                    Message = error,
                    Property = property
                }
            },
        };
    }
}

public class ApiResult : ApiResult<object>
{
    public new static ApiResult Ok()
    {
        return new ApiResult()
        {
            Success = true,
        };
    }

    public static ApiResult<T> Ok<T>(T result)
    {
        return new ApiResult<T>()
        {
            Success = true,
            Result = result,
        };
    }

    public new static ApiResult Error(Exception exception)
    {
        var msg = exception.Message;
        if (exception.InnerException != null)
        {
            msg = exception.InnerException.Message;
        }

        var res = new ApiResult()
        {
            Success = false,
            Errors = new List<Error>()
            {
                new Error()
                {
                    Message = msg
                }
            }
        };
#if DEBUG
        res.Exception = exception;
#endif
        return res;
    }

    public new static ApiResult Error(ModelStateDictionary modelState)
    {
        var res = new ApiResult()
        {
            Success = false,
            Errors = new List<Error>() { },
        };
        res.Errors = modelState.Keys.SelectMany(key =>
        {
#pragma warning disable CS8602 // I'm sure
            return modelState[key].Errors.Select(e => new Error()
#pragma warning restore CS8602
            {
                Message = e.ErrorMessage,
                Property = key
            });
        }).ToList();
        return res;
    }

#pragma warning disable CS8625 // I'm sure
    public new static ApiResult Error(string error, string property = null)
#pragma warning restore CS8625
    {
        return new ApiResult()
        {
            Success = false,
            Errors = new List<Error>()
            {
                new Error()
                {
                    Message = error,
                    Property = property
                }
            },
        };
    }
}