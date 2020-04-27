using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientService.Data.Queries
{
    public interface IClientQueries
    {
        Task<ClientFacility> GetClientFacilityAndModules(int clientId, string facilityCode);
    }
}
