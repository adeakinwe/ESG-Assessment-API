using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESG.Api.Models
{
    [Table("ESG_CHECKLIST_ITEM_SCORE")]
    public partial class ESG_CHECKLIST_ITEM_SCORE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SCOREID { get; set; }
        public int CHECKLISTITEMID { get; set; }
        public int RESPONSETYPEID { get; set; }
        public int SCORE { get; set; }
    }
}