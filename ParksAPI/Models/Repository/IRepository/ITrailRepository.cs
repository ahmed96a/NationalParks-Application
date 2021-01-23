using ParksModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParksAPI.Models.Repository.IRepository
{
    // 6. Part 2
    // -----------------------------------

    public interface ITrailRepository
    {
        IEnumerable<Trail> GetTrails();

        IEnumerable<Trail> GetTrailsInNationalPark(int npId);

        Trail GetTrail(int trailId);

        bool TrailExists(string trailName);

        bool TrailExists(int trailId);

        bool CreateTrail(Trail trail);

        bool UpdateTrail(Trail trail);

        bool DeleteTrail(Trail trail);
    }

    // -----------------------------------
}
