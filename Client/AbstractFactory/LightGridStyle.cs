using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client.AbstractFactory
{
    public  class LightGridStyle : IGridStyle
    {
        public void ApplyGridBackground(Grid grid)
        {
            grid.Background = Brushes.LightGray;
        }

        public void ApplyGridButtonStyle(Button button)
        {
            button.Background = Brushes.LightBlue;   // Light color for grid buttons
            button.Foreground = Brushes.DarkBlue;
        }
    }
}
