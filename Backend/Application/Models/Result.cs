using System.Collections.Generic;

namespace Application.Models
{
    public class Result<T>
    {
        internal Result(T data)
        {
            Succeeded = true;
            Data = data;
        }

        internal Result(IEnumerable<string> errors)
        {
            Succeeded = false;
            Errors = errors;
        }

        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public T Data { get; set; }

        public static Result<T> Success(T data)
        {
            return new Result<T>(data);
        }

        public static Result<T> Failure(IEnumerable<string> errors)
        {
            return new Result<T>(errors);
        }
    }
}
