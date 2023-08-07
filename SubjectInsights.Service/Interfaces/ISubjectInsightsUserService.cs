using SubjectInsights.Application.Entities;
using SubjectInsights.Common.ResponseHelpers;

namespace SubjectInsights.Service.Interfaces
{
    public interface ISubjectInsightsUserService
    {
        Task<CustomResponse<List<SubjectInsightsBaseFormBuilderModel>>> GetUserSubjectInsightsList(string cellPhone, FilterEntityModel filterModel, bool addfilter, int start, int pageSize);
        Task<CustomResponse<SubjectInsightsFormBuilderModel>> GetUserSubjectInsightsDetail(long subjectInsightsFormId, string cellPhone);
        CustomResponse<string> GeneratePresignedURL();
    }
}
