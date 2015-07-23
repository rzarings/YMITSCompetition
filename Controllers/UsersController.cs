using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using YMITSDeployedWebsite.Models;
using YMITSDeployedWebsite.Models.JsonItems;

namespace YMITSDeployedWebsite.Controllers
{
    public class UsersController : ApiController
    {

        private static UserDTO thisUser;

        private string siteURL = @"https://ymitsmainwebsite.azurewebsites.net/api/Users";

        //status of the page, 0 is login, 1 is logged, 2 is submitted
        private static int status = 0;



        [HttpGet]
        [ActionName("getTeamUserNumber")]
        public async Task<String> getTeamUserNumber()
        {

            var usersInTeam = new UsersInTeam();

            string thisSiteUrl = siteURL + "/GetUsers";
            using (var client = new HttpClient())
            {
                // New code:
                client.BaseAddress = new Uri(thisSiteUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(thisSiteUrl+"?TeamName="+thisUser.TeamName);
                usersInTeam = await response.Content.ReadAsAsync<UsersInTeam>();

                // usersInTeam = JsonConvert.DeserializeObject<UsersInTeam>(e);
            }
               
            return JsonConvert.SerializeObject(usersInTeam);

        }

        [HttpGet]
        [ActionName("FinishContest")]
        public async Task<IHttpActionResult> FinishContest()
        {
            string thisSiteUrl = siteURL + "/FinishContest";

            using (var client = new HttpClient())
            {
                // New code:
                client.BaseAddress = new Uri(thisSiteUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(thisSiteUrl + "?myUser=" + thisUser.SubscriptionId);
                var responseCode = response.StatusCode.ToString();

                if (responseCode.CompareTo("OK") == 0)
                {
                    status = 2;
                    return Ok();
                }
                else
                {
                    JToken tmp = getErrorMessage(response);
                    return BadRequest(tmp.ToString());
                }                 
                }
           
        }

        [HttpGet]
        [ActionName("GetCurrentPageStatus")]
        public String GetCurrentPageStatus()
        {
            var pageStatus = new PageStatus();
            pageStatus.Status = status;
            return JsonConvert.SerializeObject(pageStatus);

        }

        [HttpPost]
        [ActionName("PostNewUser")]
        public async Task<IHttpActionResult> PostNewUser(UserDTO collection)
        {
            if (ModelState.IsValid||(ModelState.Count==1&&ModelState.ContainsKey("collection.TeamName")))
            {
                string thisSiteUrl = siteURL + "/PostUser";
                collection.SubscriptionId = System.Environment.GetEnvironmentVariable("WEBSITE_OWNER_NAME");
                using (var client = new HttpClient())
                {
                    // New code:
                    client.BaseAddress = new Uri(thisSiteUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsJsonAsync(thisSiteUrl, collection);
                    var responseCode = response.StatusCode.ToString();

                    if (responseCode.CompareTo("Created") == 0) //user has been created
                    {

                        
                        thisUser = await response.Content.ReadAsAsync<UserDTO>(); ;
                        status = 1;
                        return Ok();
                    }
                    else // there is here an error
                    {
                        JToken tmp = getErrorMessage(response);
                        return BadRequest(tmp.ToString());
                    }
                }
                //todo do an async
          
            }
            return BadRequest("an exception has occured, please retry later");
        }

        private static JToken getErrorMessage(HttpResponseMessage response)
        {
            return JValue.Parse(response.Content.ReadAsStringAsync().Result);
        }


        private int getNumbersOfGuysInTeam(string teamName)
        {
            string thisSiteUrl = siteURL + "/GetUsers";
            string address = string.Format(
            thisSiteUrl + "?TeamName={0}",
            Uri.EscapeDataString(teamName));
            WebClient webClient = new WebClient();
            try
            {
                string result = webClient.DownloadString(address);
                if (result.Length > 0)
                {
                    var JsonResult = JObject.Parse(result);
                    return (int)JsonResult["NumberUsersInTeam"];
                }
                else
                    return 0; //nobody in this team

            }
            catch (Exception ex)
            {
                return 0;
            }

            }

        [HttpGet]
        [ActionName("TeamFree")]
        public IHttpActionResult TeamFree(string teamName)
        {
            var tmp = getNumbersOfGuysInTeam(teamName);
            if (tmp> 0)
                return Ok(false);
            return Ok(true);

        }
    }
}
