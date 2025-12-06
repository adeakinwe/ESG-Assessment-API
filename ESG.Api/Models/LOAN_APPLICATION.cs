using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESG.Api.Models
{
    [Table("ESG_CHECKLIST_RESPONSE")]
    public partial class LOAN_APPLICATION
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LOANAPPLICATIONID { get; set; }
        public int CUSTOMERID { get; set; }
        public int PRODUCTID { get; set; }
        public double AMOUNT { get; set; }
        public int TENORINDAYS { get; set; }
        public decimal INTERESTRATE { get; set; }
        public DateTime APPLICATIONDATE { get; set; }
        public int CREATEDBY { get; set; }
        public DateTime DATETIMECREATED { get; set; }
    }
}