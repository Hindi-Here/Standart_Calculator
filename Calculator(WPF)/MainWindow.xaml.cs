﻿using System;
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
                case 3: result += number; break;
                case 4: result -= number; break;
                case 5: result *= number; break;
                case 6: result /= number; break;
                case 7: result %= number; break;
            }
            index_last = index;
            LastNumBlock.Text = result.ToString();
            CalculatorBlock.Clear();
        }
        public void CalcSpecial_Method(int index) // выполнение мгновенных операций
        {
            var specialOperations = new Dictionary<int, Func<double, double>>
            {
                { 8, Math.Sqrt },
                { 9, x => Math.Pow(x, 2) },
                { 10, x => Math.Log(x, 2) },
                { 11, Math.Sin },
                { 12, Math.Cos },
                { 13, Math.Tan },
                { 14, Math.Log10 },
                { 15, Math.Sinh },
                { 16, Math.Cosh },
                { 17, Math.Tanh }
            };

            if (specialOperations.ContainsKey(index))
                number = specialOperations[index](number);

            CalculatorBlock.Text = number.ToString();
        }
    }
}