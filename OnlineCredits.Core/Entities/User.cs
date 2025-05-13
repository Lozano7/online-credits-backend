using System;
using System.Collections.Generic;

namespace OnlineCredits.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; } // Soliciante o Analista
        public string Status { get; set; } // Activo/Inactivo
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Relaciones
        public ICollection<CreditRequest> CreditRequests { get; set; }
        public ICollection<AuditLog> AuditLogs { get; set; }
    }
} 