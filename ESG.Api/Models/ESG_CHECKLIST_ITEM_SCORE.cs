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
        [Required]
        public int CHECKLISTITEMID { get; set; }
        [Required]
        public int RESPONSETYPEID { get; set; }
        [Required]
        public int RESPONSETYPEVALUE { get; set; }
        [Required]
        public int SCORE { get; set; }
    }
}