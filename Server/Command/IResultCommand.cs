using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Command
{
    public interface IResultCommand<T> : ICommand
    {
        T GetResults();
    }
}
