
using SubjectInsights.Application.Entities;
using SubjectInsights.Application.Interfaces.Repositories;
using SubjectInsights.Common;
using SubjectInsights.Common.ResponseHelpers;
using SubjectInsights.Logger;
using SubjectInsights.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace SubjectInsights.Service
{
    public class SubjectInsightsUserService : ISubjectInsightsUserService
    {
        private readonly ISubjectInsightsUserRepository _subjectInsightsUserRepository;
        private readonly IConfiguration _configuration;
        public SubjectInsightsUserService(ISubjectInsightsUserRepository subjectInsightsUserRepository, IConfiguration configuration)
        {
            _subjectInsightsUserRepository = subjectInsightsUserRepository;
            _configuration = configuration;
        }

        public async Task<CustomResponse<List<string>>> UploadUserSubjectInsightsImages(string cellPhone, ICollection<IFormFile> images)
        {
            var subjectInsightsImages = new List<string>();

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    // Access the image stream and convert it to a byte array
                    using var memoryStream = new MemoryStream();
                    await image.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();

                    // Convert the imageBytes to a Base64 string
                    string base64String = Convert.ToBase64String(imageBytes);
                    var imagePath = "https://sangathan-images." + await S3Helper.UploadFileToS3(base64String, cellPhone, _configuration);

                    subjectInsightsImages.Add(imagePath);
                }
            }

            return new CustomResponse<List<String>>
            {
                Code = StatusCode.Success,
                Data = subjectInsightsImages,
                TotalCount = subjectInsightsImages.Count,
                Message = "User subject insights images uploaded."
            };
        }

        public async Task<CustomResponse<List<SubjectInsightsBaseFormBuilderModel>>> GetUserSubjectInsightsList(string cellPhone, FilterEntityModel filterModel, bool addfilter, int start, int pageSize)
        {
            var _subjectInsightsList = await _subjectInsightsUserRepository.GetUserSubjectInsightsList(cellPhone, filterModel, addfilter, start, pageSize);
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
            } else
            {
                return new CustomResponse<List<SubjectInsightsBaseFormBuilderModel>>
                {
                    Code = StatusCode.Success,
                    Data = null,
                    TotalCount = 0,
                    Message = "No Record Found."
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

        public CustomResponse<string> GeneratePresignedURL()
        {
            var preSignedURL = S3Helper.GeneratePresignedURL(5, _configuration);

            return new CustomResponse<String>
            {
                Code = StatusCode.Success,
                Data = preSignedURL,
                TotalCount = 1,
                Message = "Pre Signed URL Returned."
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

        public async Task<CustomResponse<SubjectInsightsFormBuilderModel>> GetUserSubjectInsightsDetail(long subjectInsightsFormId, string cellPhone)
        {
            var _subjectInsights = await _subjectInsightsUserRepository.GetUserSubjectInsightsDetail(subjectInsightsFormId, cellPhone);
            if (_subjectInsights != null)
            {
                return new CustomResponse<SubjectInsightsFormBuilderModel>
                {
                    Code = StatusCode.Success,
                    Data = _subjectInsights,
                    Message = "Data Fetched."
                };
            } else
            {
                return new CustomResponse<SubjectInsightsFormBuilderModel>
                {
                    Code = StatusCode.Success,
                    Data = null,
                    Message = "No Record Found."
                };
            }

            CommonLogger.LogError("Subject Insights details not found, try again");
            return new CustomResponse<SubjectInsightsFormBuilderModel>
            {
                Code = StatusCode.Fail,
                Data = null,
                Message = "Error getting subject insights details."
            };
        }
    }
}
