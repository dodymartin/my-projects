using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApi.Entities
{    
    //[Table("FAC", Schema = "CMN_MSTR")]
    public class Facility
    {
        //[Column("FAC_ID")]
        public int Id { get; set; }

        //[Column("NM")]
        public string Name { get; set; }
    }
}
