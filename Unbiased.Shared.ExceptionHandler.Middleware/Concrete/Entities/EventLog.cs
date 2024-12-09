namespace Unbiased.Shared.ExceptionHandler.Middleware.Entities
{
    /// <summary>
    /// Represents an event log with relevant details.
    /// </summary>
    public class EventLog
    {
        /// <summary>
        /// Unique identifier for the event.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Type of event that occurred (e.g. error, warning, info).
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// Severity level of the event (e.g. critical, high, medium, low).
        /// </summary>
        public string EventSeverity { get; set; }

        /// <summary>
        /// Detailed message describing the event.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Date and time when the event occurred.
        /// </summary>
        public DateTime EventDate { get; set; }
    }
}
