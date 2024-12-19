using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Xaml.Behaviors;
using Client.Builder;
using Client.AbstractFactory;
using Client.GameStates;
using System.Numerics;


namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] ContextMenuItems = { "Cruiser horizontal   1x1", "Submarine horizontal   2x1",
            "Destroyer horizontal   3x1", "Battleship horizontal   4x1", "Cruiser vertical   1x1",
            "Submarine vertical   1x2", "Destroyer vertical   1x3", "Battleship vertical   1x4" };

		private Board _myBoard;
		private Board _enemyBoard;
		private BoardBuilder _boardBuilder;
		private BoardDirector _boardDirector;
        private int boardChoice = 0;

        private Button[,] MyButtons = new Button[10, 10];
        private Button[,] EnemyButtons = new Button[10, 10];

        private HubConnection Connection;
        public GameClient Client;
        private LobbyWindow _lobbyWindow;
        private string _currentGameId;

        private IThemeAbstractFactory _themeFactory;

        public bool extraShotUsed = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeConnection();
	        ShowLobbyWindow();
			SelectPowerButton.IsEnabled = false;
			SelectedPowerText.IsEnabled = false;
            
        }
        
        private void InitializeConnection()
		{
			Connection = new HubConnectionBuilder()
			    .WithUrl(@"http://localhost:4000/hubs/battleship")
			    .Build();
			
			Connection.On<int>("SendBoardChoice", OnJoinBoard);
			Connection.On<bool>("JoinResult", OnJoinResult);
			Connection.On<string, string>("ChatMessage", OnChatMessage);
			Connection.On<bool, int, int, int, bool>("ShipSet", OnSetShipResult);
			Connection.On<string>("GameState", OnGameStateChanged);
			Connection.On<int, bool, string>("PlayerReady", OnPlayerReady);
			Connection.On<int, int, int, bool>("ShotFired", OnShotFired);
			Connection.On<string>("GameWon", OnGameWon);
			Connection.On<string>("SpecialPowerEnabled", OnSpecialPowerEnabled);
			Connection.On("ExtraShotAllowed", OnExtraShotAllowed);
			Connection.On<int, int, int, bool>("UnmarkShip", UnmarkShip);
			Connection.On<string>("GameCreated", OnGameCreated);
            Connection.On<List<List<bool>>>("BoardStateUpdated", OnBoardStateUpdate);
            Connection.On("StateSaved", OnStateSaved);


            Connection.StartAsync();
		}

        private void OnBoardStateUpdate(List<List<bool>> list)
        {
            MessageBox.Show($"Board Update reached front");
            UpdateBoardUI(list);
        }

        private void UpdateBoardUI(List<List<bool>> boardState)
        {
            MessageBox.Show($"Updating board");

            for (int y = 0; y < boardState.Count; y++)
            {
                for (int x = 0; x < boardState[y].Count; x++)
                {
                    MyButtons[y, x].Content = boardState[y][x] ? "#" : ""; // Update UI
                    //MyButtons[y, x].IsEnabled = !boardState[y][x]; // Disable if occupied
                }
            }
            EnableMyBoard(true);
        }


        private void ShowLobbyWindow()
        {
	        _lobbyWindow = new LobbyWindow(Connection);
	        _lobbyWindow.GameJoined += OnGameJoined;
	        _lobbyWindow.Show();
	        this.Hide();
        }
        
        private void OnGameJoined(object sender, EventArgs e)
        {
	        _currentGameId = ((GameJoinedEventArgs)e).GameId;
	        this.Show();
        }

        private void SetTheme(string theme)
        {
            _themeFactory = theme == "Light" ? new LightThemeFactory() : new DarkThemeFactory();
            ApplyTheme();
        }


        /**
         * Private UI control methods
         */
        private void InitializeUi()
		{
			// Clear existing children from the MyBoard and EnemyBoard grids
			MyBoard.Children.Clear();
			EnemyBoard.Children.Clear();
			MyBoard.RowDefinitions.Clear();
			MyBoard.ColumnDefinitions.Clear();
			EnemyBoard.RowDefinitions.Clear();
			EnemyBoard.ColumnDefinitions.Clear();

			// Set up MyBoard and EnemyBoard with dimensions based on _myBoard and _enemyBoard
			for (int i = 0; i < _myBoard.Width + 1; i++)
			{
				MyBoard.ColumnDefinitions.Add(new ColumnDefinition());
				EnemyBoard.ColumnDefinitions.Add(new ColumnDefinition());
			}
			for (int i = 0; i < _myBoard.Height + 1; i++)
			{
				MyBoard.RowDefinitions.Add(new RowDefinition());
				EnemyBoard.RowDefinitions.Add(new RowDefinition());
			}

			// Add column and row labels for MyBoard
			for (int i = 0; i < _myBoard.ColumnLabels.Count; i++)
			{
				var label = new Label
				{
					Content = _myBoard.ColumnLabels[i],
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};
				Grid.SetRow(label, 0);
				Grid.SetColumn(label, i + 1);
				MyBoard.Children.Add(label);
			}
			for (int i = 0; i < _myBoard.RowLabels.Count; i++)
			{
				var label = new Label
				{
					Content = _myBoard.RowLabels[i],
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};
				Grid.SetColumn(label, 0);
				Grid.SetRow(label, i + 1);
				MyBoard.Children.Add(label);
			}

			// Add column and row labels for EnemyBoard
			for (int i = 0; i < _enemyBoard.ColumnLabels.Count; i++)
			{
				var label = new Label
				{
					Content = _enemyBoard.ColumnLabels[i],
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};
				Grid.SetRow(label, 0);
				Grid.SetColumn(label, i + 1);
				EnemyBoard.Children.Add(label);
			}
			for (int i = 0; i < _enemyBoard.RowLabels.Count; i++)
			{
				var label = new Label
				{
					Content = _enemyBoard.RowLabels[i],
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};
				Grid.SetColumn(label, 0);
				Grid.SetRow(label, i + 1);
				EnemyBoard.Children.Add(label);
			}

			// Initialize buttons for each cell in MyBoard and EnemyBoard
			for (int y = 0; y < _myBoard.Height; y++)
			{
				for (int x = 0; x < _myBoard.Width; x++)
				{
					var myButton = CreateButton(true, x, y);
					var enemyButton = CreateButton(false, x, y);

					MyButtons[y, x] = myButton;
					EnemyButtons[y, x] = enemyButton;

					Grid.SetRow(myButton, y + 1);
					Grid.SetColumn(myButton, x + 1);
					MyBoard.Children.Add(myButton);

					Grid.SetRow(enemyButton, y + 1);
					Grid.SetColumn(enemyButton, x + 1);
					EnemyBoard.Children.Add(enemyButton);
				}
			}

			// Disable power selection UI initially
			
		}

		public void ClearBoards()
        {
			// Clear MyBoard
			for (int y = 0; y < _myBoard.Height; y++)
			{
				for (int x = 0; x < _myBoard.Width; x++)
				{
					if (MyButtons[y, x] != null) // Check if the button exists
					{
						MyButtons[y, x].Content = "";       // Clear the content
						MyButtons[y, x].IsEnabled = false;  // Disable the button
					}
				}
			}

			// Clear EnemyBoard
			for (int y = 0; y < _enemyBoard.Height; y++)
			{
				for (int x = 0; x < _enemyBoard.Width; x++)
				{
					if (EnemyButtons[y, x] != null) // Check if the button exists
					{
						EnemyButtons[y, x].Content = "";       // Clear the content
						EnemyButtons[y, x].IsEnabled = false;  // Disable the button
					}
				}
            }
        }

        private void OnJoinBoard(int choice)
        {
			_boardBuilder = new BoardBuilder();
			_boardDirector = new BoardDirector(_boardBuilder);

			_myBoard = _boardDirector.BuildRandomBoard(choice);
			_enemyBoard = _boardDirector.BuildRandomBoard(choice);

			// Dynamically set the button array sizes based on board dimensions
			MyButtons = new Button[_myBoard.Height, _myBoard.Width];
			EnemyButtons = new Button[_enemyBoard.Height, _enemyBoard.Width];

            InitializeUi();
            ApplyTheme();
        }

        private Button CreateButton(bool myBoard, int x, int y)
        {
            Button b = new Button();
            b.IsEnabled = false;
            if (myBoard)
            {
                b.ContextMenu = new ContextMenu();
                for (int i = 0; i < ContextMenuItems.Length; i++)
                {
                    var tag = new int[] { x, y, (i % 4) + 1, i / 4 };
                    var item = new MenuItem { Header = ContextMenuItems[i], Tag = tag };
                    item.Click += HandleShipArrangement;
                    b.ContextMenu.Items.Add(item);
                }

                BehaviorCollection behaviors = Interaction.GetBehaviors(b);
                behaviors.Add(new DropDownMenuBehavior());
            }
            else
            {
                b.Tag = new int[2] { x, y };
                b.Click += HandleShot;
            }

            return b;
        }

        public void EnableMyBoard(bool enable)
        {
            foreach (Button b in MyButtons)
            {
                var content = b.Content != null ? b.Content.ToString() : "";
                b.IsEnabled = enable && content.Length == 0;
            }
        }

        public void EnableEnemyBoard(bool enable)
        {
            foreach (Button b in EnemyButtons)
            {
                var content = b.Content != null ? b.Content.ToString() : "";
                b.IsEnabled = enable && content.Length == 0;
            }
        }

        /*
         * UI event handlers
         */
        private void HandleJoin(object sender, RoutedEventArgs e)
        {
            MessagesListbox.Items.Add("Connecting to the server...");

            NicknameTextbox.IsEnabled = false;
            SeatCombobox.IsEnabled = false;
            PlacementStrategyComboBox.IsEnabled = false;
            JoinButton.IsEnabled = false;
            MessageTextbox.IsEnabled = true;
            SelectPowerButton.IsEnabled = true;
            SelectedPowerText.IsEnabled = true;

            Client = new GameClient(SeatCombobox.SelectedIndex);

            // Connection = new HubConnectionBuilder()
            //     .WithUrl(@"http://localhost:4000/hubs/battleship")
            //     .Build();
            //
            // Connection.On<int>("SendBoardChoice", OnJoinBoard);
            // Connection.On<bool>("JoinResult", OnJoinResult);
            // Connection.On<string, string>("ChatMessage", OnChatMessage);
            // Connection.On<bool, int, int, int, bool>("ShipSet", OnSetShipResult);
            // Connection.On<string>("GameState", OnGameStateChanged);
            // Connection.On<int, bool, string>("PlayerReady", OnPlayerReady);
            // Connection.On<int, int, int, bool>("ShotFired", OnShotFired);
            // Connection.On<string>("GameWon", OnGameWon);
            // Connection.On<string>("SpecialPowerEnabled", OnSpecialPowerEnabled);
            // Connection.On("ExtraShotAllowed", OnExtraShotAllowed);
            // Connection.On<int, int, int, bool>("UnmarkShip", UnmarkShip);

            ComboBoxItem selectedItem = (ComboBoxItem)PlacementStrategyComboBox.SelectedItem;
            string placementStrategy = selectedItem.Content.ToString();
             
            //Connection.StartAsync();
            Connection.SendAsync("Join", _currentGameId, NicknameTextbox.Text, Client.Seat, placementStrategy);
        }

        private void HandleShipArrangement(object sender, RoutedEventArgs e)
        {
            var b = sender as MenuItem;
            int[] tag = b.Tag as int[];
            int x = tag[0];
            int y = tag[1];
            int size = tag[2];
            bool vertical = tag[3] == 1;

            Connection.SendAsync("SetShip", _currentGameId, Client.Seat, size, x, y, vertical);
            EnableMyBoard(false);
        }

        private void HandleClearBoard(object sender, RoutedEventArgs e)
        {
            // TODO: implement this method
        }

        private void HandleShot(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int[] tag = button.Tag as int[];
            int x = tag[0];
            int y = tag[1];

            Connection.SendAsync("Fire", _currentGameId, Client.Seat, x, y);
            EnableEnemyBoard(false);
        }

        private void HandleAction(object sender, RoutedEventArgs e)
        {
            if (Client.CurrentState is ArrangingShipsState)
            {
                EnableMyBoard(false);
                Connection.SendAsync("ReadyUp", _currentGameId, Client.Seat);
            }

            ActionButton.IsEnabled = false;
        }

        private void SendChatMessage(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            if (MessageTextbox.Text.Length == 0) return;

            Connection.SendAsync("ChatMessage", _currentGameId, Client.Seat, MessageTextbox.Text);
            MessageTextbox.Text = "";
        }

        /*
         * Game event handlers
         */
        private void OnJoinResult(bool result)
        {
            if (result)
            {
                MessagesListbox.Items.Add("Occupied space: " + (Client.Seat + 1));
            }
            else
            {
                NicknameTextbox.IsEnabled = true;
                SeatCombobox.IsEnabled = true;
                JoinButton.IsEnabled = true;
                PlacementStrategyComboBox.IsEnabled = true;
                MessageTextbox.IsEnabled = false;
                MessagesListbox.Items.Add("Failed to occupy the space");
                Connection.DisposeAsync();
            }
        }

        private void OnGameStateChanged(string newState)
        {
            IGameState newStateInstance = null;

            if (newState == "NotStarted")
            {
                newStateInstance = new StoppedState();
            }
            else if (newState == "ArrangingShips")
            {
                newStateInstance = new ArrangingShipsState();
            }
            else if (newState == "Started")
            {
                newStateInstance = new StartedState();
            }

            if (newStateInstance != null)
            {
                Client.SetState(newStateInstance);
                newStateInstance.ApplyUI(this); // Call the new ApplyUI method
            }
        }



        private void OnChatMessage(string nickname, string message)
        {
            MessagesListbox.Items.Add(nickname + ": " + message);
        }

        private void OnSetShipResult(bool result, int x, int y, int size, bool vertical)
        {
            if (result)
            {
                if (vertical)
                {
                    for (int i = y; i < y + size; i++)
                    {
                        MyButtons[i, x].Content = "#";
                    }
                }
                else
                {
                    for (int i = x; i < x + size; i++)
                    {
                        MyButtons[y, i].Content = "#";
                    }
                }
            }

            EnableMyBoard(true);
        }

        private void OnPlayerReady(int seat, bool success, string nickname)
        {
            if(success)
            {
                MessagesListbox.Items.Add("Player " + nickname + " ready");
                if (seat == Client.Seat)
                {
                    Undo.IsEnabled = false;
                }
            }
            else
            {
                if(seat == Client.Seat)
                {
                    EnableMyBoard(true);
                    ActionButton.IsEnabled = true;
                }
            }
        }

        private void OnShotFired(int attacker, int x, int y, bool hit)
        {
            if (attacker == Client.Seat)
            {
                EnemyButtons[y, x].Content = hit ? "X" : "O";
            }
            else
            {
                MyButtons[y, x].Content = hit ? "X" : "O";
            }

            if (!hit)
            {
                Client.ChangeTurn();
            }

            // Enable or disable enemy board based on whose turn it is
            EnableEnemyBoard(Client.Turn == Client.Seat);
        }

        private void OnGameWon(string nickname)
        {
            MessagesListbox.Items.Add("Player " + nickname + " won");

            EnableMyBoard(false);
            EnableEnemyBoard(false);

            ActionButton.Content = "Finish";
            ActionButton.IsEnabled = true;

            Client.SetState(new StoppedState());
        }
        private async void SelectPowerButton_Click(object sender, RoutedEventArgs e)
        {
            if (PowerSelectionComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string powerType = selectedItem.Content.ToString();

                // Send selected power to the server for activation
                await Connection.SendAsync("EnableSpecialPower", _currentGameId, Client.Seat, powerType);

            }
            else
            {
                MessageBox.Show("Please select a power before confirming.");
            }
        }
        private void OnSpecialPowerEnabled(string powerType)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"Special Power '{powerType}' enabled!");
                SelectedPowerText.Text = $"Selected Power: {powerType}";
            });
        }
        private void OnStateSaved()
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"Board saved!");
            });
        }
        private void OnExtraShotAllowed()
        {
            Dispatcher.Invoke(() =>
            {
                Client.ChangeTurn();
                EnableEnemyBoard(Client.Turn == Client.Seat);

            });
        }
        private async void UndoAction(object sender, RoutedEventArgs e)
        {
            await Connection.SendAsync("Undo", _currentGameId, Client.Seat);

        }
        private void UnmarkShip(int x, int y, int size, bool vertical)
        {
            if (vertical)
            {
                for (int i = y; i < y + size; i++)
                {
                    MyButtons[i, x].Content = "";
                }
            }
            else
            {
                for (int i = x; i < x + size; i++)
                {
                    MyButtons[y, i].Content = "";
                }
            }

            EnableMyBoard(true);
        }

        private void ThemeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeSelector.SelectedItem is ComboBoxItem selectedItem)
            {
                SetTheme(selectedItem.Content.ToString());
            }
        }

        private void ApplyTheme()
        {
            if (_themeFactory == null) return;

            var buttonStyle = _themeFactory.CreateButtonStyle();
            var gridStyle = _themeFactory.CreateGridStyle();

            // Apply general UI button styles
            buttonStyle.ApplyStyle(ActionButton);
            buttonStyle.ApplyStyle(JoinButton);
            buttonStyle.ApplyStyle(SelectPowerButton);
            buttonStyle.ApplyStyle(Undo);

            // Apply grid background style
            gridStyle.ApplyGridBackground(MyBoard);
            gridStyle.ApplyGridBackground(EnemyBoard);

            // Apply style to grid buttons in MyButtons and EnemyButtons
            foreach (Button button in MyButtons)
            {
                if (button != null) gridStyle.ApplyGridButtonStyle(button);
            }

            foreach (Button button in EnemyButtons)
            {
                if (button != null) gridStyle.ApplyGridButtonStyle(button);
            }
        }
        
        private void OnGameCreated(string gameId)
		{
			
			_lobbyWindow.AddGame(gameId);
		}
        private async void SaveState_Click(object sender, RoutedEventArgs e)
        {
            await Connection.SendAsync("SavePlayerState", _currentGameId, Client.Seat);
        }

        private async void RestoreState_Click(object sender, RoutedEventArgs e)
        {
            await Connection.SendAsync("RestorePlayerState", _currentGameId, Client.Seat);
        }

    }
}
