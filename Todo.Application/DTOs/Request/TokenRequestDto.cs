using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Application.DTOs.Request;

public record TokenRequestDto(string userName, string password);
