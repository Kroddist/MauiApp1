namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnTicTacToeClicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("TicTacToePage");
        }

        private async void OnQuadraticEquationClicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("QuadraticEquationPage");
        }
    }
}
