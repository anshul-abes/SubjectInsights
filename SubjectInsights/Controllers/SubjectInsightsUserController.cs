
using SubjectInsights.Application.Entities;
using SubjectInsights.Common;
using SubjectInsights.Common.ResponseHelpers;
using SubjectInsights.Logger;
using SubjectInsights.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubjectInsights.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class SubjectInsightsUserController : Controller
    {
        private readonly ISubjectInsightsUserService _subjectInsightsUserService;
        public SubjectInsightsUserController(ISubjectInsightsUserService subjectInsightsUserService)
        {
            _subjectInsightsUserService = subjectInsightsUserService;
        }


        [HttpGet("GeneratePresignedURL")]
        public async Task<IActionResult> GeneratePresignedURL()
        {
            try
            {
                var result = _subjectInsightsUserService.GeneratePresignedURL();
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
            }

            return Ok(CustomExceptions.GenerateExceptionForApp($"Error uploading user subject insights images."));
        }


        /// <summary>
        /// Get Subject Insights list
        /// </summary>
        /// <param name="moduleAccess"></param>
        /// <returns></returns>


        [HttpPost("GetUserSubjectInsightsList")]
        public async Task<IActionResult> GetUserSubjectInsightsListFiltered([FromHeader] string cellPhone, [FromBody] FilterEntityModel request, [FromQuery(Name = "_startPage")] string start, [FromQuery(Name = "_pageSize")] string pageSize)
        {
            try
            {
                var result = await _subjectInsightsUserService.GetUserSubjectInsightsList(cellPhone, request, true, Convert.ToInt32(start), Convert.ToInt32(pageSize));
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
            }

            return Ok(CustomExceptions.GenerateExceptionForApp($"Error fetching subject insights list info"));
        }


        /// <summary>
        /// Get User Subject insights Detail
        /// </summary>
        /// <param name="subjectInsightsFormId"></param>
        /// <param name="moduleAccess"></param>
        /// <returns></returns>

        [HttpGet("{subjectInsightsFormId}", Name = "GetUserSubjectInsightsDetail")]
        public async Task<IActionResult> GetUserSubjectInsightsDetail([FromHeader] string cellPhone, [FromRoute] long subjectInsightsFormId)
        {
            try
            {
                var result = await _subjectInsightsUserService.GetUserSubjectInsightsDetail(subjectInsightsFormId, cellPhone);
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
            }

            return Ok(CustomExceptions.GenerateExceptionForApp($"Error fetching subject insights list info."));
        }
    }
}
