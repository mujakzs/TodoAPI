using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Application.DTOs.Response
{
    public record TodoResponseDto(string Name, bool IsCompleted);
}
