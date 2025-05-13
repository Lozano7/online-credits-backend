using System;

namespace OnlineCredits.Core.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Action { get; set; }  // CREATE, UPDATE, DELETE, STATUS_CHANGE
        public string EntityType { get; set; }  // CreditRequest, User, etc.
        public int EntityId { get; set; }
        public string Details { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Timestamp { get; set; }
    }
} 