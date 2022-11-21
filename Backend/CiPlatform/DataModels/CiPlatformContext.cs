using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CiPlatform.DataModels;

public partial class CiPlatformContext : DbContext
{
    public CiPlatformContext()
    {
    }

    public CiPlatformContext(DbContextOptions<CiPlatformContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CmsPage> CmsPages { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<FavouriteMission> FavouriteMissions { get; set; }

    public virtual DbSet<GoalMission> GoalMissions { get; set; }

    public virtual DbSet<Mission> Missions { get; set; }

    public virtual DbSet<MissionApplication> MissionApplications { get; set; }

    public virtual DbSet<MissionDocument> MissionDocuments { get; set; }

    public virtual DbSet<MissionInvite> MissionInvites { get; set; }

    public virtual DbSet<MissionMedium> MissionMedia { get; set; }

    public virtual DbSet<MissionRating> MissionRatings { get; set; }

    public virtual DbSet<MissionSkill> MissionSkills { get; set; }

    public virtual DbSet<MissionTheme> MissionThemes { get; set; }

    public virtual DbSet<PasswordReset> PasswordResets { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Story> Stories { get; set; }

    public virtual DbSet<StoryInvite> StoryInvites { get; set; }

    public virtual DbSet<StoryMedium> StoryMedia { get; set; }

    public virtual DbSet<Timesheet> Timesheets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSkill> UserSkills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-SII466TJ;Database=CiPlatform;Trusted_Connection=True;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.ToTable("admin");

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(16)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(16)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.ToTable("banner");

            entity.Property(e => e.BannerId).HasColumnName("banner_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Image)
                .HasMaxLength(512)
                .HasColumnName("image");
            entity.Property(e => e.SortOrder).HasColumnName("sort_order");
            entity.Property(e => e.Text)
                .HasColumnType("text")
                .HasColumnName("text");
            entity.Property(e => e.Title)
                .HasMaxLength(512)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("city");

            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.StateId).HasColumnName("state_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_city_country");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_city_state");
        });

        modelBuilder.Entity<CmsPage>(entity =>
        {
            entity.ToTable("cms_page");

            entity.Property(e => e.CmsPageId).HasColumnName("cms_page_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("comment");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.ApprovalStatus).HasColumnName("approval_status");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.Comments)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_comment_mission");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_comment_user");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("country");

            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Iso)
                .HasMaxLength(16)
                .HasColumnName("iso");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<FavouriteMission>(entity =>
        {
            entity.ToTable("favourite_mission");

            entity.Property(e => e.FavouriteMissionId).HasColumnName("favourite_mission_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.FavouriteMissions)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_favourite_mission_mission");

            entity.HasOne(d => d.User).WithMany(p => p.FavouriteMissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_favourite_mission_user");
        });

        modelBuilder.Entity<GoalMission>(entity =>
        {
            entity.ToTable("goal_mission");

            entity.Property(e => e.GoalMissionId).HasColumnName("goal_mission_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.GoalObjectiveText)
                .HasMaxLength(255)
                .HasColumnName("goal_objective_text");
            entity.Property(e => e.GoalValue).HasColumnName("goal_value");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.GoalMissions)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_goal_mission_mission");
        });

        modelBuilder.Entity<Mission>(entity =>
        {
            entity.ToTable("mission");

            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.Availability).HasColumnName("availability");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.MissionType).HasColumnName("mission_type");
            entity.Property(e => e.OrganizationDetail)
                .HasColumnType("text")
                .HasColumnName("organization_detail");
            entity.Property(e => e.OrganizationName)
                .HasMaxLength(255)
                .HasColumnName("organization_name");
            entity.Property(e => e.ShortDescription)
                .HasColumnType("text")
                .HasColumnName("short_description");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.ThemeId).HasColumnName("theme_id");
            entity.Property(e => e.Title)
                .HasMaxLength(128)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Country).WithMany(p => p.Missions)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mission_country");

            entity.HasOne(d => d.Theme).WithMany(p => p.Missions)
                .HasForeignKey(d => d.ThemeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mission_mission_theme");
        });

        modelBuilder.Entity<MissionApplication>(entity =>
        {
            entity.ToTable("mission_application");

            entity.Property(e => e.MissionApplicationId).HasColumnName("mission_application_id");
            entity.Property(e => e.AppliedAt)
                .HasColumnType("datetime")
                .HasColumnName("applied_at");
            entity.Property(e => e.ApprovalStatus).HasColumnName("approval_status");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionApplications)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mission_application_mission");

            entity.HasOne(d => d.User).WithMany(p => p.MissionApplications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mission_application_user");
        });

        modelBuilder.Entity<MissionDocument>(entity =>
        {
            entity.ToTable("mission_document");

            entity.Property(e => e.MissionDocumentId).HasColumnName("mission_document_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DocumentName)
                .HasMaxLength(255)
                .HasColumnName("document_name");
            entity.Property(e => e.DocumentPath)
                .HasMaxLength(255)
                .HasColumnName("document_path");
            entity.Property(e => e.DocumentType)
                .HasMaxLength(255)
                .HasColumnName("document_type");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionDocuments)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mission_document_mission");
        });

        modelBuilder.Entity<MissionInvite>(entity =>
        {
            entity.ToTable("mission_invite");

            entity.Property(e => e.MissionInviteId).HasColumnName("mission_invite_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.FromUserId).HasColumnName("from_user_id");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.ToUserId).HasColumnName("to_user_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.FromUser).WithMany(p => p.MissionInviteFromUsers)
                .HasForeignKey(d => d.FromUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mission_invite_user");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionInvites)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mission_invite_mission");

            entity.HasOne(d => d.ToUser).WithMany(p => p.MissionInviteToUsers)
                .HasForeignKey(d => d.ToUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mission_invite_user1");
        });

        modelBuilder.Entity<MissionMedium>(entity =>
        {
            entity.HasKey(e => e.MissionMediaId);

            entity.ToTable("mission_media");

            entity.Property(e => e.MissionMediaId).HasColumnName("mission_media_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MediaName)
                .HasMaxLength(64)
                .HasColumnName("media_name");
            entity.Property(e => e.MediaPath)
                .HasMaxLength(255)
                .HasColumnName("media_path");
            entity.Property(e => e.MediaType)
                .HasMaxLength(4)
                .HasColumnName("media_type");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionMedia)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mission_media_mission");
        });

        modelBuilder.Entity<MissionRating>(entity =>
        {
            entity.ToTable("mission_rating");

            entity.Property(e => e.MissionRatingId).HasColumnName("mission_rating_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionRatings)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mission_rating_mission");

            entity.HasOne(d => d.User).WithMany(p => p.MissionRatings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mission_rating_user");
        });

        modelBuilder.Entity<MissionSkill>(entity =>
        {
            entity.ToTable("mission_skill");

            entity.Property(e => e.MissionSkillId).HasColumnName("mission_skill_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.SkillId).HasColumnName("skill_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionSkills)
                .HasForeignKey(d => d.MissionId)
                .HasConstraintName("FK_mission_skill_mission1");

            entity.HasOne(d => d.Skill).WithMany(p => p.MissionSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mission_skill_mission");
        });

        modelBuilder.Entity<MissionTheme>(entity =>
        {
            entity.ToTable("mission_theme");

            entity.Property(e => e.MissionThemeId).HasColumnName("mission_theme_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<PasswordReset>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("password_reset");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(191)
                .HasColumnName("email");
            entity.Property(e => e.Token)
                .HasMaxLength(191)
                .HasColumnName("token");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.ToTable("skill");

            entity.Property(e => e.SkillId).HasColumnName("skill_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.SkillName)
                .HasMaxLength(64)
                .HasColumnName("skill_name");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.ToTable("state");

            entity.Property(e => e.StateId).HasColumnName("state_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_state_country");
        });

        modelBuilder.Entity<Story>(entity =>
        {
            entity.ToTable("story");

            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.PublishedAt)
                .HasColumnType("datetime")
                .HasColumnName("published_at");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.Stories)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_story_mission");

            entity.HasOne(d => d.User).WithMany(p => p.Stories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_story_user");
        });

        modelBuilder.Entity<StoryInvite>(entity =>
        {
            entity.ToTable("story_invite");

            entity.Property(e => e.StoryInviteId).HasColumnName("story_invite_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.FromUserId).HasColumnName("from_user_id");
            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.ToUserId).HasColumnName("to_user_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<StoryMedium>(entity =>
        {
            entity.HasKey(e => e.StoryMediaId);

            entity.ToTable("story_media");

            entity.Property(e => e.StoryMediaId).HasColumnName("story_media_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Path)
                .HasColumnType("text")
                .HasColumnName("path");
            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.Type)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Story).WithMany(p => p.StoryMedia)
                .HasForeignKey(d => d.StoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_story_media_story");
        });

        modelBuilder.Entity<Timesheet>(entity =>
        {
            entity.ToTable("timesheet");

            entity.Property(e => e.TimesheetId).HasColumnName("timesheet_id");
            entity.Property(e => e.Action).HasColumnName("action");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DateVolunteered)
                .HasColumnType("datetime")
                .HasColumnName("date_volunteered");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.Notes)
                .HasColumnType("text")
                .HasColumnName("notes");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.Timesheets)
                .HasForeignKey(d => d.MissionId)
                .HasConstraintName("FK_timesheet_mission");

            entity.HasOne(d => d.User).WithMany(p => p.Timesheets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_timesheet_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Avatar)
                .HasMaxLength(2048)
                .HasColumnName("avatar");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Department)
                .HasMaxLength(16)
                .HasColumnName("department");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .HasColumnName("email");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(16)
                .HasColumnName("employee_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(16)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(16)
                .HasColumnName("last_name");
            entity.Property(e => e.LinkedInUrl)
                .HasMaxLength(255)
                .HasColumnName("linked_in_url");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
            entity.Property(e => e.ProfileText)
                .HasColumnType("text")
                .HasColumnName("profile_text");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.WhyIVolunteer)
                .HasColumnType("text")
                .HasColumnName("why_i_volunteer");

            entity.HasOne(d => d.City).WithMany(p => p.Users)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_city");

            entity.HasOne(d => d.Country).WithMany(p => p.Users)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_country");
        });

        modelBuilder.Entity<UserSkill>(entity =>
        {
            entity.ToTable("user_skill");

            entity.Property(e => e.UserSkillId).HasColumnName("user_skill_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.SkillId).HasColumnName("skill_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Skill).WithMany(p => p.UserSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_skill_skill");

            entity.HasOne(d => d.User).WithMany(p => p.UserSkills)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_skill_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
