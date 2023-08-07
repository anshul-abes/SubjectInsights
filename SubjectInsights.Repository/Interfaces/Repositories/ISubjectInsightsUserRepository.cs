using SubjectInsights.Application.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubjectInsights.Application.Interfaces.Repositories
{
    public interface ISubjectInsightsUserRepository
    {       
        Task<List<SubjectInsightsBaseFormBuilderModel>> GetUserSubjectInsightsList(string cellPhone, FilterEntityModel filterModel, bool addfilter, int start, int pageSize);
        Task<SubjectInsightsFormBuilderModel> GetUserSubjectInsightsDetail(long subjectInsightsFormId, string cellPhone);
    }
}
