using Todo.Application.Contracts;
using Todo.Application.DTOs.Request;
using Todo.Application.Mappers;
using Todo.Domain.RepositoryInterface;

namespace Todo.Application.Implementations
{
    public class TodoService : ITodoService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ITodoRepository _todoRepository;

        public TodoService(
            ITodoRepository todoRepository,
            ICurrentUserService currentUserService)
        {
            _todoRepository = todoRepository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> CreateTodoAsync(CreateTodoDto todos)
        {
            var todo = todos.ConvertToTodoListDomain();

            todo.UserId = Guid.Parse(_currentUserService.GetCurrentUserId());

            await _todoRepository.AddAsync(todo);

            int rowsInserted = await _todoRepository.CommitAsync();

            return rowsInserted > 0;
        }
    }
}
