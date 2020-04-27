using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Data.Queries
{
    public class ClientQueries : IClientQueries
    {
        private string _connectionString = string.Empty;

        public ClientQueries(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }
        public async Task<ClientFacility> GetClientFacilityAndModules(int clientId, string facilityCode)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT ClientId, ClientName, FacilityId, FacilityCode, FacilityName FROM vw_ClientFacility WHERE ClientId = @clientId AND Facilitycode = @facilityCode;
                    select f.FacilityId, m.ModuleCode, m.[Description] as 'ModuleDescription'
                        FROM [dbo].[Facility] f
                        INNER JOIN [dbo].[FacilityModule] fm with (NOLOCK) ON f.facilityId = fm.facilityId
                        INNER JOIN [dbo].[Module] m with (NOLOCK) ON m.moduleId = fm.moduleid
                        WHERE f.ClientId = @clientId AND f.FacilityCode = @facilityCode";
                connection.Open();
                try { 
                    var results = connection.QueryMultiple(query, new { @clientId = clientId, facilityCode = facilityCode });
                    var clientFacility = results.ReadSingle<ClientFacility>();
                    if (clientFacility == null)
                        throw new KeyNotFoundException();
                    clientFacility.modules = results.Read<Module>().ToList();

                    return clientFacility;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                }
                return null;
                
            }
        }
    }
}
