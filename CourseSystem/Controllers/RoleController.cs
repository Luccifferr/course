using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseSystem.Controllers;

[Authorize(Roles = "SuperAdmin")]
public class RoleController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    //List all the roles created by users.
    public IActionResult Index()
    {
        var roles = _roleManager.Roles;
        return View(roles);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(IdentityRole model)
    {
        if(!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
        {
            await _roleManager.CreateAsync(new IdentityRole(model.Name));
        }

        return RedirectToAction("Index");
    }
     
    public async Task<IActionResult> Delete(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return Json(new { silindimi = false, hataMesaji = "Entity set 'ApplicationDbContext.Role'  is null." });
        }

        var result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
            return Json(new { silindimi = true });
             
        }
        foreach (var error in result.Errors)
        {
            return Json(new { silindimi = false, hataMesaji = "couldnt delete it" });
             
        }

        return Json(new { silindimi = false, hataMesaji = "something went wrong please warn your programmer" });
         
    } 
}