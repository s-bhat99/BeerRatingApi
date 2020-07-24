using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApplication1.DTO
{
    public class BeerDetails
    {
        public int Id { get; set; }

        [JsonIgnore]
        public List<UserRating> UserRatings { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
