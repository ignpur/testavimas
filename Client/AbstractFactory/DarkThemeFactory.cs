using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.AbstractFactory
{
    public class DarkThemeFactory : IThemeAbstractFactory
    {
        public IButtonStyle CreateButtonStyle()
        {
            return new DarkButtonStyle();
        }

        public IGridStyle CreateGridStyle()
        {
            return new DarkGridStyle();
        }
    }
}
