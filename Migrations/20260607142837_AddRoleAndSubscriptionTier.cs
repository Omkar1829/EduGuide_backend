using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduGuide_Backend.Migrations
{
    public partial class AddRoleAndSubscriptionTier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Columns role and subscriptionTier already exist in the database.
            // This migration only registers the model changes in EF Core history.
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
