using System.Collections.Generic;

namespace ARchGLCloud.Domain.Core.Commands
{
    public class BatchCommand<T> : Command
    {
        public List<T> Entities { get; set; }

        public override bool IsValid()
        {
            return Entities != null && Entities.Count > 0;
        }
    }
}
