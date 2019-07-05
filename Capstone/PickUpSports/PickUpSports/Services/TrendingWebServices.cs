using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.DynamicData;
using PickUpSports.Models.ViewModel.TrendingController;
using Newtonsoft.Json;
using PickUpSports.Models.ViewModel;
using static System.Int32;

namespace PickUpSports.Services
{

   public class TrendingWebServices
   {
        public async Task<T>InvokeRequestResponseService<T>(string sportName) where T : class
        {
            using (var client = new HttpClient())
            {

                var trendingValues = new[]
                {
                    new TrendingModel
                    {
                        VenueName = null,
                        SportName = sportName
                    }

                };

                var propertyCount = typeof(TrendingModel).GetProperties().Count();
                var convertValues = new string[trendingValues.Count(), propertyCount];
                for (var i = 0; i < convertValues.GetLength(0); i++)
                {

                    convertValues[i, 0] = trendingValues[i].VenueName;
                    convertValues[i, 1] = trendingValues[i].SportName;
                    
                    
                }

                
                var scoreRequest = new
                {

                    Inputs = new Dictionary<string, TrendingTable>() {
                        {
                            "input1",
                            new TrendingTable()
                            {
                                ColumnNames = new string[] {"Venue", "Sport"},
                                Values = convertValues
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                const string apiKey = ""; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/7a83674e46da4f92abbebab4311ca309/services/c2acc3251d8f42849896d7f1158c2d48/execute?api-version=2.0&details=true");

                // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)


                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var resultOutcome = JsonConvert.DeserializeObject<T>(result);
                    Debug.WriteLine("Result: {0}", result);
                    return resultOutcome;
                }
                else
                {
                    Debug.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                    Debug.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    Debug.WriteLine(responseContent);
                    return null;
                }
            }
        }
    }
}

