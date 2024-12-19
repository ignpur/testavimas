using System;

namespace Client.GameStates
{
    public class StoppedState : IGameState
    {
        public void Enter(GameClient client)
        {
            Console.WriteLine("Entering Stopped State");
        }

        public void HandleAction(GameClient client)
        {
            Console.WriteLine("No actions can be taken. The game is stopped.");
        }

        public void HandleShot(GameClient client, int x, int y)
        {
            Console.WriteLine("Cannot shoot. The game is stopped.");
        }

        public void Exit(GameClient client)
        {
            Console.WriteLine("Exiting Stopped State");
        }

        // Handles MainWindow UI logic for this state
        public void ApplyUI(MainWindow window)
        {
            window.ClearBoards();
            window.EnableMyBoard(false);
            window.EnableEnemyBoard(false);
            window.ActionButton.IsEnabled = false;
            window.MessagesListbox.Items.Add("Game has not started.");
        }
    }
}
