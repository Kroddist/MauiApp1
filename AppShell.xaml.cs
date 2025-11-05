namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            // Регистрация маршрутов для навигации
            Routing.RegisterRoute("TicTacToePage", typeof(TicTacToePage));
            Routing.RegisterRoute("QuadraticEquationPage", typeof(QuadraticEquationPage));
        }
    }
}
