﻿using ADOPSEV1._1.Data;
using ADOPSEV1._1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ADOPSEV1._1.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Register()
        {
            ViewBag.Branch = _db.branches.ToList();
            IEnumerable<User> objUserList = _db.users;
            return View(objUserList);
        }


        public IActionResult Profile(string username)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string first_name, string last_name, string email, string password, string username, int BranchId)
        {

            if (ModelState.IsValid)
            {
                User user = new User();
                user.first_name = HttpContext.Request.Form["first_name"].ToString();
                user.last_name = HttpContext.Request.Form["last_name"].ToString();
                user.email = HttpContext.Request.Form["email"].ToString();
                user.branchId = Int32.Parse(HttpContext.Request.Form["BranchId"]);
                string typedPass = HttpContext.Request.Form["password"].ToString();
                user.password = HashFun.Hash(password);
                user.username = HttpContext.Request.Form["username"].ToString();
                user.role = 0;

                _db.users.Add(user);
                _db.SaveChanges();

                return RedirectToAction("Register");
            }
            else
            {
                ViewBag.error = "not valid";
                return View("Register");
            }

        }

        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> login(UserLogin login)    //string username, string password
        {

            string message = "";
            using (var db = _db)
            {
                User chkUser;
                Boolean isEmail = IsValidEmail(login.username);
                if (isEmail)
                {
                    chkUser = db.users.Where(x => x.email == login.username).FirstOrDefault();
                }
                else
                {
                    chkUser = db.users.Where(x => x.username == login.username).FirstOrDefault();
                }
                if (chkUser != null)
                {
                    if (string.Compare(HashFun.Hash(login.password), chkUser.password) == 0)
                    {
                        var userName = User.FindFirstValue(ClaimTypes.Name);
                        //var userN = User.FindFirstValue(ClaimTypes.Role);
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, chkUser.username),
                            new Claim(ClaimTypes.Role , chkUser.role.ToString()),
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, "login");

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                        ViewBag.userName = ClaimTypes.Name;

                        return RedirectToAction("Index", "Home");


                    }
                    else
                    {
                        message = "invalid password";
                    }

                }
                else
                {
                    message = "Invalid credentials";
                    return View();
                }
            }

            ViewBag.message = message;
            return View();

        }



        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}
