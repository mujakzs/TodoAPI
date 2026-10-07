using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Application.DTOs.Request
{
    public record CreateTodoDto(string name, string description, List<CreateTodoItemsDto> Items);
}
