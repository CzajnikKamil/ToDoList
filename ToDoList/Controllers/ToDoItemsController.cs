using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Models;

public class ToDoItemsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ToDoItemsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: ToDoItems
    public async Task<IActionResult> Index()
    {
        return View(await _context.ToDoItems.ToListAsync());
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

    private bool ToDoItemExists(int id)
    {
        return _context.ToDoItems.Any(e => e.Id == id);
    }
}
