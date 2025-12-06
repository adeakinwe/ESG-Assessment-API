
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESG.Api.Models
{
    [Table("ESG_CHECKLIST_ITEM")]
    public partial class ESG_CHECKLIST_ITEM
    {   [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CHECKLISTITEMID { get; set; }
        public string CHECKLISTITEM { get; set; } = null!;
        public int RESPONSETYPEID { get; set; }
        public bool ISINUSE { get; set; }
        public int WEIGHT { get; set; }
        //public virtual EsgChecklist Checklist { get; set; } = null!;
    }
}