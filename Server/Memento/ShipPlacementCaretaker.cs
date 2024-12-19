using System.Collections.Generic;

namespace Server.Memento
{
    public class ShipPlacementCaretaker
    {
        private Stack<ShipPlacementMemento> mementos = new Stack<ShipPlacementMemento>();

        public void Save(ShipPlacementMemento memento)
        {
            mementos.Push(memento);
        }

        public ShipPlacementMemento Restore()
        {
            return mementos.Count > 0 ? mementos.Pop() : null;
        }
    }
}
