namespace EduGuide_Backend.Models;

public enum SubscriptionTier
{
    [NpgsqlTypes.PgName("NEWBIE")]
    NEWBIE,
    [NpgsqlTypes.PgName("PRO")]
    PRO,
    [NpgsqlTypes.PgName("PRO_PLUS")]
    PRO_PLUS
}
