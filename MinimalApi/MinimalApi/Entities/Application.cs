using System.ComponentModel.DataAnnotations.Schema;
using Stratos.Core;

namespace MinimalApi.Entities
{
    //[Table("APLN", Schema = "CMN_MSTR")]
    public class Application : EntityBase<Application, int>
    {
      //  [Column("APLN_ID")]
        public override int Id { get; set; }

        //[Column("EXE_NM")]
        public string ExeName { get; set; }
        //[Column("FROM_DIR_NM")]
        public string FromDirectoryName { get; set; }
        //[Column("MIN_ASMBLY_VER")]
        public string MinimumAssemblyVersion { get; set; }
        //[Column("NM")]
        public string Name { get; set; }
    }
}
