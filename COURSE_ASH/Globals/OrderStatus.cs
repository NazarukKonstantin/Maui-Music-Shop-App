namespace COURSE_ASH.Globals;

public class OrderStatus
{
    public const string DELIVERED = "Delivered";
    public const string CANCELLATION_REQUESTED = "Cancellation requested";
    public const string ACCEPTED = "Accepted";
    public const string AWAITING_CONFIRMATION = "Awaiting for confirmation";
    public const string IN_TRANSIT = "In transit";
    public const string READY = "Ready";
    public const string CANCELED = "Canceled";

    public static List<string> Statuses { get; } = new()
    {
            AWAITING_CONFIRMATION,
            DELIVERED,
            CANCELLATION_REQUESTED,
            ACCEPTED,
            IN_TRANSIT,
            READY,
            CANCELED,
    };
}
