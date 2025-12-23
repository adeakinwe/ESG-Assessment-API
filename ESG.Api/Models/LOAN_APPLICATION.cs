using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESG.Api.Models
{
    [Table("LOAN_APPLICATION")]
    public partial class LOAN_APPLICATION
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LOANAPPLICATIONID { get; set; }
        [Required]
        public int CUSTOMERID { get; set; }
        [Required]
        public int PRODUCTID { get; set; }
        [Required]
        public double AMOUNT { get; set; }
        [Required]
        public int TENOR { get; set; }
        [Required]
        public decimal INTERESTRATE { get; set; }
        [Required]
        public string LOANPURPOSE { get; set; } = null!;
        [Required]
        public int CURRENCYID { get; set; }
        public int APPROVALSTATUSID { get; set; }
        [Required]
        public DateTime APPLICATIONDATE { get; set; }
        public int CREATEDBY { get; set; } = 1;
        public DateTime DATETIMECREATED { get; set; } = DateTime.Now;
        public bool? SUBMITTEDFORAPPRAISAL { get; set; }
        [StringLength(30)]
        public string? APPLREFERENCENUMBER { get; set; }
    }
}