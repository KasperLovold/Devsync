using Microsoft.AspNetCore.Mvc;

namespace DevSync.Common;

public abstract class ValidationResult<T>
{
    private ValidationResult() { }

    private sealed class Success(T value) : ValidationResult<T>
    {
        public T Value { get; } = value ?? throw new ArgumentNullException(nameof(value));
    }

    private sealed class Failure(IActionResult error) : ValidationResult<T>
    {
        public IActionResult ErrorResult { get; } = error ?? throw new ArgumentNullException(nameof(error));
    }

    public static ValidationResult<T> FromValue(T value) => new Success(value);
    public static ValidationResult<T> FromError(IActionResult error) => new Failure(error);

    public bool IsSuccess => this is Success;
    public bool IsFailure => this is Failure;

    public T? GetValueOrDefault() => this is Success s ? s.Value : default;
    public IActionResult? GetErrorOrDefault() => this is Failure f ? f.ErrorResult : null;

    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<IActionResult, TResult> onFailure)
    {
        return this switch
        {
            Success s => onSuccess(s.Value),
            Failure f => onFailure(f.ErrorResult),
            _ => throw new InvalidOperationException("Unknown state."),
        };
    }

    public async Task<ValidationResult<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
    
        switch (this)
        {
            case Success s:
            {
                var result = await func(s.Value);
                return ValidationResult<TResult>.FromValue(result);
            }
            case Failure f:
                return ValidationResult<TResult>.FromError(f.ErrorResult);
            default:
                throw new InvalidOperationException("Unknown state.");
        }
    }

    public async Task<ValidationResult<TResult>> BindAsync<TResult>(
        Func<T, Task<ValidationResult<TResult>>> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return this switch
        {
            Success s => await func(s.Value),
            Failure f => ValidationResult<TResult>.FromError(f.ErrorResult),
            _ => throw new InvalidOperationException("Unknown state.")
        };
    }    
}

public static class ResultExtensions
{
    public static async Task<ValidationResult<TResult>> BindAsync<T, TResult>(
        this Task<ValidationResult<T>> task,
        Func<T, Task<ValidationResult<TResult>>> func)
    {
        var result = await task;
        return await result.BindAsync(func);
    }
}
