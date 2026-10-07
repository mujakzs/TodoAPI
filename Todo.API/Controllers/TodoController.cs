using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.Contracts;
using Todo.Application.DTOs.Request;
using Todo.Application.DTOs.Response;

namespace Todo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {

        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<TodoResponseDto> todoList = [];

            todoList.Add(new TodoResponseDto(Name: "Start making baby", IsCompleted: true));
            todoList.Add(new TodoResponseDto(Name: "Start making apps", IsCompleted: true));
           
            return Ok(todoList);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTodoDto todoDto) // [FromBody] Tell ASP.NET Core to get the value from the HTTP request body
        {
            var created = await _todoService.CreateTodoAsync(todoDto);
            return Created();
        }
    }
}
