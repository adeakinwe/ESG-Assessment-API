using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESG.Api.Models
{
    [Table("ESG_CHECKLIST_SUMMARY")]
    public partial class ESG_CHECKLIST_SUMMARY
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SUMMARYID { get; set; }
        [Required]
        public int LOANAPPLICATIONID { get; set; }
        [Required]
        public double TOTALSCORE { get; set; }
        [Required]
        public double TOTALWEIGHT { get; set; }
        [Required]
        public double AVGSCORE { get; set; }
        [Required]
        public int RATINGID { get; set; }
        public string? COMMENT_ { get; set; }
        public int CREATEDBY { get; set; } = 1;
        public DateTime DATETIMECREATED { get; set; } = DateTime.Now;
        public decimal CONFIDENCE { get; set; }
    }
}