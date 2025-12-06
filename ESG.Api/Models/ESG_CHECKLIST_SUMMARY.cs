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
        public int CHECKLISTITEMID { get; set; }
        [Required]
        public int LOANAPPLICATIONID { get; set; }
        [Required]
        public int TOTALSCORE { get; set; }
        [Required]
        public int TOTALWEIGHT { get; set; }
        [Required]
        public int RATINGID { get; set; }
        public string COMMENT_ { get; set; } = null!;
        public int CREATEDBY { get; set; } = 1;
        public DateTime DATETIMECREATED { get; set; } = DateTime.Now;
    }
}