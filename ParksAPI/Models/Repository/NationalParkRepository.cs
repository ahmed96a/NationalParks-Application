using Microsoft.EntityFrameworkCore;
using ParksAPI.Models.Repository.IRepository;
using ParksModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParksAPI.Models.Repository
{
    // 3. Part 1
    // --------------------------------

    public class NationalParkRepository : INationalParkRepository
    {
        // 3. Part 2
        // ---------------------

        private readonly AppDbContext appDbContext;

        public NationalParkRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
            appDbContext.NationalParks.Add(nationalPark);
            return appDbContext.SaveChanges() > 0 ? true : false;
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            appDbContext.NationalParks.Remove(nationalPark);
            return appDbContext.SaveChanges() > 0 ? true : false;
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
            return appDbContext.NationalParks.FirstOrDefault(np => np.Id == nationalParkId);
        }

        public IEnumerable<NationalPark> GetNationalParks()
        {
            return appDbContext.NationalParks.ToList();
        }

        public bool NationalParkExists(string nationalParkName)
        {
            return appDbContext.NationalParks.Any(np => np.Name.ToLower().Trim() == nationalParkName.ToLower().Trim());
        }

        public bool NationalParkExists(int nationalParkId)
        {
            return appDbContext.NationalParks.Any(np => np.Id == nationalParkId);
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            appDbContext.NationalParks.Update(nationalPark);
            return appDbContext.SaveChanges() > 0 ? true : false;
        }

        // ---------------------
    }

    // --------------------------------
}
