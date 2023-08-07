using SubjectInsights.Application.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubjectInsights.Application.Interfaces.Repositories
{
    public interface ISubjectInsightsAdminRepository
    {
        Task<List<SubjectInsightsAccessModulesModel>> CheckSubjectInsightsAccess(string cellphone);
        Task<List<SubjectInsightsBaseFormBuilderModel>> GetSubjectInsightsList(FilterEntityModel filterModel, bool addfilter, int start, int pageSize);
        Task<SubjectInsightsFormBuilderModel> GetSubjectInsightsDetail(long subjectInsightsFormId);
        Task<bool> DeleteSubjectInsights(long id, string updatedBy);
        Task<bool> CreateSubjectInsights(SubjectInsightsFormBuilderModel subjectInsightsFormBuilderModel);
        Task<bool> UpdateSubjectInsights(SubjectInsightsFormBuilderModel subjectInsightsFormBuilderModel);
        Task<bool> CheckIfSubjectInsightsAlreadyExists(string subjectInsightsName, int subjectInsightsId);
    }
}
