using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCredits.Application.DTOs;
using OnlineCredits.Application.Services;
using OnlineCredits.Core.Entities;
using OnlineCredits.Infrastructure.Data;

namespace OnlineCredits.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly AppDbContext _context;

        public AuditService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(string action, string entityType, int entityId, string details, string userId, string userName)
        {
            var auditLog = new AuditLog
            {
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                Details = details,
                UserId = userId,
                UserName = userName,
                Timestamp = DateTime.UtcNow
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLogDto>> GetAuditLogsAsync()
        {
            return await _context.AuditLogs
                .OrderByDescending(a => a.Timestamp)
                .Select(a => new AuditLogDto
                {
                    Id = a.Id,
                    Action = a.Action,
                    EntityType = a.EntityType,
                    EntityId = a.EntityId,
                    Details = a.Details,
                    UserName = a.UserName,
                    Timestamp = a.Timestamp
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLogDto>> GetAuditLogsByEntityAsync(string entityType, int entityId)
        {
            return await _context.AuditLogs
                .Where(a => a.EntityType == entityType && a.EntityId == entityId)
                .OrderByDescending(a => a.Timestamp)
                .Select(a => new AuditLogDto
                {
                    Id = a.Id,
                    Action = a.Action,
                    EntityType = a.EntityType,
                    EntityId = a.EntityId,
                    Details = a.Details,
                    UserName = a.UserName,
                    Timestamp = a.Timestamp
                })
                .ToListAsync();
        }
    }
} 