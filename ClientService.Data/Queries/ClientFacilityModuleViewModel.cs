using System;
using System.Collections.Generic;
using System.Text;

namespace ClientService.Data.Queries
{
    public class ClientFacility
    {
        public Int64 ClientId { get; set; }
        public string ClientName { get; set; }
        public Int64 FacilityId { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        
        public List<Module> modules { get; set; }
    }

    public class Module
    {
        public Int64 FacilityId { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleDescription { get; set; }
    }

    public class Facility
    {
        public Int64 FacilityId { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
    }
}
