//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using EssentialPhoto.Data;
//using EssentialPhoto.Models;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using System.IO;
//using Microsoft.AspNetCore.Authorization;

//namespace M_N_update.Models {

//    [Authorize]//garante que só pessoas autenticadas têm acesso
//    public class LessonsController : Controller
//    {
//        private readonly EssentialPhotoDB _context;

//        /// <summary>
//        /// este atributo contém os dados da app web no servidor
//        /// </summary>
//        private readonly IWebHostEnvironment _caminho;

//        public LessonsController(EssentialPhotoDB context, IWebHostEnvironment caminho)
//        {
//            _context = context;
//            _caminho = caminho;
//        }

//        // GET: Lessons
//        public async Task<IActionResult> Index()
//        {
//            var lessonsList = _context.Lessons
//                        .OrderByDescending(l => l.ID)
//                        .ToListAsync();

//            //retorna a lista de lições
//            return View(await lessonsList);
//        }

//        // GET: Lessons/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var lesson = await _context.Lessons.Include(l => l.LessonCommentsList).FirstOrDefaultAsync(m => m.ID == id);
//            lesson = await _context.Lessons.Include(l => l.CategoriesList).FirstOrDefaultAsync(c => c.ID == id);

//            if (lesson == null)
//            {
//                return NotFound();
//            }

//            return View(lesson);
//        }

//        // GET: Lessons/Create
//        public IActionResult Create()
//        {
//            //Lista todos as categorias guardadas na BD por ordem alfabética
//            ViewBag.ListaDeCategorias = _context.Categories.OrderBy(c => c.Nome).ToList();
//            return View();
//        }

//        //..........................................................................................................................................

//        // POST: Lessons/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("ID,Foto,Nome,Description,Content")] Lesson lesson, IFormFile fotografia, string[] CategoriaEscolhida)
//        {

//            // <- ADIÇÃO DE CATEGORIAS À AULA ->
//            // avalia se o array com a lista de categorias escolhidas associadas à aula está vazio ou não
//            if (CategoriaEscolhida.Count() == 0)
//            {
//                //É gerada uma mensagem de erro
//                ModelState.AddModelError("", "É necessário selecionar pelo menos uma categoria.");
//                // gerar a lista Categorias que podem ser associadas à aula
//                ViewBag.ListaDeCategorias = _context.Categories.OrderBy(c => c.Nome).ToList();
//                // devolver controlo à View
//                return View(lesson);
//            }

//            // criar uma lista com os objetos escolhidos das Categorias
//            List<Category> listaDeCategoriasEscolhidas = new List<Category>();
//            // Para cada objeto escolhido..
//            foreach (string item in CategoriaEscolhida)
//            {
//                //procurar a categoria
//                Category Categoria = _context.Categories.Find(Convert.ToInt32(item));
//                // adicionar a Categoria à lista
//                listaDeCategoriasEscolhidas.Add(Categoria);
//            }

//            // adicionar a lista ao objeto de "Lesson"
//            lesson.CategoriesList = listaDeCategoriasEscolhidas;

//            // vars auxiliares
//            string nomeImagem = "";
//            bool validade;

//            if (fotografia == null)
//            {
//                // não há ficheiro
//                // definir fotografia por defeito   
//                nomeImagem = "photo1.jpg";
//                // associar este ficheiro aos dados da Fotografia do cão
//                lesson.Foto = nomeImagem;
//                //não é necessário guardar a imagem no disco rígido do Servidor
//                validade = false;
//            }
//            else
//            {
//                // existe ficheiro. É imagem?
//                if (fotografia.ContentType == "image/jpeg"  fotografia.ContentType == "image/png")
//                {
//                    // definir o novo nome da fotografia     
//                    Guid g;
//                    g = Guid.NewGuid();
//                    nomeImagem = lesson.ID + "_" + g.ToString();
//                    //determinar a extensão
//                    string extensao = Path.GetExtension(fotografia.FileName).ToLower();
//                    //nome completo da imagem
//                    nomeImagem = nomeImagem + extensao;
//                    // associar a fotografia à lesson
//                    lesson.Foto = nomeImagem;
//                    //é necessário guardar a imagem no disco rígido do Servidor
//                    validade = true;
//                }
//                else
//                {
//                    //não é necessário guardar a imagem no disco rígido do Servidor
//                    validade = false;
//                    ModelState.AddModelError("", "O formato da imagem não é suportado.");
//                    ViewBag.ListaDeCategorias = _context.Categories.OrderBy(c => c.Nome).ToList();
//                    // devolver controlo à View
//                    return View(lesson);
//                }
//            }

//            // localização do armazenamento da imagem
//            string localizacaoFicheiro = _caminho.WebRootPath;
//            nomeImagem = Path.Combine(localizacaoFicheiro, "Multimedia", nomeImagem);



//            // avalia se os dados fornecidos estão de acordo com o modelo
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    //adição da nova lesson à bd
//                    _context.Lessons.Add(lesson);
//                    // consolidar os dados na base de dados
//                    await _context.SaveChangesAsync();
//                    //se não foi fornecido uma fotografia, não há necessidade de a guardar
//                    if (validade != false)
//                    {
//                        // se cheguei aqui, tudo correu bem
//                        // vou guardar, agora, no disco rígido do Servidor a imagem
//                        using var stream = new FileStream(nomeImagem, FileMode.Create);
//                        await fotografia.CopyToAsync(stream);
//                        // redirecionar para a página de início
//                        return RedirectToAction("Index");
//                    }
//                }
//                catch (Exception)
//                {
//                    //gera uma mensagem de erro
//                    ModelState.AddModelError("", "Ocorreu um erro desconhecido. " +
//                                                     "Pedimos deculpa pela ocorrência.");
//                }
//            }
//            // redirecionar para a página de início
//            return RedirectToAction("Index");
//        }

//        //..........................................................................................................................................

//        // GET: Lessons/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }
//            var lesson = await _context.Lessons.Include(l => l.CategoriesList).FirstOrDefaultAsync(m => m.ID == id);

//            //código por defeito
//            //var lesson = await _context.Lessons.FindAsync(id);
//            if (lesson == null)
//            {
//                return NotFound();
//            }

//            ViewBag.ListaDeCategorias = _context.Categories.OrderBy(c => c.Nome).ToList();

//            HttpContext.Session.SetInt32("NumLessonEmEdicao", lesson.ID);

//            return View(lesson);
//        }

//        // POST: Lessons/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("ID,Foto,Description,Nome,Content")] Lesson lesson, IFormFile fotografia, string FotoAntiga, int[] CategoriaEscolhida)
//        {
//            if (id != lesson.ID)
//            {
//                return NotFound();
//            }

//            //**************************************************************************************************

//            // criar uma lista com os objetos escolhidos das Categorias

//            //var lesson = await _context.Lessons.Include(l => l.CategoriesList).FirstOrDefaultAsync(m => m.ID == id);

//            // lista das categorias associadas às lições
//            var listaOldCategorias =(from l in _context.Lessons
//                                     where l.ID == id
//                                     select l.CategoriesList).FirstOrDefault()
//                                     ;

//                                     //from c in l.CategoriesList.SelectMany(c => c..Where(c => c..ID == l.ID)
//                                     //select c.ID;

//            // houve alteração de dados na edição da Lesson
//            var diferenca = listaOldCategorias.Select(c => CategoriaEscolhida.Contains(c.ID));
           
//            // avalia se há diferencas
//            if (diferenca.Any()) {
//                int hasgdj = 0; }


//          //  _context.Lessons.Where(l => l.ID == id).Select(l => l.CategoriesList).
//        // Where(c => CategoriaEscolhida.Contains(c.ID)).Select(c => c.ID);

//        // verificar se hou


//        List < Category > listaDeCategoriasEscolhidas = new List<Category>();
//            // Para cada objeto escolhido..
//            foreach (int item in CategoriaEscolhida)
//            {
//                //procurar a categoria
//                Category Categoria = await _context.Categories.FindAsync(item);
//                // adicionar a Categoria à lista
//                listaDeCategoriasEscolhidas.Add(Categoria);
//            }

//            lesson.CategoriesList = listaDeCategoriasEscolhidas;
//            //**************************************************************************************************

//            // recuperar o ID do objeto enviado para o browser
//            var numIdLesson = HttpContext.Session.GetInt32("NumLessonEmEdicao");

//            // e compará-lo com o ID recebido
//            // se forem iguais, continuamos
//            // se forem diferentes, não fazemos a alteração

//            if (numIdLesson == null || numIdLesson != lesson.ID)
//            {
//                // se entro aqui, é pq houve problemas

//                // redirecionar para a página de início
//                return RedirectToAction("Index");
//            }

//            // vars auxiliares
//            string nomeImagem = "";
//            bool validade;

//            if (fotografia == null)
//            {
//                lesson.Foto = FotoAntiga;
//                //nomeImagem = FotoAntiga;
//                validade = false;
//            }
//            else
//            {
//                // existe ficheiro. É imagem?
//                if (fotografia.ContentType == "image/jpeg" || fotografia.ContentType == "image/png")
//                {
//                    // definir o novo nome da fotografia     
//                    Guid g;
//                    g = Guid.NewGuid();
//                    nomeImagem = lesson.ID + "_" + g.ToString();
//                    //determinar a extensão
//                    string extensao = Path.GetExtension(fotografia.FileName).ToLower();
//                    //nome completo da imagem
//                    nomeImagem = nomeImagem + extensao;
//                    // associar a fotografia à lesson
//                    lesson.Foto = nomeImagem;
//                    //é necessário guardar a imagem no disco rígido do Servidor
//                    validade = true;
//                }
//                else
//                {
//                    //não é necessário guardar a imagem no disco rígido do Servidor
//                    validade = false;
//                    lesson.Foto = FotoAntiga;
//                    ModelState.AddModelError("", "O formato da imagem não é suportado.");
//                    ViewBag.ListaDeCategorias = _context.Categories.OrderBy(c => c.Nome).ToList();
//                    // devolver controlo à View
//                    return View(lesson);
//                }
//            }

//            // localização do armazenamento da imagem
//            string localizacaoFicheiro = _caminho.WebRootPath;
//            nomeImagem = Path.Combine(localizacaoFicheiro, "Multimedia", nomeImagem);

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(lesson);
//                    await _context.SaveChangesAsync();

//                    if (validade != false)
//                    {
//                        // se cheguei aqui, tudo correu bem
//                        // vou guardar, agora, no disco rígido do Servidor a imagem
//                        using var stream = new FileStream(nomeImagem, FileMode.Create);
//                        await fotografia.CopyToAsync(stream);
//                        // redirecionar para a página de início
//                        ViewBag.ListaDeCategorias = _context.Categories.OrderBy(c => c.Nome).ToList();
//                        // e, apagar o ficheiro antigo?
//                        // não esquecer que tem de recolher o nome antigo antes de adicionar o novo.
//                        //   FotoAntiga

//                        return RedirectToAction("Details", "Lessons", new { id = lesson.ID });
//                    }
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!LessonExists(lesson.ID))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction("Details", "Lessons", new { id = lesson.ID });
//            }

//            ViewBag.ListaDeCategorias = _context.Categories.OrderBy(c => c.Nome).ToList();
//            return RedirectToAction("Details", "Lessons", new { id = lesson.ID });
//        }

//        // GET: Lessons/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var lesson = await _context.Lessons
//                .FirstOrDefaultAsync(m => m.ID == id);
//            if (lesson == null)
//            {
//                return NotFound();
//            }

//            return View(lesson);
//        }

//        // POST: Lessons/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var lesson = await _context.Lessons.FindAsync(id);
//            _context.Lessons.Remove(lesson);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool LessonExists(int id)
//        {
//            return _context.Lessons.Any(e => e.ID == id);
//        }
//    }
//}
