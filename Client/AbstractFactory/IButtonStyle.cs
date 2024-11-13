﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Client.AbstractFactory
{
    public interface IButtonStyle
    {
        void ApplyStyle(Button button);
    }
}
