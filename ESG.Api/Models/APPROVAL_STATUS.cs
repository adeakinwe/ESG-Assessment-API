using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESG.Api.Models
{
    [Table("APPROVAL_STATUS")]
    public partial class APPROVAL_STATUS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int APPROVALSTATUSID { get; set; }
        [Required]
        public string NAME { get; set; } = null!;
    }
}