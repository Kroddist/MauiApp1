namespace MauiApp1
{
    public partial class TicTacToePage : ContentPage
    {
        private string[,] board = new string[3, 3];
        private string currentPlayer = "X";
        private bool gameActive = true;
        private int scoreX = 0;
        private int scoreO = 0;
        private Button[,] buttons = new Button[3, 3];

        public TicTacToePage()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            buttons[0, 0] = Btn00; buttons[0, 1] = Btn01; buttons[0, 2] = Btn02;
            buttons[1, 0] = Btn10; buttons[1, 1] = Btn11; buttons[1, 2] = Btn12;
            buttons[2, 0] = Btn20; buttons[2, 1] = Btn21; buttons[2, 2] = Btn22;

            ResetBoard();
        }

        private void ResetBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = "";
                    buttons[i, j].Text = "";
                    buttons[i, j].BackgroundColor = Color.FromArgb("#F5F5F5");
                }
            }
            currentPlayer = "X";
            gameActive = true;
            UpdateCurrentPlayerDisplay();
        }

        private void UpdateCurrentPlayerDisplay()
        {
            CurrentPlayerLabel.Text = $"Ход игрока: {currentPlayer}";
            if (currentPlayer == "X")
            {
                CurrentPlayerLabel.TextColor = Color.FromArgb("#1976D2");
                CurrentPlayerBorder.Stroke = Color.FromArgb("#1976D2");
            }
            else
            {
                CurrentPlayerLabel.TextColor = Color.FromArgb("#D32F2F");
                CurrentPlayerBorder.Stroke = Color.FromArgb("#D32F2F");
            }
        }

        private async void OnCellClicked(object? sender, EventArgs e)
        {
            if (!gameActive) return;

            var button = sender as Button;
            if (button == null) return;

            var parameter = button.CommandParameter as string;
            if (parameter == null) return;

            var coords = parameter.Split(',');
            int row = int.Parse(coords[0]);
            int col = int.Parse(coords[1]);

            if (board[row, col] != "") return;

            await button.ScaleTo(0.9, 50);
            await button.ScaleTo(1.0, 50);

            board[row, col] = currentPlayer;
            button.Text = currentPlayer;
            button.TextColor = currentPlayer == "X" ? 
                Color.FromArgb("#1976D2") : Color.FromArgb("#D32F2F");

            button.Scale = 0;
            await button.ScaleTo(1.2, 100);
            await button.ScaleTo(1.0, 100);

            if (CheckWinner())
            {
                gameActive = false;
                await HighlightWinningCells();
                await DisplayAlert("Победа!", $"Игрок {currentPlayer} победил!", "OK");
                
                if (currentPlayer == "X")
                {
                    scoreX++;
                    ScoreXLabel.Text = $"Побед: {scoreX}";
                }
                else
                {
                    scoreO++;
                    ScoreOLabel.Text = $"Побед: {scoreO}";
                }
                return;
            }

            if (IsBoardFull())
            {
                gameActive = false;
                await DisplayAlert("Ничья!", "Поле заполнено! Ничья!", "OK");
                return;
            }

            currentPlayer = currentPlayer == "X" ? "O" : "X";
            UpdateCurrentPlayerDisplay();
        }

        private bool CheckWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] != "" && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                    return true;
            }

            for (int j = 0; j < 3; j++)
            {
                if (board[0, j] != "" && board[0, j] == board[1, j] && board[1, j] == board[2, j])
                    return true;
            }

            if (board[0, 0] != "" && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
                return true;

            if (board[0, 2] != "" && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
                return true;

            return false;
        }

        private Task HighlightWinningCells()
        {
            Color winColor = currentPlayer == "X" ? 
                Color.FromArgb("#C5E1A5") : Color.FromArgb("#FFCCBC");

            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] != "" && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                {
                    for (int j = 0; j < 3; j++)
                        buttons[i, j].BackgroundColor = winColor;
                    return Task.CompletedTask;
                }
            }

            for (int j = 0; j < 3; j++)
            {
                if (board[0, j] != "" && board[0, j] == board[1, j] && board[1, j] == board[2, j])
                {
                    for (int i = 0; i < 3; i++)
                        buttons[i, j].BackgroundColor = winColor;
                    return Task.CompletedTask;
                }
            }

            if (board[0, 0] != "" && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
            {
                buttons[0, 0].BackgroundColor = winColor;
                buttons[1, 1].BackgroundColor = winColor;
                buttons[2, 2].BackgroundColor = winColor;
                return Task.CompletedTask;
            }

            if (board[0, 2] != "" && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
            {
                buttons[0, 2].BackgroundColor = winColor;
                buttons[1, 1].BackgroundColor = winColor;
                buttons[2, 0].BackgroundColor = winColor;
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        private bool IsBoardFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == "")
                        return false;
                }
            }
            return true;
        }

        private async void OnNewGameClicked(object? sender, EventArgs e)
        {
            await GameBoard.FadeTo(0, 200);
            ResetBoard();
            await GameBoard.FadeTo(1, 200);
        }

        private async void OnResetScoreClicked(object? sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Сброс счета", "Вы уверены, что хотите сбросить счет?", "Да", "Нет");
            if (answer)
            {
                scoreX = 0;
                scoreO = 0;
                ScoreXLabel.Text = "Побед: 0";
                ScoreOLabel.Text = "Побед: 0";
                ResetBoard();
            }
        }
    }
}

