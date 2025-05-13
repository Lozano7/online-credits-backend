using System;

namespace OnlineCredits.Application.DTOs
{
    public class AuditLogDto
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string Details { get; set; }
        public string UserName { get; set; }
        public DateTime Timestamp { get; set; }
    }
} 