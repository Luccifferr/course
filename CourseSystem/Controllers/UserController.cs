using CourseSystem.Data;
using CourseSystem.Models;
using CourseSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 

namespace CourseSystem.Controllers;

[Authorize(Roles = "Admin, SuperAdmin")]

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;

    public UserController(
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        RoleManager<IdentityRole> roleManager, 
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _roleManager = roleManager;
        _context = context;
    }


    // list all users
    [Authorize(Roles = "Admin, SuperAdmin")]

    public IActionResult Index()
    {
        var users = _userManager.Users;
        return View(users);
    }
    [Authorize(Roles = "SuperAdmin")]
    public IActionResult CreateAdmin()
    {
        return View();
    }
    [Authorize(Roles = "Admin, SuperAdmin")]
    public IActionResult CreateTeacher()
    {
        return View();
    }
    [Authorize(Roles = "Admin, SuperAdmin")]

    public IActionResult CreateStudent()
    {
        ViewData["GradeId"] = new SelectList(_context.Grades, "Id", "Name");
        return View();
    }

    // POST: Courses/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> CreateAdmin([Bind("FirstName,LastName, Username, Email, Password, GradeId")] UserViewModel userViewModel)
    {
        var passwordValidator = new PasswordValidator<ApplicationUser>();
        var result = await passwordValidator.ValidateAsync(_userManager, null!, userViewModel.Password);

        if (ModelState.IsValid)
        {
            if (result.Succeeded)
            {
                if (await _roleManager.RoleExistsAsync("Admin"))
                {
                    await CreateTempUser(userViewModel, "Admin");
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Role doesn't exist.");
            }
            ModelState.AddModelError(string.Empty, "Password doesn't fit the criteria.");
        }
        return View(userViewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> CreateTeacher([Bind("FirstName,LastName, Username, Email, Password, GradeId")] UserViewModel userViewModel)
    {
        var passwordValidator = new PasswordValidator<ApplicationUser>();
        var result = await passwordValidator.ValidateAsync(_userManager, null!, userViewModel.Password);

        if (ModelState.IsValid)
        {
            if (result.Succeeded)
            {
                if (await _roleManager.RoleExistsAsync("Teacher"))
                {
                    await CreateTempUser(userViewModel, "Teacher");
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Role doesn't exist.");
            }
            ModelState.AddModelError(string.Empty, "Password doesn't fit the criteria.");
        }
        return View(userViewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> CreateStudent([Bind("FirstName,LastName, Username, Email, Password, GradeId")] UserViewModel userViewModel)
    {
        var passwordValidator = new PasswordValidator<ApplicationUser>();
        var result = await passwordValidator.ValidateAsync(_userManager, null!, userViewModel.Password);

        if (ModelState.IsValid)
        {
            if (result.Succeeded)
            {
                if (await _roleManager.RoleExistsAsync("Student"))
                {
                    await CreateTempUser(userViewModel, "Student");
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Role doesn't exist.");
            }
            ModelState.AddModelError(string.Empty, "Password doesn't fit the criteria.");
        }
        ViewData["GradeId"] = new SelectList(_context.Grades, "Id", "Name");
        return View(userViewModel);
    }
    [Authorize(Roles = "Admin, SuperAdmin")]
    private async Task CreateTempUser(UserViewModel userViewModel, string role)
    {
        var user = CreateUser();

        await _userStore.SetUserNameAsync(user, userViewModel.Username, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, userViewModel.Email, CancellationToken.None);

        user.FirstName = userViewModel.FirstName;
        user.LastName = userViewModel.LastName;
        user.FullName = $"{userViewModel.FirstName} {userViewModel.LastName}";
        user.GradeId = userViewModel.GradeId;

        await _userManager.CreateAsync(user, userViewModel.Password);
        await _userManager.AddToRoleAsync(user, role);
    }
    [Authorize(Roles = "Admin, SuperAdmin")]
    // GET: Courses/Edit/5
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null || _context.Users == null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var userViewModel = new UserViewModel();
        userViewModel.FirstName = user.FirstName;
        userViewModel.LastName = user.LastName;
        userViewModel.Username = user.UserName;
        userViewModel.Email = user.Email;
        userViewModel.Password = string.Empty;
        if(await _userManager.IsInRoleAsync(user, "Student"))
        {
            ViewData["GradeId"] = new SelectList(_context.Grades, "Id", "Name", user.GradeId);
        }
        return View(userViewModel);
    }

    // POST: Courses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> Edit(string id, [Bind("FirstName, LastName, Username, Email, Password, GradeId")] UserViewModel userViewModel)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (!ModelState.IsValid) return View(userViewModel);
        user.FirstName = userViewModel.FirstName;
        user.LastName = userViewModel.LastName;
        user.FullName = userViewModel.FirstName + " " + userViewModel.LastName;
        user.UserName = userViewModel.Username;
        user.Email = userViewModel.Email;
        user.GradeId = userViewModel.GradeId;
        if (userViewModel.Password != null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, userViewModel.Password);
        }
        await _userManager.UpdateAsync(user);
        return RedirectToAction(nameof(Index));
    }
    // GET: Courses/Delete/5 
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<JsonResult> Delete(string id)
    {
        if (_context.Users == null)
        { 
            return Json(new { silindimi = false, hataMesaji = "L�tfen ilk �nce e�itim paketleri silin." }); 
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }
        return Json(new { silindimi = true });
         
    }
    [Authorize(Roles = "Admin, SuperAdmin")]
    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                                                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }
    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)_userStore;
    }
}