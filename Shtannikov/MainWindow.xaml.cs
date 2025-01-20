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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shtannikov
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!double.TryParse(XTextBox.Text, out double x))
                {
                    MessageBox.Show("Ошибка Введено некорректное значение x!","Ошибка");
                }
                if (!double.TryParse(YTextBox.Text, out double y))
                {
                    MessageBox.Show("Ошибка Введено некорректное значение y!", "Ошибка");
                }

                Func<double, double> selectedFunction = SelectFunction();

                if (selectedFunction == null)
                {
                    MessageBox.Show("Пожалуйста, выберите функцию.");
                    return;
                }

                double result = C(x, y, selectedFunction);
                ResultTextBox.Text = result.ToString("F4");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            XTextBox.Clear();
            YTextBox.Clear();
            ResultTextBox.Text = "0";
        }

        private Func<double, double> SelectFunction()
        {
            if (ShRadioButton.IsChecked == true) return Math.Sinh; 
            if (SquareRadioButton.IsChecked == true) return x => Math.Pow(x, 2); 
            if (ExpRadioButton.IsChecked == true) return Math.Exp; 
            return null;
        }


        private double C(double x, double y, Func<double, double> f)
        {
            if (x - y == 0) // x - y = 0
            {
                return Math.Pow(f(x), 2) + Math.Pow(y, 2) + Math.Sin(y);
            }
            else if (x - y > 0) // x - y > 0
            {
                return Math.Pow(f(x) - y, 2) + Math.Cos(y);
            }
            else // x - y < 0
            {
                double derivative = Derivative(f, x); // Численная производная
                return -derivative + Math.Tan(y);
            }
        }

        // Численное вычисление производной функции f(x) в точке x
        private double Derivative(Func<double, double> f, double x, double h = 1e-5)
        {
            return (f(x + h) - f(x - h)) / (2 * h);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) 
        {
           MessageBoxResult result = MessageBox.Show("Вы точно хотите выйти?", "Предупреждение", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    e.Cancel = false;
                    break;
                case MessageBoxResult.No:
                    e.Cancel = true;
                    break;
            }  
        }
    }
}