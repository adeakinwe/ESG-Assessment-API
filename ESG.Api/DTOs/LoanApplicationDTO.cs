namespace ESG.API.DTOs
{
    public class LoanApplicationForCreationDTO
    {
        public int productId { get; set; }
        public int customerId { get; set; }
        public double amount { get; set; }
        public int tenorInDays { get; set; }
        public decimal interestRate { get; set; }
        public string loanPurpose { get; set; } = null!;
        public DateTime applicationDate { get; set; }
        public int createdBy { get; set; }
        public DateTime dateTimeCreated { get; set; }
        public int currencyId { get; set; }
    }

    public class LoanApplicationForReturnDTO
    {
        public int loanApplicationId { get; set; }
        public int productId { get; set; }
        public int sectorId { get; set; }
        public string customerName { get; set; } = null!;
        public string productName { get; set; } = null!;
        public double amount { get; set; }
        public int tenorInDays { get; set; }
        public decimal interestRate { get; set; }
        public string loanPurpose { get; set; } = null!;
        public DateTime applicationDate { get; set; }
        public string currencyCode { get; set; } = null!;
        public string sectorName { get; set; } = null!;
        public int approvalStatusId { get; set; }
        public required string statusName { get; set; }
        public int riskRatingId { get; set; }
        public required string riskRating { get; set; }
        public bool? submittedForAppraisal { get; set; }
    }
}