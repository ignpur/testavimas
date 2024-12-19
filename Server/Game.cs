using Server.Strategy.ShipPlacement;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.AbstractFactory;
using System.Numerics;
using Server.Decorator;
using Server.Command;
using Server.Observer;
using System.Xml.Serialization;
using Server.Memento;
using Server.Prototype;
using Server.Proxy;

namespace Server
{
    public class Game : IGame
    {
        public GameState State { get; set; }
        public IPlayer[] Players { get; set; }
        public int Turn { get; set; }

        private CommandInvoker[] Invokers;
		private int choice = 0;
        private readonly ShipPlacementCaretaker[] caretakers = { new ShipPlacementCaretaker(), new ShipPlacementCaretaker() };

        //Observer needed 
        private GameData gameData = new GameData();
        private List<IObserver> observers = new List<IObserver>();
        
		    public Game()
        {
            Players = new IPlayer[2];
            Invokers = new CommandInvoker[2];
        }

        public void Initialize()
        {
            State = GameState.NotStarted;
            Players[0]?.Initialize();
            Players[1]?.Initialize();
        }

        public void Start()
        {
            State = GameState.Started;
            Turn = 0;
        }

        /// <summary>
        /// Adds player to the game.
        /// </summary>
        /// <param name="name">A nickname of the player</param>
        /// <param name="seat">A seat the player will sit on</param>
        /// <returns>
        /// false when unable to seat a player
        /// true otherwise
        /// </returns>
        public bool JoinPlayer(string name, int seat, IShipPlacementStrategy strategy)
        {
            if (seat != 0 && seat != 1) return false;
            if (Players[seat] != null) return false;
            if (State != GameState.NotStarted) return false;
            IPlayer player;
            if (seat == 0){
                player = new PlayerOne(name, strategy);
            }
            else{
                player = new PlayerTwo(name, strategy);
            }
            
            player.SetBoardSize(choice);
             
            Players[seat] = player;
            Invokers[seat] = new CommandInvoker();

            return true;
        }
        public int setBoardChoice()
        {
			if (choice == 0)
			{
				Random _random = new Random();
				choice = _random.Next(1, 4);
			}
            return choice;
        }
        /// <summary>
        /// Removes player from the game and initializes all the fields
        /// </summary>
        /// <param name="seat">Seat the player is sitting on</param>
        /// <returns>
        /// false if the seat parameter is invalid
        /// true otherwise
        /// </returns>
        public bool DisconnectPlayer(int seat)
        {
            if (seat != 0 && seat != 1) return false;

            Players[seat] = null;
            Players[1 - seat]?.Initialize();
            State = GameState.NotStarted;
            return true;
        }

        /// <summary>
        /// Marks player as ready to start the game
        /// </summary>
        /// <param name="player">Player to be marked</param>
        /// <returns>
        /// false if the player hasn't set all of their ships
        /// true otherwise
        /// </returns>
        public bool ReadyUp(int player)
        {
            if (player != 0 && player != 1) return false;
            if (Players[player] == null) return false;

            return Players[player].ReadyUp();
        }

        /// <summary>
        /// Sets a ship on the board
        /// </summary>
        /// <param name="player">Player that requested to set the ship</param>
        /// <param name="size">Size of a ship</param>
        /// <param name="x">X coordinate </param>
        /// <param name="y"></param>
        /// <returns>
        /// false if parameters were incorrect
        /// true otherwise
        /// </returns>
        public ShipHelper SetShip(int player, int size, int x, int y, bool vertical)
        {
            if (player != 0 && player != 1) return new ShipHelper(false, 0, 0, 0, false);
            if (State != GameState.ArrangingShips) return new ShipHelper(false, 0, 0, 0, false);
            //var ship = Ship.GetShipBySize(size);
            //if (ship == Ship.None) return false;

            IResultCommand<ShipHelper> command = new PlaceShipCommand(Players[player], size, x, y, vertical);
            Invokers[player].ExecuteCommand(command);
            
            return command.GetResults();
        }

        /// <summary>
        /// Fires a shot at the opposing player's board
        /// </summary>
        /// <param name="attacker">Attacker</param>
        /// <param name="x">X coordinate of the shot</param>
        /// <param name="y">Y coordinate of the shot</param>
        /// <returns>
        /// false if the parameters were incorrect
        /// true otherwise
        /// </returns>
        public Shot Fire(int attacker, int x, int y)
        {
            if (attacker != 0 && attacker != 1) return new Shot(x, y, false);
            if (State != GameState.Started) return new Shot(x, y, false);
            if (Turn != attacker) return new Shot(x, y, false);

            var shot = Players[1 - attacker].HandleIncomingFire(x, y);
            //Console.WriteLine("Shot fired");
            gameData.StatusMessage = $"Player {attacker} fired at ({x}, {y}): " + (shot.result ? "Hit" : "Miss");
            Notify();
            if ((!shot.result && Players[attacker] is ExtraShotDecorator extraShotPlayer && !extraShotPlayer.IsExtraShotAvailable())
                || (!shot.result && !(Players[attacker] is ExtraShotDecorator)))
            {
                Turn = 1 - Turn;
            }

            ////Notify();

            return shot;
        }

        public bool AreBothPlayersConnected()
        {
            return Players[0] != null && Players[1] != null;
        }

        public bool AreBothPlayersReady()
        {
            return Players[0]?.getReady() == true && Players[1]?.getReady() == true;
        }

        /// <summary>
        /// Method checks whether a game has finished or not
        /// </summary>
        /// <param name="player">Seat of a player that has just fired a shot</param>
        /// <returns>
        /// -1 when precondition fails
        /// 0 when the game is still in progress
        /// 1 when the player has won
        /// </returns>
        public int CheckWin(int player)
        {
            if (player != 0 && player != 1) return -1;
            if (Players[1 - player].HasEnabledShips()) return 0;
            return 1;
        }
        public ICommand CheckLastCommand(int player)
        {
            return Invokers[player].CheckLast();
        }
        public void Undo(int player)
        {
            Invokers[player].Undo();
        }


        ///OBSERVER CLASSES
        public void Attach(IObserver observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in observers)
            {
                observer.Update(gameData);
            }
        }
        //////END OF OBSVER CODE

        public void SavePlayerState(int player)
        {
            if (player < 0 || player > 1) return;
            var playerState = Players[player]?.SaveState();
            if (playerState != null)
                caretakers[player].Save(playerState);
        }
        public List<List<bool>> RestorePlayerState(int player)
        {
            if (player < 0 || player > 1) return null;
            var memento = caretakers[player].Restore();
            if (memento != null)
                Players[player]?.RestoreState(memento);

            return Players[player].GetBoardUI();
        }
    }
}

