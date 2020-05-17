using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ClientService.Data.Queries
{
    public class ClientQueries : IClientQueries
    {
        private string _connectionString = string.Empty;
        private string _cacheConnString = string.Empty;

        public ClientQueries(string constr, string cachContStr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
            _cacheConnString = !string.IsNullOrWhiteSpace(constr) ? cachContStr : throw new ArgumentNullException(nameof(cachContStr));
        }
        public async Task<ClientFacility> GetClientFacilityAndModules(Int64 clientId, string facilityCode)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_cacheConnString);
            IDatabase db = redis.GetDatabase();
            var var = db.StringGet("ClientFacilityModule" + clientId + facilityCode);
            if (!var.IsNull)
            {
                return JsonConvert.DeserializeObject<ClientFacility>(var);
            }

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
                    var results = connection.QueryMultiple(query, new { @clientId = clientId, @facilityCode = facilityCode });
                    var clientFacility = results.ReadSingle<ClientFacility>();
                    if (clientFacility == null)
                        throw new KeyNotFoundException();
                    clientFacility.modules = results.Read<Module>().ToList();

                    db.StringSet("ClientFacilityModle" + clientId + facilityCode, JsonConvert.SerializeObject(clientFacility), new TimeSpan(24,0,0));

                    return clientFacility;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                }
                return null;
                
            }
        }

        public async Task<ClientFacility> GetClientFacilityAndModulesByFacilityId(Int64 clientId, Int64 facilityId)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_cacheConnString);
            IDatabase db = redis.GetDatabase();
            var var = db.StringGet("ClientFacilityModule" + clientId + facilityId);
            if (!var.IsNull)
            {
                return JsonConvert.DeserializeObject<ClientFacility>(var);
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT ClientId, ClientName, FacilityId, FacilityCode, FacilityName FROM vw_ClientFacility WHERE ClientId = @clientId AND Facilitycode = @facilityCode;
                    select f.FacilityId, m.ModuleCode, m.[Description] as 'ModuleDescription'
                        FROM [dbo].[Facility] f
                        INNER JOIN [dbo].[FacilityModule] fm with (NOLOCK) ON f.facilityId = fm.facilityId
                        INNER JOIN [dbo].[Module] m with (NOLOCK) ON m.moduleId = fm.moduleid
                        WHERE f.ClientId = @clientId AND f.FacilityId = @facilityId";
                connection.Open();
                try
                {
                    var results = connection.QueryMultiple(query, new { @clientId = clientId, @facilityId = facilityId });
                    var clientFacility = results.ReadSingle<ClientFacility>();
                    if (clientFacility == null)
                        throw new KeyNotFoundException();
                    clientFacility.modules = results.Read<Module>().ToList();

                    db.StringSet("ClientFacilityModle" + clientId + facilityId, JsonConvert.SerializeObject(clientFacility), new TimeSpan(24, 0, 0));

                    return clientFacility;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                }
                return null;

            }
        }

        public async Task<IEnumerable<Facility>> GetFacilityByClientId(long clientId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<Facility>(
                   @"SELECT FacilityId, FacilityCode,FacilityName
                     FROM Facility where ClientID = @clientId"
                        , new { clientId }
                    );

                if (result.AsList().Count == 0)
                    throw new KeyNotFoundException();

                return result;
            }
        }
    }
}
