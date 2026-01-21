using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserRoles.Models;
using UserRoles.ViewModels;

namespace UserRoles.Controllers
{
    // Allow Admin as well as User and Manager to edit/view profile
    [Authorize(Roles = "User,Manager,Admin")]
    public class ProfileController : Controller
    {
        private readonly UserManager<Users> _userManager;

        public ProfileController(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }

        // ================= VIEW PROFILE =================
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var model = new ProfileViewModel
            {
                FirstName = user.Name,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                IsEditMode = false
            };

            return View(model);
        }

        // ================= EDIT PROFILE =================
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var model = new ProfileViewModel
            {
                FirstName = user.Name,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                IsEditMode = true
            };

            return View("Index", model);
        }

        // ================= SAVE PROFILE =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.IsEditMode = true;
                return View("Index", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            user.Name = model.FirstName.Trim();
            user.MobileNumber = model.MobileNumber.Trim();

            await _userManager.UpdateAsync(user);

            TempData["Success"] = "Profile updated successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
