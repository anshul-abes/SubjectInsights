
using SubjectInsights.Application.Entities;
using SubjectInsights.Common;
using SubjectInsights.Common.ResponseHelpers;
using SubjectInsights.Logger;
using SubjectInsights.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SubjectInsights.API.Controllers
{
    [Route("api/[controller]")]
    public class SubjectInsightsAdminController : Controller
    {
        private readonly ISubjectInsightsAdminService _subjectInsightsAdminService;
        public SubjectInsightsAdminController(ISubjectInsightsAdminService subjectInsightsAdminService)
        {
            _subjectInsightsAdminService = subjectInsightsAdminService;
        }

        /// <summary>
        /// Get Subject Insights access Permissions
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>

        [AllowAnonymous]
        [HttpGet("CheckSubjectInsightsAccess")]
        public async Task<IActionResult> CheckSubjectInsightsAccess([FromHeader] string mobile)
        {
            try
            {
                if (string.IsNullOrEmpty(mobile))
                {
                    return Ok(CustomExceptions.GenerateExceptionForApp($"Cellphone number is required."));
                }
                var result = await _subjectInsightsAdminService.CheckSubjectInsightsAccess(mobile);
                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
            }

            return Ok(CustomExceptions.GenerateExceptionForApp($"Error fetching subject insights access info."));
        }

        /// <summary>
        /// Get Subject Insights list
        /// </summary>
        /// <param name="moduleAccess"></param>
        /// <returns></returns>

        [AllowAnonymous]
        [HttpGet("GetSubjectInsightsList")]
        public async Task<IActionResult> GetSubjectInsightsList([FromHeader] string moduleAccess, [FromQuery(Name = "_startPage")] string start, [FromQuery(Name = "_pageSize")] string pageSize)
        {
            try
            {
                if (ModuleAccess.SubjectCreteAccess.Equals(moduleAccess))
                {
                    var result = await _subjectInsightsAdminService.GetSubjectInsightsList(null, false, Convert.ToInt32(start), Convert.ToInt32(pageSize));
                    return Ok(result);
                }

                return Ok(CustomExceptions.GenerateExceptionForApp($"Error fetching subject insights list info."));
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
            }

            return Ok(CustomExceptions.GenerateExceptionForApp($"Error fetching subject insights list info"));
        }

        /// <summary>
        /// Get Even list
        /// </summary>
        /// <param name="moduleAccess"></param>
        /// <returns></returns>

        [AllowAnonymous]
        [HttpPost("GetSubjectsInsightsListFiltered")]
        public async Task<IActionResult> GetSubjectsInsightsListFiltered([FromHeader] string moduleAccess, [FromBody] FilterEntityModel request, [FromQuery(Name = "_startPage")] string start, [FromQuery(Name = "_pageSize")] string pageSize)
        {
            try
            {
                if (ModuleAccess.SubjectCreteAccess.Equals(moduleAccess))
                {
                    var result = await _subjectInsightsAdminService.GetSubjectInsightsList(request, true, Convert.ToInt32(start), Convert.ToInt32(pageSize));
                    return Ok(result);
                }

                return Ok(CustomExceptions.GenerateExceptionForApp($"No access to subject insights creation."));
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
            }

            return Ok(CustomExceptions.GenerateExceptionForApp($"Error fetching subject insights list info."));
        }

        /// <summary>
        /// Get Subject Insights Detail
        /// </summary>
        /// <param name="subjectInsightsFormId"></param>
        /// <param name="moduleAccess"></param>
        /// <returns></returns>

        [AllowAnonymous]
        [HttpGet("{subjectInsightsFormId}", Name = "GetSubjectInsightsDetail")]
        public async Task<IActionResult> GetSubjectInsightsDetail([FromHeader] string moduleAccess, [FromRoute] long subjectInsightsFormId)
        {
            try
            {
                if (ModuleAccess.SubjectCreteAccess.Equals(moduleAccess))
                {
                    var result = await _subjectInsightsAdminService.GetSubjectInsightsDetail(subjectInsightsFormId);
                    return Ok(result);
                }

                return Ok(CustomExceptions.GenerateExceptionForApp($"No access to subject insights creation"));
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
            }

            return Ok(CustomExceptions.GenerateExceptionForApp($"Error fetching subject insights list info."));
        }

        /// <summary>
        /// Get JWT
        /// </summary>
        /// <param name="subjectInsightsFormBuilder"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("", Name = "CreateSubjectInsights")]
        public async Task<IActionResult> CreateSubjectInsights([FromHeader] string moduleAccess, [FromBody] SubjectInsightsFormBuilderModel subjectInsightsFormBuilder)
        {
            try
            {

                var result = await _subjectInsightsAdminService.CreateSubjectInsights(subjectInsightsFormBuilder);
                return Ok(result);

            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
            }

            return Ok(CustomExceptions.GenerateExceptionForApp($"Error creating subject insights."));
        }

        /// <summary>
        /// Get JWT
        /// </summary>
        /// <param name="subjectInsightsFormBuilder"></param>
        /// <returns></returns>
        [HttpPut("", Name = "UpdateSubjectInsights")]
        public async Task<IActionResult> UpdateSubjectInsights([FromHeader] string moduleAccess, [FromBody] SubjectInsightsFormBuilderModel subjectInsightsFormBuilder)
        {
            try
            {

                var result = await _subjectInsightsAdminService.UpdateSubjectInsights(subjectInsightsFormBuilder);

                return Ok(result);
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
            }

            return Ok(CustomExceptions.GenerateExceptionForApp($"Error updating subject insights."));
        }

        /// <summary>
        /// Delete user category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteSubjectInsights")]
        public async Task<IActionResult> DeleteSubjectInsights([FromHeader] string moduleAccess, [FromHeader] string updatedBy, [FromRoute] long id)
        {
            try
            {
                if (ModuleAccess.SubjectCreteAccess.Equals(moduleAccess))
                {
                    var result = await _subjectInsightsAdminService.DeleteSubjectInsights(id, updatedBy);
                    return Ok(result);
                }
                return Ok(CustomExceptions.GenerateExceptionForApp($"No access to subject insights deletion"));

            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
            }

            return Ok(CustomExceptions.GenerateExceptionForApp($"Error deleting subject insights details"));
        }
    }
}
