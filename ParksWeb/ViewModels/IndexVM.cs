using ParksModels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParksWeb.ViewModels
{
    // 11. Part 2
    // -------------------

    public class IndexVM
    {
        public IEnumerable<NationalParkDto> NationalParkList { get; set; }

        public IEnumerable<TrailDto> TrailList { get; set; }
    }

    // -------------------
}
