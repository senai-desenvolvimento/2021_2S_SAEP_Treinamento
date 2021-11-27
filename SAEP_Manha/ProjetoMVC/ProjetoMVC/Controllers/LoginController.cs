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
    public class LoginController : Controller
    {
        private readonly Contexto _context;

        public int SessaoPerfil { get; set; }

        public LoginController(Contexto context)
        {
            _context = context;
        }

        // GET: Login
        public async Task<IActionResult> Index()
        {
            ViewBag.Perfil = HttpContext.Session.GetString("_Perfil");

            var contexto = _context.Usuarios.Include(u => u.Perfis);
            return View(await contexto.ToListAsync());
        }

        // GET: Login/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios
                .Include(u => u.Perfis)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // GET: Login/Create
        public IActionResult Create()
        {
            ViewData["IdPerfis"] = new SelectList(_context.Perfis, "Id", "Id");
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Senha,IdPerfis")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuarios);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPerfis"] = new SelectList(_context.Perfis, "Id", "Id", usuarios.IdPerfis);
            return View(usuarios);
        }

        // GET: Login/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }
            ViewData["IdPerfis"] = new SelectList(_context.Perfis, "Id", "Id", usuarios.IdPerfis);
            return View(usuarios);
        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Senha,IdPerfis")] Usuarios usuarios)
        {
            if (id != usuarios.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarios);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuariosExists(usuarios.Id))
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
            ViewData["IdPerfis"] = new SelectList(_context.Perfis, "Id", "Id", usuarios.IdPerfis);
            return View(usuarios);
        }

        // GET: Login/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios
                .Include(u => u.Perfis)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarios = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuarios);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool UsuariosExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }


        [TempData]
        public string Mensagem { get; set; }

        [Route("Logar")]
        [HttpPost]
        public IActionResult Logar(IFormCollection form)
        {
            var usuarios =  _context.Usuarios
                .Include(u => u.Perfis)
                .FirstOrDefault(m => m.Senha == form["Senha"].ToString());

            // Redirecionamos o usuário logado caso encontrado
            if (usuarios != null)
            {
                HttpContext.Session.SetString("_Perfil", usuarios.IdPerfis.ToString());
                return LocalRedirect("~/equipamentos");
            }

            Mensagem = "Dados incorretos, tente novamente...";
            return RedirectToAction(nameof(Index));
        }

        [Route("Logout")]
        public IActionResult Logout()
        {
            
            HttpContext.Session.Remove("_Perfil");
            return RedirectToAction(nameof(Index));
        }

    }
}
