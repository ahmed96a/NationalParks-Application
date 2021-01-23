using ParksModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParksAPI.Models.Repository.IRepository
{
    // 3. Part 1
    // --------------------------------

    public interface INationalParkRepository
    {
        IEnumerable<NationalPark> GetNationalParks();

        NationalPark GetNationalPark(int nationalParkId);

        bool NationalParkExists(string nationalParkName);

        bool NationalParkExists(int nationalParkId);

        bool CreateNationalPark(NationalPark nationalPark);

        bool UpdateNationalPark(NationalPark nationalPark);

        bool DeleteNationalPark(NationalPark nationalPark);
    }

    // --------------------------------
}
