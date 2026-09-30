using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using ValidationException = FluentValidation.ValidationException;
using System.Text;

namespace TaskManager.Application.Behaviours;

// Runs BEFORE every handler — checks if the request is valid first
// If invalid, throws an exception immediately — handler never runs
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    // IEnumerable = a collection of ALL validators registered for this request type
    // There might be zero, one, or many validators for any given command
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    // Constructor — DI injects all validators that match this request type automatically
    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // If no validators exist for this request, skip straight to the handler
        if (!_validators.Any())
            return await next();

        // Run ALL validators against the request
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // Collect all failures across all validators
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        // If any failures found, throw — handler never runs
        if (failures.Count != 0)
            throw new ValidationException(failures);

        // All good — pass through to the handler
        return await next();
    }
}