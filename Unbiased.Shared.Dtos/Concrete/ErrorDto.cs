using Unbiased.Shared.Dtos.Abstract;

namespace Unbiased.Shared.Dtos.Concrete
{
    public class ErrorDto:ISharedDtos
    {
        public List<string> Errors { get; set; }
    }
}
