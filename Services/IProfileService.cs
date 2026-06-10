using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EduGuide_Backend.DTO.Profile;

namespace EduGuide_Backend.Services
{
    public interface IProfileService
    {
        // Core Profile
        Task<StudentProfileDto> GetOrCreateProfileAsync(Guid userId);
        Task<StudentProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileRequestDto req);
        Task<StudentProfileDto> CalculateProfileCompletionAsync(Guid userId);

        // Academic Records & Marks
        Task<AcademicRecordDto> AddAcademicRecordAsync(Guid userId, AcademicRecordRequestDto req);
        Task<AcademicRecordDto> UpdateAcademicRecordAsync(Guid userId, Guid recordId, AcademicRecordRequestDto req);
        Task DeleteAcademicRecordAsync(Guid userId, Guid recordId);
        Task<SubjectMarkDto> AddSubjectMarkAsync(Guid userId, Guid recordId, SubjectMarkRequestDto req);
        Task DeleteSubjectMarkAsync(Guid userId, Guid markId);

        // Interests
        Task<List<InterestDto>> GetInterestsAsync(Guid userId);
        Task<InterestDto> AddInterestAsync(Guid userId, InterestRequestDto req);
        Task DeleteInterestAsync(Guid userId, Guid interestId);

        // Career Goals
        Task<List<CareerGoalDto>> GetCareerGoalsAsync(Guid userId);
        Task<CareerGoalDto> AddCareerGoalAsync(Guid userId, CareerGoalRequestDto req);
        Task<CareerGoalDto> UpdateCareerGoalAsync(Guid userId, Guid goalId, CareerGoalRequestDto req);
        Task DeleteCareerGoalAsync(Guid userId, Guid goalId);

        // Strengths
        Task<List<StrengthDto>> GetStrengthsAsync(Guid userId);
        Task<StrengthDto> AddStrengthAsync(Guid userId, StrengthWeaknessRequestDto req);
        Task DeleteStrengthAsync(Guid userId, Guid strengthId);

        // Weaknesses
        Task<List<WeaknessDto>> GetWeaknessesAsync(Guid userId);
        Task<WeaknessDto> AddWeaknessAsync(Guid userId, StrengthWeaknessRequestDto req);
        Task DeleteWeaknessAsync(Guid userId, Guid weaknessId);

        // Skills
        Task<List<StudentSkillDto>> GetSkillsAsync(Guid userId);
        Task<StudentSkillDto> AddSkillAsync(Guid userId, StudentSkillRequestDto req);
        Task DeleteSkillAsync(Guid userId, Guid skillId);
        Task<List<SkillSearchResponseDto>> SearchSkillsAsync(string query);

        // Certifications
        Task<List<CertificationDto>> GetCertificationsAsync(Guid userId);
        Task<CertificationDto> AddCertificationAsync(Guid userId, CertificationRequestDto req);
        Task<CertificationDto> UpdateCertificationAsync(Guid userId, Guid certId, CertificationRequestDto req);
        Task DeleteCertificationAsync(Guid userId, Guid certId);
    }
}
