using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TutesWebApp.Models
{
    public class Tutorials
    {
        [Key]
        public int TutorialID { get; set; }

        [Required]
        [StringLength(100)]
        public string TutorialTitle { get; set; }

        [Required]
        [StringLength(200)]
        public string PhotoName { get; set; }

        [Required]
        [StringLength(50)]
        public string ExtName { get; set; }

        [Required]
        [StringLength(100)]
        public string PhotoType { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int CourseID { get; set; }

        [Required]
        public bool Online { get; set; }

        public CourseInfo Course { get; set; }

        public ICollection<Topics> Topics { get; set; }

        public ICollection<Feedback> Feedbacks { get; set; }

        [NotMapped]
        public BufferedSingleFileUploadDb FileUpload { get; set; }
    }
    public class BufferedSingleFileUploadDb
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }
    }
}
