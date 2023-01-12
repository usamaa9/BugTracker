namespace PurpleRock.BugTracker.Infrastructure.Persistence.Dto;

public class BugDto : Item
{
    /// <summary>
    /// Title of the bug.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Description of the bug.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Date the bug was opened.
    /// </summary>
    public DateTime? DateOpened { get; set; }

    /// <summary>
    /// Date the bug was closed.
    /// </summary>
    public DateTime? DateClosed { get; set; }

    /// <summary>
    /// The id of the person the bug is assigned to.
    /// </summary>
    public string? PersonAssignedId { get; set; }
}
