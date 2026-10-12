using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
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
        private readonly IDistributedCache _distributedCache;

        public TodoController(ITodoService todoService, ILogger<TodoController> logger, IDistributedCache distributedCache)
        {
            _todoService = todoService;
            _logger = logger;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation($"Executing GET method inside a TodoController at {DateTime.Now}");

            var cache = await _distributedCache.GetStringAsync("todo");

            if (cache != null)
            {
                return Ok(JsonSerializer.Deserialize<List<TodoResponseDto>>(cache));
            }

            var todoList = await _todoService.GetItems();

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            await _distributedCache.SetStringAsync("todo", JsonSerializer.Serialize(todoList), options);
             

            _logger.LogInformation($"Execution of GET method inside a TodoController completed at {DateTime.Now}");

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
