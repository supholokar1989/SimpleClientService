using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientService.Data.Queries
{
    public interface IClientQueries
    {
        Task<ClientFacility> GetClientFacilityAndModules(Int64 clientId, string facilityCode);
        Task<ClientFacility> GetClientFacilityAndModulesByFacilityId(Int64 clientId, Int64 facilityId);
        Task<IEnumerable<Facility>> GetFacilityByClientId(Int64 clientId);
    }
}
