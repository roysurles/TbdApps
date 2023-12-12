using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RecipeApp.Mvc.Models;

public class StudentViewModel
{
    public StudentViewModel()
    {
        StudentAddress ??= new();
    }

    public int Id { get; set; }

    public string? Name { get; set; }

    [BindRequired]
    public List<StudentAddressViewModel> StudentAddress { get; set; }
}

public class StudentAddressViewModel
{
    public string? Address { get; set; }
    public string? City { get; set; }
}