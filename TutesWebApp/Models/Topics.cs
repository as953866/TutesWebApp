using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TutesWebApp.Models
{
    public class Topics
    {
        [Key]
        public int TopicID { get; set; }

        [Required]
        [StringLength(100)]
        public string TopicName { get; set; }

        [Required]
        public int TutorialID { get; set; }


        public Tutorials Tutorial { get; set; }
    }
}
