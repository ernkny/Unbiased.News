using Dapper;
using System;
using System.Data;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.DataAccess.Connections;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Concrete
{
	/// <summary>
	/// Repository for news-related operations.
	/// </summary>
	public class NewsRepository : INewsRepository
	{
		private readonly UnbiasedSqlConnection _connection;
		private readonly IServiceProvider _serviceProvider;
		private readonly IEventAndActivityLog _eventAndActivityLog;

		/// <summary>
		/// Initializes a new instance of the <see cref="NewsRepository"/> class.
		/// </summary>
		/// <param name="connection">The Unbiased SQL connection.</param>
		public NewsRepository(UnbiasedSqlConnection connection, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
		{
			_connection = connection;
			_serviceProvider = serviceProvider;
			_eventAndActivityLog = eventAndActivityLog;
		}

		/// <summary>
		/// Retrieves all generated news asynchronously.
		/// </summary>
		/// <param name="language">The language code to filter news by.</param>
		/// <returns>A collection of <see cref="GeneratedNew"/> objects.</returns>
		public async Task<IEnumerable<GeneratedNew>> GetAllGeneratedNewsAsync(string language)
		{
			try
			{
				using (var connection = _connection.CreateConnection())
				{
					var parameters = new DynamicParameters();
					parameters.Add("@Language", language);
					return await connection.QueryAsync<GeneratedNew>("UB_sp_GetAllGeneratedNews", parameters, commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message} - {exception.StackTrace}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// Retrieves all generated news asynchronously.
		/// </summary>
		/// <param name="language">The language code to filter news by.</param>
		/// <returns>A collection of <see cref="GeneratedNew"/> objects.</returns>
		public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsForSiteMapAsync(string language)
		{
			try
			{
				using (var connection = _connection.CreateConnection())
				{
					var parameters = new DynamicParameters();
					parameters.Add("@Language", language);
					return await connection.QueryAsync<GenerateNewsWithImageDto>("UB_sp_GetRecentNewsForSiteMap", parameters, commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message} - {exception.StackTrace}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Retrieves all generated news with images asynchronously.
		/// </summary>
		/// <param name="categoryId">The category ID to filter news by.</param>
		/// <param name="pageNumber">The page number for pagination.</param>
		/// <param name="language">The language code to filter news by.</param>
		/// <param name="title">Optional title filter for searching news.</param>
		/// <returns>A collection of <see cref="GenerateNewsWithImageDto"/> objects.</returns>
		public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsWithImageAsync(int categoryId, int pageNumber, string language, string? title)
		{
			try
			{
				var parameters = new DynamicParameters();
				parameters.Add("@PageNumber", pageNumber);
				parameters.Add("@CategoryId", categoryId);
				parameters.Add("@Language", language);
				parameters.Add("@Title", title);
				using (var connection = _connection.CreateConnection())
				{
					return await connection.QueryAsync<GenerateNewsWithImageDto>("UB_sp_GetAllGeneratedNewsWithImagePath", parameters, commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message} - {exception.StackTrace}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Gets the count of all generated news items with images for a specific category.
		/// </summary>
		/// <param name="categoryId">The category ID to count news items from.</param>
		/// <param name="title">Optional title filter for counting matching news.</param>
		/// <returns>The count of news items matching the criteria.</returns>
		public async Task<int> GetAllGeneratedNewsWithImageCountAsync(int categoryId, string? title)
		{
			try
			{
				var parameters = new DynamicParameters();
				parameters.Add("@CategoryId", categoryId);
				parameters.Add("@Title", title);
				using (var connection = _connection.CreateConnection())
				{
					return await connection.QueryFirstAsync<int>("UB_sp_GetAllGeneratedNewsWithImagePathCount", parameters, commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message} - {exception.StackTrace}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Retrieves banner news items for a specific category and language.
		/// </summary>
		/// <param name="categoryId">The category ID to retrieve banner news from.</param>
		/// <param name="language">The language of the news items to retrieve.</param>
		/// <returns>A collection of banner news items with images.</returns>
		public async Task<IEnumerable<GenerateNewsWithImageDto>> GetBannerGeneratedNewsWithImageAsync(int categoryId, string language)
		{
			try
			{
				using (var connection = _connection.CreateConnection())
				{
					var parameters = new DynamicParameters();
					parameters.Add("@CategoryId", categoryId);
					parameters.Add("@Language", language);
					return await connection.QueryAsync<GenerateNewsWithImageDto>("UB_sp_GetGeneratedNewsForBannerWithImagePath", parameters, commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message} - {exception.StackTrace}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Retrieves a specific news item by its ID with associated image.
		/// </summary>
		/// <param name="id">The unique identifier of the news item to retrieve.</param>
		/// <returns>The requested news item with its associated image.</returns>
		public async Task<GenerateNewsWithImageDto> GetGeneratedNewsByIdWithImageAsync(string id)
		{
			try
			{
				var parameters = new DynamicParameters();
				parameters.Add("@id", id);
				using (var connection = _connection.CreateConnection())
				{
					return await connection.QueryFirstAsync<GenerateNewsWithImageDto>("UB_sp_GetGeneratedNewsById", parameters, commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message} - {exception.StackTrace}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Retrieves a specific news item by its unique URL with associated image.
		/// </summary>
		/// <param name="uniqUrlPath">The unique URL path of the news item to retrieve.</param>
		/// <returns>The requested news item with its associated image.</returns>
		public async Task<GenerateNewsWithImageDto> GetGeneratedNewsByUniqUrlWithImageAsync(string uniqUrlPath)
		{
			try
			{
				var parameters = new DynamicParameters();
				parameters.Add("@uniqurlpath", uniqUrlPath);
				using (var connection = _connection.CreateConnection())
				{
					return await connection.QueryFirstAsync<GenerateNewsWithImageDto>("UB_sp_GetGeneratedNewsByUniqUrlPath", parameters, commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message} - {exception.StackTrace}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Retrieves the top latest news items from a specific category, excluding the news item with the provided unique URL.
		/// </summary>
		/// <param name="categoryId">The category ID to retrieve news from.</param>
		/// <param name="uniqUrlPath">The unique URL path of the news item to exclude from results.</param>
		/// <param name="language">The language of the news items to retrieve.</param>
		/// <returns>A collection of the latest top news items from the specified category.</returns>
		public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllLastTopGeneratedNewsWithCategoryIdForDetailAsync(int categoryId, string uniqUrlPath, string language)
		{
			try
			{
				var parameters = new DynamicParameters();
				parameters.Add("@id", uniqUrlPath);
				parameters.Add("@categoryid", categoryId);
				parameters.Add("@language", language);
				using (var connection = _connection.CreateConnection())
				{
					return await connection.QueryAsync<GenerateNewsWithImageDto>("UB_sp_GetAllLastTopGeneratedNewsWithCategoryId", parameters, commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message} - {exception.StackTrace}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Retrieves questions and answers related to a specific match.
		/// </summary>
		/// <param name="MatchId">The unique identifier of the match to retrieve Q&A for.</param>
		/// <returns>A collection of questions and answers for the specified match.</returns>
		public async Task<IEnumerable<QuestionAnswerDto>> GetAllQuestionsAndAnswerWithMatchIdAsync(string MatchId)
		{
			try
			{
				var parameters = new DynamicParameters();
				parameters.Add("@MatchId", MatchId);
				using (var connection = _connection.CreateConnection())
				{
					return await connection.QueryAsync<QuestionAnswerDto>("UB_sp_GetAllQuestionsAndAnswersWithMatchId", parameters, commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message} - {exception.StackTrace}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Retrieves statistical information about all news items in the system.
		/// </summary>
		/// <returns>Statistical data about news items.</returns>
		public async Task<StatisticsDto> GetNewsStatisticsAsync()
		{
			try
			{
				using (var connection = _connection.CreateConnection())
				{
					return await connection.QueryFirstAsync<StatisticsDto>("UB_sp_GetStaticsForHomePage", commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message} - {exception.StackTrace}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

	}
}
