using Microsoft.EntityFrameworkCore;
using ParksAPI.Models.Repository.IRepository;
using ParksModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParksAPI.Models.Repository
{
    // 6. Part 2
    // ---------------------

    public class TrailRepository: ITrailRepository
    {
        private readonly AppDbContext appDbContext;

        public TrailRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public bool CreateTrail(Trail trail)
        {
            // Test
            /*
            try
            {
                appDbContext.Trails.Add(trail);
                return appDbContext.SaveChanges() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                // log exception
                return false;
            }
            */

            appDbContext.Trails.Add(trail);
            return appDbContext.SaveChanges() > 0 ? true : false;
        }

        public bool DeleteTrail(Trail trail)
        {
            appDbContext.Trails.Remove(trail);
            return appDbContext.SaveChanges() > 0 ? true : false;
        }

        public Trail GetTrail(int trailId)
        {
            return appDbContext.Trails.Include(t => t.NationalPark).SingleOrDefault(t => t.Id == trailId);
        }

        public IEnumerable<Trail> GetTrails()
        {
            return appDbContext.Trails.Include(t => t.NationalPark).ToList();
        }

        public IEnumerable<Trail> GetTrailsInNationalPark(int npId)
        {
            // if NationalPark not exist then return null
            if (!appDbContext.NationalParks.Any(np => np.Id == npId))
            {
                return null;
            }

            return appDbContext.Trails.Include(t => t.NationalPark).Where(t => t.NationalParkId == npId).ToList();
        }

        public bool TrailExists(string trailName)
        {
            return appDbContext.Trails.Any(t => t.Name.ToLower().Trim() == trailName.ToLower().Trim());
        }

        public bool TrailExists(int trailId)
        {
            return appDbContext.Trails.Any(t => t.Id == trailId);
        }

        public bool UpdateTrail(Trail trail)
        {
            appDbContext.Trails.Update(trail);
            return appDbContext.SaveChanges() > 0 ? true : false;
        }
    }

    // ---------------------
}
