using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserRoles.Models;
using UserRoles.ViewModels;
using UserRoles.Services;
using System.Security.Cryptography;


namespace UserRoles.Controllers
{
    [Authorize(Roles = "User,Manager,Admin")]
    public class ProfileController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly IEmailService _emailService;

        public ProfileController(
            UserManager<Users> userManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        // ================= VIEW PROFILE =================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var model = new ProfileViewModel
            {
                FirstName = user.Name,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                IsEditMode = false,
                CanEditEmail = User.IsInRole("Admin")
            };

            return View(model);
        }

        // ================= EDIT PROFILE =================
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var model = new ProfileViewModel
            {
                FirstName = user.Name,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                IsEditMode = true,               // 🔑 THIS ENABLES EDIT
                CanEditEmail = User.IsInRole("Admin")
            };

            // 🔁 IMPORTANT: reuse Index view
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
                model.CanEditEmail = User.IsInRole("Admin");
                return View("Index", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // ✅ Always update normal fields
            user.Name = model.FirstName.Trim();
            user.MobileNumber = model.MobileNumber.Trim();

            // 🔐 ADMIN EMAIL CHANGE (PENDING – SAFE)
            if (User.IsInRole("Admin") && user.Email != model.Email)
            {
                var code = System.Security.Cryptography.RandomNumberGenerator
                    .GetInt32(100000, 999999)
                    .ToString();

                user.PendingEmail = model.Email.Trim();
                user.EmailChangeLoginCode = code;
                user.EmailChangeCodeExpiry = DateTime.UtcNow.AddMinutes(10);

                await _userManager.UpdateAsync(user);

                await _emailService.SendEmailAsync(
                    user.PendingEmail,
                    "Confirm your new admin email",
                    $@"
            <p>You requested to change your admin email.</p>
            <p><strong>Login Code:</strong> {code}</p>
            <p>This code expires in 10 minutes.</p>
            "
                );

                TempData["Success"] =
                    "A login code has been sent to the new email. " +
                    "Your current login remains active until confirmed.";

                return RedirectToAction(nameof(Index)); // ✅ RETURN HERE
            }

            // ✅ Non-admin OR admin without email change
            await _userManager.UpdateAsync(user);

            TempData["Success"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Index)); // ✅ FINAL RETURN (FIX)
        }


    }
}

