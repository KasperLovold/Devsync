using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DevSync.Common;

public class ValidationResult<T>
{
    public bool IsValid { get; }
    public IActionResult? ErrorResult { get; }
    public T? Value { get; }

    private ValidationResult(bool isValid, IActionResult? errorResult, T? value)
    {
        IsValid = isValid;
        ErrorResult = errorResult;
        Value = value;
    }

    public static ValidationResult<T> Success(T value) => 
        new(true, null, value ?? throw new ArgumentNullException(nameof(value), "Value cannot be null on success."));

    public static ValidationResult<T> Failure(IActionResult error) => 
        new(false, error ?? throw new ArgumentNullException(nameof(error)), default);

    public ValidationResult<TResult> Map<TResult>(Func<T, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return IsValid
            ? ValidationResult<TResult>.Success(func(Value!))
            : ValidationResult<TResult>.Failure(ErrorResult!);
    }

    public ValidationResult<TResult> Bind<TResult>(Func<T, ValidationResult<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return IsValid
            ? func(Value!)
            : ValidationResult<TResult>.Failure(ErrorResult!);
    }

    public static ValidationResult<T> Return(T value) => Success(value);
    
    public Task<ValidationResult<T>> AsTask() => Task.FromResult(this);
    
    public async Task<ValidationResult<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        
        if (!IsValid)
            return ValidationResult<TResult>.Failure(ErrorResult!);
            
        var result = await func(Value!);
        return ValidationResult<TResult>.Success(result);
    }
    
    public async Task<ValidationResult<TResult>> BindAsync<TResult>(Func<T, Task<ValidationResult<TResult>>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        
        if (!IsValid)
            return ValidationResult<TResult>.Failure(ErrorResult!);
            
        return await func(Value!);
    }
}

public static class ValidationResultExtensions
{
    public static async Task<ValidationResult<TResult>> MapAsync<T, TResult>(
        this Task<ValidationResult<T>> task, 
        Func<T, TResult> func)
    {
        var result = await task;
        return result.Map(func);
    }
    
    public static async Task<ValidationResult<TResult>> MapAsync<T, TResult>(
        this Task<ValidationResult<T>> task, 
        Func<T, Task<TResult>> func)
    {
        var result = await task;
        return await result.MapAsync(func);
    }
    
    public static async Task<ValidationResult<TResult>> BindAsync<T, TResult>(
        this Task<ValidationResult<T>> task, 
        Func<T, ValidationResult<TResult>> func)
    {
        var result = await task;
        return result.Bind(func);
    }
    
    public static async Task<ValidationResult<TResult>> BindAsync<T, TResult>(
        this Task<ValidationResult<T>> task, 
        Func<T, Task<ValidationResult<TResult>>> func)
    {
        var result = await task;
        return await result.BindAsync(func);
    }
}