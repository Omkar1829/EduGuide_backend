using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduGuide_Backend.DTO.Profile;
using EduGuide_Backend.Services;

namespace EduGuide_Backend.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        private Guid GetUserId()
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr))
            {
                throw new UnauthorizedAccessException("User ID not found in token claims.");
            }
            return Guid.Parse(userIdStr);
        }

        // ─── Profile ──────────────────────────────────────────

        [HttpGet("profile/profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.GetOrCreateProfileAsync(userId);
                return Ok(new { success = true, message = "Profile fetched successfully", data = result });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("profile/profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.UpdateProfileAsync(userId, request);
                return Ok(new { success = true, message = "Profile updated successfully", data = result });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("profile/profile/completion")]
        public async Task<IActionResult> GetProfileCompletion()
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.CalculateProfileCompletionAsync(userId);
                return Ok(new { success = true, message = "Profile completion calculated", data = result });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // ─── Academic Records ─────────────────────────────────

        [HttpPost("profile/profile/academic-records")]
        public async Task<IActionResult> AddAcademicRecord([FromBody] AcademicRecordRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.AddAcademicRecordAsync(userId, request);
                return Ok(new { success = true, message = "Academic record added successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("profile/profile/academic-records/{recordId}")]
        public async Task<IActionResult> UpdateAcademicRecord(Guid recordId, [FromBody] AcademicRecordRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.UpdateAcademicRecordAsync(userId, recordId, request);
                return Ok(new { success = true, message = "Academic record updated successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("profile/profile/academic-records/{recordId}")]
        public async Task<IActionResult> DeleteAcademicRecord(Guid recordId)
        {
            try
            {
                var userId = GetUserId();
                await _profileService.DeleteAcademicRecordAsync(userId, recordId);
                return Ok(new { success = true, message = "Academic record deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("profile/profile/academic-records/{recordId}/marks")]
        public async Task<IActionResult> AddSubjectMark(Guid recordId, [FromBody] SubjectMarkRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.AddSubjectMarkAsync(userId, recordId, request);
                return Ok(new { success = true, message = "Subject mark added successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("profile/profile/marks/{markId}")]
        public async Task<IActionResult> DeleteSubjectMark(Guid markId)
        {
            try
            {
                var userId = GetUserId();
                await _profileService.DeleteSubjectMarkAsync(userId, markId);
                return Ok(new { success = true, message = "Subject mark deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // ─── Interests ────────────────────────────────────────

        [HttpGet("profile/profile/interests")]
        public async Task<IActionResult> GetInterests()
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.GetInterestsAsync(userId);
                return Ok(new { success = true, message = "Interests fetched successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("profile/profile/interests")]
        public async Task<IActionResult> AddInterest([FromBody] InterestRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.AddInterestAsync(userId, request);
                return Ok(new { success = true, message = "Interest added successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("profile/profile/interests/{interestId}")]
        public async Task<IActionResult> DeleteInterest(Guid interestId)
        {
            try
            {
                var userId = GetUserId();
                await _profileService.DeleteInterestAsync(userId, interestId);
                return Ok(new { success = true, message = "Interest deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // ─── Career Goals ──────────────────────────────────────

        [HttpGet("profile/profile/career-goals")]
        public async Task<IActionResult> GetCareerGoals()
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.GetCareerGoalsAsync(userId);
                return Ok(new { success = true, message = "Career goals fetched successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("profile/profile/career-goals")]
        public async Task<IActionResult> AddCareerGoal([FromBody] CareerGoalRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.AddCareerGoalAsync(userId, request);
                return Ok(new { success = true, message = "Career goal added successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("profile/profile/career-goals/{goalId}")]
        public async Task<IActionResult> UpdateCareerGoal(Guid goalId, [FromBody] CareerGoalRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.UpdateCareerGoalAsync(userId, goalId, request);
                return Ok(new { success = true, message = "Career goal updated successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("profile/profile/career-goals/{goalId}")]
        public async Task<IActionResult> DeleteCareerGoal(Guid goalId)
        {
            try
            {
                var userId = GetUserId();
                await _profileService.DeleteCareerGoalAsync(userId, goalId);
                return Ok(new { success = true, message = "Career goal deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // ─── Strengths ────────────────────────────────────────

        [HttpGet("profile/profile/strengths")]
        public async Task<IActionResult> GetStrengths()
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.GetStrengthsAsync(userId);
                return Ok(new { success = true, message = "Strengths fetched successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("profile/profile/strengths")]
        public async Task<IActionResult> AddStrength([FromBody] StrengthWeaknessRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.AddStrengthAsync(userId, request);
                return Ok(new { success = true, message = "Strength added successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("profile/profile/strengths/{strengthId}")]
        public async Task<IActionResult> DeleteStrength(Guid strengthId)
        {
            try
            {
                var userId = GetUserId();
                await _profileService.DeleteStrengthAsync(userId, strengthId);
                return Ok(new { success = true, message = "Strength deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // ─── Weaknesses ────────────────────────────────────────

        [HttpGet("profile/profile/weaknesses")]
        public async Task<IActionResult> GetWeaknesses()
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.GetWeaknessesAsync(userId);
                return Ok(new { success = true, message = "Weaknesses fetched successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("profile/profile/weaknesses")]
        public async Task<IActionResult> AddWeakness([FromBody] StrengthWeaknessRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.AddWeaknessAsync(userId, request);
                return Ok(new { success = true, message = "Weakness added successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("profile/profile/weaknesses/{weaknessId}")]
        public async Task<IActionResult> DeleteWeakness(Guid weaknessId)
        {
            try
            {
                var userId = GetUserId();
                await _profileService.DeleteWeaknessAsync(userId, weaknessId);
                return Ok(new { success = true, message = "Weakness deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // ─── Skills ───────────────────────────────────────────

        [HttpGet("profile/profile/skills")]
        public async Task<IActionResult> GetSkills()
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.GetSkillsAsync(userId);
                return Ok(new { success = true, message = "Skills fetched successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("profile/profile/skills")]
        public async Task<IActionResult> AddSkill([FromBody] StudentSkillRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.AddSkillAsync(userId, request);
                return Ok(new { success = true, message = "Skill added successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("profile/profile/skills/{skillId}")]
        public async Task<IActionResult> DeleteSkill(Guid skillId)
        {
            try
            {
                var userId = GetUserId();
                await _profileService.DeleteSkillAsync(userId, skillId);
                return Ok(new { success = true, message = "Skill deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("profile/skills/search")]
        public async Task<IActionResult> SearchSkills([FromQuery] string q)
        {
            try
            {
                var result = await _profileService.SearchSkillsAsync(q);
                return Ok(new { success = true, message = "Skills search completed", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // ─── Certifications ───────────────────────────────────

        [HttpGet("profile/profile/certifications")]
        public async Task<IActionResult> GetCertifications()
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.GetCertificationsAsync(userId);
                return Ok(new { success = true, message = "Certifications fetched successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("profile/profile/certifications")]
        public async Task<IActionResult> AddCertification([FromBody] CertificationRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.AddCertificationAsync(userId, request);
                return Ok(new { success = true, message = "Certification added successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("profile/profile/certifications/{certId}")]
        public async Task<IActionResult> UpdateCertification(Guid certId, [FromBody] CertificationRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                var result = await _profileService.UpdateCertificationAsync(userId, certId, request);
                return Ok(new { success = true, message = "Certification updated successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("profile/profile/certifications/{certId}")]
        public async Task<IActionResult> DeleteCertification(Guid certId)
        {
            try
            {
                var userId = GetUserId();
                await _profileService.DeleteCertificationAsync(userId, certId);
                return Ok(new { success = true, message = "Certification deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
