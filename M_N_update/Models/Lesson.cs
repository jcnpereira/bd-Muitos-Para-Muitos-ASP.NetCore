using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace M_N_update.Models {
   public class Lesson {

      public Lesson() {
         // inicializar a lista de categorias de cada uma das Lessons
         CategoriesList = new HashSet<Category>();
      }

      /// <summary>
      /// Identificador de cada comentário da Lesson
      /// </summary>
      [Key]
      public int ID { get; set; }

      /// <summary>
      /// Nome da Lesson
      /// </summary>
      [Required(ErrorMessage = "O Nome é de preenchimento obrigatório.")]
      [StringLength(40)]
      public string Nome { get; set; }

      /// <summary>
      /// Descricao do que se trata na Lesson
      /// </summary>
      [Required(ErrorMessage = "A Descrição é de preenchimento obrigatório.")]
      public string Description { get; set; }

      

      /// <summary>
      /// Lista de categorias associadas à Lesson
      /// </summary>
      [Required(ErrorMessage = "É obrigatório escolher uma categoria.")]
      public virtual ICollection<Category> CategoriesList { get; set; }



   }
}
