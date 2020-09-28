using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TutesWebApp.Models;

namespace TutesWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CourseInfo> Courses { get; set; }
        public DbSet<Tutorials> Tutorials { get; set; }
        public DbSet<Topics> Topics { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
    }
}
