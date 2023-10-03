using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.IO;
using VanitiProject.Models;

namespace VanitiProject.Controllers
{
    public class BeersController : ApiController
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IHttpClientWrapper _httpClientTest;
        private const string PunkApiEndpoint = "https://api.punkapi.com/v2/beers";

        public BeersController() { }

        public BeersController(IHttpClientWrapper httpClient)
        {
            _httpClientTest = httpClient;
        }

        public HttpClient Client { get; set; }

        /// <summary>
        /// Gets a list of beers based on the search parameter.
        /// </summary>
        /// <param name="search">The search term for filtering beers by name.</param>
        /// <returns>A list of beers matching the search.</returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetBeers(string search = "")
        {
            try
            {
                var Url = $"{PunkApiEndpoint}?beer_name={HttpUtility.UrlEncode(search)}";

                var response = await _httpClient.GetAsync(Url);

                if (response.IsSuccessStatusCode)
                {
                    var beers = await response.Content.ReadAsAsync<List<Beer>>();
                    return Ok(beers);
                }

                return InternalServerError(new Exception("Failed to get data from Punk API."));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Makes a request to the Punk API to retrieve beers based on the search parameter.
        /// </summary>
        /// <param name="search">The search term for filtering beers by name.</param>
        /// <returns>list of BeerApiResponse objects from the API.</returns>
        private async Task<List<BeerApiResponse>> GetBeersFromApi(string search)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.punkapi.com/v2/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"beers?beer_name={search}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<BeerApiResponse>>();
                }
                return new List<BeerApiResponse>();
            }
        }

        /// <summary>
        /// Searches for beers by name and adds user ratings from a database if exists.
        /// </summary>
        /// <param name="name">The name of the beer to search for.</param>
        /// <returns>List of beers with user ratings.</returns>
        [HttpGet]
        [Route("api/beers/search")]
        public async Task<IHttpActionResult> SearchBeersByName(string name)
        {
            try
            {
                // Get the data of the database.json file
                var databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "database.json");
                var ratings = new List<UserRatingWrapper>();
                if (File.Exists(databasePath))
                {
                    var existingJson = File.ReadAllText(databasePath);
                    ratings = JsonConvert.DeserializeObject<List<UserRatingWrapper>>(existingJson);
                }

                var beers = await GetBeersFromApi(name);

                var qResult = from beer in beers
                                  select new
                                  {
                                      id = beer.Id,
                                      name = beer.Name,
                                      description = beer.Description,
                                      userRatings = ratings
                                                   .Where(r => r.BeerId == beer.Id)
                                                   .Select(r => new
                                                   {
                                                       username = r.Rating.UserName,
                                                       rating = r.Rating.RatingValue,
                                                       comments = r.Rating.Comments
                                                   })
                                                   .ToList()
                                  };

                return Ok(qResult);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
