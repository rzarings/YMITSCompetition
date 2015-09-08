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
        public async Task<IHttpActionResult> getTeamUserNumber()
        {

            UsersInTeam usersInTeam;

            string thisSiteUrl = siteURL + "/getTeamUserNumber";
            using (var client = new HttpClient())
            {
                // New code:
                client.BaseAddress = new Uri(thisSiteUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
             
                    var response = await client.GetAsync(thisSiteUrl + "?subscriptionId=" + thisUser.SubscriptionId);
                var responseCode = response.StatusCode.ToString();

                if (responseCode.CompareTo("OK") == 0)
                {
                    usersInTeam = await response.Content.ReadAsAsync<UsersInTeam>();
                    return Ok(JsonConvert.SerializeObject(usersInTeam));

                }
                else
                {
                    JToken tmp = getErrorMessage(response);
                    return BadRequest(tmp.ToString());
                }


            }
               
           

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
        public async Task<IHttpActionResult> GetCurrentPageStatus()
        {
          
                string thisSiteUrl = siteURL + "/GetStatusPage";
                using (var client = new HttpClient())
                {
                    // New code:
                    client.BaseAddress = new Uri(thisSiteUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    var response = await client.GetAsync(thisSiteUrl + "?subscriptionId=" + "{" + Environment.GetEnvironmentVariable("WEBSITE_OWNER_NAME").Substring(0, 36) + "}");
                    var responseCode = response.StatusCode.ToString();
                    PageStatus pageStatus;
                    if (responseCode.CompareTo("OK") == 0)
                    {
                        pageStatus = await response.Content.ReadAsAsync<PageStatus>();
                        status = pageStatus.Status;
                        thisUser = pageStatus.user;
                        return Ok(JsonConvert.SerializeObject(pageStatus));

                    }
                    else
                    {
                        JToken tmp = getErrorMessage(response);
                        return BadRequest(tmp.ToString());
                    }

                
            }
    
        }

        [HttpPost]
        [ActionName("PostNewUser")]
        public async Task<IHttpActionResult> PostNewUser(UserDTO collection)
        {
            if (ModelState.IsValid||(ModelState.Count==1&&ModelState.ContainsKey("collection.TeamName")))
            {
                string thisSiteUrl = siteURL + "/PostUser";
                collection.SubscriptionId = "{"+ Environment.GetEnvironmentVariable("WEBSITE_OWNER_NAME").Substring(0,36)+"}";
                using (var client = new HttpClient())
                {
                    // New code:
                    client.BaseAddress = new Uri(thisSiteUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //enable in test case
                  /*  if (collection.SubscriptionId == null) {
                        Random random = new Random();
                        int randomNumber = random.Next(0, 100);
                        collection.SubscriptionId = randomNumber.ToString(); ;
                    }*/
                    
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
                    return Int32.Parse(result);
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
