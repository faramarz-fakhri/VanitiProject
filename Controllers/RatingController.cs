using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using VanitiProject.Models;

namespace VanitiProject.Controllers
{
    /// <summary>
    /// Controller for handling ratings.
    /// </summary>
    public class RatingController : ApiController
    {
        private readonly IHttpClientWrapper _httpClient;

        public HttpClient Client { get; set; }

        public RatingController(IHttpClientWrapper httpClient)
        {
            _httpClient = httpClient;
        }

        public RatingController() { }

        /// <summary>
        /// Adds a rating for a specific beer.
        /// </summary>
        /// <param name="id">The ID of the beer to rate.</param>
        /// <param name="rating">The rating details including username, rating value, and comments.</param>
        /// <returns>An HTTP action result showing the status of the rating operation.</returns>
        [HttpPost]
        [Route("api/ratings/{id:int}")]
        [UsernameValidation]
        public async Task<IHttpActionResult> AddRating(int id, [FromBody] Rating rating)
        {
            // Make a request to the Punk API to check if the beer exists
            var beer = await GetBeerByIdAsync(id);

            if (beer == null)
            {
                return BadRequest("Invalid beer id.");
            }

            // Validate rating value (1 to 5)
            var ratingValidator = new RatingValidator();
            var res = ratingValidator.Validate(rating);
            if (!res.IsValid)
            {
                return BadRequest(res.Errors.FirstOrDefault()?.ErrorMessage);
            }

            // Add rating to database.json + beerId
            var databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "database.json");
            var ratings = new List<object>();

            if (File.Exists(databasePath))
            {
                var existingJson = File.ReadAllText(databasePath);
                ratings = JsonConvert.DeserializeObject<List<object>>(existingJson);
            }

            var userRating = new
            {
                UserName = rating.UserName,
                RatingValue = rating.RatingValue,
                Comments = rating.Comments
            };

            var ratingData = new
            {
                BeerId = id,
                Rating = userRating,
            };

            ratings.Add(ratingData);
            File.WriteAllText(databasePath, JsonConvert.SerializeObject(ratings, Formatting.Indented));

            return Ok("Rating added successfully.");
        }

        /// <summary>
        /// Fetches beer data by its unique identifier from the Punk API, asynchronously .
        /// </summary>
        /// <param name="id">The ID of the beer.</param>
        /// <returns>An object containing data of the requested beer if found,
        /// or null if does not exist.
        /// </returns>
        private async Task<Beer> GetBeerByIdAsync(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.punkapi.com/v2/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"beers/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var beers = await response.Content.ReadAsAsync<List<Beer>>();
                    return beers.FirstOrDefault();
                }

                return null;
            }
        }

    }
}
