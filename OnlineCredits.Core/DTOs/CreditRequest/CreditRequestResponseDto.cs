namespace OnlineCredits.Core.DTOs.CreditRequest
{
    public class CreditRequestResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int TermInMonths { get; set; }
        public decimal MonthlyIncome { get; set; }
        public int WorkSeniority { get; set; }
        public string EmploymentType { get; set; }
        public decimal CurrentDebt { get; set; }
        public string Purpose { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? RejectionReason { get; set; }
        public decimal? ApprovedAmount { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? MonthlyPayment { get; set; }
        // Datos básicos del usuario
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
    }
} 