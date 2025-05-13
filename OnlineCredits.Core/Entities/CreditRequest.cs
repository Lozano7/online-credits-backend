using System;
using System.Collections.Generic;

namespace OnlineCredits.Core.Entities
{
    public class CreditRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public int TermInMonths { get; set; }
        public decimal MonthlyIncome { get; set; }
        public int WorkSeniority { get; set; } // años
        public string EmploymentType { get; set; } // Dependiente/Independiente
        public decimal CurrentDebt { get; set; }
        public string Purpose { get; set; }
        public string Status { get; set; } // Pendiente/Aprobado/Rechazado
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? EvaluatedBy { get; set; }
        public DateTime? EvaluationDate { get; set; }
        public string? RejectionReason { get; set; }
        public decimal? ApprovedAmount { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? MonthlyPayment { get; set; }

        // Relaciones
        public virtual User? User { get; set; }
        public virtual User? Analyst { get; set; }
        public virtual ICollection<Document>? Documents { get; set; }
        public virtual ICollection<CreditEvaluation>? CreditEvaluations { get; set; }
    }
} 