using System;
using System.Collections.Generic;
using System.Text;
using Todo.Application.DTOs.Request;
using Todo.Application.DTOs.Response;

namespace Todo.Application.Contracts
{
   public interface ITodoService
    {
        public Task<bool> CreateTodoAsync(CreateTodoDto todoDto);

        Task<List<TodoResponseDto>> GetItems();

    }
}
