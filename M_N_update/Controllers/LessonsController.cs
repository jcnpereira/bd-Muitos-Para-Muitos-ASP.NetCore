using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using M_N_update.Data;
using M_N_update.Models;

namespace M_N_update.Controllers {

   public class LessonsController : Controller {

      private readonly M_N_updateDB _context;

      public LessonsController(M_N_updateDB context) {
         _context = context;
      }

      /// <summary>
      /// Index
      /// Listar todas as Lições
      /// </summary>
      /// <returns></returns>
      public async Task<IActionResult> Index() {
         return View(await _context.Lessons.ToListAsync());
      }


      /// <summary>
      /// Mostrar os Detalhes de uma lição
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public async Task<IActionResult> Details(int? id) {

         if (id == null) {
            return NotFound();
         }

         // procurar as Lições e as respetivas Categorias
         var lesson = await _context.Lessons
                                    .Include(l => l.CategoriesList)
                                    .FirstOrDefaultAsync(m => m.ID == id);

         if (lesson == null) {
            return NotFound();
         }

         // lista de todas as categorias existentes
         ViewBag.ListaDeCategorias = _context.Categories.OrderBy(c => c.Nome).ToList();

         return View(lesson);
      }

      /// <summary>
      /// Mostra a página dpo Create
      /// HttpGet
      /// </summary>
      /// <returns></returns>
      public IActionResult Create() {

         // obtém a lista de todos as categorias guardadas na BD, por ordem alfabética
         ViewBag.ListaDeCategorias = _context.Categories.OrderBy(c => c.Nome).ToList();

         return View();
      }

      // POST: Lessons/Create
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create([Bind("ID,Nome,Description")] Lesson lesson, int[] CategoriaEscolhida) {
         
         // avalia se o array com a lista de categorias escolhidas associadas à aula está vazio ou não
         if (CategoriaEscolhida.Length == 0) {
            //É gerada uma mensagem de erro
            ModelState.AddModelError("", "É necessário selecionar pelo menos uma categoria.");
            // gerar a lista Categorias que podem ser associadas à aula
            ViewBag.ListaDeCategorias = _context.Categories.OrderBy(c => c.Nome).ToList();
            // devolver controlo à View
            return View(lesson);
         }

         // criar uma lista com os objetos escolhidos das Categorias
         List<Category> listaDeCategoriasEscolhidas = new List<Category>();
         // Para cada objeto escolhido..
         foreach (int item in CategoriaEscolhida) {
            //procurar a categoria
            Category Categoria = _context.Categories.Find(item);
            // adicionar a Categoria à lista
            listaDeCategoriasEscolhidas.Add(Categoria);
         }

         // adicionar a lista ao objeto de "Lesson"
         lesson.CategoriesList = listaDeCategoriasEscolhidas;



         if (ModelState.IsValid) {
            _context.Add(lesson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
         }
         return View(lesson);
      }




      /// <summary>
      /// HttpGet
      /// Mostra a página para Editar as Lições
      /// </summary>
      /// <param name="id">identificador da Lição a editar </param>
      /// <returns></returns>
      public async Task<IActionResult> Edit(int? id) {
        
         // o conteúdo deste método é exatamente igual ao do método Details...

         if (id == null) {
            return NotFound();
         }

         var lesson = await _context.Lessons
                                    .Include(l => l.CategoriesList)
                                    .FirstOrDefaultAsync(m => m.ID == id);

         if (lesson == null) {
            return NotFound();
         }

         ViewBag.ListaDeCategorias = _context.Categories.OrderBy(c => c.Nome).ToList();

         return View(lesson);
      }



      /// <summary>
      /// Edição dos dados de uma Lesson
      /// </summary>
      /// <param name="id">Id da Lesson</param>
      /// <param name="newLesson">novos dados a associar à Lesson</param>
      /// <param name="CategoriaEscolhida">Lista de Categorias a que a Lesson deve estar associada</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, [Bind("ID,Nome,Description")] Lesson newLesson, int[] CategoriaEscolhida) {
        
         if (id != newLesson.ID) {
            return NotFound();
         }

         // https://www.thereformedprogrammer.net/updating-many-to-many-relationships-in-ef-core-5-and-above/

         // dados anteriormente guardados da Lesson
         var lesson = await _context.Lessons
                                    .Where(l => l.ID == id)
                                    .Include(l => l.CategoriesList)
                                    .FirstOrDefaultAsync();

         // obter a lista dos IDs das Categorias associadas à lição, antes da edição
         var oldListaCategorias = lesson.CategoriesList
                                        .Select(c => c.ID)
                                        .ToList();

         // avaliar se o utilizador alterou alguma Category associada à Lesson
         // adicionadas -> lista de categorias adicionadas
         // retiradas   -> lista de categorias retiradas
         var adicionadas = CategoriaEscolhida.Except(oldListaCategorias);
         var retiradas = oldListaCategorias.Except(CategoriaEscolhida.ToList());

         // se alguma Category foi adicionada ou retirada
         // é necessário alterar a lista de categorias 
         // associada à Lesson
         if (adicionadas.Any() || retiradas.Any()) {

            if (retiradas.Any()) {
               // retirar a Category 
               foreach (int oldCategory in retiradas) {
                  var categoryToRemove = lesson.CategoriesList.FirstOrDefault(c => c.ID == oldCategory);
                  lesson.CategoriesList.Remove(categoryToRemove);
               }
            }
            if (adicionadas.Any()) {
               // adicionar a Category 
               foreach (int newCategory in adicionadas) {
                  var categoryToAdd = await _context.Categories.FirstOrDefaultAsync(c => c.ID == newCategory);
                  lesson.CategoriesList.Add(categoryToAdd);
               }
            }
         }


         if (ModelState.IsValid) {
            try {
               /* a EF só permite a manipulação de um único objeto de um mesmo tipo
               *  por esse motivo, como estamos a usar o objeto 'lesson'
               *  temos de o atualizar com os dados que vêm da View
               */
               lesson.Description = newLesson.Description;
               lesson.Nome = newLesson.Nome;

               // adição do objeto 'lesson' para atualização
               _context.Update(lesson);
               // 'commit' da atualização
               await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
               if (!LessonExists(lesson.ID)) {
                  return NotFound();
               }
               else {
                  throw;
               }
            }
            return RedirectToAction(nameof(Index));
         }
         return View(lesson);
      }

      /// <summary>
      /// HttpGet
      /// Mostra a página de 
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public async Task<IActionResult> Delete(int? id) {

         // o conteúdo deste método é exatamente igual ao do método Details...

         if (id == null) {
            return NotFound();
         }

         var lesson = await _context.Lessons
                                   .Include(l => l.CategoriesList)
                                   .FirstOrDefaultAsync(m => m.ID == id);

         if (lesson == null) {
            return NotFound();
         }

         ViewBag.ListaDeCategorias = _context.Categories.OrderBy(c => c.Nome).ToList();

         return View(lesson);
      }




      // POST: Lessons/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id) {
         var lesson = await _context.Lessons.FindAsync(id);
         _context.Lessons.Remove(lesson);
         await _context.SaveChangesAsync();
         return RedirectToAction(nameof(Index));
      }




      private bool LessonExists(int id) {
         return _context.Lessons.Any(e => e.ID == id);
      }
   }
}
