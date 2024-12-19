using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client
{
    public partial class LobbyWindow : Window
    {
        public event EventHandler GameJoined;
        private readonly HubConnection _connection;
        private List<string> _games = new();
        
        public LobbyWindow(HubConnection connection)
        {
            InitializeComponent();
            _connection = connection;
            _ = LoadActiveGames();
        }
        
        private void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameList.SelectedItem is string gameId)
            {
                GameJoined?.Invoke(this, new GameJoinedEventArgs(gameId));
                this.Close();
            }
        }

        private async Task LoadActiveGames()
        {
            var activeGames = await _connection.InvokeAsync<List<string>>("GetActiveGames");
            if (activeGames.Any())
            {
                _games = activeGames;
                GameList.ItemsSource = _games;
            }
        }
        
        private async void CreateGameButton_Click(object sender, RoutedEventArgs e)
        {
            var gameId = await _connection.InvokeAsync<string>("CreateGame");
            await LoadActiveGames();
        }
        
        public void AddGame(string gameId)
        {
            _games.Add(gameId);
            GameList.ItemsSource = null;
            GameList.ItemsSource = _games;
        }
    }
}