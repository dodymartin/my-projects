using System.ComponentModel.DataAnnotations.Schema;
using Stratos.Core;

namespace MinimalApi.Entities
{
    //[Table("APLN_FAC", Schema = "CMN_MSTR")]
    public class ApplicationFacility : EntityBase<ApplicationFacility, int>
    {
        //[Column("APLN_FAC_ID")]
        public override int Id { get; set; }

        //[Column("APLN_ID")]
        public int ApplicationId { get; set; }
        public Application Application { get; set; }

        //[Column("FAC_ID")]
        public int FacilityId { get; set; }
        public Facility Facility { get; set; }

        //[Column("MIN_ASMBLY_VER")]
        public string MinimumAssemblyVersion { get; set; }
    }
}
