using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Application.Contracts
{
    public interface ICurrentUserService
    {
        string GetCurrentUserId();
    }
}
