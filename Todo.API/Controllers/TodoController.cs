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
        private readonly ILogger<TodoController> _logger;

        public TodoController(ITodoService todoService, ILogger<TodoController> logger)
        {
            _todoService = todoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<TodoResponseDto> todoList = [];

            todoList.Add(new TodoResponseDto(Name: "Start making baby", Description: "Make a baby", Metadata: null));
            todoList.Add(new TodoResponseDto(Name: "Start making apps", Description: "Make some apps", Metadata: null));
            
            _logger.LogInformation("Retrieving todo items");
            return Ok(todoList);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTodoDto todoDto) // [FromBody] Tell ASP.NET Core to get the value from the HTTP request body
        {
            var created = await _todoService.CreateTodoAsync(todoDto);
            _logger.LogInformation("Todo item created");
            return Created();
        }
    }
}
