using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client.AbstractFactory
{
    public class DarkGridStyle : IGridStyle
    {
    public void ApplyGridBackground(Grid grid)
    {
        grid.Background = Brushes.DarkSlateGray;
    }

    public void ApplyGridButtonStyle(Button button)
    {
        button.Background = Brushes.DarkSlateGray;
            if (button.Content is string symbol && (symbol == "#" || symbol == "X" || symbol == "O"))
            {
                button.Foreground = Brushes.Yellow;  // Always make symbols yellow
            }
        }
    }
}
