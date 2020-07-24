using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTO;

namespace WebApplication1.Repository
{
    public interface IBeerRatingRepository
    {
        Task SaveRatings(int id, UserRating userRating);

        Task<bool> IsValidId(int id);

        Task<IEnumerable<BeerDetails>> GetBeerDetails(string name);
    }
}
