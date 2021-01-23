using ParksModels.Dtos;
using ParksModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParksWeb.Repository.IRepository
{
    // 8. Part 8
    // ---------------------------------

    public interface ITrailRepository
    {
        Task<TrailDto> GetAsync(string Url, int Id, string token);

        Task<IEnumerable<TrailDto>> GetAllSync(string Url, string token);

        Task<bool> CreateAsync(string Url, TrailCreateDto objToCreate, string token);

        Task<bool> UpdateAsync(string Url, int trailId, TrailUpdateDto objToUpdate, string token);

        Task<bool> DeleteAsync(string Url, int Id, string token);
    }

    // ---------------------------------
}
