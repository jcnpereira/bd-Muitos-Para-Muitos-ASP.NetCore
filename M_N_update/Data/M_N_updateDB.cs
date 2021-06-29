using M_N_update.Models;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace M_N_update.Data {
   public class M_N_updateDB : DbContext {

      public M_N_updateDB(DbContextOptions<M_N_updateDB> options) : base(options) { }


      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
         optionsBuilder.EnableSensitiveDataLogging();
      }

      protected override void OnModelCreating(ModelBuilder builder) {
         base.OnModelCreating(builder);


         var listaCategorias = new List<Category> {
            new Category { ID = 1, Nome = "A" },
            new Category { ID = 2, Nome = "B" },
            new Category { ID = 3, Nome = "C" },
            new Category { ID = 4, Nome = "D" }
         }
         ;

         builder.Entity<Category>().HasData(
            listaCategorias
         );

         builder.Entity<Lesson>().HasData(
            new Lesson {
               ID = 1,
               Nome = "name First Lesson",
               Description = "description First Lesson"
            },
            new Lesson {
               ID = 2,
               Nome = "name Second Lesson",
               Description = "description Second Lesson"
            },
            new Lesson {
               ID = 3,
               Nome = "name Third Lesson",
               Description = "description Third Lesson "
            }
         );


         builder.Entity("CategoryLesson").HasData(
            new Dictionary<string, object> { ["CategoriesListID"] = 1, ["LessonListID"] = 1 },
            new Dictionary<string, object> { ["CategoriesListID"] = 1, ["LessonListID"] = 2 },
            new Dictionary<string, object> { ["CategoriesListID"] = 2, ["LessonListID"] = 1 },
            new Dictionary<string, object> { ["CategoriesListID"] = 3, ["LessonListID"] = 2 },
            new Dictionary<string, object> { ["CategoriesListID"] = 4, ["LessonListID"] = 3 }
         );


      }

      public DbSet<Lesson> Lessons { get; set; }
      public DbSet<Category> Categories { get; set; }

   }
}
