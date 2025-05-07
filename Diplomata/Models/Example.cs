using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diplomata.Models
{
    public class Example : BaseModel
    {
        [Required]
        [Column(TypeName = "TEXT")]
        public string Name { get; set; }
    }
}

