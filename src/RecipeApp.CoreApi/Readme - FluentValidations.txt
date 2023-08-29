
https://stackoverflow.com/questions/75030673/fluentvalidation-automatic-validation-not-working

https://docs.fluentvalidation.net/en/latest/aspnet.html#using-the-asp-net-validation-pipeline
public static class FluentValidationExtensions
{
  public static IDictionary<string, string[]> ToDictionary(this ValidationResult validationResult)
    {
      return validationResult.Errors
        .GroupBy(x => x.PropertyName)
        .ToDictionary(
          g => g.Key,
          g => g.Select(x => x.ErrorMessage).ToArray()
        );
    }
}

https://stackoverflow.com/questions/74246450/auto-api-validation-with-fluentvalidation

https://thecodeblogger.com/2022/11/23/adding-fluent-validation-in-asp-net-core-web-apis/

https://medium.com/@dsylebee/fluentvalidation-automatic-validation-b1182e99bb32

https://github.com/FluentValidation/FluentValidation/issues/1959