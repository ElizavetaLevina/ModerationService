using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ModerationService.BLL.Interfaces;
using ModerationService.Common.DTO;
using ModerationService.DAL.Models;

namespace ModerationService.DAL.Repositories
{
	/// <summary>
	/// Реализация репозитория для работы с результатами модерации
	/// </summary>
	/// <param name="dbContext">Контекст базы данных</param>
	/// <param name="mapper">Автомаппер</param>
	public class ModerationResultsRepository(ApplicationDbContext dbContext, IMapper mapper) : IModerationResultsRepository
	{
		private readonly ApplicationDbContext _dbContext = dbContext;
		private readonly IMapper _mapper = mapper;

		public async Task<List<ModerationResultDTO>> GetModerationResults(CancellationToken token = default)
		{
			return await _mapper.ProjectTo<ModerationResultDTO>(_dbContext.ModerationResults).ToListAsync(token);
		}

		public async Task SaveModerationResult(ModerationResultDTO postSubmitted, CancellationToken token = default)
		{
			var moderationResult = _mapper.Map<ModerationResultEntity>(postSubmitted);
			_dbContext.Add(moderationResult);
			await _dbContext.SaveChangesAsync(token);
		}

		public async Task DeleteModerationResult(int moderationResultId, CancellationToken token = default)
		{
			_dbContext.ModerationResults.Remove(await _dbContext.ModerationResults.FirstAsync(c => c.Id == moderationResultId, token));
		}
	}
}
