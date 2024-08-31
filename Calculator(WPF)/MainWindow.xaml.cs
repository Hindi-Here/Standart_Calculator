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
using System.Text.RegularExpressions;

namespace Calculator_WPF_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double number, result;
        public int index, index_last;
        public bool StartCalc = true, MethodCalc = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumButton_Click(object sender, RoutedEventArgs e)
        {
            Button send = (Button)sender;
            CalculatorBlock.Text += send.Content;
        }

        private void CalculatorBlock_TextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("^[\\D]+", RegexOptions.Compiled | RegexOptions.CultureInvariant);
            e.Handled = reg.IsMatch(e.Text);
        }
        private void FuncButton_Click(object sender, RoutedEventArgs e)
        {
            Button send = (Button)sender;

            index = send.TabIndex;   //запись текущей операции и запоминание числа
            _ = CalculatorBlock.Text == "" ? number = 0 : number = Convert.ToDouble(CalculatorBlock.Text);

            if (StartCalc && index <= 7)  // запоминание самого первого числа, введенного в калькуляторе
            {
                LastNumBlock.Text = CalculatorBlock.Text;
                result += number;
                StartCalc = false;
            }

            if (index <= 7)  // проверка на использование метода
            {
                OperationBlock.Text = send.Content.ToString();  //блок вывода выбранной операции

               Calc_Method(index);
                
            }
            else
                CalcSpecial_Method(index);
        }

        private void InversButton_Click(object sender, RoutedEventArgs e) //инверсия числа
        {
            CalculatorBlock.Text = (Convert.ToDouble(CalculatorBlock.Text) * -1).ToString();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e) //Очистка полей
        {
            List<TextBox> control = new List<TextBox>() { CalculatorBlock, OperationBlock, LastNumBlock };
            foreach (TextBox s in control)
                s.Clear();

            NullResult();
        }

        private void EqualityButton_Click(object sender, RoutedEventArgs e) //кнопка равенства
        {
            OperationBlock.Clear();
            LastNumBlock.Clear();

            _ = CalculatorBlock.Text == "" ? number = 0 : number = Convert.ToDouble(CalculatorBlock.Text);
            Calc_Method(index);   //выполнение последнего действия
            CalculatorBlock.Text = result.ToString();

            NullResult();
        }
        void NullResult()
        {
            StartCalc = true;
            index_last = 0;
            number = 0;
            result = 0;
        }  // обнуление всех результатов вычислений

        public void Calc_Method(int index) // выполнение длительных операций
        {
            switch (index_last)
            {
                case 3:
                    result += number;
                    break;
                case 4:
                    result -= number;
                    break;
                case 5:
                    result *= number;
                    break;
                case 6:
                    result /= number;
                    break;
                case 7:
                    result %= number;
                    break;
            }
            index_last = index;
            LastNumBlock.Text = result.ToString();
            CalculatorBlock.Clear();
        }
        public void CalcSpecial_Method(int index) // выполнение мгновенных операций
        {
            switch (index)
            {
                case 8:
                    number = Math.Sqrt(number);
                    break;
                case 9:
                    number = Math.Pow(number, 2);
                    break;
                case 10:
                    number = Math.Log(number, 2);
                    break;
                case 11:
                    number = Math.Sin(number);
                    break;
                case 12:
                    number = Math.Cos(number);
                    break;
                case 13:
                    number = Math.Tan(number);
                    break;
                case 14:
                    number = Math.Log10(number);
                    break;
                case 15:
                    number = Math.Sinh(number);
                    break;
                case 16:
                    number = Math.Cosh(number);
                    break;
                case 17:
                    number = Math.Tanh(number);
                    break;
            }
            CalculatorBlock.Text = number.ToString();
        }
    }
}
