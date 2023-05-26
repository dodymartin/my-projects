using System.Net;
using System.Reflection;
using FluentValidation;
using FluentValidation.Internal;

namespace MinimalApi;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class ValidateAttribute : Attribute
{
    public string[] RuleSetNames { get; }

    public ValidateAttribute() { }

    public ValidateAttribute(params string[] ruleSetNames)
    {
        RuleSetNames = ruleSetNames;
    }
}

public static class ValidationFilter
{
    public static EndpointFilterDelegate ValidationFilterFactory(EndpointFilterFactoryContext context, EndpointFilterDelegate next)
    {
        // Look for [Validate] attribute
        var validationDescriptors = GetValidators(context.MethodInfo, context.ApplicationServices);

        // If found, add Validate filter to endpoint
        if (validationDescriptors.Any())
            return invocationContext => Validate(validationDescriptors, invocationContext, next);

        // Otherwise, add pass-thru
        return invocationContext => next(invocationContext);
    }

    private static async ValueTask<object?> Validate(IEnumerable<ValidationDescriptor> validationDescriptors, EndpointFilterInvocationContext invocationContext, EndpointFilterDelegate next)
    {
        // Validate argument with the Validator for that type
        foreach (var descriptor in validationDescriptors)
        {
            var argument = invocationContext.Arguments[descriptor.ArgumentIndex];

            if (argument is not null)
            {
                var validationResult = descriptor.RuleSetNames is null || descriptor.RuleSetNames.Length == 0
                    ? await descriptor.Validator.ValidateAsync(
                        new ValidationContext<object>(argument))
                    : await descriptor.Validator.ValidateAsync(
                        new ValidationContext<object>(argument, 
                        new PropertyChain(), 
                        new RulesetValidatorSelector(descriptor.RuleSetNames)));

                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary(),
                        statusCode: (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        return await next.Invoke(invocationContext);
    }

    private static IEnumerable<ValidationDescriptor> GetValidators(MethodInfo methodInfo, IServiceProvider serviceProvider)
    {
        // Look for Validate attribute, if found, get
        // the Validator from DI, create a Descriptor
        // and return, pass along any RuleSetNames
        var parameters = methodInfo.GetParameters();

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            var validateAttribute = parameter.GetCustomAttribute<ValidateAttribute>();
            if (validateAttribute is not null)
            {
                var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);

                // Note that FluentValidation validators needs to be registered as singleton
                IValidator? validator = serviceProvider.GetService(validatorType) as IValidator;

                if (validator is not null)
                    yield return new ValidationDescriptor { ArgumentIndex = i, ArgumentType = parameter.ParameterType, Validator = validator, RuleSetNames = validateAttribute.RuleSetNames };
            }
        }
    }

    private class ValidationDescriptor
    {
        public required int ArgumentIndex { get; init; }
        public required Type ArgumentType { get; init; }
        public required IValidator Validator { get; init; }
        public string[] RuleSetNames { get; init; }
    }
}
