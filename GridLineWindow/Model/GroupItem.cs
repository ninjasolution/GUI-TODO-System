using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GridLineWindow.Model
{
    public class GroupItem : ReactiveObject
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Color Background { get; set; }
        public int Border { get; set; }
        public ICommand Action { get; set; }
        private Thickness margin = new Thickness(0, 0, 0, 0);
        public Thickness Margin
        {
            get => margin;
            set => this.RaiseAndSetIfChanged(ref margin, value);
        }
        public DateTime TimeSpan { get; set; }
    }
}
