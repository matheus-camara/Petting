using EntityAbstractions.Persistence.Interfaces;
using Notify;

namespace Customers.Infra
{
    internal class ErrorContext : IErrorContext
    {
        private readonly INotificationContext _context;

        public ErrorContext(INotificationContext context)
        {
            _context = context;
        }

        public bool HasErrors()
        {
            return _context.HasNotifications;
        }
    }
}
