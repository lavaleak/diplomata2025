using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diplomata.Editor.Models.Database
{
    public class Example : DatabaseBaseModel
    {
        [Required]
        [Column(TypeName = "TEXT")]
        public string Name { get; set; }
    }
}

