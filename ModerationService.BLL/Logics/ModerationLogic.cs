using BogaNet.BWF;
using ModerationService.BLL.Interfaces;

namespace ModerationService.BLL.Logics
{
	public class ModerationLogic : IModerationLogic
	{
		public bool IsApproved(string title, string textPost)
		{
			var fullText = $"{title} {textPost}";
			return !Pacifier.Instance.Contains(fullText);
		}
	}
}
