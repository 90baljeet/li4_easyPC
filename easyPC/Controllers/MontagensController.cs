using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using easyPC.Data;
using easyPC.Models;

namespace easyPC.Controllers
{
    public class MontagensController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MontagensController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Montagens
        public async Task<IActionResult> Index()
        {
            // Verifica atrasos
            var montagens = await _context.Montagens.Include(m => m.Encomenda).ToListAsync();
            foreach (var m in montagens)
            {
                if (m.EtapaAtual < 6 && m.DataFim < DateTime.Now && m.Encomenda.Estado != "atrasada")
                {
                    m.Encomenda.Estado = "atrasada";
                    _context.Update(m.Encomenda);
                }
            }

            await _context.SaveChangesAsync();

            // Carregar montagens novamente para visualização
            var atualizadas = _context.Montagens.Include(m => m.Encomenda);
            return View(await atualizadas.ToListAsync());
        }



        // POST: Montagens/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EtapaAtual,DataInicio,DataFim,EncomendaId")] Montagem montagem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(montagem);

                // Atualizar estado da encomenda
                var encomenda = await _context.Encomendas.FindAsync(montagem.EncomendaId);
                if (encomenda != null && encomenda.Estado == "encomendada")
                {
                    encomenda.Estado = "em montagem";
                    _context.Update(encomenda);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EncomendaId"] = new SelectList(_context.Encomendas, "Id", "Id", montagem.EncomendaId);
            return View(montagem);

        }
        // POST: Montagens/AvancarEtapa/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AvancarEtapa(int id)
        {
            var montagem = await _context.Montagens.FindAsync(id);
            if (montagem == null)
                return NotFound();

            if (montagem.EtapaAtual < 6)
            {
                montagem.EtapaAtual += 1;
                if (montagem.EtapaAtual >= 6)
                {
                    var encomenda = await _context.Encomendas.FindAsync(montagem.EncomendaId);
                    if (encomenda != null)
                    {
                        encomenda.Estado = "concluída";
                        _context.Update(encomenda);
                        await _context.SaveChangesAsync();
                    }
                }

                _context.Update(montagem);
                await _context.SaveChangesAsync();
                TempData["EtapaMessage"] = "Etapa avançada com sucesso!";
            }
            else
            {
                TempData["EtapaMessage"] = "A montagem já está completa (Etapa 6).";
            }

            return RedirectToAction("Details", "Encomendas", new { id = montagem.EncomendaId });
        }

    }
}
