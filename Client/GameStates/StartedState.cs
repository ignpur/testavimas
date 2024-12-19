using System;

namespace Client.GameStates
{
    public class StartedState : IGameState
    {
        public void Enter(GameClient client)
        {
            Console.WriteLine("Entering Started State");
        }

        public void HandleAction(GameClient client)
        {
            Console.WriteLine("Game is in progress.");
        }

        public void HandleShot(GameClient client, int x, int y)
        {
            Console.WriteLine($"Shot fired at ({x}, {y})");
        }

        public void Exit(GameClient client)
        {
            Console.WriteLine("Exiting Started State");
        }

        // Handles MainWindow UI logic for this state
        public void ApplyUI(MainWindow window)
        {
            window.extraShotUsed = false;
            window.MessagesListbox.Items.Add("Game started. Player 1's turn.");
            window.EnableMyBoard(false);
            window.EnableEnemyBoard(window.Client.Turn == window.Client.Seat);
        }
    }
}
