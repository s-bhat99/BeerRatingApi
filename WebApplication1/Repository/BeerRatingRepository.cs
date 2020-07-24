using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using WebApplication1.DTO;
using System.Net.Http;

namespace WebApplication1.Repository
{
    public class BeerRatingRepository : IBeerRatingRepository
    {
        HttpClient httpClient = new HttpClient();
        string beerApi = "https://api.punkapi.com/v2/beers";
        string jsonFilePath = @"..\ratings.json";

        public async Task SaveRatings(int id, UserRating userRating)
        {
            userRating.Id = id;
            var jsonString = File.ReadAllText(jsonFilePath);
            List<UserRating> userRatings = new List<UserRating>();
            if (jsonString != string.Empty)
                userRatings = JsonSerializer.Deserialize<List<UserRating>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            userRatings.Add(userRating);
            using (FileStream file = File.Create(jsonFilePath))
            {
               await JsonSerializer.SerializeAsync(file, userRatings);
            }
        }
        
        public async Task<IEnumerable<BeerDetails>> GetBeerDetails(string name)
        {
            HttpResponseMessage response = await httpClient.GetAsync(string.Concat(beerApi, "?beer_name=",name));
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string jsonString = null;
            if (File.Exists(jsonFilePath))
                jsonString = File.ReadAllText(jsonFilePath);
            List<UserRating> userRatings = new List<UserRating>();
            if (! string.IsNullOrEmpty(jsonString))
                userRatings = JsonSerializer.Deserialize<List<UserRating>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


            var reply = await response.Content.ReadAsStringAsync();
            var beerDetails = JsonSerializer.Deserialize<List<BeerDetails>>(reply, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (beerDetails != null)
            {
                foreach(var item in userRatings)
                {
                    var beerExists = (from beerDetail in beerDetails
                     where beerDetail.Id == item.Id
                     select beerDetail).FirstOrDefault();
                    if (beerExists != null)
                    {
                        if (beerExists.UserRatings == null)
                            beerExists.UserRatings = new List<UserRating>() { item };
                        else
                            beerExists.UserRatings.Add(item);
                    }
                }
            }
            return beerDetails;
        }

        public async Task<bool> IsValidId(int id)
        {
            HttpResponseMessage response = await httpClient.GetAsync(string.Concat(beerApi, "/", id));
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
    }
}
