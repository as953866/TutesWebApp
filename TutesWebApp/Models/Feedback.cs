using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TutesWebApp.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackID { get; set; }

        [Required]
        [StringLength(1000)]
        public string FeedbackText { get; set; }

        [Required]
        [StringLength(100)]
        public string EmailID { get; set; }


        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int TutorialID { get; set; }


        public Tutorials Tutorial { get; set; }
    }
}
