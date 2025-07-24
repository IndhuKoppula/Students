using Azure;
using Microsoft.Extensions.Logging;
using Revalsys.AddModule.DAL;
using Revalsys.AddModule.RevalCommon;
using Revalsys.AddModule.RevalProperties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Revalsys.AddModule.BAL
{
    public class ModuleBAL
    {
        private readonly ConfigurationSettingsListDTO _ConfigurationSettingsListDTO;
        private readonly ILogger<General> _logger;
        public ModuleBAL(ConfigurationSettingsListDTO objConfigurationSettingsListDTO, ILogger<General> logger)
        {
            _ConfigurationSettingsListDTO = objConfigurationSettingsListDTO;
            _logger = logger;
        }
        //*********************************
        // Layer                        :   BAL Layer
        // Method Name                  :   InsertModuleDetailAsync
        // Method Description           :   This method is used to insert the Data into Module.
        // Author                       :   Teja Nikhitha
        // Creation Date                :   26 Dec 2024
        // Input Parameters             :   objModuleListDTO
        // Modified Date                : 
        // Modified Reason              :
        // Return Values                :   objResponse
        //----------------------------------------------------------------------------------------------------
        //  Version             Author                      Date                        Remarks       
        // ---------------------------------------------------------------------------------------------------
        //  1.0                  Nikhitha                  26 Dec 2024                 Creation
        // 1.1                   Nikhitha                  31 Dec 2024                 Get method creation for Project.
        //1.2                    Nikhitha                  04 Jan 2025                 Added mandatory fields and static values is passed.
        //**********************************
        // <summary>
        // <c>InsertModuleDetailAsync</c> This method is used to save the Data into Module.
        // <param>objModuleListResponseDTO</param>
        // <returns>objResponse</returns> //It returns the Message 
        // </summary>   

        public async Task<APIResponseListDTO> InsertModuleDetailAsync(ModuleListResponseDTO objModuleListResponseDTO)
        {
            APIResponseListDTO objApiResponse = new APIResponseListDTO();
            ModuleDAL objModuleDAL;
            string strModuleName = string.Empty;
            string strModuleCode = string.Empty;
            string strModuleId = string.Empty;
            bool IsActive = false;
            string ProjectId = string.Empty;
            object objresponse = 0;
            int ResponseId, code;
            int ErrorCode = 0;
            string strErrorMessage = string.Empty;
            ModuleListDTO objmoduleListDTO;
            string output = string.Empty;
            string strUpdatedBy = string.Empty;
            string Id;
            object ReturnValue = null;
            General objGeneral = new General(_logger);
            DataTable dt;
            try
            {
                #region Validation
                if (ErrorCode == 0)
                {
                    if (!string.IsNullOrEmpty(objModuleListResponseDTO.ModuleName))
                    {
                        strModuleName = objModuleListResponseDTO.ModuleName;
                    }
                    else
                    {
                        ErrorCode = Convert.ToInt32(General.ResponseCodes.ModuleNameRequired);
                        strErrorMessage = General.ReturnMessage.ModuleNameRequired;
                    }
                }

                if (ErrorCode == 0)
                {
                    if (!string.IsNullOrEmpty(objModuleListResponseDTO.ModuleCode))
                    {
                        strModuleCode = objModuleListResponseDTO.ModuleCode;
                    }
                    else
                    {
                        ErrorCode = Convert.ToInt32(General.ResponseCodes.ModuleCodeRequired);
                        strErrorMessage = General.ReturnMessage.ModuleCodeRequired;
                    }
                }
                if (ErrorCode == 0)
                {
                    if (!string.IsNullOrEmpty(objModuleListResponseDTO.ProjectId))
                    {
                        ProjectId = objModuleListResponseDTO.ProjectId;
                    }
                    else
                    {
                        ErrorCode = Convert.ToInt32(General.ResponseCodes.ProjectIdRequired);
                        strErrorMessage = General.ReturnMessage.ProjectIdRequired;
                    }
                }
                if (ErrorCode == 0)
                {
                    if (!string.IsNullOrEmpty(objModuleListResponseDTO.IsActive))
                    {
                        IsActive = Convert.ToBoolean(objModuleListResponseDTO.IsActive);

                    }
                    else
                    {
                        ErrorCode = Convert.ToInt32(General.ResponseCodes.IsActiveRequired);
                        strErrorMessage = General.ReturnMessage.IsActiveRequired;
                    }
                }
                #endregion

                #region Validation with MasterTable
                if (ErrorCode == 0)
                {

                    objModuleDAL = new ModuleDAL(_ConfigurationSettingsListDTO,_logger);
                    objGeneral.ILoggerInformation("InsertModuleDetailAsync", " before checking master table data");
                    objresponse = await objModuleDAL.CheckProjectExists(ProjectId);
                    ResponseId = Convert.ToInt32(objresponse);
                    objGeneral.ILoggerInformation("InsertModuleDetailAsync", " after checking master table data");
                    if (ResponseId > 0)
                    {
                        objmoduleListDTO = new ModuleListDTO();
                        objmoduleListDTO.ModuleName = strModuleName;
                        objmoduleListDTO.ModuleCode = strModuleCode;
                        objmoduleListDTO.ProjectId = ResponseId;
                        objmoduleListDTO.IsActive = IsActive;
                        strModuleId = objModuleListResponseDTO.ModuleId;
                        objmoduleListDTO.Id = strModuleId;
                        strUpdatedBy = objModuleListResponseDTO.UpdatedBy;
                        objmoduleListDTO.UpdatedBy = strUpdatedBy;
                        objGeneral.ILoggerInformation("InsertModuleDetailAsync", " before inserting data");
                        dt = await objModuleDAL.InsertModuleDataToDbAsync(objmoduleListDTO);
                        objGeneral.ILoggerInformation("InsertModuleDetailAsync", "  after inserting data");
                        //string strResponseId = (dt.Rows[0]["ReturnId"]).ToString();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            var DataList = new List<object>();
                            foreach (DataRow row in dt.Rows)
                            {
                                var DataDetails = new
                                {
                                    Id = row["Id"].ToString()
                                };
                                DataList.Add(DataDetails);
                            }
                            int Code = Convert.ToInt32(dt.Rows[0]["Code"]);
                            if (Code == 0
                                )
                            {
                                objApiResponse.ReturnCode = Convert.ToInt32(General.ResponseCodes.Success);
                                objApiResponse.ReturnMessage = General.ReturnMessage.Success;
                                objApiResponse.Data = DataList;
                            }
                            else
                            {

                                objApiResponse.ReturnCode = Convert.ToInt32(General.ResponseCodes.Updated);
                                objApiResponse.ReturnMessage = General.ReturnMessage.Updated;
                                objApiResponse.Data = DataList;
                            }                             
                        }
                        

                    }
                    else
                    {
                        ErrorCode = Convert.ToInt32(General.ResponseCodes.InvalidProjectId);
                        strErrorMessage = General.ReturnMessage.InvalidProjectId;
                    }

                }
                


                #endregion
                if (ErrorCode > 0)
                {
                    objApiResponse.ReturnCode = ErrorCode;
                    objApiResponse.ReturnMessage = strErrorMessage;
                }

            }
            catch (Exception ex)
            {
               // objGeneral.ILoggerError("Duplicate Data not allowed",  ex.TrackTrace);
                objGeneral.ILoggerError("InsertModuleDetailAsync", " Duplicate data not allowed");
                objApiResponse.ReturnCode = (int)General.ResponseCodes.DataAlreadyExisted;
                objApiResponse.ReturnMessage = General.ReturnMessage.DataAlreadyExisted;
            }
            return objApiResponse;
        }


        public async Task<APIResponseListDTO> GetModuleDetailsByIdAsync(string Id)
        {
            ModuleDAL objModuleDAL = null;
            DataTable dt = null;
            General objGeneral = new General(_logger);
            APIResponseListDTO objResponse = new APIResponseListDTO();
            try
            {
                objGeneral.ILoggerInformation("GetModuleDetailsByIdAsync", " before Getting data");
                objModuleDAL = new ModuleDAL(_ConfigurationSettingsListDTO,_logger);
                dt = await objModuleDAL.GetModuleDetailsByIdfromDbAsync(Id);
                objGeneral.ILoggerInformation("GetModuleDetailsByIdAsync", " after Getting data");
                int code = Convert.ToInt32(dt.Rows[0]["ReturnCode"]);
                if (code > 0)
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        var ModuleList = new List<object>();
                        foreach (DataRow row in dt.Rows)
                        {
                            var ModuleDetails = new
                            {
                                ModuleName = row["ModuleName"].ToString(),
                                ModuleCode = row["ModuleCode"].ToString(),
                                ProjectId = row["ProjectId"],
                                IsActive = row["IsActive"]
                            };
                            ModuleList.Add(ModuleDetails);
                        }
                        objResponse.ReturnCode = Convert.ToInt32(General.ResponseCodes.Success);
                        objResponse.ReturnMessage = General.ReturnMessage.DataFetched;
                        objResponse.Data = ModuleList;
                    }
                }
                else
                {
                    objResponse.ReturnCode = Convert.ToInt32(General.ResponseCodes.NoDataFound);
                    objResponse.ReturnMessage = General.ReturnMessage.NoDataFound;
                }
            }
            catch (Exception ex)
            {
                objResponse.ReturnCode = (int)General.ResponseCodes.InternalError;
                objResponse.ReturnMessage = $"{General.ReturnMessage.InternalError}{ex.Message}";
                objResponse.Data = new List<object>();
            }
            return objResponse;

        }


        public async Task<APIResponseListDTO> DeleteModuleDetailsByIdAsync(ModuleListResponseDTO objModuleListResponseDTO)
        {
            string strReturnMessage = string.Empty;
            string strDeletedBy = string.Empty;
            string strId = string.Empty;
            ModuleDAL objModuleDAL = null;
            object Response;
            ModuleListDTO objmoduleListDTO = new ModuleListDTO();
            int ErrorCode = 0;
            APIResponseListDTO objResponse = new APIResponseListDTO();
            General objGeneral = new General(_logger);
            try
            {
                if (ErrorCode == 0)
                {

                    if (!string.IsNullOrEmpty(objModuleListResponseDTO.Id))
                    {
                        strId = objModuleListResponseDTO.Id;
                    }
                    else
                    {
                        ErrorCode = Convert.ToInt32(General.ResponseCodes.IdRequired);
                        strReturnMessage = General.ReturnMessage.IdRequired;
                    }
                }
                if (ErrorCode == 0)
                {
                    if (!string.IsNullOrEmpty(objModuleListResponseDTO.DeletedBy))
                    {
                        strDeletedBy = objModuleListResponseDTO.DeletedBy;
                    }
                    else
                    {
                        ErrorCode = Convert.ToInt32(General.ResponseCodes.DeletedByRequired);
                        strReturnMessage = General.ReturnMessage.DeletedByRequired;
                    }
                }
                if (ErrorCode == 0)
                {
                    objmoduleListDTO.DateDeleted = DateTime.Now;
                    objmoduleListDTO.DeletedBy = strDeletedBy;
                    objmoduleListDTO.Id = strId;
                    objGeneral.ILoggerInformation("DeleteModuleDetailsByIdAsync", " before Deleting data");
                    objModuleDAL = new ModuleDAL(_ConfigurationSettingsListDTO,_logger);
                    Response = await objModuleDAL.DeleteModuleDetailsByIdfromDbAsync(objmoduleListDTO);
                    objGeneral.ILoggerInformation("DeleteModuleDetailsByIdAsync", " After Deleting data");
                    int ResponseId = Convert.ToInt32(Response);

                    if (ResponseId == 0)
                    {
                        objResponse.ReturnCode = Convert.ToInt32(General.ResponseCodes.Success);
                        objResponse.ReturnMessage = General.ReturnMessage.Success;
                    }
                    else
                    {
                        objResponse.ReturnCode = Convert.ToInt32(General.ResponseCodes.NoDataFound);
                        objResponse.ReturnMessage = General.ReturnMessage.NoDataFound;
                    }
                }
                if (ErrorCode > 0)
                {
                    objResponse.ReturnCode = ErrorCode;
                    objResponse.ReturnMessage = strReturnMessage;
                }
            }


            catch (Exception ex)
            {
                objResponse.ReturnCode = (int)General.ResponseCodes.InternalError;
                objResponse.ReturnMessage = $"{General.ReturnMessage.InternalError}{ex.Message}";
                objResponse.Data = new List<object>();
            }
            return objResponse;
        }




        //*********************************
        // Layer                        :   BAL Layer
        // Method Name                  :   GetProjectDetails
        // Method Description           :   This method is used to Get the Data from Master table.
        // Author                       :   Teja Nikhitha
        // Creation Date                :   26 Dec 2024
        // Input Parameters             :   
        // Modified Date                : 
        // Modified Reason              :
        // Return Values                :   objResponse
        //----------------------------------------------------------------------------------------------------
        //  Version             Author                      Date                        Remarks       
        // ---------------------------------------------------------------------------------------------------
        //  1.0                 Teja Nikhitha                  31 Dec 2024                 Creation
        //**********************************
        // <summary>
        // <c>GetProjectDetailAsync</c> This method is used to Get the Data from Master Table.
        // <param></param>
        // <returns>objResponse</returns> //It returns the Message 
        // </summary>   

        public async Task<APIResponseListDTO> GetProjectDetailsAsync()
        {
            ModuleDAL objModuleDAL = null;
            DataTable dt = null;
            APIResponseListDTO objResponse = new APIResponseListDTO();
            General objGeneral = new General(_logger);
            try
            {
                objGeneral.ILoggerInformation("GetProjectDetailsAsync", " before getting project details data");
                objModuleDAL = new ModuleDAL(_ConfigurationSettingsListDTO,_logger);
                dt = await objModuleDAL.GetProjectDetailsfromDbAsync();
                objGeneral.ILoggerInformation("GetProjectDetailsAsync", " After getting project details data");

                if (dt != null && dt.Rows.Count > 0)
                {
                    var ProjectList = new List<object>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var ProjectDetails = new
                        {
                            ProjectName = row["ProjectName"].ToString(),
                            Id = row["Id"].ToString()
                        };
                        ProjectList.Add(ProjectDetails);
                    }
                    objResponse.ReturnCode = Convert.ToInt32(General.ResponseCodes.Success);
                    objResponse.ReturnMessage = General.ReturnMessage.DataFetched;
                    objResponse.Data = ProjectList;
                }
                else
                {
                    objResponse.ReturnCode = Convert.ToInt32(General.ResponseCodes.Failure);
                    objResponse.ReturnMessage = General.ReturnMessage.NoDataFound;
                }
            }
            catch (Exception ex)
            {
                objResponse.ReturnCode = Convert.ToInt32(General.ResponseCodes.InternalError);
                objResponse.ReturnMessage = $"{General.ReturnMessage.InternalError}{ex.Message}";
                objResponse.Data = new List<object>();
            }
            return objResponse;
        }
    }
}



