using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Infrastructure.Persistence.Entities
{
    public class TodoAppDbContext : DbContext
    {
        public TodoAppDbContext(DbContextOptions<TodoAppDbContext> options) : base(options)
        {
        }

        //"I want to provide my own database configuration." So im overriding EF Core's default behavior.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoAppDbContext).Assembly);

            modelBuilder.Entity<TodoList>() //Means: I am configuring the TodoList database entity.
                .HasOne(x => x.User)                    // Each TodoList has one User
                .WithMany()                             // Each User can have many TodoLists
                .HasForeignKey(x => x.UserId)           // The foreign key in TodoList that points to UserId
                .OnDelete(DeleteBehavior.NoAction);     // When a User is deleted, DO NOTHING automatically

            modelBuilder.Entity<TodoItem>()
                .HasOne(x => x.TodoList)
                .WithMany(x => x.TodoItems)
                .HasForeignKey(x => x.TodoListId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(x => x.TodoItem)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.TodoItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TodoItemTag>()
                .HasOne(x => x.TodoItem)
                .WithMany(x => x.TodoItemTags)
                .HasForeignKey(x => x.TodoItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TodoItemTag>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.TodoItemTags)
                .HasForeignKey(x => x.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }

  /*      The easiest way to remember it
   *      
            "I have a TodoItem."
            "I have a TodoList."
            "I have a User."
            "I have a Comment."

            "TodoItem belongs to TodoList."

            "TodoList belongs to User."

            "Comment belongs to TodoItem."

            "Comment belongs to User."

            "TodoItem and Tag are connected through TodoItemTag."

            "If TodoList is deleted, delete its TodoItems."

            "If User is deleted, don't automatically delete TodoLists." */

        public DbSet<User> Users { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TodoItemTag> TodoItemTags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }


    }
}
