using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParksModels.Dtos;
using ParksModels.Models;

namespace ParksWeb.Repository.IRepository
{
    // 8. Part 8
    // ------------------------------------

    public interface INationalParkRepository
    {
        Task<NationalParkDto> GetAsync(string Url, int Id, string token);

        Task<IEnumerable<NationalParkDto>> GetAllSync(string Url, string token);

        Task<bool> CreateAsync(string Url, NationalParkCreateDto objToCreate, string token);

        Task<bool> UpdateAsync(string Url, int nationalParkId, NationalParkUpdateDto objToUpdate, string token);

        Task<bool> DeleteAsync(string Url, int Id, string token);
    }

    // ------------------------------------
}
