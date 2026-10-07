using System;
using System.Collections.Generic;
using System.Text;
using Todo.Application.DTOs.Request;
using Todo.Domain.DomainEntities;

namespace Todo.Application.Mappers;

public static class TodoMappingExtension
{
    extension(CreateTodoDto todo)
    {
        public TodoListDomain ConvertToTodoListDomain()
        {
            return new TodoListDomain()
            {
                Description = todo.description,
                Name = todo.name,
                TodoItems = todo.Items.Select(x => new TodoItemDomain()
                {
                    Description = x.description,
                    Title = x.title,
                    Priority = x.priority,
                    DueDate = x.dueDate,
                    ReminderDate = x.remiderDate,
                })
                .ToList()
            };
        }
    }
}
