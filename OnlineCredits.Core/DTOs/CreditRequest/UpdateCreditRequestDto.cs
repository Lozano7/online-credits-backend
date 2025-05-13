namespace OnlineCredits.Core.DTOs.CreditRequest
{
    public class UpdateCreditRequestDto
    {
        public decimal Amount { get; set; }
        public int TermInMonths { get; set; }
        public decimal MonthlyIncome { get; set; }
        public int WorkSeniority { get; set; }
        public string EmploymentType { get; set; }
        public decimal CurrentDebt { get; set; }
        public string Purpose { get; set; }
    }
} 