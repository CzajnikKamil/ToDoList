using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToDoItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: ToDoItems/ToggleCompletion
        [HttpPost]
        public async Task<IActionResult> ToggleCompletion(int id, bool isCompleted)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null)
            {
                // jesli zadanie nie zostało znalezione, zwracaj odpowiedź JSON z błędem
                return Json(new { success = false, message = "Zadanie nie zostało znalezione." });
            }

            try
            {
                // aktualizacja statusu zadania
                toDoItem.IsCompleted = isCompleted;
                _context.Update(toDoItem);
                await _context.SaveChangesAsync();

                // zwróć odpowiedź JSON z informacją o powodzeniu
                return Json(new { success = true, message = "Status zadania został zaktualizowany." });
            }
            catch (Exception ex)
            {
                // w przypadku błędu zwróć odpowiedź JSON z informacją o błędzie
                return Json(new { success = false, message = $"Błąd przy zapisie: {ex.Message}" });
            }
        }

        // GET: ToDoItems
        public async Task<IActionResult> Index()
        {
            var ToDoItems = await _context.ToDoItems.ToListAsync();
            return View(ToDoItems);
        }

        // GET: ToDoItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDoItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,IsCompleted")] ToDoItem toDoItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toDoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDoItem);
        }

        // GET: ToDoItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null)
            {
                return NotFound();
            }
            return View(toDoItem);
        }

        // POST: ToDoItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,IsCompleted")] ToDoItem toDoItem)
        {
            if (id != toDoItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDoItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoItemExists(toDoItem.Id))
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
            return View(toDoItem);
        }

        // GET: ToDoItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoItem = await _context.ToDoItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            return View(toDoItem);
        }

        // POST: ToDoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            _context.ToDoItems.Remove(toDoItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // sprawdzenie czy zadanie istnieje w bazie
        private bool ToDoItemExists(int id)
        {
            return _context.ToDoItems.Any(e => e.Id == id);
        }
    }
}
