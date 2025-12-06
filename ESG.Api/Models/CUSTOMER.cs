using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESG.Api.Models
{
    [Table("CUSTOMER")]
    public partial class CUSTOMER
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CUSTOMERID { get; set; }
        [Required]
        public string CUSTOMERCODE { get; set; } = null!;
        [Required]
        public string FIRSTNAME { get; set; }  = null!;
        [Required]
        public string LASTNAME { get; set; } = null!;
        [Required]
        [MaxLength(10)]
        public string GENDER { get; set; }  = null!;
        [Required]
        public string OCCUPATION { get; set; } = null!;
        [Required]
        public string ADDRESS { get; set; } = null!;
        [Required]
        public int CREATEDBY { get; set; }
        [Required]
        public DateTime DATETIMECREATED { get; set; }
    }
}