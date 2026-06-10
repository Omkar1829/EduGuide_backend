namespace EduGuide_Backend.Models
{
    public enum Gender
    {
        [NpgsqlTypes.PgName("MALE")]
        MALE,
        [NpgsqlTypes.PgName("FEMALE")]
        FEMALE,
        [NpgsqlTypes.PgName("OTHER")]
        OTHER,
        [NpgsqlTypes.PgName("PREFER_NOT_TO_SAY")]
        PREFER_NOT_TO_SAY
    }
}
