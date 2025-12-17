using System.ComponentModel.DataAnnotations;
using ESGG.Api.Enums;

namespace ESG.Api.Models
{
    public class ESG_AI_RECOMMENDATION
    {
        public int ID { get; set; }
        public int LOANAPPLICATIONID { get; set; }
        [StringLength(50)]
        public string STAGE { get; set; } = default!; // PRE_SCREEN / FINAL
        [StringLength(50)]
        public short RISKLEVEL { get; set; }
        public required string RECOMMENDATION  { get; set; }
        public decimal CONFIDENCE { get; set; }
        public string PAYLOAD { get; set; } = default!; // JSON
        [StringLength(30)]
        public string MODELVERSION { get; set; } = "v1.0";
        public int CREATEDBY { get; set; } = 1;
        public DateTime DATETIMECREATED { get; set; } = DateTime.Now;
        public int? LASTUPDATEDBY { get; set; }
        public DateTime? DATETIMEUPDATED { get; set; }
    }
}