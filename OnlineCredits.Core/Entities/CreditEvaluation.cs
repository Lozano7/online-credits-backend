using System;

namespace OnlineCredits.Core.Entities
{
    public class CreditEvaluation
    {
        public int Id { get; set; }
        public int CreditRequestId { get; set; }
        public int? EvaluatedBy { get; set; }
        public DateTime EvaluationDate { get; set; }
        public int? Score { get; set; }
        public string AutomaticEvaluation { get; set; }
        public string ManualEvaluation { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Relaciones
        public CreditRequest CreditRequest { get; set; }
    }
} 