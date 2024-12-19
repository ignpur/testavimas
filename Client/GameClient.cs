using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.GameStates;

namespace Client
{
    public class GameClient
    {
        public IGameState CurrentState { get; private set; }
        public int Seat { get; }
        public int Turn { get; private set; }

        public GameClient(int seat)
        {
            Seat = seat;
            Turn = 0;
            SetState(new StoppedState());
        }

        public void ChangeTurn()
        {
            Turn = 1 - Turn;
        }

        public bool IsMyTurn()
        {
            return Seat == Turn;
        }

        public void SetState(IGameState newState)
        {
            CurrentState?.Exit(this);
            CurrentState = newState;
            CurrentState?.Enter(this);
        }

        public void HandleAction()
        {
            CurrentState.HandleAction(this);
        }

        public void HandleShot(int x, int y)
        {
            CurrentState.HandleShot(this, x, y);
        }
    }

}
