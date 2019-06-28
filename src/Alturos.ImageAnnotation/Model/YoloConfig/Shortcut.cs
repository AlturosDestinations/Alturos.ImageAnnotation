using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alturos.ImageAnnotation.Model.YoloConfig
{
    public class Shortcut : YoloConfigElement
    {
        public string From { get; set; }
        public string Activation { get; set; } = "linear";
    }
}
