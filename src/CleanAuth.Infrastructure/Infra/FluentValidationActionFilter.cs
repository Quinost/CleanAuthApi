using Clean.Shared;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanAuth.Infrastructure.Infra;
internal sealed class FluentValidationActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach(var model in context.ActionArguments.Values.Where(static x => x?.GetType().IsClass ?? false))
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(model!.GetType());

            if (context.HttpContext.RequestServices.GetService(validatorType) is IValidator validator)
            {
                var valdiationContext = new ValidationContext<dynamic>(model);
                var validationResult = await validator.ValidateAsync(valdiationContext);

                if (!validationResult.IsValid)
                {
                    context.Result = MapToBadRequest(validationResult);
                    return;
                }
            }
        }

        await next();
    }

    private static BadRequestObjectResult MapToBadRequest(ValidationResult validationResult)
    {
        var result = Result.Failed(validationResult.Errors.Select(x => x.ErrorMessage).ToArray());
        return new BadRequestObjectResult(result);
    }
}
