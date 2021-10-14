using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Quap.Models
{
    public class Tag : BaseModel
    {
        [Required]
        [MaxLength(150)]
        public string name { get; set; }

        [MaxLength(512)]
        public string description { get; set; }

        public ICollection<QuestionTag> questions = new Collection<QuestionTag>();
    }
}