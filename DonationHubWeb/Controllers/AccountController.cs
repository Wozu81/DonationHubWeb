using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DonationHubWeb.ViewModel.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DonationHubWeb.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        {
            var usersIdentity = _userManager.Users.ToList();
            List<UserAdminViewModel> users = new List<UserAdminViewModel>();
            foreach (var item in usersIdentity)
            {
                var claims = await _userManager.GetClaimsAsync(item);
                var nameClaim = claims.Where(x => x.Type == "FirstName").FirstOrDefault();
                var surnameClaim = claims.Where(x => x.Type == "LastName").FirstOrDefault();

                var roles = await _userManager.GetRolesAsync(item);

                var user = new UserAdminViewModel()
                {
                    ID = item.Id,
                    Name = nameClaim.Value,
                    Surname = surnameClaim.Value,
                    Role = roles.FirstOrDefault()

                };
                users.Add(user);
            }
            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.RemoveFromRoleAsync(user, "User");
            await _userManager.AddToRoleAsync(user, "Blocked");
            return RedirectToAction("List");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActiveUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.RemoveFromRoleAsync(user, "Blocked");
            await _userManager.AddToRoleAsync(user, "User");
            return RedirectToAction("List");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Account", "Profile");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var isUserExist = await _userManager.FindByNameAsync(viewModel.Email);
                if (isUserExist == null)
                {
                    var user = new IdentityUser(viewModel.Email) { Email = viewModel.Email };
                    var result = await _userManager.CreateAsync(user, viewModel.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddClaimAsync(user, new Claim("FirstName", viewModel.Name));
                        await _userManager.AddClaimAsync(user, new Claim("LastName", viewModel.Lastname));
                        //-------
                        var login = await _signInManager.PasswordSignInAsync(viewModel.Email,
                            viewModel.Password, true, false);
                        if (login.Succeeded)
                        {
                            if (_roleManager.Roles.Any(x => x.Name == "Admin"))
                            {
                                await _userManager.AddToRoleAsync(user, "User");
                            }
                            else
                            {
                                var identityRole = new IdentityRole("Admin"); //User
                                var createResult = await _roleManager.CreateAsync(identityRole);
                                if (createResult.Succeeded)
                                {
                                    await _userManager.AddToRoleAsync(user, "Admin");
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Cant Add Role");
                                }

                                var identityRoleBlocked = new IdentityRole("Blocked");
                                var createResultBlocked = await _roleManager.CreateAsync(identityRoleBlocked);
                                if (!createResultBlocked.Succeeded)
                                {
                                    ModelState.AddModelError("", "Cant Add Role Blocked");
                                }
                            }

                            //-----------------------------
                            await _signInManager.RefreshSignInAsync(user);
                            return RedirectToAction("Login", "Account");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Nie można się zarejestrować!");
                        }
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Użytkownik o tym emailu już jest zarejestrowany!");
                    return View(viewModel);
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var login = await _signInManager.PasswordSignInAsync
                    (model.Login, model.Password, model.RememberMe, false);
                if (login.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Nie można się zalogować!");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var claims = await _userManager.GetClaimsAsync(user);
            var model = new EditUserViewModel()
            {
                ID = user.Id,
                Name = claims.Where(x => x.Type == "FirstName").FirstOrDefault().Value,
                Surname = claims.Where(x => x.Type == "LastName").FirstOrDefault().Value,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(viewModel.Email);
                user.UserName = viewModel.Email;
                user.NormalizedUserName = viewModel.Email.ToUpper();
                user.Email = viewModel.Email;
                user.NormalizedEmail = viewModel.Email.ToUpper();

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    //Change first and last name
                    var claims = await _userManager.GetClaimsAsync(user);
                    var nameClaim = claims.Where(x => x.Type == "FirstName").FirstOrDefault();
                    var surnameClaim = claims.Where(x => x.Type == "LastName").FirstOrDefault();
                    await _userManager.ReplaceClaimAsync(user, nameClaim, new Claim("FirstName", viewModel.Name));
                    await _userManager.ReplaceClaimAsync(user, surnameClaim, new Claim("LastName", viewModel.Surname));
                    //-------
                    var isSignIn = await _signInManager.CanSignInAsync(user);
                    if (isSignIn)
                    {
                        await _signInManager.RefreshSignInAsync(user);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Nie można się zarejestrować!");
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(userId);
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, viewModel.Password);

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var isSignIn = await _signInManager.CanSignInAsync(user);
                    if (isSignIn)
                    {
                        await _signInManager.RefreshSignInAsync(user);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Nie można się zarejestrować!");
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(viewModel);
        }
    }
}
