using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.AbstractFactory
{
    public class LightThemeFactory : IThemeAbstractFactory
    {
        public IButtonStyle CreateButtonStyle()
        {
            return new LightButtonStyle();
        }

        public IGridStyle CreateGridStyle()
        {
            return new LightGridStyle();
        }
    }
}
