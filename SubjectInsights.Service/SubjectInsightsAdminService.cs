
using SubjectInsights.Application.Entities;
using SubjectInsights.Application.Interfaces.Repositories;
using SubjectInsights.Common.ResponseHelpers;
using SubjectInsights.Logger;
using SubjectInsights.Service.Interfaces;
namespace SubjectInsights.Service
{
    public class SubjectInsightsAdminService : ISubjectInsightsAdminService
    {
        private readonly ISubjectInsightsAdminRepository _subjectInsightsAdminRepository;
        public SubjectInsightsAdminService(ISubjectInsightsAdminRepository subjectInsightsAdminRepository)
        {
            _subjectInsightsAdminRepository = subjectInsightsAdminRepository;
        }

        public async Task<CustomResponse<List<SubjectInsightsAccessModulesModel>>> CheckSubjectInsightsAccess(string cellphone)
        {
            var _accessList = await _subjectInsightsAdminRepository.CheckSubjectInsightsAccess(cellphone);
            if (_accessList != null)
            {
                return new CustomResponse<List<SubjectInsightsAccessModulesModel>>
                {
                    Code = StatusCode.Success,
                    Data = _accessList,
                    Message = "Data Fetched."
                };
            }

            CommonLogger.LogError("Provide Subject Insights access not found, try again");
            return new CustomResponse<List<SubjectInsightsAccessModulesModel>>
            {
                Code = StatusCode.Fail,
                Data = null,
                Message = "Error getting Subject Insights access details."
            };
        }

        public async Task<CustomResponse<List<SubjectInsightsBaseFormBuilderModel>>> GetSubjectInsightsList(FilterEntityModel filterModel, bool addfilter, int start, int pageSize)
        {
            var _subjectInsightsList = await _subjectInsightsAdminRepository.GetSubjectInsightsList(filterModel, addfilter, start, pageSize);
            if (_subjectInsightsList != null)
            {
                var totalRecords = _subjectInsightsList.Count;
                var pageResult = GetPagedData(_subjectInsightsList, start, pageSize).ToList();
                return new CustomResponse<List<SubjectInsightsBaseFormBuilderModel>>
                {
                    Code = StatusCode.Success,
                    Data = pageResult,
                    TotalCount = totalRecords,
                    Message = "Data Fetched."
                };
            }

            CommonLogger.LogError("Subject Insights list not found, try again");
            return new CustomResponse<List<SubjectInsightsBaseFormBuilderModel>>
            {
                Code = StatusCode.Fail,
                Data = null,
                Message = "Error getting Subject Insights lists."
            };
        }

        public IEnumerable<SubjectInsightsBaseFormBuilderModel> GetPagedData(List<SubjectInsightsBaseFormBuilderModel> subjectInsights, int start, int pageSize)
        {
            if (pageSize > 0 && start > 0)
            {
                return subjectInsights.Skip((start - 1) * pageSize).Take(pageSize);
            }
            else if (pageSize > 0)
            {
                return subjectInsights.Take(pageSize);
            }
            return subjectInsights;
        }

        public async Task<CustomResponse<SubjectInsightsFormBuilderModel>> GetSubjectInsightsDetail(long subjectInsightsFormId)
        {
            var _subjectInsights = await _subjectInsightsAdminRepository.GetSubjectInsightsDetail(subjectInsightsFormId);
            if (_subjectInsights != null)
            {
                return new CustomResponse<SubjectInsightsFormBuilderModel>
                {
                    Code = StatusCode.Success,
                    Data = _subjectInsights,
                    Message = "Data Fetched."
                };
            }

            CommonLogger.LogError("Subject Insights details not found, try again");
            return new CustomResponse<SubjectInsightsFormBuilderModel>
            {
                Code = StatusCode.Fail,
                Data = null,
                Message = "Error getting Subject Insights details."
            };
        }

        public async Task<bool> CheckIfSubjectInsightsAlreadyExists(string subjectInsightsName, int subjectInsightsId = 0)
        {
            return await _subjectInsightsAdminRepository.CheckIfSubjectInsightsAlreadyExists(subjectInsightsName, subjectInsightsId);

        }

        public async Task<CustomResponse<bool>> CreateSubjectInsights(SubjectInsightsFormBuilderModel subjectInsightsFormBuilderModel)
        {
            var _subjectInsights = await _subjectInsightsAdminRepository.CreateSubjectInsights(subjectInsightsFormBuilderModel);
            if (_subjectInsights)
            {
                return new CustomResponse<bool>
                {
                    Code = StatusCode.Success,
                    Data = true,
                    Message = "SubjectInsights Created."
                };
            }
            else
            {
                return new CustomResponse<bool>
                {
                    Code = StatusCode.Fail,
                    Data = false,
                    Message = "Error Creating SubjectInsights."
                };
            }
        }

        public async Task<CustomResponse<bool>> UpdateSubjectInsights(SubjectInsightsFormBuilderModel subjectInsightsFormBuilderModel)
        {
            var _subjectInsights = await _subjectInsightsAdminRepository.UpdateSubjectInsights(subjectInsightsFormBuilderModel);
            if (_subjectInsights)
            {
                return new CustomResponse<bool>
                {
                    Code = StatusCode.Success,
                    Data = true,
                    Message = "Subject Insights Updated."
                };
            }
            else
            {
                return new CustomResponse<bool>
                {
                    Code = StatusCode.Fail,
                    Data = false,
                    Message = "Error Updating Subject Insights."
                };
            }
        }

        public async Task<CustomResponse<bool>> DeleteSubjectInsights(long id, string updatedBy)
        {
            var _result = await _subjectInsightsAdminRepository.DeleteSubjectInsights(id, updatedBy);
            return new CustomResponse<bool>
            {
                Code = StatusCode.Success,
                Data = true,
                Message = "Subject Insights Deleted."
            };

        }

    }
}
