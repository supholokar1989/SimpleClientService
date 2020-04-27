using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ClientService.Data.Queries;
using System.Linq;
using System.Collections.Generic;

namespace ClientService.API.Grpc
{
    public class ClientService : ClientApiRetrieval.ClientApiRetrievalBase
    {
        private readonly ILogger<ClientService> _logger;
        private readonly IClientQueries _clientQueries;

        public ClientService(ILogger<ClientService> logger, IClientQueries clientQueries)
        {
            _logger = logger;
            _clientQueries = clientQueries;
        }

        public override async Task<ModulesResponse> FindModulesByClientIdAndFacilityCode(ModulesRequest request, ServerCallContext context)
        {
            ClientFacility clientFacility = await _clientQueries.GetClientFacilityAndModules(request.ClientId, request.FacilityCode);
            if (clientFacility != null)
            {
                List<Modules> moduleList = new List<Modules>();
                foreach (Module m in clientFacility.modules)
                {
                    moduleList.Add(new Modules { FacilityId = m.FacilityId, ModuleCode = m.ModuleCode, ModuleDescription = m.ModuleDescription });
                }

                ModulesResponse response = new ModulesResponse { ClientId = clientFacility.ClientId,
                    ClientName = clientFacility.ClientName,
                    FacilityId = clientFacility.FacilityId,
                    FacilityCode = clientFacility.FacilityCode,
                    FacilityName = clientFacility.FacilityName,
                    Data = { moduleList}
                };
                
                return response;
            }

            context.Status = new Status(StatusCode.NotFound, $"Client with id {request.ClientId} and Facility with code {request.FacilityCode} does not exist");
            return null;
        }

    }
}
