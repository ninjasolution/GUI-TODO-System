using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridLineWindow.Model
{
    public class ApplicationInfo
    {
        public int splashTime { get; set; }
        public float splashCornerRadius { get; set; }
        public int splashDuration { get; set; }
        public int sliderPosition { get; set; }
        public string mainwindowBackgroundColor { get; set; }
        public string currentProjectUrl { get; set; }
        public string[] projectList { get; set; }
    }
}
