namespace RecipeApp.Mvc.Controllers;
public class StudentController : Controller
{
    public IActionResult Index()
    {
        var model = new StudentViewModel();
        model.StudentAddress.Add(new StudentAddressViewModel());
        model.StudentAddress.Add(new StudentAddressViewModel());

        return View(model);
    }

    [HttpPost]
    public IActionResult Index(StudentViewModel studentViewModel)
    {
        return View(studentViewModel);
    }

    [HttpPost]
    public IActionResult Add(StudentViewModel studentViewModel)
    {
        studentViewModel.StudentAddress.Add(new StudentAddressViewModel());

        ModelState.Clear();
        return View("Index", studentViewModel);
    }
}
