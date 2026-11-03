namespace ModerationService.BLL.Interfaces
{
	public interface ICommentModerationLogic
	{
		Task CommentModeration(CancellationToken token);
	}
}