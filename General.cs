using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revalsys.AddModule.RevalCommon
{
    public class General
    {
        /*
            * Author Name            :  Teja Nikhitha  
            * Create Date            :  26 Dec 2024
            * Modified Date          : 
            * Modified Reason        : 
            * Layer                  :  RevalCommon
            * Modified By            : 
            * Description            :  This class have the General ReturnCodes and ReturnMessages.
        */

        public readonly ILogger<General> _logger;
        public General(ILogger<General> logger)
        {
            _logger = logger;
        }

        public enum ResponseCodes
        {
            Success = 0,
            Failure = 1,
            ModuleNameRequired=2,
            ModuleCodeRequired = 3,
            InvalidProjectId=4,
            ProjectIdRequired=5,
            DataAlreadyExisted=6,
            IsActiveRequired=7,           
            AlreadyDeleted=8,
            IdRequired=9,
            DeletedByRequired=10,
            DataFetched=11,
            InternalError=12,
            NoDataFound = 13,
            Updated=14
        }
        public static class ReturnMessage
        {
            public const string Success = "Success";
            public const string Failure = "Failure";
            public const string Updated = "Updated";
            public const string ModuleNameRequired = "Module Name Required";
            public const string ModuleCodeRequired = "Module Code Required";
            public const string InvalidProjectId = "Invalid ProjectId";
            public const string ProjectIdRequired = "ProjectId Required";
            public const string DataAlreadyExisted = "Data Already exists";
            public const string IsActiveRequired = "IsActive Required";
            public const string DataFetched = "Data Fetched Successfully";           
            public const string AlreadyDeleted = "Already Deleted";
            public const string IdRequired = "Id Required ";
            public const string DeletedByRequired = "Mandatory field";
            public const string InternalError = "Error occur while performing the operation ";
            public const string NoDataFound = "Data was not Found";

        }

        public void ILoggerInformation(string MethodName,string Message)
        {
            _logger.LogInformation($"Method Name:{MethodName},Message:{Message}");
        }

        public void ILoggerError(string MethodName,string ExceptionMessage)
        {
            _logger.LogError ($"Method Name:{MethodName},Exception Message:{ExceptionMessage}");
        }

    }
}
