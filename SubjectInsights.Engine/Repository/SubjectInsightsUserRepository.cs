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

namespace SubjectInsights.Infrastructure.Repository
{
    public class SubjectInsightsUserRepository : ISubjectInsightsUserRepository
    {
        private readonly IConfiguration _configuration;
        public SubjectInsightsUserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<List<SubjectInsightsBaseFormBuilderModel>> GetUserSubjectInsightsList(string cellPhone, FilterEntityModel filterModel, bool addfilter, int start, int pageSize)
        {
            try
            {
                string query = @"SELECT id,title,state,tag,subjectimage from subjectformbuilder WHERE IsDeleted=0 and isactive=1 order by createdat desc";
                using MySqlConnection connection = new(_configuration.GetConnectionString("DefaultConnection"));
                var result = await connection.QueryAsync<SubjectInsightsBaseFormBuilderModel>(query.ToString());

                return result.ToList();

            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
                return null;
            }
        }

        public async Task<SubjectInsightsFormBuilderModel> GetUserSubjectInsightsDetail(long subjectInsightsFormId, string cellPhone)
        {
            try
            {
                try
                {
                    string query = @"SELECT * FROM subjectformbuilder WHERE id=" + subjectInsightsFormId + " AND isDeleted=0 and isactive=1";

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
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
                return null;
            }
        }
    }
}