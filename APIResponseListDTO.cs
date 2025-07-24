using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revalsys.AddModule.RevalProperties
{
    /*
            * Author Name            :  Teja Nikhitha  
            * Create Date            :  26 Dec 2024
            * Modified Date          : 
            * Modified Reason        : 
            * Layer                  :  RevalProperties
            * Modified By            : 
            * Description            :  This class have the Data Transfer Objects to get response.
        */
    public class APIResponseListDTO
    {
        public int ReturnCode { get; set; }
        public string ReturnMessage { get; set; } = string.Empty;       
        public List<object> Data { get; set; }     
    }
}
