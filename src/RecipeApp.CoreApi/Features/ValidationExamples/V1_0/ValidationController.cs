namespace RecipeApp.CoreApi.Features.ValidationExamples.V1_0;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Validation", Name = "Validation")]
[ApiController]
[AllowAnonymous]
public class ValidationController : BaseApiController
{
    [HttpPost, Route("DataAnnotations")]
    public IActionResult DataAnnotations([FromBody] DataAnnotationsModel dataAnnotationsModel)
    {
        return Ok(dataAnnotationsModel);
    }

    [HttpPost, Route("FluentValidations")]
    public IActionResult FluentValidations([FromBody] FluentValidationsModel fluentValidationsModel)
    {
        var validator = new FluentValidationsModelValidator();
        var result = validator.Validate(fluentValidationsModel);
        var isValid = result.IsValid;

        return Ok(fluentValidationsModel);
    }
}

public class DataAnnotationsModel
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }
}

public class FluentValidationsModel
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
}

public class FluentValidationsModelValidator : AbstractValidator<FluentValidationsModel>
{
    public FluentValidationsModelValidator()
    {
        RuleFor(x => x.FirstName).NotNull();
        RuleFor(x => x.LastName).NotNull();
    }
}