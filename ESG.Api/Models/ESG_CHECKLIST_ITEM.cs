
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESG.Api.Models
{
    [Table("ESG_CHECKLIST_ITEM")]
    public partial class ESG_CHECKLIST_ITEM
    {   [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CHECKLISTITEMID { get; set; }
        [Required]
        public string CHECKLISTITEM { get; set; } = null!;
        [Required]
        public int RESPONSETYPEID { get; set; }
        [Required]
        public bool ISINUSE { get; set; }
        [Required]
        public int WEIGHT { get; set; }
        //public virtual EsgChecklist Checklist { get; set; } = null!;
    }
}