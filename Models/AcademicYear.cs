namespace EduGuide_Backend.Models
{
    public enum AcademicYear
    {
        [NpgsqlTypes.PgName("FRESHMAN")]
        FRESHMAN,
        [NpgsqlTypes.PgName("SOPHOMORE")]
        SOPHOMORE,
        [NpgsqlTypes.PgName("JUNIOR")]
        JUNIOR,
        [NpgsqlTypes.PgName("SENIOR")]
        SENIOR,
        [NpgsqlTypes.PgName("GRADUATE")]
        GRADUATE,
        [NpgsqlTypes.PgName("POST_GRADUATE")]
        POST_GRADUATE
    }
}
