using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InfrastructureLayer.Entities.ToDo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Todo;

namespace RazorPagesWeb.Client.Pages
{
    public class ToDoModel : PageModel
    {
        private readonly IToDoService _toDoService;

        public ToDoModel(IToDoService toDoService)
        {
            this._toDoService = toDoService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ToDoItemModel ToDoItem { get; set; }

        public IList<ToDoItem> ToDoItems { get; private set; }

        public class ToDoItemModel
        {
            [Required]
            [Display(Name = "Your to-do item")]
            public string Description { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ToDoItems = await _toDoService.GetToDoItems();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var toDoItem = new ToDoItem()
                {
                    Description = ToDoItem.Description,
                };
                var result = await _toDoService.CreateToDoItem(toDoItem);
            }
            StatusMessage = "The to-do item added to your list.";
            return RedirectToPage();
        }
    }
}