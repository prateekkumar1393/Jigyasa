using System.Collections.Generic;

namespace Application.CQRS
{
    public abstract class GenericVm<T>
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public abstract T Data { get; set; }
    }
}
