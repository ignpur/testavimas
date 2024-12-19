using Server.AbstractFactory;
using Server.Prototype;
using System;

namespace Server.Memento
{
    public class ShipPlacementMemento
    {
        public ShipAbstract[,] BoardState { get; private set; }
        public int[] ArrangedShipsCount { get; private set; }


        public ShipPlacementMemento(ShipAbstract[,] boardState, int[] arrangedShipsCount)
        {
            BoardState = boardState.Clone() as ShipAbstract[,];
            ArrangedShipsCount = (int[])arrangedShipsCount.Clone();

        }
    }
}
