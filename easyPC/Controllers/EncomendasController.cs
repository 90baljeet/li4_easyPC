using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using easyPC.Data;
using easyPC.Models;

namespace easyPC.Controllers
{
    public class EncomendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EncomendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Encomendas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Encomendas.Include(e => e.Cliente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Encomendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encomenda = await _context.Encomendas
                .Include(e => e.Cliente)
                .Include(e => e.EncomendaComponentes)
                    .ThenInclude(ec => ec.Componente)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (encomenda == null)
            {
                return NotFound();
            }

            // Carregar montagem associada manualmente
            encomenda.Montagem = await _context.Montagens
                .FirstOrDefaultAsync(m => m.EncomendaId == encomenda.Id);

            return View(encomenda);
        }


        // GET: Encomendas/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Email");
            ViewData["CPU"] = new SelectList(_context.Componentes.Where(c => c.Tipo == TipoComponente.CPU), "Id", "Modelo");
            ViewData["RAM"] = new SelectList(_context.Componentes.Where(c => c.Tipo == TipoComponente.RAM), "Id", "Modelo");
            ViewData["Disco"] = new SelectList(_context.Componentes.Where(c => c.Tipo == TipoComponente.Disco), "Id", "Modelo");
            ViewData["Fonte"] = new SelectList(_context.Componentes.Where(c => c.Tipo == TipoComponente.Fonte), "Id", "Modelo");
            ViewData["Caixa"] = new SelectList(_context.Componentes.Where(c => c.Tipo == TipoComponente.Caixa), "Id", "Modelo");
            ViewData["Montagens"] = new SelectList(_context.Montagens, "Id", "NomeMontagem");
            ViewData["Montagens"] = new SelectList(_context.Montagens, "Id", "Id");


            return View();
        }

        // POST: Encomendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,Estado,ClienteId,MontagemId")] Encomenda encomenda, int[] SelectedComponentes)
        {
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    Console.WriteLine($"Erro em {entry.Key}: {error.ErrorMessage}");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Encomendas.Add(encomenda);
                await _context.SaveChangesAsync();

                if (SelectedComponentes != null && SelectedComponentes.Any())
                {
                    foreach (var compId in SelectedComponentes)
                    {
                        _context.EncomendaComponentes.Add(new EncomendaComponente
                        {
                            EncomendaId = encomenda.Id,
                            ComponenteId = compId
                        });
                    }

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            // Repopula os dropdowns se falhar
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Email", encomenda.ClienteId);
            ViewData["CPU"] = new SelectList(_context.Componentes.Where(c => c.Tipo == TipoComponente.CPU), "Id", "Modelo");
            ViewData["RAM"] = new SelectList(_context.Componentes.Where(c => c.Tipo == TipoComponente.RAM), "Id", "Modelo");
            ViewData["Disco"] = new SelectList(_context.Componentes.Where(c => c.Tipo == TipoComponente.Disco), "Id", "Modelo");
            ViewData["Fonte"] = new SelectList(_context.Componentes.Where(c => c.Tipo == TipoComponente.Fonte), "Id", "Modelo");
            ViewData["Caixa"] = new SelectList(_context.Componentes.Where(c => c.Tipo == TipoComponente.Caixa), "Id", "Modelo");
            ViewData["Montagens"] = new SelectList(_context.Montagens, "Id", "NomeMontagem");
            ViewData["Montagens"] = new SelectList(_context.Montagens, "Id", "Id");


            return View(encomenda);
        }


        // GET: Encomendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encomenda = await _context.Encomendas.FindAsync(id);
            if (encomenda == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Email", encomenda.ClienteId);
            return View(encomenda);
        }

        // POST: Encomendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,Estado,ClienteId")] Encomenda encomenda)
        {
            if (id != encomenda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(encomenda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EncomendaExists(encomenda.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Email", encomenda.ClienteId);
            return View(encomenda);
        }

        // GET: Encomendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encomenda = await _context.Encomendas
                .Include(e => e.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (encomenda == null)
            {
                return NotFound();
            }

            return View(encomenda);
        }

        // POST: Encomendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var encomenda = await _context.Encomendas.FindAsync(id);
            if (encomenda != null)
            {
                _context.Encomendas.Remove(encomenda);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EncomendaExists(int id)
        {
            return _context.Encomendas.Any(e => e.Id == id);
        }
    }
}
