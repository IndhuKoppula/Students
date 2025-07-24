using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Revalsys.AddModule.RevalCommon;
using Revalsys.AddModule.RevalProperties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revalsys.AddModule.DAL
{
    public class ModuleDAL
    {
        public string strConnectionString { get; set; } = string.Empty;
        private readonly ILogger<General> _logger;
        public ModuleDAL(ConfigurationSettingsListDTO objConfigurationSettingsListDTO, ILogger<General> logger)
        {
            strConnectionString = objConfigurationSettingsListDTO.ConnectionString;
            _logger = logger;
        }
        //*********************************
        // Layer                        :   DAL Layer
        // Method Name                  :   GetProjectDetailsfromDbAsync
        // Method Description           :   This method is used to Get the Data .
        // Author                       :   Teja Nikhitha
        // Creation Date                :   26 Dec 2024           
        // Input Parameters             :   
        // Modified Date                : 
        // Modified Reason              :
        // Return Values                :   dt                   
        //----------------------------------------------------------------------------------------------------
        //  Version             Author                      Date                        Remarks           
        // ---------------------------------------------------------------------------------------------------
        //  1.0                 Teja Nikhitha                  31 Dec 2024                 Creation
        //**********************************
        // <summary>
        // <c>GetProjectDetailsfromDbAsync</c> This method is used to Get the Data .
        // <param>GUID</param>
        // <returns>result</returns> //It returns the GUID 
        // </summary>
        public async Task<DataTable> GetProjectDetailsfromDbAsync()
        {
            
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(strConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand())
                {
                    sqlCmd.Connection = connection;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = @"dbo.usp_Module_GetRevalModule";              
                    SqlDataAdapter sda = new SqlDataAdapter(sqlCmd);
                    sda.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                }
            }
            return dt;
        }

        //*********************************
        // Layer                        :   DAL Layer
        // Method Name                  :   CheckProjectExists
        // Method Description           :   This method is used to check whether the Data present or not.
        // Author                       :   Teja Nikhitha
        // Creation Date                :   26 Dec 2024
        // Input Parameters             :   GUID
        // Modified Date                : 
        // Modified Reason              :
        // Return Values                :   result
        //----------------------------------------------------------------------------------------------------
        //  Version             Author                      Date                        Remarks       
        // ---------------------------------------------------------------------------------------------------
        //  1.0                 Nikhitha                  26 Dec 2024                 Creation              
        //**********************************
        // <summary>
        // <c>CheckProjectExists</c> This method is used to check whether the Data present or not.
        // <param>GUID</param>
        // <returns>result</returns> //It returns the Id 
        // </summary>


        public async Task<object> CheckProjectExists(string ID)
        {
            General objGeneral = new General(_logger);
            object result;
            using ( SqlConnection connection = new SqlConnection(strConnectionString))
            {
                connection.Open();
                using (SqlCommand sqlCmd = new SqlCommand())
                {
                    sqlCmd.Connection = connection;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = @"uspCheckProjectId";
                    sqlCmd.Parameters.Add("ProjectGuId", SqlDbType.NVarChar).Value = ID;
                    objGeneral.ILoggerInformation("CheckProjectExists", " before checking master table data");
                    result = sqlCmd.ExecuteScalar();
                    objGeneral.ILoggerInformation("CheckProjectExists", " before checking master table data");
                }
            }
            return result;
        }

        //*********************************
        // Layer                        :   DAL Layer
        // Method Name                  :   InsertModuleDataToDbAsync
        // Method Description           :   This method is used to insert the Data .
        // Author                       :   Teja Nikhitha
        // Creation Date                :   26 Dec 2024
        // Input Parameters             :   objModuleListDTO
        // Modified Date                : 
        // Modified Reason              :
        // Return Values                :   GUID
        //----------------------------------------------------------------------------------------------------
        //  Version             Author                      Date                        Remarks       
        // ---------------------------------------------------------------------------------------------------
        //  1.0                  Nikhitha                  31 Dec 2024                 Creation
        //  1.1                 Nikhitha                  02 Jan 2025                 By using GUID inserting the mastertable fields
        // 1.2                 Nikhitha                   03  Jan 2024                Adding the Mandatory fields
        //1.3                 Nikhitha                    04 Dec 2024                 Creating the new SP for mandatory fields with condition. 
        //**********************************
        // <summary>
        // <c>InsertModuleDataToDbAsync</c> This method is used to insert the Data .
        // <param>GUID</param>
        // <returns>result</returns> //It returns the GUID 
        // </summary>       
        public async Task<DataTable> InsertModuleDataToDbAsync(ModuleListDTO objModuleListDTO)
        {
            DataTable dt = new DataTable();
            General objGeneral=new General(_logger);
            using (SqlConnection connection = new SqlConnection(strConnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand sqlCmd = new SqlCommand("usp_tblModule_SaveModuleData", connection))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    // Adding parameters for the combined procedure                   
                    sqlCmd.Parameters.Add("@ModuleName", SqlDbType.NVarChar).Value = objModuleListDTO.ModuleName;
                    sqlCmd.Parameters.Add("@ModuleCode", SqlDbType.NVarChar).Value = objModuleListDTO.ModuleCode;
                    sqlCmd.Parameters.Add("@ProjectId", SqlDbType.Int).Value = objModuleListDTO.ProjectId; 
                    sqlCmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = objModuleListDTO.IsActive;
                    sqlCmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = objModuleListDTO.UpdatedBy;
                    sqlCmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = objModuleListDTO.Id;
                    objGeneral.ILoggerInformation("InsertModuleDataToDbAsync", " before inserting data");
                    SqlDataAdapter sda = new SqlDataAdapter(sqlCmd);
                    objGeneral.ILoggerInformation("InsertModuleDataToDbAsync", " After inserting data");
                    sda.Fill(dt);
                    if(dt!=null && dt.Rows.Count>0)
                    {
                        return dt;
                    }
                   
                }
            }
            return dt;
        }
        public async Task<DataTable> GetModuleDetailsByIdfromDbAsync(string Id)
        {
            DataTable dt = new DataTable();
            General objGeneral = new General(_logger);
            using (SqlConnection connection = new SqlConnection(strConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand())
                {
                    sqlCmd.Connection = connection;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = @"[dbo].[usp_Module_GetModuleById]";
                    objGeneral.ILoggerInformation("GetModuleDetailsByIdfromDbAsync", " before getting data");
                    sqlCmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id;
                    SqlDataAdapter sda = new SqlDataAdapter(sqlCmd);
                    objGeneral.ILoggerInformation("GetModuleDetailsByIdfromDbAsync", " after getting data");
                    sda.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                }
            }
            return dt;
        }

        public async Task<object> DeleteModuleDetailsByIdfromDbAsync(ModuleListDTO objModuleListDTO)
        {
            object result;
            General objGeneral = new General(_logger);
            using (SqlConnection connection = new SqlConnection(strConnectionString))
            {
                connection.Open();
                using (SqlCommand sqlCmd = new SqlCommand())
                {
                    sqlCmd.Connection = connection;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = @"[dbo].[usp_Module_DeleteModuleById]";
                    sqlCmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = objModuleListDTO.Id;
                    sqlCmd.Parameters.Add("@DeletedBy", SqlDbType.NVarChar).Value = objModuleListDTO.DeletedBy;
                    sqlCmd.Parameters.Add("@DateDeleted", SqlDbType.DateTime2).Value = objModuleListDTO.DateDeleted;
                    objGeneral.ILoggerInformation("DeleteModuleDetailsByIdfromDbAsync", "Before Deleting Data");
                    result = sqlCmd.ExecuteScalar();
                    objGeneral.ILoggerInformation("DeleteModuleDetailsByIdfromDbAsync", "After Deleting Data");
                }
            }
            return result;
        }
    }
}

