using Microsoft.EntityFrameworkCore;
using RubyOnBrain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyOnBrain.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<EntryType> EntryTypes { get; set; }
        public DbSet<ProgLang> ProgLangs { get; set; }
        public DbSet<Topic> Topics { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<UserCourse> UserCourses { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //        "Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=RubyOnBrainDB");
        //}

        //Метод для WebApi
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                    .HasMany(c => c.Users)
                    .WithMany(s => s.Courses)
                    .UsingEntity<UserCourse>(
                        j => j
                        .HasOne(c => c.User)
                        .WithMany(t => t.UserCourses)
                        .HasForeignKey("UserId"),
                        j => j
                        .HasOne(pt => pt.Course)
                        .WithMany(p => p.UserCourses)
                        .HasForeignKey("CourseId"),
                        j => {
                            j.Property(pt => pt.Rating).HasDefaultValue(0);
                            j.ToTable("UserCourses");
                        });

        }
    }
}
