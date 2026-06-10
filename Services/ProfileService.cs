using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EduGuide_Backend.Models;
using EduGuide_Backend.DTO.Profile;

namespace EduGuide_Backend.Services
{
    public class ProfileService : IProfileService
    {
        private readonly EgaidbContext _context;

        public ProfileService(EgaidbContext context)
        {
            _context = context;
        }

        private StudentProfileDto MapToDto(StudentProfile p, string email, string firstName, string lastName)
        {
            return new StudentProfileDto
            {
                Id = p.Id,
                UserId = p.UserId,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender?.ToString(),
                PhoneNumber = p.PhoneNumber,
                Address = p.Address,
                City = p.City,
                State = p.State,
                Country = p.Country,
                Bio = p.Bio,
                ProfileComplete = p.ProfileComplete,
                CompletionPct = p.CompletionPct,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,

                AcademicRecords = p.AcademicRecords.Select(ar => new AcademicRecordDto
                {
                    Id = ar.Id,
                    Institution = ar.Institution,
                    Degree = ar.Degree,
                    FieldOfStudy = ar.FieldOfStudy,
                    StartYear = ar.StartYear,
                    EndYear = ar.EndYear,
                    Gpa = ar.Gpa,
                    Percentage = ar.Percentage,
                    IsCurrent = ar.IsCurrent,
                    SubjectMarks = ar.SubjectMarks.Select(sm => new SubjectMarkDto
                    {
                        Id = sm.Id,
                        SubjectName = sm.SubjectName,
                        Marks = sm.Marks,
                        MaxMarks = sm.MaxMarks,
                        Grade = sm.Grade
                    }).ToList()
                }).ToList(),

                Interests = p.Interests.Select(i => new InterestDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Category = i.Category,
                    Level = i.Level
                }).ToList(),

                CareerGoals = p.CareerGoals.Select(cg => new CareerGoalDto
                {
                    Id = cg.Id,
                    Title = cg.Title,
                    Description = cg.Description,
                    TargetYear = cg.TargetYear,
                    Priority = cg.Priority
                }).ToList(),

                Strengths = p.Strengths.Select(s => new StrengthDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Category = s.Category,
                    Evidence = s.Evidence
                }).ToList(),

                Weaknesses = p.Weaknesses.Select(w => new WeaknessDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Category = w.Category,
                    Evidence = w.Evidence
                }).ToList(),

                Skills = p.StudentSkills.Select(ss => new StudentSkillDto
                {
                    Id = ss.Id,
                    SkillId = ss.SkillId,
                    SkillName = ss.Skill != null ? ss.Skill.Name : string.Empty,
                    Category = ss.Skill != null ? ss.Skill.Category : string.Empty,
                    Level = ss.Level,
                    YearsExp = ss.YearsExp,
                    IsVerified = ss.IsVerified
                }).ToList(),

                Certifications = p.Certifications.Select(c => new CertificationDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Issuer = c.Issuer,
                    IssueDate = c.IssueDate,
                    ExpiryDate = c.ExpiryDate,
                    CredentialUrl = c.CredentialUrl
                }).ToList()
            };
        }

        // Core Profile
        public async Task<StudentProfileDto> GetOrCreateProfileAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.StudentProfile)
                    .ThenInclude(p => p.AcademicRecords)
                        .ThenInclude(ar => ar.SubjectMarks)
                .Include(u => u.StudentProfile)
                    .ThenInclude(p => p.Interests)
                .Include(u => u.StudentProfile)
                    .ThenInclude(p => p.CareerGoals)
                .Include(u => u.StudentProfile)
                    .ThenInclude(p => p.Strengths)
                .Include(u => u.StudentProfile)
                    .ThenInclude(p => p.Weaknesses)
                .Include(u => u.StudentProfile)
                    .ThenInclude(p => p.StudentSkills)
                        .ThenInclude(ss => ss.Skill)
                .Include(u => u.StudentProfile)
                    .ThenInclude(p => p.Certifications)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var profile = user.StudentProfile;
            if (profile == null)
            {
                profile = new StudentProfile
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Country = "India",
                    ProfileComplete = false,
                    CompletionPct = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.StudentProfiles.Add(profile);
                await _context.SaveChangesAsync();
                
                // Fetch again to ensure related models are populated (even if empty)
                return await GetOrCreateProfileAsync(userId);
            }

            return MapToDto(profile, user.Email, user.FirstName, user.LastName);
        }

        public async Task<StudentProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileRequestDto req)
        {
            var user = await _context.Users
                .Include(u => u.StudentProfile)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var p = user.StudentProfile;
            if (p == null)
            {
                p = new StudentProfile
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Country = "India",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.StudentProfiles.Add(p);
            }

            p.DateOfBirth = req.DateOfBirth;
            p.PhoneNumber = req.PhoneNumber;
            p.Address = req.Address;
            p.City = req.City;
            p.State = req.State;
            p.Country = req.Country ?? p.Country;
            p.Bio = req.Bio;

            if (!string.IsNullOrEmpty(req.Gender))
            {
                if (Enum.TryParse<Gender>(req.Gender, true, out var parsedGender))
                {
                    p.Gender = parsedGender;
                }
                else
                {
                    throw new ArgumentException("Invalid gender. Allowed: MALE, FEMALE, OTHER, PREFER_NOT_TO_SAY");
                }
            }
            else
            {
                p.Gender = null;
            }

            p.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await CalculateProfileCompletionAsync(userId);
        }

        public async Task<StudentProfileDto> CalculateProfileCompletionAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.StudentProfile)
                    .ThenInclude(p => p.AcademicRecords)
                        .ThenInclude(ar => ar.SubjectMarks)
                .Include(u => u.StudentProfile)
                    .ThenInclude(p => p.Interests)
                .Include(u => u.StudentProfile)
                    .ThenInclude(p => p.CareerGoals)
                .Include(u => u.StudentProfile)
                    .ThenInclude(p => p.Strengths)
                .Include(u => u.StudentProfile)
                    .ThenInclude(p => p.Weaknesses)
                .Include(u => u.StudentProfile)
                    .ThenInclude(p => p.StudentSkills)
                        .ThenInclude(ss => ss.Skill)
                .Include(u => u.StudentProfile)
                    .ThenInclude(p => p.Certifications)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.StudentProfile == null)
            {
                throw new KeyNotFoundException("Profile not found");
            }

            var p = user.StudentProfile;
            int filledCount = 0;

            if (p.DateOfBirth.HasValue) filledCount++;
            if (p.Gender.HasValue) filledCount++;
            if (!string.IsNullOrWhiteSpace(p.PhoneNumber)) filledCount++;
            if (!string.IsNullOrWhiteSpace(p.Bio)) filledCount++;
            if (!string.IsNullOrWhiteSpace(p.City)) filledCount++;
            if (!string.IsNullOrWhiteSpace(p.State)) filledCount++;
            if (p.AcademicRecords.Any()) filledCount++;
            if (p.Interests.Any()) filledCount++;
            if (p.CareerGoals.Any()) filledCount++;
            if (p.StudentSkills.Any()) filledCount++;

            p.CompletionPct = filledCount * 10;
            p.ProfileComplete = p.CompletionPct == 100;
            p.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToDto(p, user.Email, user.FirstName, user.LastName);
        }

        // Academic Records
        public async Task<AcademicRecordDto> AddAcademicRecordAsync(Guid userId, AcademicRecordRequestDto req)
        {
            var profile = await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) throw new KeyNotFoundException("Profile not found");

            var record = new AcademicRecord
            {
                Id = Guid.NewGuid(),
                ProfileId = profile.Id,
                Institution = req.Institution,
                Degree = req.Degree,
                FieldOfStudy = req.FieldOfStudy,
                StartYear = req.StartYear,
                EndYear = req.EndYear,
                Gpa = req.Gpa,
                Percentage = req.Percentage,
                IsCurrent = req.IsCurrent,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.AcademicRecords.Add(record);
            await _context.SaveChangesAsync();

            await CalculateProfileCompletionAsync(userId);

            return new AcademicRecordDto
            {
                Id = record.Id,
                Institution = record.Institution,
                Degree = record.Degree,
                FieldOfStudy = record.FieldOfStudy,
                StartYear = record.StartYear,
                EndYear = record.EndYear,
                Gpa = record.Gpa,
                Percentage = record.Percentage,
                IsCurrent = record.IsCurrent,
                SubjectMarks = new List<SubjectMarkDto>()
            };
        }

        public async Task<AcademicRecordDto> UpdateAcademicRecordAsync(Guid userId, Guid recordId, AcademicRecordRequestDto req)
        {
            var record = await _context.AcademicRecords
                .Include(ar => ar.SubjectMarks)
                .FirstOrDefaultAsync(ar => ar.Id == recordId && ar.Profile.UserId == userId);

            if (record == null) throw new KeyNotFoundException("Academic record not found");

            record.Institution = req.Institution;
            record.Degree = req.Degree;
            record.FieldOfStudy = req.FieldOfStudy;
            record.StartYear = req.StartYear;
            record.EndYear = req.EndYear;
            record.Gpa = req.Gpa;
            record.Percentage = req.Percentage;
            record.IsCurrent = req.IsCurrent;
            record.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new AcademicRecordDto
            {
                Id = record.Id,
                Institution = record.Institution,
                Degree = record.Degree,
                FieldOfStudy = record.FieldOfStudy,
                StartYear = record.StartYear,
                EndYear = record.EndYear,
                Gpa = record.Gpa,
                Percentage = record.Percentage,
                IsCurrent = record.IsCurrent,
                SubjectMarks = record.SubjectMarks.Select(sm => new SubjectMarkDto
                {
                    Id = sm.Id,
                    SubjectName = sm.SubjectName,
                    Marks = sm.Marks,
                    MaxMarks = sm.MaxMarks,
                    Grade = sm.Grade
                }).ToList()
            };
        }

        public async Task DeleteAcademicRecordAsync(Guid userId, Guid recordId)
        {
            var record = await _context.AcademicRecords
                .FirstOrDefaultAsync(ar => ar.Id == recordId && ar.Profile.UserId == userId);

            if (record != null)
            {
                _context.AcademicRecords.Remove(record);
                await _context.SaveChangesAsync();

                await CalculateProfileCompletionAsync(userId);
            }
        }

        public async Task<SubjectMarkDto> AddSubjectMarkAsync(Guid userId, Guid recordId, SubjectMarkRequestDto req)
        {
            var record = await _context.AcademicRecords
                .FirstOrDefaultAsync(ar => ar.Id == recordId && ar.Profile.UserId == userId);

            if (record == null) throw new KeyNotFoundException("Academic record not found");

            var mark = new SubjectMark
            {
                Id = Guid.NewGuid(),
                AcademicRecordId = recordId,
                SubjectName = req.SubjectName,
                Marks = req.Marks,
                MaxMarks = req.MaxMarks,
                Grade = req.Grade,
                CreatedAt = DateTime.UtcNow
            };

            _context.SubjectMarks.Add(mark);
            await _context.SaveChangesAsync();

            return new SubjectMarkDto
            {
                Id = mark.Id,
                SubjectName = mark.SubjectName,
                Marks = mark.Marks,
                MaxMarks = mark.MaxMarks,
                Grade = mark.Grade
            };
        }

        public async Task DeleteSubjectMarkAsync(Guid userId, Guid markId)
        {
            var mark = await _context.SubjectMarks
                .FirstOrDefaultAsync(sm => sm.Id == markId && sm.AcademicRecord.Profile.UserId == userId);

            if (mark != null)
            {
                _context.SubjectMarks.Remove(mark);
                await _context.SaveChangesAsync();
            }
        }

        // Interests
        public async Task<List<InterestDto>> GetInterestsAsync(Guid userId)
        {
            var interests = await _context.Interests
                .Where(i => i.Profile.UserId == userId)
                .ToListAsync();

            return interests.Select(i => new InterestDto
            {
                Id = i.Id,
                Name = i.Name,
                Category = i.Category,
                Level = i.Level
            }).ToList();
        }

        public async Task<InterestDto> AddInterestAsync(Guid userId, InterestRequestDto req)
        {
            var profile = await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) throw new KeyNotFoundException("Profile not found");

            var existing = await _context.Interests
                .FirstOrDefaultAsync(i => i.ProfileId == profile.Id && i.Name.ToLower() == req.Name.ToLower());

            if (existing != null)
            {
                throw new InvalidOperationException("Interest already exists");
            }

            var interest = new Interest
            {
                Id = Guid.NewGuid(),
                ProfileId = profile.Id,
                Name = req.Name,
                Category = req.Category,
                Level = req.Level,
                CreatedAt = DateTime.UtcNow
            };

            _context.Interests.Add(interest);
            await _context.SaveChangesAsync();

            await CalculateProfileCompletionAsync(userId);

            return new InterestDto
            {
                Id = interest.Id,
                Name = interest.Name,
                Category = interest.Category,
                Level = interest.Level
            };
        }

        public async Task DeleteInterestAsync(Guid userId, Guid interestId)
        {
            var interest = await _context.Interests
                .FirstOrDefaultAsync(i => i.Id == interestId && i.Profile.UserId == userId);

            if (interest != null)
            {
                _context.Interests.Remove(interest);
                await _context.SaveChangesAsync();

                await CalculateProfileCompletionAsync(userId);
            }
        }

        // Career Goals
        public async Task<List<CareerGoalDto>> GetCareerGoalsAsync(Guid userId)
        {
            var goals = await _context.CareerGoals
                .Where(cg => cg.Profile.UserId == userId)
                .ToListAsync();

            return goals.Select(cg => new CareerGoalDto
            {
                Id = cg.Id,
                Title = cg.Title,
                Description = cg.Description,
                TargetYear = cg.TargetYear,
                Priority = cg.Priority
            }).ToList();
        }

        public async Task<CareerGoalDto> AddCareerGoalAsync(Guid userId, CareerGoalRequestDto req)
        {
            var profile = await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) throw new KeyNotFoundException("Profile not found");

            var goal = new CareerGoal
            {
                Id = Guid.NewGuid(),
                ProfileId = profile.Id,
                Title = req.Title,
                Description = req.Description,
                TargetYear = req.TargetYear,
                Priority = req.Priority,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.CareerGoals.Add(goal);
            await _context.SaveChangesAsync();

            await CalculateProfileCompletionAsync(userId);

            return new CareerGoalDto
            {
                Id = goal.Id,
                Title = goal.Title,
                Description = goal.Description,
                TargetYear = goal.TargetYear,
                Priority = goal.Priority
            };
        }

        public async Task<CareerGoalDto> UpdateCareerGoalAsync(Guid userId, Guid goalId, CareerGoalRequestDto req)
        {
            var goal = await _context.CareerGoals
                .FirstOrDefaultAsync(cg => cg.Id == goalId && cg.Profile.UserId == userId);

            if (goal == null) throw new KeyNotFoundException("Career goal not found");

            goal.Title = req.Title;
            goal.Description = req.Description;
            goal.TargetYear = req.TargetYear;
            goal.Priority = req.Priority;
            goal.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new CareerGoalDto
            {
                Id = goal.Id,
                Title = goal.Title,
                Description = goal.Description,
                TargetYear = goal.TargetYear,
                Priority = goal.Priority
            };
        }

        public async Task DeleteCareerGoalAsync(Guid userId, Guid goalId)
        {
            var goal = await _context.CareerGoals
                .FirstOrDefaultAsync(cg => cg.Id == goalId && cg.Profile.UserId == userId);

            if (goal != null)
            {
                _context.CareerGoals.Remove(goal);
                await _context.SaveChangesAsync();

                await CalculateProfileCompletionAsync(userId);
            }
        }

        // Strengths
        public async Task<List<StrengthDto>> GetStrengthsAsync(Guid userId)
        {
            var items = await _context.Strengths
                .Where(s => s.Profile.UserId == userId)
                .ToListAsync();

            return items.Select(s => new StrengthDto
            {
                Id = s.Id,
                Name = s.Name,
                Category = s.Category,
                Evidence = s.Evidence
            }).ToList();
        }

        public async Task<StrengthDto> AddStrengthAsync(Guid userId, StrengthWeaknessRequestDto req)
        {
            var profile = await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) throw new KeyNotFoundException("Profile not found");

            var existing = await _context.Strengths
                .FirstOrDefaultAsync(s => s.ProfileId == profile.Id && s.Name.ToLower() == req.Name.ToLower());
            if (existing != null) throw new InvalidOperationException("Strength already exists");

            var strength = new Strength
            {
                Id = Guid.NewGuid(),
                ProfileId = profile.Id,
                Name = req.Name,
                Category = req.Category,
                Evidence = req.Evidence,
                CreatedAt = DateTime.UtcNow
            };

            _context.Strengths.Add(strength);
            await _context.SaveChangesAsync();

            return new StrengthDto
            {
                Id = strength.Id,
                Name = strength.Name,
                Category = strength.Category,
                Evidence = strength.Evidence
            };
        }

        public async Task DeleteStrengthAsync(Guid userId, Guid strengthId)
        {
            var item = await _context.Strengths
                .FirstOrDefaultAsync(s => s.Id == strengthId && s.Profile.UserId == userId);

            if (item != null)
            {
                _context.Strengths.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        // Weaknesses
        public async Task<List<WeaknessDto>> GetWeaknessesAsync(Guid userId)
        {
            var items = await _context.Weaknesses
                .Where(w => w.Profile.UserId == userId)
                .ToListAsync();

            return items.Select(w => new WeaknessDto
            {
                Id = w.Id,
                Name = w.Name,
                Category = w.Category,
                Evidence = w.Evidence
            }).ToList();
        }

        public async Task<WeaknessDto> AddWeaknessAsync(Guid userId, StrengthWeaknessRequestDto req)
        {
            var profile = await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) throw new KeyNotFoundException("Profile not found");

            var existing = await _context.Weaknesses
                .FirstOrDefaultAsync(w => w.ProfileId == profile.Id && w.Name.ToLower() == req.Name.ToLower());
            if (existing != null) throw new InvalidOperationException("Weakness already exists");

            var weakness = new Weakness
            {
                Id = Guid.NewGuid(),
                ProfileId = profile.Id,
                Name = req.Name,
                Category = req.Category,
                Evidence = req.Evidence,
                CreatedAt = DateTime.UtcNow
            };

            _context.Weaknesses.Add(weakness);
            await _context.SaveChangesAsync();

            return new WeaknessDto
            {
                Id = weakness.Id,
                Name = weakness.Name,
                Category = weakness.Category,
                Evidence = weakness.Evidence
            };
        }

        public async Task DeleteWeaknessAsync(Guid userId, Guid weaknessId)
        {
            var item = await _context.Weaknesses
                .FirstOrDefaultAsync(w => w.Id == weaknessId && w.Profile.UserId == userId);

            if (item != null)
            {
                _context.Weaknesses.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        // Skills
        public async Task<List<StudentSkillDto>> GetSkillsAsync(Guid userId)
        {
            var items = await _context.StudentSkills
                .Include(ss => ss.Skill)
                .Where(ss => ss.Profile.UserId == userId)
                .ToListAsync();

            return items.Select(ss => new StudentSkillDto
            {
                Id = ss.Id,
                SkillId = ss.SkillId,
                SkillName = ss.Skill != null ? ss.Skill.Name : string.Empty,
                Category = ss.Skill != null ? ss.Skill.Category : string.Empty,
                Level = ss.Level,
                YearsExp = ss.YearsExp,
                IsVerified = ss.IsVerified
            }).ToList();
        }

        public async Task<StudentSkillDto> AddSkillAsync(Guid userId, StudentSkillRequestDto req)
        {
            var profile = await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) throw new KeyNotFoundException("Profile not found");

            var skill = await _context.Skills
                .FirstOrDefaultAsync(s => s.Name.ToLower() == req.SkillName.ToLower());

            if (skill == null)
            {
                skill = new Skill
                {
                    Id = Guid.NewGuid(),
                    Name = req.SkillName,
                    Category = "General",
                    CreatedAt = DateTime.UtcNow
                };
                _context.Skills.Add(skill);
                await _context.SaveChangesAsync();
            }

            var studentSkill = await _context.StudentSkills
                .Include(ss => ss.Skill)
                .FirstOrDefaultAsync(ss => ss.ProfileId == profile.Id && ss.SkillId == skill.Id);

            if (studentSkill != null)
            {
                throw new InvalidOperationException("Skill already added to profile");
            }

            studentSkill = new StudentSkill
            {
                Id = Guid.NewGuid(),
                ProfileId = profile.Id,
                SkillId = skill.Id,
                Level = req.Level,
                YearsExp = req.YearsExp,
                IsVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.StudentSkills.Add(studentSkill);
            await _context.SaveChangesAsync();

            await CalculateProfileCompletionAsync(userId);

            return new StudentSkillDto
            {
                Id = studentSkill.Id,
                SkillId = skill.Id,
                SkillName = skill.Name,
                Category = skill.Category,
                Level = studentSkill.Level,
                YearsExp = studentSkill.YearsExp,
                IsVerified = studentSkill.IsVerified
            };
        }

        public async Task DeleteSkillAsync(Guid userId, Guid skillId)
        {
            var item = await _context.StudentSkills
                .FirstOrDefaultAsync(ss => ss.Id == skillId && ss.Profile.UserId == userId);

            if (item != null)
            {
                _context.StudentSkills.Remove(item);
                await _context.SaveChangesAsync();

                await CalculateProfileCompletionAsync(userId);
            }
        }

        public async Task<List<SkillSearchResponseDto>> SearchSkillsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<SkillSearchResponseDto>();
            }

            var skills = await _context.Skills
                .Where(s => s.Name.ToLower().Contains(query.ToLower()))
                .Take(20)
                .ToListAsync();

            return skills.Select(s => new SkillSearchResponseDto
            {
                Id = s.Id,
                Name = s.Name,
                Category = s.Category
            }).ToList();
        }

        // Certifications
        public async Task<List<CertificationDto>> GetCertificationsAsync(Guid userId)
        {
            var items = await _context.Certifications
                .Where(c => c.Profile.UserId == userId)
                .ToListAsync();

            return items.Select(c => new CertificationDto
            {
                Id = c.Id,
                Name = c.Name,
                Issuer = c.Issuer,
                IssueDate = c.IssueDate,
                ExpiryDate = c.ExpiryDate,
                CredentialUrl = c.CredentialUrl
            }).ToList();
        }

        public async Task<CertificationDto> AddCertificationAsync(Guid userId, CertificationRequestDto req)
        {
            var profile = await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) throw new KeyNotFoundException("Profile not found");

            var cert = new Certification
            {
                Id = Guid.NewGuid(),
                ProfileId = profile.Id,
                Name = req.Name,
                Issuer = req.Issuer,
                IssueDate = req.IssueDate,
                ExpiryDate = req.ExpiryDate,
                CredentialUrl = req.CredentialUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.Certifications.Add(cert);
            await _context.SaveChangesAsync();

            return new CertificationDto
            {
                Id = cert.Id,
                Name = cert.Name,
                Issuer = cert.Issuer,
                IssueDate = cert.IssueDate,
                ExpiryDate = cert.ExpiryDate,
                CredentialUrl = cert.CredentialUrl
            };
        }

        public async Task<CertificationDto> UpdateCertificationAsync(Guid userId, Guid certId, CertificationRequestDto req)
        {
            var cert = await _context.Certifications
                .FirstOrDefaultAsync(c => c.Id == certId && c.Profile.UserId == userId);

            if (cert == null) throw new KeyNotFoundException("Certification not found");

            cert.Name = req.Name;
            cert.Issuer = req.Issuer;
            cert.IssueDate = req.IssueDate;
            cert.ExpiryDate = req.ExpiryDate;
            cert.CredentialUrl = req.CredentialUrl;

            await _context.SaveChangesAsync();

            return new CertificationDto
            {
                Id = cert.Id,
                Name = cert.Name,
                Issuer = cert.Issuer,
                IssueDate = cert.IssueDate,
                ExpiryDate = cert.ExpiryDate,
                CredentialUrl = cert.CredentialUrl
            };
        }

        public async Task DeleteCertificationAsync(Guid userId, Guid certId)
        {
            var cert = await _context.Certifications
                .FirstOrDefaultAsync(c => c.Id == certId && c.Profile.UserId == userId);

            if (cert != null)
            {
                _context.Certifications.Remove(cert);
                await _context.SaveChangesAsync();
            }
        }
    }
}
