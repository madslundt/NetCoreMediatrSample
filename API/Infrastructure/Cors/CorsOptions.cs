using System.Collections.Generic;

namespace API.Infrastructure.Cors
{
    public class CorsOptions
    {
        public ICollection<string> Origins { get; set; } = new List<string>();
    }
}
