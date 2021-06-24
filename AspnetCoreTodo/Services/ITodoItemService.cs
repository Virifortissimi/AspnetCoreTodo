using System;
using System.Threading.Tasks;
using AspnetCoreTodo.Data;
using AspnetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;

namespace AspnetCoreTodo.Services
{
    public interface ITodoItemService
    {
        Task<TodoItem[]> GetIncompleteItemsAsync(IdentityUser user);
        Task<bool> AddItemAsync(TodoItem newItem, IdentityUser user);
        Task<bool> MarkDoneAsync(Guid id, IdentityUser user);

    }
}