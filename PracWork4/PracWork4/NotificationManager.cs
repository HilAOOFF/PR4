namespace PracWork4
{
    public class NotificationManager
    {
        public IEnumerable<Notification> FilterAndSort(IEnumerable<Notification> notifications, NotificationFilterOptions options)
        {
            if (notifications == null) throw new ArgumentNullException(nameof(notifications), "Список не может быть null.");
            if (options == null) return notifications;

            var query = notifications.AsQueryable();

            //фильтрация
            if (options.IsRead.HasValue) query = query.Where(n => n.IsRead == options.IsRead.Value);
            if (options.Types != null && options.Types.Length > 0) query = query.Where(n => options.Types.Contains(n.Type));
            if (!string.IsNullOrWhiteSpace(options.SearchText))
            {
                query = query.Where(n => (n.Title != null && n.Title.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase)) ||
                                         (n.Content != null && n.Content.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase)));
            }
            if (options.MinPriority.HasValue) query = query.Where(n => n.Priority >= options.MinPriority.Value);

            //сортировка
            if (options.SortBy.HasValue)
            {
                switch (options.SortBy.Value)
                {
                    case SortNotificationBy.Date: query = options.Descending ? query.OrderByDescending(n => n.CreatedAt) : query.OrderBy(n => n.CreatedAt); break;
                    case SortNotificationBy.Priority: query = options.Descending ? query.OrderByDescending(n => n.Priority) : query.OrderBy(n => n.Priority); break;
                    case SortNotificationBy.Title: query = options.Descending ? query.OrderByDescending(n => n.Title) : query.OrderBy(n => n.Title); break;
                }
            }

            return query.ToList();
        }
    }
}
