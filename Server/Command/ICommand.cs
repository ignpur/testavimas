using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Command
{
    public interface ICommand
    {
        void Execute();
        void Undo();

    }
}
