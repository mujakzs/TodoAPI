namespace Todo.Application.Common
{
    public sealed record Error(string Code, string Description, ErrorType Type)
    {
        /// <summary>
        /// The supplied data fails a validation rule.Example: a required email field is empty.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Error NotFound(string code, string description) => new(code, description, ErrorType.NotFound);

        /// <summary>
        /// The requested resource does not exist. Example: a user ID does not match any user.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Error Validation(string code, string description) => new(code, description, ErrorType.Validation);

        /// <summary>
        /// The operation conflicts with the current state of the resource. Example: registering an email address that is already registered.
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Error Conflict(string code, string description) => new(code, description, ErrorType.Conflict);

        /// <summary>
        /// The request lacks valid authentication credentials or has invalid credentials. Example: an invalid email/password combination during login.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Error Unauthorized(string code, string description) => new(code, description, ErrorType.Unauthorized);

        /// <summary>
        /// A general or unexpected operation failure that does not fit a more specific category.Example: an unexpected internal operation failure.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Error Failure(string code, string description) => new(code, description, ErrorType.Failure);
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

        /// <summary>
        /// The request is malformed or otherwise invalid for the operation. Example: a request contains an invalid parameter format.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Error BadRequest(string code, string description) => new(code, description, ErrorType.BadRequest);
    }
}