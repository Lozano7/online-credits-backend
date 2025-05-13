using System;

namespace OnlineCredits.Core.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public int CreditRequestId { get; set; }
        public string DocumentType { get; set; } // Ej: Boleta de pago, DNI, etc.
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        public string Status { get; set; } // Pendiente/Aprobado/Rechazado
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Relaciones
        public CreditRequest CreditRequest { get; set; }
    }
} 