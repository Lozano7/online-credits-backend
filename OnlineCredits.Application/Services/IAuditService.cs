using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineCredits.Application.DTOs;

namespace OnlineCredits.Application.Services
{
    public interface IAuditService
    {
        Task LogActionAsync(string action, string entityType, int entityId, string details, string userId, string userName);
        Task<IEnumerable<AuditLogDto>> GetAuditLogsAsync();
        Task<IEnumerable<AuditLogDto>> GetAuditLogsByEntityAsync(string entityType, int entityId);
    }
} 