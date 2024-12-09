namespace Unbiased.Shared.ExceptionHandler.Middleware.Entities
{
    /// <summary>
    /// Represents an activity log entity.
    /// </summary>
    public class ActivityLog
    {
        /// <summary>
        /// Gets or sets the unique identifier of the activity.
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user who performed the activity.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the type of action performed (e.g. "Create", "Update", "Delete").
        /// </summary>
        public string ActionType { get; set; }

        /// <summary>
        /// Gets or sets a brief message describing the activity.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the endpoint URL where the activity occurred.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Gets or sets the IP address of the client that made the request.
        /// </summary>
        public string XForwardedFor { get; set; }

        /// <summary>
        /// Gets or sets the referer URL (if any) that led to the activity.
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the activity occurred.
        /// </summary>
        public DateTime ActivityDate { get; set; }

        /// <summary>
        /// Gets or sets the IP address of the client that made the request.
        /// </summary>
        public string IP { get; set; }
    }
}
