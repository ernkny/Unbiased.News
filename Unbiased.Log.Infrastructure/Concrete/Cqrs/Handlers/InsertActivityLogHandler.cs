using MediatR;
using Unbiased.Log.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Log.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Log.Infrastructure.Concrete.Cqrs.Handlers
{
    /// <summary>
    /// Handles the InsertActivityLogCommand by inserting the activity log into the repository.
    /// </summary>
    public class InsertActivityLogHandler : IRequestHandler<InsertActivityLogCommand,bool>
    {
        private readonly ILogRepository _logRepository;

        /// <summary>
        /// Initializes a new instance of the InsertActivityLogHandler class.
        /// </summary>
        /// <param name="logRepository">The repository for logging operations.</param>
        public InsertActivityLogHandler(ILogRepository? logRepository)
        {
            _logRepository = logRepository;
        }

        /// <summary>
        /// Handles the InsertActivityLogCommand by inserting the activity log into the repository.
        /// </summary>
        /// <param name="request">The command to insert the activity log.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the log was inserted successfully.</returns>
        public async Task<bool> Handle(InsertActivityLogCommand request, CancellationToken cancellationToken) 
        {
            var result = await _logRepository.InsertActivityLogAsync(request.ActivityLog);
            return result;
        }
    }
}
