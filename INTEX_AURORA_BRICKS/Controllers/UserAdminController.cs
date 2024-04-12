
using INTEX_AURORA_BRICKS.Models;
using Microsoft.AspNetCore.Authorization;

namespace INTEX_AURORA_BRICKS.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UserAdminController : Controller
{
    private readonly UserManager<Customers> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserAdminController(UserManager<Customers> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> UserAdmin()
    {
        var users = _userManager.Users.ToList();
        var userRolesViewModel = new List<UserRoles>();

        foreach (Customers user in users)
        {
            var thisViewModel = new UserRoles();
            thisViewModel.UserId = user.Id;
            thisViewModel.Email = user.Email;
            thisViewModel.Roles = await GetUserRoles(user);
            userRolesViewModel.Add(thisViewModel);
        }
        ViewData["Roles"] = _roleManager.Roles.Select(r => r.Name).ToList();

        return View(userRolesViewModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    
    public IActionResult ListUsers()
    {
        var users = _userManager.Users;
        return View(users);
    }

   

    private async Task<List<string>> GetUserRoles(Customers user)
    {
        return new List<string>(await _userManager.GetRolesAsync((Customers)user));
    }
    [Authorize(Roles = "Admin")]

    [HttpPost]
    public async Task<IActionResult> UserAdmin(string userId, List<string> roles)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var userRoles = await _userManager.GetRolesAsync(user);
        var allRoles = _roleManager.Roles.ToList();
        var selectedRoles = roles;

        var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
        if (!result.Succeeded)
        {
            // Handle the error
            return View();
        }

        result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
        if (!result.Succeeded)
        {
            // Handle the error
            return View();
        }

        return RedirectToAction("UserAdmin");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> EditUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
            return View("NotFound");
        }

        // GetClaimsAsync retunrs the list of user Claims
        var userClaims = await _userManager.GetClaimsAsync(user);
        // GetRolesAsync returns the list of user Roles
        var userRoles = await _userManager.GetRolesAsync(user);

        var model = new EditUserViewModel
        {
            Id = user.Id,
            first_name = user.first_name,
            last_name = user.last_name,
            country_of_residence = user.country_of_residence,
            gender = user.gender,
            age = user.age
            
        };

        return View(model);
    }

    
    [HttpPost]
    public async Task<IActionResult> EditUser(EditUserViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);

        if (user == null)
        {
            ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
            return View("NotFound");
        }
        else
        {
            // Update user properties from the model
            user.first_name = model.first_name;
            user.last_name = model.last_name;
            user.country_of_residence = model.country_of_residence;
            user.gender = model.gender;
            user.age = model.age;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
            return View("NotFound");
        }
        else
        {
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("ListUsers");
        }
    }


}