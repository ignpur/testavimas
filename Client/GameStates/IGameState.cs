namespace Client.GameStates
{
    public interface IGameState
    {
        void Enter(GameClient client);
        void HandleAction(GameClient client);
        void HandleShot(GameClient client, int x, int y);
        void Exit(GameClient client);
        void ApplyUI(MainWindow window); // Ensure ApplyUI is part of this interface
    }
}
