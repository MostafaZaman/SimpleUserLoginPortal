using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DNDAuth.Models;
using System.Net.Mail;
using System.Net;
using System.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Transactions;

namespace DNDAuth.Controllers
{
    public class UserController : Controller
    {
        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {

            //ViewBag.SecurityQuestionList = ToSelectList(SecurityQuestionList(), "SecurityQuesID", "SecurityQues");
            ViewBag.SecurityQuesID = LoadAllSecurityQuestions();
            return View();

        }

        [HttpGet]
        public ActionResult PasswordRecovery()
        {

            var SessionVal = HttpContext.Session.GetString("EmailID");

            if (string.IsNullOrEmpty(SessionVal))
            {
                return RedirectToAction("Login", "User");
            }

            string SecQues = "";
            using (DNDAuthDBContext dc = new DNDAuthDBContext())
            {
                var SecQID = dc.Users.Where(x => x.EmailID == SessionVal).First().SecurityQuesID;
                SecQues = dc.SecurityQuestions.Where(x => x.SecurityQuesID == SecQID).First().SecurityQues;
            }

            ViewBag.SecurityQues = SecQues;

            HttpContext.Session.SetString("SecQues", SecQues);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordRecovery(User usr)
        {

            bool Status = false;
            string message = "";

            var SessionVal = HttpContext.Session.GetString("EmailID");

            if (string.IsNullOrEmpty(SessionVal))
            {
                return RedirectToAction("Login", "User");
            }

            string SecurityAns = "";

            using (DNDAuthDBContext dc = new DNDAuthDBContext())
            {
                SecurityAns = dc.Users.Where(x => x.EmailID == SessionVal).First().SecurityAnswer;
            }

            if (string.Compare(usr.SecurityAnswer, SecurityAns) == 0)
            {
                Status = true;
                return RedirectToAction("ChangePassword", "User");
            }
            else
            {
                message = "Answer is Incorrect. Try Again.";
                ViewBag.Message = message;
                ViewBag.Status = Status;

                string SecQues = HttpContext.Session.GetString("SecQues");

                ViewBag.SecurityQues = SecQues;

                //return RedirectToAction("PasswordRecovery", "User");
            }

            //ViewBag.SecurityQuestionList = ToSelectList(SecurityQuestionList(), "ID", "SecurityQues");
            return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            var SessionVal = HttpContext.Session.GetString("EmailID");

            if (string.IsNullOrEmpty(SessionVal))
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(User usr)
        {

            var SessionVal = HttpContext.Session.GetString("EmailID");

            if (string.IsNullOrEmpty(SessionVal))
            {
                return RedirectToAction("Login", "User");
            }

            using (DNDAuthDBContext dc = new DNDAuthDBContext())
            {
                var RealUserData = dc.Users.Where(x => x.EmailID == SessionVal).FirstOrDefault();

                RealUserData.Password = Crypto.Hash(usr.Password);
                RealUserData.ConfirmPassword = Crypto.Hash(usr.ConfirmPassword);

                dc.Users.Update(RealUserData);
                dc.SaveChanges();
            }

            return View();
        }

        //public IEnumerable<SecurityQuestions> SecurityQuestionList()
        //{
        //    using (DNDAuthDBContext dc = new DNDAuthDBContext())
        //    {
        //        var result = dc.SecurityQuestions.ToList();

        //        return result;
        //    }

        //}

        //[NonAction]
        //public SelectList ToSelectList(IEnumerable<SecurityQuestions> table, string valueField, string textField)
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();

        //    foreach (SecurityQuestions row in table)
        //    {
        //        list.Add(new SelectListItem()
        //        {
        //            Text = row.SecurityQues.ToString(),
        //            Value = row.SecurityQuesID.ToString()
        //        });
        //    }

        //    return new SelectList(list, "Value", "Text");
        //}

        public static SelectList LoadAllSecurityQuestions()
        {

            using (DNDAuthDBContext dc = new DNDAuthDBContext())
            {
                var result = dc.SecurityQuestions.ToList();
                var items = result.Select(x => new { x.SecurityQuesID, x.SecurityQues }).ToList();
                items.Insert(0, new { SecurityQuesID = 0, SecurityQues = "---- Select ----" });
                return new SelectList(items, "SecurityQuesID", "SecurityQues");
            }

        }

        //Registration POST action 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User user)
        {

            using (DNDAuthDBContext dc = new DNDAuthDBContext())
            {
                bool Status = false;
                string message = "";
                ViewBag.SecurityQuesID = LoadAllSecurityQuestions();
                //ViewBag.SecurityQuestionList = ToSelectList(SecurityQuestionList(), "SecurityQuesID", "SecurityQues");

                using (var transaction = dc.Database.BeginTransaction())
                {

                    try
                    {
                        //
                        // Model Validation 
                        if (ModelState.IsValid)
                        {

                            #region //Email is already Exist 
                            var isExist = IsEmailExist(user.EmailID);
                            if (isExist)
                            {

                                ModelState.AddModelError("EmailExist", "Email already exist");
                                return View(user);
                            }
                            #endregion

                            #region Generate Activation Code 
                            user.ActivationCode = Guid.NewGuid();
                            #endregion

                            #region  Password Hashing 
                            user.Password = Crypto.Hash(user.Password);
                            user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword); //
                            #endregion
                            user.IsEmailVerified = false;

                            #region Save to Database

                            dc.Users.Add(user);
                            dc.SaveChanges();

                            //Send Email to User
                            SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                            message = "Registration successfully done. Account activation link " +
                                " has been sent to your email id:" + user.EmailID;
                            Status = true;

                            transaction.Commit();

                            #endregion
                        }
                        else
                        {
                            message = "Invalid Request";
                        }

                        ViewBag.Message = message;
                        ViewBag.Status = Status;
                        return View(user);

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        string Error = ex.Message.ToString();
                        ViewBag.Message = Error;
                        ViewBag.Status = Status;

                        return View(user);
                    }
                }
            }
        }
        //Verify Account  

        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (DNDAuthDBContext dc = new DNDAuthDBContext())
            {
                //dc.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                // Confirm password does not match issue on save changes
                var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        //Login 
        [HttpGet]
        public ActionResult Login()
        {
            //ViewBag.ReturnUrl = returnUrl;

            if (!string.IsNullOrEmpty(Request.QueryString.Value))
                return RedirectToAction("Login");

            return View();
        }

        //Login POST
        //[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin login)
        {
            try
            {

                string message = "";
                using (DNDAuthDBContext dc = new DNDAuthDBContext())
                {
                    var v = dc.Users.Where(a => a.EmailID == login.EmailID).FirstOrDefault();
                    if (v != null)
                    {
                        if (!v.IsEmailVerified)
                        {
                            ViewBag.Message = "Please verify your email first";
                            return View();
                        }

                        if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                        {

                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, v.EmailID)
                            };

                            ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                            await HttpContext.SignInAsync(principal);

                            HttpContext.Session.SetString("EmailID", v.EmailID.ToString());

                            return RedirectToAction("Index", "Home");

                        }
                        else
                        {
                            message = "Invalid credential provided";
                        }
                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                ViewBag.Message = message;
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message.ToString();
                return View();
            }
        }

        //Logout
        //[Authorize]
        [HttpPost]
        public ActionResult Logout()
        {

            //        await HttpContext.SignOutAsync(
            //CookieAuthenticationDefaults.AuthenticationScheme);

            var SessionVal = HttpContext.Session.GetString("EmailID");

            HttpContext.Session.SetString("EmailID", string.Empty);

            return RedirectToAction("Login", "User");
        }


        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (DNDAuthDBContext dc = new DNDAuthDBContext())
            {
                var v = dc.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            //var verifyUrl = "/User/VerifyAccount/" + activationCode;

            //var uriBuilder = new UriBuilder
            //{
            //    Scheme = Request.Scheme,
            //    Host = Request.Host.ToString(),
            //    Path = $"/User/VerifyAccount/{activationCode}"
            //};

            //        var urlBuilder =
            //new System.UriBuilder(Request.Url.AbsoluteUri)
            //{
            //    Path = Url.Action("Action", "Controller"),
            //    Query = null,
            //};

            //Uri uri = urlBuilder.Uri;
            //string url = urlBuilder.ToString();

            //var request = HttpContextAccessor.HttpContext.Request;
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = Request.Scheme;
            uriBuilder.Host = Request.Host.Host.ToString();
            uriBuilder.Port = Convert.ToInt32(Request.Host.Port.ToString());
            uriBuilder.Path = $"/User/VerifyAccount/{activationCode}";
            uriBuilder.Query = Request.QueryString.ToString();


            var link = uriBuilder.Uri;

            //uriBuilder.Port = -1;

            //var link = uriBuilder.Uri;

            //var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("zaman@dropndot.com", "Drop N Dot");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "mzDD$$@@**"; // Replace with actual password
            string subject = "Your account is successfully created!";

            string body = "<br/><br/>We are excited to tell you that your Drop N Dot account is" +
                " successfully created. Please click on the below link to verify your account" +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";

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
                try
                {
                    smtp.Send(message);
                }
                catch (Exception)
                {
                    throw;
                }

        }



    }
}