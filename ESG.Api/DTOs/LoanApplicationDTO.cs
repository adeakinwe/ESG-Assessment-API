namespace ESG.API.DTOs
{
    public class LoanApplicationForCreationDTO
    {
        public int productId { get; set; }
        public double amount { get; set; }
        public int tenorInDays { get; set; }
        public decimal interestRate { get; set; }
        public string loanPurpose { get; set; } = null!;
        public DateTime applicationDate { get; set; }
        public int createdBy { get; set; }
        public DateTime dateTimeCreated { get; set; }
    }
}