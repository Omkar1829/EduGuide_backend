using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduGuide_Backend.Models;

public partial class EgaidbContext : DbContext
{
    public EgaidbContext(DbContextOptions<EgaidbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AcademicRecord> AcademicRecords { get; set; }

    public virtual DbSet<CareerGoal> CareerGoals { get; set; }

    public virtual DbSet<CareerRoadmap> CareerRoadmaps { get; set; }

    public virtual DbSet<Certification> Certifications { get; set; }

    public virtual DbSet<ChatHistory> ChatHistories { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Interest> Interests { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<KnowledgeArticle> KnowledgeArticles { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<QuizResult> QuizResults { get; set; }

    public virtual DbSet<Recommendation> Recommendations { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<ResumeAnalysis> ResumeAnalyses { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<Strength> Strengths { get; set; }

    public virtual DbSet<StudentProfile> StudentProfiles { get; set; }

    public virtual DbSet<StudentSkill> StudentSkills { get; set; }

    public virtual DbSet<SubjectMark> SubjectMarks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCourse> UserCourses { get; set; }

    public virtual DbSet<UserJob> UserJobs { get; set; }

    public virtual DbSet<Weakness> Weaknesses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("AcademicYear", new[] { "FRESHMAN", "SOPHOMORE", "JUNIOR", "SENIOR", "GRADUATE", "POST_GRADUATE" })
            .HasPostgresEnum("ChatRole", new[] { "USER", "ASSISTANT", "SYSTEM" })
            .HasPostgresEnum("Gender", new[] { "MALE", "FEMALE", "OTHER", "PREFER_NOT_TO_SAY" })
            .HasPostgresEnum("NotificationType", new[] { "RECOMMENDATION", "QUIZ_RESULT", "ROADMAP_UPDATE", "COURSE_UPDATE", "JOB_ALERT", "SYSTEM", "CHAT" })
            .HasPostgresEnum("QuizCategory", new[] { "CAREER_INTEREST", "PERSONALITY", "SKILL_ASSESSMENT", "APTITUDE", "LEARNING_STYLE" })
            .HasPostgresEnum("RecommendationStatus", new[] { "PENDING", "ACCEPTED", "REJECTED", "EXPIRED" })
            .HasPostgresEnum("RecommendationType", new[] { "CAREER", "STREAM", "COURSE", "SKILL", "JOB" })
            .HasPostgresEnum("ResumeStatus", new[] { "PENDING", "ANALYZED", "FAILED" })
            .HasPostgresEnum("SubscriptionTier", new[] { "NEWBIE", "PRO", "PRO_PLUS" })
            .HasPostgresEnum("UserRole", new[] { "STUDENT", "ADMIN" });

        modelBuilder.Entity<AcademicRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("academic_records_pkey");

            entity.ToTable("academic_records");

            entity.HasIndex(e => e.ProfileId, "academic_records_profileId_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Degree).HasColumnName("degree");
            entity.Property(e => e.EndYear).HasColumnName("endYear");
            entity.Property(e => e.FieldOfStudy).HasColumnName("fieldOfStudy");
            entity.Property(e => e.Gpa).HasColumnName("gpa");
            entity.Property(e => e.Institution).HasColumnName("institution");
            entity.Property(e => e.IsCurrent).HasColumnName("isCurrent");
            entity.Property(e => e.Percentage).HasColumnName("percentage");
            entity.Property(e => e.ProfileId).HasColumnName("profileId");
            entity.Property(e => e.StartYear).HasColumnName("startYear");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Profile).WithMany(p => p.AcademicRecords)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("academic_records_profileId_fkey");
        });

        modelBuilder.Entity<CareerGoal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("career_goals_pkey");

            entity.ToTable("career_goals");

            entity.HasIndex(e => e.ProfileId, "career_goals_profileId_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Priority)
                .HasDefaultValue(1)
                .HasColumnName("priority");
            entity.Property(e => e.ProfileId).HasColumnName("profileId");
            entity.Property(e => e.TargetYear).HasColumnName("targetYear");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Profile).WithMany(p => p.CareerGoals)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("career_goals_profileId_fkey");
        });

        modelBuilder.Entity<CareerRoadmap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("career_roadmaps_pkey");

            entity.ToTable("career_roadmaps");

            entity.HasIndex(e => e.UserId, "career_roadmaps_userId_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsCompleted).HasColumnName("isCompleted");
            entity.Property(e => e.Phases)
                .HasColumnType("jsonb")
                .HasColumnName("phases");
            entity.Property(e => e.Progress).HasColumnName("progress");
            entity.Property(e => e.TargetCareer).HasColumnName("targetCareer");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.CareerRoadmaps)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("career_roadmaps_userId_fkey");
        });

        modelBuilder.Entity<Certification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("certifications_pkey");

            entity.ToTable("certifications");

            entity.HasIndex(e => e.ProfileId, "certifications_profileId_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.CredentialUrl).HasColumnName("credentialUrl");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("expiryDate");
            entity.Property(e => e.IssueDate)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("issueDate");
            entity.Property(e => e.Issuer).HasColumnName("issuer");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.ProfileId).HasColumnName("profileId");

            entity.HasOne(d => d.Profile).WithMany(p => p.Certifications)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("certifications_profileId_fkey");
        });

        modelBuilder.Entity<ChatHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chat_history_pkey");

            entity.ToTable("chat_history");

            entity.HasIndex(e => new { e.UserId, e.CreatedAt }, "chat_history_userId_createdAt_idx");

            entity.HasIndex(e => new { e.UserId, e.SessionId }, "chat_history_userId_sessionId_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Metadata)
                .HasColumnType("jsonb")
                .HasColumnName("metadata");
            entity.Property(e => e.SessionId).HasColumnName("sessionId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.ChatHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("chat_history_userId_fkey");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("courses_pkey");

            entity.ToTable("courses");

            entity.HasIndex(e => e.Category, "courses_category_idx");

            entity.HasIndex(e => e.IsActive, "courses_isActive_idx");

            entity.HasIndex(e => e.Provider, "courses_provider_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Currency)
                .HasDefaultValueSql("'INR'::text")
                .HasColumnName("currency");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.EnrolledCount).HasColumnName("enrolledCount");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Provider).HasColumnName("provider");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
            entity.Property(e => e.Url).HasColumnName("url");
        });

        modelBuilder.Entity<Interest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("interests_pkey");

            entity.ToTable("interests");

            entity.HasIndex(e => e.ProfileId, "interests_profileId_idx");

            entity.HasIndex(e => new { e.ProfileId, e.Name }, "interests_profileId_name_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Level)
                .HasDefaultValue(1)
                .HasColumnName("level");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.ProfileId).HasColumnName("profileId");

            entity.HasOne(d => d.Profile).WithMany(p => p.Interests)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("interests_profileId_fkey");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("jobs_pkey");

            entity.ToTable("jobs");

            entity.HasIndex(e => e.Category, "jobs_category_idx");

            entity.HasIndex(e => e.Company, "jobs_company_idx");

            entity.HasIndex(e => e.IsActive, "jobs_isActive_idx");

            entity.HasIndex(e => e.PostedAt, "jobs_postedAt_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Company).HasColumnName("company");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Experience).HasColumnName("experience");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.Location).HasColumnName("location");
            entity.Property(e => e.PostedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("postedAt");
            entity.Property(e => e.SalaryRange).HasColumnName("salaryRange");
            entity.Property(e => e.Skills).HasColumnName("skills");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Type)
                .HasDefaultValueSql("'full-time'::text")
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
            entity.Property(e => e.Url).HasColumnName("url");
        });

        modelBuilder.Entity<KnowledgeArticle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("knowledge_articles_pkey");

            entity.ToTable("knowledge_articles");

            entity.HasIndex(e => e.Category, "knowledge_articles_category_idx");

            entity.HasIndex(e => e.Industry, "knowledge_articles_industry_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Industry).HasColumnName("industry");
            entity.Property(e => e.Summary).HasColumnName("summary");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
            entity.Property(e => e.Url).HasColumnName("url");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("notifications_pkey");

            entity.ToTable("notifications");

            entity.HasIndex(e => new { e.UserId, e.CreatedAt }, "notifications_userId_createdAt_idx");

            entity.HasIndex(e => new { e.UserId, e.IsRead }, "notifications_userId_isRead_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Data)
                .HasColumnType("jsonb")
                .HasColumnName("data");
            entity.Property(e => e.IsRead).HasColumnName("isRead");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.ReadAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("readAt");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("notifications_userId_fkey");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quizzes_pkey");

            entity.ToTable("quizzes");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CompletedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("completedAt");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.MaxScore).HasColumnName("maxScore");
            entity.Property(e => e.Questions)
                .HasColumnType("jsonb")
                .HasColumnName("questions");
            entity.Property(e => e.StartedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("startedAt");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'in_progress'::text")
                .HasColumnName("status");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.TotalScore).HasColumnName("totalScore");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Quizzes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("quizzes_userId_fkey");
        });

        modelBuilder.Entity<QuizResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quiz_results_pkey");

            entity.ToTable("quiz_results");

            entity.HasIndex(e => e.QuizId, "quiz_results_quizId_idx");

            entity.HasIndex(e => e.UserId, "quiz_results_userId_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Analysis)
                .HasColumnType("jsonb")
                .HasColumnName("analysis");
            entity.Property(e => e.Answers)
                .HasColumnType("jsonb")
                .HasColumnName("answers");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.MaxScore).HasColumnName("maxScore");
            entity.Property(e => e.Percentage).HasColumnName("percentage");
            entity.Property(e => e.QuizId).HasColumnName("quizId");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Quiz).WithMany(p => p.QuizResults)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("quiz_results_quizId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.QuizResults)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("quiz_results_userId_fkey");
        });

        modelBuilder.Entity<Recommendation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("recommendations_pkey");

            entity.ToTable("recommendations");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Confidence).HasColumnName("confidence");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("expiresAt");
            entity.Property(e => e.Metadata)
                .HasColumnType("jsonb")
                .HasColumnName("metadata");
            entity.Property(e => e.Reasoning)
                .HasColumnType("jsonb")
                .HasColumnName("reasoning");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Recommendations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("recommendations_userId_fkey");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("refresh_tokens_pkey");

            entity.ToTable("refresh_tokens");

            entity.HasIndex(e => e.Token, "refresh_tokens_token_idx");

            entity.HasIndex(e => e.Token, "refresh_tokens_token_key").IsUnique();

            entity.HasIndex(e => e.UserId, "refresh_tokens_userId_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("expiresAt");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("refresh_tokens_userId_fkey");
        });

        modelBuilder.Entity<ResumeAnalysis>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("resume_analysis_pkey");

            entity.ToTable("resume_analysis");

            entity.HasIndex(e => e.UserId, "resume_analysis_userId_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Analysis)
                .HasColumnType("jsonb")
                .HasColumnName("analysis");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Feedback)
                .HasColumnType("jsonb")
                .HasColumnName("feedback");
            entity.Property(e => e.FileName).HasColumnName("fileName");
            entity.Property(e => e.FileUrl).HasColumnName("fileUrl");
            entity.Property(e => e.ParsedContent)
                .HasColumnType("jsonb")
                .HasColumnName("parsedContent");
            entity.Property(e => e.Recommendations)
                .HasColumnType("jsonb")
                .HasColumnName("recommendations");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.ResumeAnalyses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("resume_analysis_userId_fkey");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("skills_pkey");

            entity.ToTable("skills");

            entity.HasIndex(e => e.Category, "skills_category_idx");

            entity.HasIndex(e => e.Name, "skills_name_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Strength>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("strengths_pkey");

            entity.ToTable("strengths");

            entity.HasIndex(e => e.ProfileId, "strengths_profileId_idx");

            entity.HasIndex(e => new { e.ProfileId, e.Name }, "strengths_profileId_name_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Evidence).HasColumnName("evidence");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.ProfileId).HasColumnName("profileId");

            entity.HasOne(d => d.Profile).WithMany(p => p.Strengths)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("strengths_profileId_fkey");
        });

        modelBuilder.Entity<StudentProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("student_profiles_pkey");

            entity.ToTable("student_profiles");

            entity.HasIndex(e => e.UserId, "student_profiles_userId_idx");

            entity.HasIndex(e => e.UserId, "student_profiles_userId_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Bio).HasColumnName("bio");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.CompletionPct).HasColumnName("completionPct");
            entity.Property(e => e.Country)
                .HasDefaultValueSql("'India'::text")
                .HasColumnName("country");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("dateOfBirth");
            entity.Property(e => e.PhoneNumber).HasColumnName("phoneNumber");
            entity.Property(e => e.ProfileComplete).HasColumnName("profileComplete");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithOne(p => p.StudentProfile)
                .HasForeignKey<StudentProfile>(d => d.UserId)
                .HasConstraintName("student_profiles_userId_fkey");
        });

        modelBuilder.Entity<StudentSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("student_skills_pkey");

            entity.ToTable("student_skills");

            entity.HasIndex(e => e.ProfileId, "student_skills_profileId_idx");

            entity.HasIndex(e => new { e.ProfileId, e.SkillId }, "student_skills_profileId_skillId_key").IsUnique();

            entity.HasIndex(e => e.SkillId, "student_skills_skillId_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.IsVerified).HasColumnName("isVerified");
            entity.Property(e => e.Level)
                .HasDefaultValue(1)
                .HasColumnName("level");
            entity.Property(e => e.ProfileId).HasColumnName("profileId");
            entity.Property(e => e.SkillId).HasColumnName("skillId");
            entity.Property(e => e.YearsExp).HasColumnName("yearsExp");

            entity.HasOne(d => d.Profile).WithMany(p => p.StudentSkills)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("student_skills_profileId_fkey");

            entity.HasOne(d => d.Skill).WithMany(p => p.StudentSkills)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("student_skills_skillId_fkey");
        });

        modelBuilder.Entity<SubjectMark>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("subject_marks_pkey");

            entity.ToTable("subject_marks");

            entity.HasIndex(e => e.AcademicRecordId, "subject_marks_academicRecordId_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AcademicRecordId).HasColumnName("academicRecordId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Grade).HasColumnName("grade");
            entity.Property(e => e.Marks).HasColumnName("marks");
            entity.Property(e => e.MaxMarks)
                .HasDefaultValue(100.0)
                .HasColumnName("maxMarks");
            entity.Property(e => e.SubjectName).HasColumnName("subjectName");

            entity.HasOne(d => d.AcademicRecord).WithMany(p => p.SubjectMarks)
                .HasForeignKey(d => d.AcademicRecordId)
                .HasConstraintName("subject_marks_academicRecordId_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_idx");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AvatarUrl).HasColumnName("avatarUrl");
            entity.Property(e => e.ChatLimitRemaining)
                .HasDefaultValue(5)
                .HasColumnName("chatLimitRemaining");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FirstName).HasColumnName("firstName");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsVerified).HasColumnName("isVerified");
            entity.Property(e => e.LastLimitReset)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("lastLimitReset");
            entity.Property(e => e.LastLoginAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("lastLoginAt");
            entity.Property(e => e.LastName).HasColumnName("lastName");
            entity.Property(e => e.PasswordHash).HasColumnName("passwordHash");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
        });

        modelBuilder.Entity<UserCourse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_courses_pkey");

            entity.ToTable("user_courses");

            entity.HasIndex(e => e.CourseId, "user_courses_courseId_idx");

            entity.HasIndex(e => new { e.UserId, e.CourseId }, "user_courses_userId_courseId_key").IsUnique();

            entity.HasIndex(e => e.UserId, "user_courses_userId_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CompletedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("completedAt");
            entity.Property(e => e.CourseId).HasColumnName("courseId");
            entity.Property(e => e.EnrolledAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("enrolledAt");
            entity.Property(e => e.Progress).HasColumnName("progress");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'enrolled'::text")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Course).WithMany(p => p.UserCourses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("user_courses_courseId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserCourses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_courses_userId_fkey");
        });

        modelBuilder.Entity<UserJob>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_jobs_pkey");

            entity.ToTable("user_jobs");

            entity.HasIndex(e => e.JobId, "user_jobs_jobId_idx");

            entity.HasIndex(e => e.UserId, "user_jobs_userId_idx");

            entity.HasIndex(e => new { e.UserId, e.JobId }, "user_jobs_userId_jobId_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AppliedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("appliedAt");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.JobId).HasColumnName("jobId");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'saved'::text")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Job).WithMany(p => p.UserJobs)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("user_jobs_jobId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserJobs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_jobs_userId_fkey");
        });

        modelBuilder.Entity<Weakness>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("weaknesses_pkey");

            entity.ToTable("weaknesses");

            entity.HasIndex(e => e.ProfileId, "weaknesses_profileId_idx");

            entity.HasIndex(e => new { e.ProfileId, e.Name }, "weaknesses_profileId_name_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Evidence).HasColumnName("evidence");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.ProfileId).HasColumnName("profileId");

            entity.HasOne(d => d.Profile).WithMany(p => p.Weaknesses)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("weaknesses_profileId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
