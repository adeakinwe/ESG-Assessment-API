using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESG.Api.Models
{
    [Table("ESG_CHECKLIST_ASSESSMENT")]
    public partial class ESG_CHECKLIST_ASSESSMENT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int CHECKLISTDETAILID { get; set; }
        [Required]
        public int CHECKLISTITEMID { get; set; }
        [Required]
        public int LOANAPPLICATIONID { get; set; }
        [Required]
        public bool RESPONSETYPEID { get; set; }
        [Required]
        public int SCORE { get; set; }
        [Required]
        public int WEIGHT { get; set; }
        public string COMMENT_ { get; set; } = null!;
        public int CREATEDBY { get; set; } = 1;
        public DateTime DATETIMECREATED { get; set; } = DateTime.Now;
    }
}