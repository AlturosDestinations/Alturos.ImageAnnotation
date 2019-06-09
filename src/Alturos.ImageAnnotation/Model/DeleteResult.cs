using System.Collections.Generic;

namespace Alturos.ImageAnnotation.Model
{
    public class DeleteResult
    {
        public bool Successful { get; set; }
        public List<string> FailedImages { get; set; }
    }
}
