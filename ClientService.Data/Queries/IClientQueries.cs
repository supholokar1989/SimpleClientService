using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientService.Data.Queries
{
    public interface IClientQueries
    {
        Task<ClientFacility> GetClientFacilityAndModules(Int64 clientId, string facilityCode);
    }
}
