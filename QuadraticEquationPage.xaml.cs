using System.Globalization;
using Microsoft.Maui.Controls.Shapes;

namespace MauiApp1
{
    public partial class QuadraticEquationPage : ContentPage
    {
        public QuadraticEquationPage()
        {
            InitializeComponent();
        }

        private void OnEntryChanged(object? sender, TextChangedEventArgs e)
        {
            UpdateEquationDisplay();
        }

        private void UpdateEquationDisplay()
        {
            if (string.IsNullOrWhiteSpace(EntryA.Text) && 
                string.IsNullOrWhiteSpace(EntryB.Text) && 
                string.IsNullOrWhiteSpace(EntryC.Text))
            {
                EquationBorder.IsVisible = false;
                return;
            }

            double a = ParseDouble(EntryA.Text);
            double b = ParseDouble(EntryB.Text);
            double c = ParseDouble(EntryC.Text);

            string equation = BuildEquationString(a, b, c);
            EquationLabel.Text = equation;
            EquationBorder.IsVisible = true;
        }

        private string BuildEquationString(double a, double b, double c)
        {
            string result = "";

            if (a != 0)
            {
                if (a == 1)
                    result = "x²";
                else if (a == -1)
                    result = "-x²";
                else
                    result = $"{a}x²";
            }

            if (b != 0)
            {
                if (result != "")
                {
                    if (b > 0)
                        result += $" + {(b == 1 ? "" : b.ToString())}x";
                    else
                        result += $" - {(b == -1 ? "" : Math.Abs(b).ToString())}x";
                }
                else
                {
                    if (b == 1)
                        result = "x";
                    else if (b == -1)
                        result = "-x";
                    else
                        result = $"{b}x";
                }
            }

            if (c != 0)
            {
                if (result != "")
                {
                    if (c > 0)
                        result += $" + {c}";
                    else
                        result += $" - {Math.Abs(c)}";
                }
                else
                    result = c.ToString();
            }

            if (result == "")
                result = "0";

            return result + " = 0";
        }

        private async void OnSolveClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntryA.Text))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, введите коэффициент a", "OK");
                return;
            }

            double a = ParseDouble(EntryA.Text);
            double b = ParseDouble(EntryB.Text);
            double c = ParseDouble(EntryC.Text);

            if (a == 0)
            {
                await DisplayAlert("Ошибка", "Коэффициент a не может быть равен 0.\nЭто не квадратное уравнение!", "OK");
                return;
            }

            await ResultBorder.FadeTo(0, 100);
            await StepsBorder.FadeTo(0, 100);

            SolveQuadraticEquation(a, b, c);

            ResultBorder.IsVisible = true;
            StepsBorder.IsVisible = true;

            await ResultBorder.FadeTo(1, 300);
            await StepsBorder.FadeTo(1, 300);
        }

        private void SolveQuadraticEquation(double a, double b, double c)
        {
            ResultContent.Clear();
            StepsContent.Clear();

            var titleLabel = new Label
            {
                Text = "📊 Результат",
                FontSize = 22,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#512BD4")
            };
            ResultContent.Add(titleLabel);

            AddStep("1. Вычисляем дискриминант:", false);
            AddStep($"D = b² - 4ac", false);
            AddStep($"D = ({b})² - 4 × ({a}) × ({c})", false);
            AddStep($"D = {b * b} - {4 * a * c}", false);

            double discriminant = b * b - 4 * a * c;
            AddStep($"D = {discriminant:F2}", true);

            var discriminantLabel = new Label
            {
                Text = $"Дискриминант D = {discriminant:F2}",
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#666666")
            };
            ResultContent.Add(discriminantLabel);

            if (discriminant > 0)
            {
                var statusLabel = new Label
                {
                    Text = "✅ Два различных корня",
                    FontSize = 16,
                    TextColor = Color.FromArgb("#4CAF50"),
                    Margin = new Thickness(0, 5, 0, 15)
                };
                ResultContent.Add(statusLabel);

                AddStep("\n2. Дискриминант больше нуля, уравнение имеет два корня:", false);
                AddStep($"x₁ = (-b + √D) / (2a)", false);
                AddStep($"x₂ = (-b - √D) / (2a)", false);

                double sqrtD = Math.Sqrt(discriminant);
                AddStep($"\n√D = √{discriminant:F2} = {sqrtD:F4}", true);

                double x1 = (-b + sqrtD) / (2 * a);
                double x2 = (-b - sqrtD) / (2 * a);

                AddStep($"\n3. Подставляем значения:", false);
                AddStep($"x₁ = (-({b}) + {sqrtD:F4}) / (2 × {a})", false);
                AddStep($"x₁ = {-b + sqrtD:F4} / {2 * a}", false);
                AddStep($"x₁ = {x1:F6}", true);

                AddStep($"\nx₂ = (-({b}) - {sqrtD:F4}) / (2 × {a})", false);
                AddStep($"x₂ = {-b - sqrtD:F4} / {2 * a}", false);
                AddStep($"x₂ = {x2:F6}", true);

                AddRootDisplay("x₁ =", x1, "#4CAF50");
                AddRootDisplay("x₂ =", x2, "#2196F3");
            }
            else if (discriminant == 0)
            {
                var statusLabel = new Label
                {
                    Text = "✅ Один корень (два совпадающих)",
                    FontSize = 16,
                    TextColor = Color.FromArgb("#FF9800"),
                    Margin = new Thickness(0, 5, 0, 15)
                };
                ResultContent.Add(statusLabel);

                AddStep("\n2. Дискриминант равен нулю, уравнение имеет один корень:", false);
                AddStep($"x = -b / (2a)", false);

                double x = -b / (2 * a);

                AddStep($"\n3. Подставляем значения:", false);
                AddStep($"x = -({b}) / (2 × {a})", false);
                AddStep($"x = {-b} / {2 * a}", false);
                AddStep($"x = {x:F6}", true);

                AddRootDisplay("x =", x, "#FF9800");
            }
            else
            {
                var statusLabel = new Label
                {
                    Text = "❌ Нет действительных корней",
                    FontSize = 16,
                    TextColor = Color.FromArgb("#F44336"),
                    Margin = new Thickness(0, 5, 0, 15)
                };
                ResultContent.Add(statusLabel);

                AddStep("\n2. Дискриминант меньше нуля:", false);
                AddStep("Уравнение не имеет действительных корней.", true);
                AddStep("(Корни являются комплексными числами)", false);

                var noRootsLabel = new Label
                {
                    Text = "В области действительных чисел корней нет",
                    FontSize = 16,
                    TextColor = Color.FromArgb("#666666"),
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 10)
                };
                ResultContent.Add(noRootsLabel);
            }

            if (discriminant >= 0)
            {
                AddStep("\n4. Проверка корней:", true);
                AddStep("Подставим найденные корни в исходное уравнение", false);
            }
        }

        private void AddStep(string text, bool isResult)
        {
            var label = new Label
            {
                Text = text,
                FontSize = isResult ? 16 : 15,
                FontAttributes = isResult ? FontAttributes.Bold : FontAttributes.None,
                TextColor = isResult ? Color.FromArgb("#512BD4") : Color.FromArgb("#666666"),
                Margin = new Thickness(0, 2)
            };
            StepsContent.Add(label);
        }

        private void AddRootDisplay(string prefix, double value, string colorHex)
        {
            var border = new Border
            {
                StrokeThickness = 0,
                BackgroundColor = Color.FromArgb(colorHex),
                Padding = new Thickness(20, 15),
                Margin = new Thickness(0, 5)
            };

            border.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) };

            var label = new Label
            {
                Text = $"{prefix} {value:F6}",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White,
                HorizontalOptions = LayoutOptions.Center
            };

            border.Content = label;
            ResultContent.Add(border);
        }

        private double ParseDouble(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            text = text.Replace(',', '.');
            
            if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                return result;
            
            return 0;
        }

        private async void OnClearClicked(object? sender, EventArgs e)
        {
            await Task.WhenAll(
                ResultBorder.FadeTo(0, 200),
                StepsBorder.FadeTo(0, 200),
                EquationBorder.FadeTo(0, 200)
            );

            EntryA.Text = string.Empty;
            EntryB.Text = string.Empty;
            EntryC.Text = string.Empty;
            
            ResultBorder.IsVisible = false;
            StepsBorder.IsVisible = false;
            EquationBorder.IsVisible = false;
            
            ResultContent.Clear();
            StepsContent.Clear();
        }
    }
}

