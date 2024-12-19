public abstract class PlayerTurnTemplate
{
    public void ExecuteTurn()
    {
        TakeShot();
        UpdateGameState();
    }

    protected abstract void TakeShot();
    protected abstract void UpdateGameState();
}
