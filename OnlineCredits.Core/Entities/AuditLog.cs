using System;

namespace OnlineCredits.Core.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; } // Creación/Actualización/Cambio de Estado
        public string EntityType { get; set; } // Tipo de entidad afectada
        public int EntityId { get; set; } // Id de la entidad afectada
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; }
        public string IPAddress { get; set; }

        // Relaciones
        public User User { get; set; }
    }
} 