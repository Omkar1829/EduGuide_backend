namespace EduGuide_Backend.Models;

public enum UserRole
{
    [NpgsqlTypes.PgName("STUDENT")]
    STUDENT,
    [NpgsqlTypes.PgName("ADMIN")]
    ADMIN
}
