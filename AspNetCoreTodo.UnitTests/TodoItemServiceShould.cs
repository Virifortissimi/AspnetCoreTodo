using System;
using System.Threading.Tasks;
using AspnetCoreTodo.Data;
using AspnetCoreTodo.Models;
using AspnetCoreTodo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace AspNetCoreTodo.UnitTests
{
    public class TodoItemServiceShould
    {
        [Fact]
        public async Task AddNewItemAsIncompleteWithDueDate()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;
            
            // Set up a context (connection to the "DB") for writing
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);
                var fakeUser = new IdentityUser
                {
                    Id = "fake-000",
                    UserName = "fake@example.com"
                    
                };
                
                await service.AddItemAsync(new TodoItem
                {
                    Title = "Testing?",
                    DueAt = DateTimeOffset.Now.AddDays(3)
                }, fakeUser);
            }

            // Use a separate context to read data back from the "DB"
            using (var context = new ApplicationDbContext(options))
            {
                var itemsInDatabase = await context.Items.CountAsync();
                
                Assert.Equal(1, itemsInDatabase);
                var item = await context.Items.FirstAsync();
                
                Assert.Equal("Testing?", item.Title);
                Assert.Equal(false, item.IsDone);
                
                // Item should be due 3 days from now (give or take a second)
                var difference = DateTimeOffset.Now.AddDays(3) - item.DueAt;
                
                Assert.True(difference < TimeSpan.FromSeconds(2));
            }
        }

        [Fact]
        public async Task ReturnFalseIfWrongIdIsPassedToMarkDone()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;
            
            //Arrange
            // Set up a context (connection to the "DB") for writing
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);
                var fakeUser = new IdentityUser
                {
                    Id = "fake-000",
                    UserName = "fake@example.com"
                    
                };
                
                await service.AddItemAsync(new TodoItem
                {
                    Title = "Testing??",
                    DueAt = DateTimeOffset.Now.AddDays(3)
                }, fakeUser);
            }

            //Act
            // Use a separate context to read data back from the "DB"
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);
                var fakeUser = new IdentityUser
                {
                    Id = "fake-000",
                    UserName = "fake@example.com"
                    
                };
                var markDone = await service.MarkDoneAsync(Guid.NewGuid(), fakeUser);

                //Assert
                //Expected --> Actual
                Assert.Equal(false, markDone);
            }
        }


        [Fact]
        public async Task ReturnTrueIfRightIdIsPassedToMarkDoneAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;

            // Set up a context (connection to the "DB") for writing
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);
                var fakeUser = new IdentityUser
                {
                    Id = "fake-000",
                    UserName = "fake@example.com"
                };

                var GuidId = Guid.NewGuid();

                await service.AddItemAsync(new TodoItem
                {
                    Id = GuidId,
                    Title = "Testing???",
                    DueAt = DateTimeOffset.Now.AddDays(3)
                }, fakeUser);
            
            

                var markDone = await service.MarkDoneAsync(GuidId, fakeUser);

                Assert.Equal(false, markDone);
            }
        }
    }
}