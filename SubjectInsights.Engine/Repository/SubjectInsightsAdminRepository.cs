using Dapper;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using SubjectInsights.Logger;
using SubjectInsights.Application.Interfaces.Repositories;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using SubjectInsights.Application.Entities;
using System.Text;

namespace SubjectInsights.Infrastructure.Repository
{
    public class SubjectInsightsAdminRepository : ISubjectInsightsAdminRepository
    {
        private readonly IConfiguration _configuration;
        public SubjectInsightsAdminRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<SubjectInsightsAccessModulesModel>> CheckSubjectInsightsAccess(string cellPhone)
        {
            try
            {
                string query = $"SELECT eventaccessmodules.* FROM eventaccessmodules INNER JOIN eventaccess ON eventaccessmodules   .EventAccessId = eventaccess.Id WHERE eventaccess.Mobile=" + cellPhone + " AND eventaccess.IsActive=1";

                using MySqlConnection connection = new(_configuration.GetConnectionString("DefaultConnection"));
                var result = await connection.QueryAsync<SubjectInsightsAccessModulesModel>(query);
                return result.ToList();
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
                return null;
            }
        }

        public async Task<List<SubjectInsightsBaseFormBuilderModel>> GetSubjectInsightsList(FilterEntityModel filterModel, bool addfilter, int start, int pageSize)
        {
            try
            {
                StringBuilder query = new();

                if (addfilter)
                {
                    query.Append(@"SELECT id,title,state,tag,subjectimage,createdat, isactive,updatedby,createdbyemail from subjectformbuilder WHERE IsDeleted=0");
                    var parameters = new
                    {
                        filterModel.TitleName,
                        filterModel.State,
                        filterModel.Tag,
                        filterModel.IsPublished
                    };

                    if (!string.IsNullOrEmpty(filterModel.TitleName))
                    {
                        query.Append($" AND title like '%{filterModel.TitleName}%'");
                    }

                    query.Append($" order by createdat desc");

                    using MySqlConnection connection = new(_configuration.GetConnectionString("DefaultConnection"));
                    var result = await connection.QueryAsync<SubjectInsightsBaseFormBuilderModel>(query.ToString(), parameters);

                    return result.ToList();
                }
                else
                {
                    query.Append(@"id,title,state,tag,subjectimage, createdat, isactive,updatedby,createdbyemail from subjectformbuilder order by createdat desc");

                    using MySqlConnection connection = new(_configuration.GetConnectionString("DefaultConnection"));
                    var result = await connection.QueryAsync<SubjectInsightsBaseFormBuilderModel>(query.ToString());

                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
                return null;
            }
        }

        public async Task<SubjectInsightsFormBuilderModel> GetSubjectInsightsDetail(long subjectInsightsFormId)
        {
            try
            {
                string query = @"SELECT * FROM subjectformbuilder WHERE id=" + subjectInsightsFormId + " AND isDeleted=0";

                using MySqlConnection connection = new(_configuration.GetConnectionString("DefaultConnection"));
                var result = await connection.QueryAsync<SubjectInsightsFormBuilderModel>(query);

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
                return null;
            }
        }

        public async Task<bool> CheckIfSubjectInsightsAlreadyExists(string subjectInsightsName, int subjectInsightsFormId = 0)
        {
            try
            {
                string query = string.Empty;
                if (subjectInsightsFormId != 0)
                {
                    query = @"SELECT * FROM subjectformbuilder WHERE title='" + subjectInsightsName + "' AND isActive=1 AND isDeleted=0 AND id != " + subjectInsightsFormId + "";
                }
                else
                {
                    query = @"SELECT * FROM subjectformbuilder WHERE title='" + subjectInsightsName + "' AND isActive=1 AND isDeleted=0";
                }


                using MySqlConnection connection = new(_configuration.GetConnectionString("DefaultConnection"));
                var result = await connection.QueryAsync<SubjectInsightsFormBuilderModel>(query);
                return result.Any();
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
                return false;
            }
        }

        public async Task<bool> CreateSubjectInsights(SubjectInsightsFormBuilderModel subjectInsightsFormBuilderModel)
        {
            try
            {
                var parameters = new
                {
                    subjectInsightsFormBuilderModel.Title,
                    subjectInsightsFormBuilderModel.State,
                    subjectInsightsFormBuilderModel.Tag,
                    subjectInsightsFormBuilderModel.SubjectImage,
                    subjectInsightsFormBuilderModel.IsActive,
                    subjectInsightsFormBuilderModel.SubjectContentForm,
                    createdAt = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata")),
                    subjectInsightsFormBuilderModel.UpdatedBy,
                    subjectInsightsFormBuilderModel.CreatedByEmail
                };
                string query = "INSERT INTO subjectformbuilder (title,state,tag,subjectimage,isActive,contentform,createdAt,updatdby,createdbyemail)" +
                    " VALUES (@Title,@State,@Tag,@SubjectImage,@IsActive,@SubjectContentForm,@createdAt,@UpdatedBy,@UpdatedByEmail) ";

                using MySqlConnection connection = new(_configuration.GetConnectionString("DefaultConnection"));
                var identity = await connection.ExecuteScalarAsync<long>(query, parameters);

                return true;
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
                return false;
            }
        }

        public async Task<bool> UpdateSubjectInsights(SubjectInsightsFormBuilderModel subjectInsightsFormBuilderModel)
        {
            try
            {
                var parameters = new
                {
                    subjectInsightsFormBuilderModel.Id,
                    subjectInsightsFormBuilderModel.Title,
                    subjectInsightsFormBuilderModel.State,
                    subjectInsightsFormBuilderModel.Tag,
                    subjectInsightsFormBuilderModel.SubjectImage,
                    subjectInsightsFormBuilderModel.IsActive,
                    subjectInsightsFormBuilderModel.SubjectContentForm,
                    updatedAt = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata")),
                    subjectInsightsFormBuilderModel.UpdatedBy,
                    subjectInsightsFormBuilderModel.CreatedByEmail
                };
                string query = "UPDATE subjectformbuilder SET title=@Title,state=@State,tag=@Tag,subjectImage=@SubjectImage,isActive=@isActive,contentform=@SubjectContentForm,updatedAt=@updatedAt,updatedby=@UpdatedBy,createdbyemail=@CreatedByEmail" +
                    "  WHERE Id=@Id";

                using MySqlConnection connection = new(_configuration.GetConnectionString("DefaultConnection"));
                var identity = await connection.ExecuteScalarAsync<long>(query, parameters);

                return true;
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
                return false;
            }
        }

        public async Task<bool> DeleteSubjectInsights(long id, string updatedBy)
        {
            try
            {
                var parameters = new
                {
                    id,
                    isDeleted = 1,
                    updatedBy,
                    updatedAt = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata"))
                };
                string query = "UPDATE subjectformbuilder SET IsDeleted = @isDeleted, UpdatedBy = @updatedBy, UpdatedAt=@updatedAt" + " WHERE Id=@id";

                using MySqlConnection connection = new(_configuration.GetConnectionString("DefaultConnection"));
                var result = await connection.ExecuteScalarAsync(query, parameters);
                return true;

            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
                return false;
            }
        }
    }
}
