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
        public int CHECKLISTITEMID { get; set; }
        public int LOANAPPLICATIONID { get; set; }
        public int TOTALSCORE { get; set; }
        public int TOTALWEIGHT { get; set; }
        public int RATINGID { get; set; }
        public string COMMENT_ { get; set; } = null!;
        public int CREATEDBY { get; set; } = 1;
        public DateTime DATETIMECREATED { get; set; } = DateTime.Now;
    }
}