using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client.AbstractFactory
{
    public class LightButtonStyle : IButtonStyle
    {
        public void ApplyStyle(Button button)
        {
            button.Background = Brushes.White;
            button.Foreground = Brushes.Black;
        }
    }
}
