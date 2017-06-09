using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnigmaWeb.Models;
using System.Net.Mail;
using System.Net;
using System.Web.UI;
using System.Web.Security;

namespace EnigmaWeb.Models
{
    public class UserController : Controller
    {
        //Register 
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Exclude = "IsEmailVerified,ActivationCode")] User user)
        {
            bool Status = false;
            string message = "";

            if (ModelState.IsValid)
            {
                var IsExist = IsEmailExist(user.Email);
                if (IsExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
                user.ActivationCode = Guid.NewGuid();
                
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);

                user.IsEmailVerified = false;

                using (MyDatabaseEntities db = new MyDatabaseEntities())
                {
                    db.Users.Add(user);
                    db.SaveChanges();

                    SendVerificationLinkEmail(user.Email, user.ActivationCode.ToString());
                    message = "Registration successfully done. Account activation link" +
                        "has been sent to your email:" + user.Email;
                    Status = true;
                }
            }

            else
            {
                message = "Invalid request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View();
        }
        //Verify Account
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                db.Configuration.ValidateOnSaveEnabled = false; // to avoid issue with password do not match
                var v = db.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    db.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }
        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                var v = db.Users.Where(a => a.Email == login.Email).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20;
                        var ticket = new FormsAuthenticationTicket(login.Email, login.RememberMe, timeout);
                        string encrypt = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypt);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        message = "invalid username or password";
                    }
                }
                else
                {
                    message = "invalid username or password";
                }
            }
            ViewBag.Message = message;
            return View();
        }
        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
        [NonAction]
        public bool IsEmailExist(string email)
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                var v = db.Users.Where(a => a.Email == email).FirstOrDefault();
                return v != null;
            }

        }

        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {

            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("mindaugas.dirma@gmail.com", "Enigma Web App");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "k3bu99pl67";
            string subject = "Your account is successfully created";
            string body = "<br/><br/>We are happy to tell you that your Enigma Web App account is" +
                " successfully created. Please click on the following link to vrify your account" +
                " <br/><br/><a href = '" + link + "'>" + link + "</a> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
            ScriptManager.RegisterClientScriptBlock(null, GetType(), "AlertBox", "alert('Your password send to your email id');", true);
        }
    }
}
