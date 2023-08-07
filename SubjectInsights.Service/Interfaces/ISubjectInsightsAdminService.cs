using SubjectInsights.Application.Entities;
using SubjectInsights.Common.ResponseHelpers;

namespace SubjectInsights.Service.Interfaces
{
    public interface ISubjectInsightsAdminService
    {
        Task<CustomResponse<List<SubjectInsightsAccessModulesModel>>> CheckSubjectInsightsAccess(string cellphone);
        Task<CustomResponse<List<SubjectInsightsBaseFormBuilderModel>>> GetSubjectInsightsList(FilterEntityModel filterModel, bool addfilter, int start, int pageSize);
        Task<CustomResponse<SubjectInsightsFormBuilderModel>> GetSubjectInsightsDetail(long subjectInsightsFormId);
        Task<CustomResponse<bool>> DeleteSubjectInsights(long id, string updatedBy);
        Task<CustomResponse<bool>> CreateSubjectInsights(SubjectInsightsFormBuilderModel subjectInsightsFormBuilderModel);
        Task<CustomResponse<bool>> UpdateSubjectInsights(SubjectInsightsFormBuilderModel subjectInsightsFormBuilderModel);
        Task<bool> CheckIfSubjectInsightsAlreadyExists(string subjectInsightsName, int subjectInsightsId);
    }
}
