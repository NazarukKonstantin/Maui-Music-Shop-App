namespace COURSE_ASH.Globals;

public class OrderStatus
{
    public const string CANCELLATION_REQUESTED = "Cancellation requested";
    public const string ACCEPTED = "Accepted";
    public const string AWAITING_CONFIRMATION = "Awaiting for confirmation";
    public const string IN_TRANSIT = "In transit";
    public const string COMPLETED = "Completed";
    public const string CANCELED = "Canceled";

    public static List<string> Statuses { get; } = new()
    {
            AWAITING_CONFIRMATION,
            CANCELLATION_REQUESTED,
            ACCEPTED,
            IN_TRANSIT,
            COMPLETED,
            CANCELED,
    };
}
