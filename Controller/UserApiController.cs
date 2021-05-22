using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Newtonsoft.Json;
using surer_backend.Database;
using surer_backend.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace surer_backend.Controller
{
    [ApiController]
    public class UserApiController : ControllerBase
    {
        protected SurerContext dbcontext;
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;

        public UserApiController(SurerContext dbcontext,IJWTAuthenticationManager jWTAuthenticationManager)
        {
            this.dbcontext = dbcontext;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("api/[controller]/viewUser")]
        public ActionResult viewUser(string userId)
        {
            User user = dbcontext.User.Where(x=>x.Id==userId).FirstOrDefault();
            return Content(JsonConvert.SerializeObject(user));

        }
        [AllowAnonymous]
        [HttpPost]
        [Route("api/[controller]/loginUser")]
        public ActionResult LoginUser([FromBody]LoginUserModel userModel)
        {
            User user = dbcontext.User.Where(x => x.Email == userModel.Email).FirstOrDefault();
            if (user == null)
            {
                Object response = new
                {
                    message = "User does not exit",
                    code = HttpStatusCode.NoContent
                };
                return Content(JsonConvert.SerializeObject(response));
            }
            using (MD5 md5Hash = MD5.Create())
            {
                string password = MD5Hash.GetMd5Hash(md5Hash, userModel.Password);
                if(user.Password == password)
                {
                    string token = jWTAuthenticationManager.Authenticate(user.FirstName);
                    Object response = new
                    {
                        token = token,
                        user=user,
                        code = HttpStatusCode.OK
                    };
                    return Content(JsonConvert.SerializeObject(response));
                }
                else
                {
                    Object response = new
                    {
                        message = "Password Incorrect",
                        code = HttpStatusCode.NoContent
                    };
                    return Content(JsonConvert.SerializeObject(response));
                }

            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("api/[controller]/registerUser")]
        public ActionResult RegisterUser([FromBody]User value)
        {
            User user = dbcontext.User.Where(x => x.Email == value.Email).FirstOrDefault();
            if(user != null)
            {
                Object response = new
                {
                    message = "Email Already Existed",
                    code = HttpStatusCode.Conflict
                };
                return Content(JsonConvert.SerializeObject(response));
            }
            else
            {
                using (MD5 md5Hash = MD5.Create())
                {
                    User newUser = new User();
                    newUser.Id = Guid.NewGuid().ToString();
                    newUser.FirstName = value.FirstName;
                    newUser.LastName = value.LastName;
                    newUser.Email = value.Email;
                    newUser.Password = MD5Hash.GetMd5Hash(md5Hash, value.Password);
                    if(!value.ContactNumber.Equals(0))
                    {
                        newUser.ContactNumber = value.ContactNumber;
                    }
                    dbcontext.Add(newUser);
                    dbcontext.SaveChanges();
                }
                Object response = new
                {
                    message = "  Created ",
                    code = HttpStatusCode.Created
                };
                return Content(JsonConvert.SerializeObject(response));

            }
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("api/[controller]/getCarPark")]
        public  async Task<IActionResult> ViewCarPark()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("https://api.data.gov.sg/v1/transport/carpark-availability");
                var jsonstring = await response.Content.ReadAsStringAsync();
                return Content(jsonstring);

            }

        }
        
    }
}
