using System;

namespace Client.GameStates
{
    public class ArrangingShipsState : IGameState
    {
        public void Enter(GameClient client)
        {
            Console.WriteLine("Entering ArrangingShips State");
        }

        public void HandleAction(GameClient client)
        {
            Console.WriteLine("Ships are being arranged.");
        }

        public void HandleShot(GameClient client, int x, int y)
        {
            Console.WriteLine("Cannot shoot while arranging ships.");
        }

        public void Exit(GameClient client)
        {
            Console.WriteLine("Exiting ArrangingShips State");
        }

        // Handles MainWindow UI logic for this state
        public void ApplyUI(MainWindow window)
        {
            window.MessagesListbox.Items.Add("Both players connected. It’s time to place the ships.");
            window.ActionButton.Content = "Ready";
            window.ActionButton.IsEnabled = true;
            window.EnableMyBoard(true);  // Ensure these methods are public in MainWindow.xaml.cs
            window.EnableEnemyBoard(false);  // Ensure these methods are public in MainWindow.xaml.cs
        }
    }
}
