using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Revalsys.AddModule.RevalProperties
{
    /*
           * Author Name            :  Teja Nikhitha  
           * Create Date            :  26 Dec 2024
           * Modified Date          : 
           * Modified Reason        : 
           * Layer                  :  RevalProperties
           * Modified By            : 
           * Description            :  This class have the Data Transfer Objects to insert data.
       */
    public class ModuleListDTO
    {
        public string ModuleName { get; set; } = string.Empty;
        public string ModuleCode { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        public string PublishedBy { get; set; } = string.Empty;
        public DateTime DatePublished { get; set; }
        public bool DisplayOnWeb { get; set; }
        public int SortOrder { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; } = string.Empty;
        public DateTime DateDeleted { get; set; }
        public string Id { get; set; } = string.Empty;
    }

    public class ModuleListResponseDTO  
    {
        public string ModuleId { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public string ModuleCode { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public string ProjectId { get; set; } = string.Empty;
        public string IsPublished { get; set; } = string.Empty;
        public string PublishedBy { get; set; } = string.Empty;
        public string DatePublished { get; set; } = string.Empty;
        public string DisplayOnWeb { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string DateCreated { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public string LastUpdated { get; set; } = string.Empty;
        public string IsDeleted { get; set; } = string.Empty;
        public string DeletedBy { get; set; } = string.Empty;
        public string DateDeleted { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
    }
}
