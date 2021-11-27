using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoMVC.Data;
using ProjetoMVC.Models;

namespace ProjetoMVC.Controllers
{
    public class EquipamentosController : Controller
    {
        private readonly Contexto _context;

        public EquipamentosController(Contexto context)
        {
            _context = context;
        }

        // GET: Equipamentos
        public async Task<IActionResult> Index()
        {
            ViewBag.Perfil = HttpContext.Session.GetString("_Perfil");
            return View(await _context.Equipamentos.ToListAsync());
        }

        // GET: Equipamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Perfil = HttpContext.Session.GetString("_Perfil");
            

            if (id == null)
            {
                return NotFound();
            }

            var equipamentos = await _context.Equipamentos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipamentos == null)
            {
                return NotFound();
            }

            ListarComentarios(equipamentos.Id);

            return View(equipamentos);
        }

        // GET: Equipamentos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Equipamentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Equipamento,Imagem,Descricao,Ativo,Data")] Equipamentos equipamentos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equipamentos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(equipamentos);
        }

        // GET: Equipamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipamentos = await _context.Equipamentos.FindAsync(id);
            if (equipamentos == null)
            {
                return NotFound();
            }
            return View(equipamentos);
        }

        // POST: Equipamentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Equipamento,Imagem,Descricao,Ativo,Data")] Equipamentos equipamentos)
        {
            if (id != equipamentos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipamentos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipamentosExists(equipamentos.Id))
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
            return View(equipamentos);
        }

        // GET: Equipamentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipamentos = await _context.Equipamentos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipamentos == null)
            {
                return NotFound();
            }

            return View(equipamentos);
        }

        // POST: Equipamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipamentos = await _context.Equipamentos.FindAsync(id);
            _context.Equipamentos.Remove(equipamentos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipamentosExists(int id)
        {
            return _context.Equipamentos.Any(e => e.Id == id);
        }


        [HttpPost]
        public async Task<IActionResult> Comentar(IFormCollection form)
        {
            Comentarios comentario = new Comentarios();

            comentario.IdEquipamentos = int.Parse(form["IdEquipamentos"]);
            comentario.IdPerfis       = int.Parse(form["IdPerfis"]);
            comentario.Comentario = form["Comentario"];
            comentario.Data = DateTime.Now;

            _context.Comentarios.Add(comentario);
            ListarComentarios(comentario.IdEquipamentos);

            await _context.SaveChangesAsync();
            return LocalRedirect("~/");
        }

        public void ListarComentarios(int id)
        {
            var comentarios =  _context.Comentarios
                .ToList()
                .Where(m => m.IdEquipamentos == id);

            if(comentarios != null)
            {
                ViewBag.Comentarios = comentarios;
            }
        }
    }
}
