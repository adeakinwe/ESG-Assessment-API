using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESG.Api.Models
{
    [Table("ESG_CHECKLIST_DETAIL")]
    public partial class ESG_CHECKLIST_DETAIL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CHECKLISTDETAILID { get; set; }
        public int CHECKLISTITEMID { get; set; }
        public bool RESPONSETYPEID { get; set; }
        public int SCORE { get; set; }
        public int WEIGHT { get; set; }
        public string COMMENT_ { get; set; } = null!;
        public int CREATEDBY { get; set; } = 1;
        public DateTime DATETIMECREATED { get; set; } = DateTime.Now;
        //public virtual EsgChecklist Checklist { get; set; } = null!;
    }
}