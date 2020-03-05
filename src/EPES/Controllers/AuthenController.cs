using EPES.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EPES.Controllers
{
    public class AuthenController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Index(string Username, string Timestamp, string Signature)
        {
            ViewBag.Username = Username;
            ViewBag.TimeStamp = Timestamp;
            ViewBag.Signature = Signature;

            var key = "qwertyuiop[]asdfghjkl;'zxcvbnm,./";
            var data = key + Timestamp;

            var hexHashed = Hasher.SHA1(data);
            //var toEndcode = Hasher.ToTIS620(hexHashed);
            var toEndcode = Encoding.UTF8.GetBytes(hexHashed);
            var bCode = Convert.ToBase64String(toEndcode);

            ViewBag.Data = data;
            ViewBag.Hexhased = hexHashed;
            ViewBag.SignatureEPES = bCode;

            if (Signature == bCode)
            {
                SSOProductionService.ServiceSoapClient serviceSoapClient = new SSOProductionService.ServiceSoapClient(SSOProductionService.ServiceSoapClient.EndpointConfiguration.ServiceSoap);
                SSOProductionService.UserCls userCls = await serviceSoapClient.Get_SSO_UserAsync(Username, "EPES", "123456789");

                if (userCls != null)
                {
                    // Create Group
                    if ((await _roleManager.FindByNameAsync("Admin")) == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
                    }

                    if ((await _roleManager.FindByNameAsync("Manager")) == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole { Name = "Manager" });
                    }

                    if ((await _roleManager.FindByNameAsync("User")) == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole { Name = "User" });
                    }

                    if ((await _roleManager.FindByNameAsync("Special")) == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole { Name = "Special" });
                    }
                    //End Create Group
                    //if (userCls.UserID == null)
                    //{
                    //    return View();
                    //}
                    var userLocal = await _userManager.FindByNameAsync(Username);

                    //var userLocal = await _userManager.FindByNameAsync(userCls.UserID);

                    if (userLocal == null)
                    {
                        SSOProductionService.LocCls locCls = await serviceSoapClient.Get_Location_NameAsync("EPES", "123456789", userCls.UserOfficeCode);

                        var user = new ApplicationUser
                        {
                            UserName = userCls.UserID,
                            Title = userCls.UserPrefix,
                            Email = userCls.UserEMail,
                            PIN = userCls.UserPIN,
                            FName = userCls.UserNameTH,
                            LName = userCls.UserSurNameTH,
                            PosName = userCls.UserRank,
                            Class = userCls.UserLevelNew,
                            OfficeId = userCls.UserOfficeCode,
                            OfficeName = locCls.LocName
                        };

                        string url = "http://10.20.29.26:8002/webservices/GetHeadOffice.php";
                        var webRequest = WebRequest.Create(url) as HttpWebRequest;
                        if (webRequest != null)
                        {
                            webRequest.ContentType = "application/json";
                            //webRequest.UserAgent = "Nothing";
                            webRequest.Method = "POST";
                            webRequest.Timeout = 5000;
                            var USER_ID = "PB157094";
                            var USER_PWD = "Wit4337new1";
                            try
                            {
                                using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                                {
                                    string json = "{\"USER_ID\":\""+ USER_ID + "\"," +
                                                  "\"USER_PWD\":\"" + USER_PWD + "\"," +
                                                  "\"Region\":\"" + userCls.UserOfficeCode + "\"}";
                                    streamWriter.Write(json);
                                }

                                using (var s = webRequest.GetResponse().GetResponseStream())
                                {
                                    using (var sr = new StreamReader(s))
                                    {
                                        var approverAsJson = sr.ReadToEnd();
                                        var approver = JsonConvert.DeserializeObject<Approver>(approverAsJson);
                                        if (approver != null)
                                        {
                                            if (approver.Detail.UserID == userCls.UserID)
                                            {
                                                user.approver = true;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (WebException ex)
                            {
                                user.approver = false;
                            }
                        }

                        var resultCreate = await _userManager.CreateAsync(user, "P@ssw0rd");

                        SSOProductionService.TransCls transCls = await serviceSoapClient.Get_SSO_TransactionAsync(user.UserName, "EPES", "123456789");
                        if (transCls.AppTransID == "EPES-Admin")
                        {
                            if (!(await _userManager.IsInRoleAsync(user, "Admin")))
                            {
                                var roles = await _userManager.GetRolesAsync(user);
                                await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                                await _userManager.AddToRoleAsync(user, "Admin");
                            }
                        }
                        else if (transCls.AppTransID == "EPES-Manager")
                        {
                            if (!(await _userManager.IsInRoleAsync(user, "Manager")))
                            {
                                var roles = await _userManager.GetRolesAsync(user);
                                await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                                await _userManager.AddToRoleAsync(user, "Manager");
                            }
                        }
                        else if (transCls.AppTransID == "EPES-Normal")
                        {
                            if (!(await _userManager.IsInRoleAsync(user, "User")))
                            {
                                var roles = await _userManager.GetRolesAsync(user);
                                await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                                await _userManager.AddToRoleAsync(user, "User");
                            }
                        }
                        else if (transCls.AppTransID == "EPES-Special")
                        {
                            if (!(await _userManager.IsInRoleAsync(user, "Special")))
                            {
                                var roles = await _userManager.GetRolesAsync(user);
                                await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                                await _userManager.AddToRoleAsync(user, "Special");
                            }
                        }

                        var result = await _signInManager.PasswordSignInAsync(user.UserName, "P@ssw0rd", true, lockoutOnFailure: false);

                        if (result.Succeeded)
                        {
                            return Redirect("~/Home/IndexMember");
                        }
                        else
                        {
                            return Unauthorized(); // Http status code 401
                        }
                    }// End Create New User Local
                    else
                    {
                        SSOProductionService.TransCls transCls = await serviceSoapClient.Get_SSO_TransactionAsync(Username, "EPES", "123456789");

                        string url = "http://10.20.29.26:8002/webservices/GetHeadOffice.php";
                        var webRequest = WebRequest.Create(url) as HttpWebRequest;
                        if (webRequest != null)
                        {
                            webRequest.ContentType = "application/json";
                            //webRequest.UserAgent = "Nothing";
                            webRequest.Method = "POST";
                            webRequest.Timeout = 5000;
                            var APP_ID = "EPES";
                            var APP_PWD = "123456789";
                            try
                            {
                                using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                                {
                                    string json = "{\"USER_ID\":\"" + APP_ID + "\"," +
                                                  "\"USER_PWD\":\"" + APP_PWD + "\"," +
                                                  "\"OFFICE_CODE\":\"" + userCls.UserOfficeCode + "\"}";
                                    streamWriter.Write(json);
                                }

                                using (var s = webRequest.GetResponse().GetResponseStream())
                                {
                                    using (var sr = new StreamReader(s))
                                    {
                                        var approverAsJson = sr.ReadToEnd();
                                        var approver = JsonConvert.DeserializeObject<Approver>(approverAsJson);
                                        if (approver != null)
                                        {
                                            if (approver.Detail.UserID == userCls.UserID)
                                            {
                                                userLocal.approver = true;
                                                await _userManager.UpdateAsync(userLocal);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (WebException ex)
                            {
                                userLocal.approver = false;
                                await _userManager.UpdateAsync(userLocal);
                            }
                        }



                        if (transCls.AppTransID == "EPES-Admin")
                        {
                            if (!(await _userManager.IsInRoleAsync(userLocal, "Admin")))
                            {
                                var roles = await _userManager.GetRolesAsync(userLocal);
                                await _userManager.RemoveFromRolesAsync(userLocal, roles.ToArray());
                                await _userManager.AddToRoleAsync(userLocal, "Admin");
                            }
                        }
                        else if (transCls.AppTransID == "EPES-Manager")
                        {
                            if (!(await _userManager.IsInRoleAsync(userLocal, "Manager")))
                            {
                                var roles = await _userManager.GetRolesAsync(userLocal);
                                await _userManager.RemoveFromRolesAsync(userLocal, roles.ToArray());
                                await _userManager.AddToRoleAsync(userLocal, "Manager");
                            }
                        }
                        else if (transCls.AppTransID == "EPES-Normal")
                        {
                            if (!(await _userManager.IsInRoleAsync(userLocal, "User")))
                            {
                                var roles = await _userManager.GetRolesAsync(userLocal);
                                await _userManager.RemoveFromRolesAsync(userLocal, roles.ToArray());
                                await _userManager.AddToRoleAsync(userLocal, "User");
                            }
                        }
                        else if (transCls.AppTransID == "EPES-Special")
                        {
                            if (!(await _userManager.IsInRoleAsync(userLocal, "Special")))
                            {
                                var roles = await _userManager.GetRolesAsync(userLocal);
                                await _userManager.RemoveFromRolesAsync(userLocal, roles.ToArray());
                                await _userManager.AddToRoleAsync(userLocal, "Special");
                            }
                        }

                        var result = await _signInManager.PasswordSignInAsync(userLocal.UserName, "P@ssw0rd", true, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            //ViewBag.tran = transCls.AppTransID;
                            //return View();
                            return Redirect("~/Home/IndexMember");
                        }
                        else
                        {
                            return Unauthorized(); // Http status code 401
                        }
                    }
                }
                else
                {
                    return Unauthorized(); // Http status code 401
                }
            }
            else
            {
                return Unauthorized(); // Http status code 401
            }

            //return View();
        }
    }

    public static class Hasher
    {
        private static string GenerateHashString(HashAlgorithm algo, string text)
        {
            // Compute hash from text parameter
            algo.ComputeHash(Encoding.UTF8.GetBytes(text));

            // Get has value in array of bytes
            var result = algo.Hash;
            //return result;

            // Return as hexadecimal string
            return string.Join(
                string.Empty,
                result.Select(x => x.ToString("x2")));

        }

        public static string MD5(string text)
        {
            var result = default(string);

            using (var algo = new MD5CryptoServiceProvider())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }

        public static string SHA1(string text)
        {
            var result = default(string);

            using (var algo = new SHA1Managed())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }

        public static string SHA256(string text)
        {
            var result = default(string);

            using (var algo = new SHA256Managed())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }

        public static string SHA384(string text)
        {
            var result = default(string);

            using (var algo = new SHA384Managed())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }

        public static string SHA512(string text)
        {
            var result = default(string);

            using (var algo = new SHA512Managed())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }

        public static byte[] ToTIS620(string utf8String)
        {
            List<byte> buffer = new List<byte>();
            byte utf8Identifier = 224;
            for (var i = 0; i < utf8String.Length; i++)
            {
                string utf8Char = utf8String.Substring(i, 1);
                byte[] utf8CharBytes = Encoding.UTF8.GetBytes(utf8Char);
                if (utf8CharBytes.Length > 1 && utf8CharBytes[0] == utf8Identifier)
                {
                    var tis620Char = (utf8CharBytes[2] & 0x3F);
                    tis620Char |= ((utf8CharBytes[1] & 0x3F) << 6);
                    tis620Char |= ((utf8CharBytes[0] & 0x0F) << 12);
                    tis620Char -= (0x0E00 + 0xA0);
                    byte tis620Byte = (byte)tis620Char;
                    tis620Byte += 0xA0;
                    tis620Byte += 0xA0;
                    buffer.Add(tis620Byte);
                }
                else
                {
                    buffer.Add(utf8CharBytes[0]);
                }
            }
            return buffer.ToArray();
        }
        public static byte[] ToUTF8(byte[] tis620Bytes)
        {
            List<byte> buffer = new List<byte>();
            byte safeAscii = 126;
            for (var i = 0; i < tis620Bytes.Length; i++)
            {
                if (tis620Bytes[i] > safeAscii)
                {
                    if (((0xa1 <= tis620Bytes[i]) && (tis620Bytes[i] <= 0xda))
                        || ((0xdf <= tis620Bytes[i]) && (tis620Bytes[i] <= 0xfb)))
                    {
                        var utf8Char = 0x0e00 + tis620Bytes[i] - 0xa0;
                        byte utf8Byte1 = (byte)(0xe0 | (utf8Char >> 12));
                        buffer.Add(utf8Byte1);
                        byte utf8Byte2 = (byte)(0x80 | ((utf8Char >> 6) & 0x3f));
                        buffer.Add(utf8Byte2);
                        byte utf8Byte3 = (byte)(0x80 | (utf8Char & 0x3f));
                        buffer.Add(utf8Byte3);
                    }
                }
                else
                {
                    buffer.Add(tis620Bytes[i]);
                }
            }
            return buffer.ToArray();
        }
    }
}