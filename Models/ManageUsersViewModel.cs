using AspnetCoreTodo.Data;
using Microsoft.AspNetCore.Identity;

namespace AspnetCoreTodo.Models
{
    public class ManageUsersViewModel
    {
        public IdentityUser[] Administrators { get; set; }
        public IdentityUser[] Everyone { get; set;}
    }
}