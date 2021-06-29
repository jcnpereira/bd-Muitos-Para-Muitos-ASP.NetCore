using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace M_N_update.Models {
   public class Category {

      public Category() {
         //inicializar a lista de lessons de cada uma das categorias
         LessonList = new HashSet<Lesson>();
      }

      /// <summary>
      /// Identificador de cada Category
      /// </summary>
      [Key]
      public int ID { get; set; }

      /// <summary>
      /// Nome da Category
      /// </summary>
      [Required(ErrorMessage = "o Nome é de preenchimento obrigatório.")]
      [StringLength(40)]
      public string Nome { get; set; }


      // Associar a Category às aulas
      /// <summary>
      /// Lista das Lessons da Category
      /// </summary>
      public virtual ICollection<Lesson> LessonList { get; set; }

   }
}
