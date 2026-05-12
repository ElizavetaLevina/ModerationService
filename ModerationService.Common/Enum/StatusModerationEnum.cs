namespace ModerationService.Common.Enum
{
    /// <summary>
    /// Статус модерации для поста
    /// </summary>
    public enum StatusModerationEnum
    {
        /// <summary>
        /// В ожидании
        /// </summary>
        Pending,

        /// <summary>
        /// Одобрено
        /// </summary>
        Approved,

        /// <summary>
        /// Отклонено
        /// </summary>
        Rejected
    }
}
