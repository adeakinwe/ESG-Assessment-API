using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESG.Api.Models
{   
    [Table("ESG_CHECKLIST_RESPONSE")]
    public partial class ESG_CHECKLIST_RESPONSE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RESPONSETYPEID { get; set; }
        public int NAME { get; set; }
    }
}