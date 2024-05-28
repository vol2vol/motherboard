using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace YourProjectName
{
    public partial class MainWindow : Window
    {
        private string correctAnswer;
        private bool isCpuSocketTest, isRamTest, isPcieTest, isBiosTest, isQuartzTest, isBiosBatteryTest, is12VTest, is5VTest, is3_3VTest, isUsbTest, isResistanceTest;
        private string explanation;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TestMethodToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton clickedButton = sender as ToggleButton;
            foreach (var button in new[] { ResistanceButton, VoltageButton, OscilloscopeButton, RamTesterButton, PcieTesterButton, CpuSocketTesterButton})
            {
                if (button != clickedButton)
                    button.IsChecked = false;
            }
            clickedButton.IsChecked = true;
            ResetFlagsAndUI();
        }

        private void ResetFlagsAndUI()
        {
            isCpuSocketTest = isRamTest = isPcieTest = isBiosTest = isQuartzTest = isBiosBatteryTest = is12VTest = is5VTest = is3_3VTest = isUsbTest = isResistanceTest = false;
            HideQuestionAndAnswers();
            OscilloscopeImage.Visibility = Visibility.Collapsed;
        }

        private void Voltage3_3V_Click(object sender, RoutedEventArgs e)
        {
            if (VoltageButton.IsChecked == true)
                TestVoltage(3.3, 3.0, 3.6, "линии 3.3V", "+3,3 ± 0,3 вольт");
            else if (ResistanceButton.IsChecked == true)
                TestResistance(100, 10000, "линии 3.3V", "от 100 Ом до 10 кОм");
            else
                OutputTextBlock.Text = "Выбран неправильный метод тестирования.";
        }

        private void Voltage5V_Click(object sender, RoutedEventArgs e)
        {
            if (VoltageButton.IsChecked == true)
                TestVoltage(5, 4.5, 5.5, "линии 5V", "+5 ± 0,5 вольт");
            else if (ResistanceButton.IsChecked == true)
                TestResistance(100, 10000, "линии 5V", "от 100 Ом до 10 кОм");
            else
                OutputTextBlock.Text = "Выбран неправильный метод тестирования.";
        }

        private void Voltage12V_Click(object sender, RoutedEventArgs e)
        {
            if (VoltageButton.IsChecked == true)
                TestVoltage(12, 11.8, 13.2, "линии 12V", "+12V ± 1,2 вольта");
            else if (ResistanceButton.IsChecked == true)
                TestResistance(1000, 20000, "линии 12V", "от 1 кОм до 20 кОм");
            else
                OutputTextBlock.Text = "Выбран неправильный метод тестирования.";
        }

        private void TestVoltage(double nominalVoltage, double minVoltage, double maxVoltage, string lineName, string normalRange)
        {
            Random rand = new Random();
            double voltage = (rand.Next(2) == 0) ? (nominalVoltage - 1) + rand.NextDouble() * 2 : minVoltage + rand.NextDouble() * (maxVoltage - minVoltage);
            OutputTextBlock.Text = $"Напряжение {lineName}: {voltage:F1} В";
            correctAnswer = (voltage >= minVoltage && voltage <= maxVoltage) ? "Да" : "Нет";
            explanation = $"Так как нормальные значения для {lineName} должны быть в диапазоне {normalRange}.";
            ShowQuestionAndAnswers();
        }

        private void TestResistance(double minResistance, double maxResistance, string lineName, string normalRange)
        {
            Random rand = new Random();
            double resistance = (rand.Next(2) == 0) ? rand.NextDouble() * minResistance : minResistance + rand.NextDouble() * (maxResistance - minResistance);
            OutputTextBlock.Text = $"Сопротивление {lineName}: {resistance:F1} Ом";
            correctAnswer = (resistance >= minResistance && resistance <= maxResistance) ? "Да" : "Нет";
            explanation = $"Так как нормальные значения сопротивлений для {lineName} должны быть в диапазоне {normalRange}.";
            ShowQuestionAndAnswers();
        }

        private void SocketButton_Click(object sender, RoutedEventArgs e)
        {
            if (CpuSocketTesterButton.IsChecked == true)
                TestRandom("сокета ЦПУ", new[] { "Все индикаторы горят красным", "Часть индикаторов не горит", "Ни один из индикаторов не горит" }, "Да", "Нет", "при исправном сокете процессора все индикаторы должны гореть красным.");
            else
                OutputTextBlock.Text = "Выбран неправильный метод тестирования.";
        }

        private void RamButton_Click(object sender, RoutedEventArgs e)
        {
            if (RamTesterButton.IsChecked == true)
                TestRandom("слота ОЗУ", new[] { "Все индикаторы горят красным", "Часть индикаторов не горит", "Ни один из индикаторов не горит" }, "Да", "Нет", "при исправном слоте ОЗУ все индикаторы должны гореть красным.");
            else
                OutputTextBlock.Text = "Выбран неправильный метод тестирования.";
        }

        private void ResistanceButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void PcieButton_Click(object sender, RoutedEventArgs e)
        {
            if (PcieTesterButton.IsChecked == true)
                TestRandom("слота PCIe", new[] { "Есть сигнал", "Нет сигнала" }, "Да", "Нет", "должен быть сигнал.");
            else
                OutputTextBlock.Text = "Выбран неправильный метод тестирования.";
        }

        private void TestRandom(string element, string[] messages, string correctMsg, string incorrectMsg, string additionalExplanation)
        {
            Random rand = new Random();
            int index = rand.Next(messages.Length);
            OutputTextBlock.Text = messages[index];
            correctAnswer = (index == 0) ? correctMsg : incorrectMsg;
            explanation = $"Так как {additionalExplanation}";
            ShowQuestionAndAnswers();
        }

        private void BiosButton_Click(object sender, RoutedEventArgs e)
        {
            if (OscilloscopeButton.IsChecked == true)
                TestOscilloscope("BIOS", "так как нет осциллограммы.", "так как есть осциллограмма.");
            else
                OutputTextBlock.Text = "Выбран неправильный метод тестирования.";
        }

        private void QuartzButton_Click(object sender, RoutedEventArgs e)
        {
            if (OscilloscopeButton.IsChecked == true)
                TestOscilloscope("кварцевого резонатора", "так как нет осциллограммы.", "так как есть осциллограмма.");
            else
                OutputTextBlock.Text = "Выбран неправильный метод тестирования.";
        }

        private void TestOscilloscope(string element, string incorrectExplanation, string correctExplanation)
        {
            string[] images = { "pack://application:,,,/faultyoscilloscope.jpg", "pack://application:,,,/workingoscilloscope.jpg" };
            Random rand = new Random();
            int index = rand.Next(images.Length);
            string selectedImage = images[index];

            BitmapImage bitmap = new BitmapImage(new Uri(selectedImage));
            OscilloscopeImage.Source = bitmap;
            OscilloscopeImage.Visibility = Visibility.Visible;

            correctAnswer = (index == 0) ? "Нет" : "Да";
            explanation = (index == 0) ? incorrectExplanation : correctExplanation;

            ShowQuestionAndAnswers();
        }

        private void BiosBatteryButton_Click(object sender, RoutedEventArgs e)
        {
            if (VoltageButton.IsChecked == true)
                TestVoltage(3.0, 2.7, 3.3, "батарейки BIOS", "+3 ± 0,3 вольт");
            else
                OutputTextBlock.Text = "Выбран неправильный метод тестирования.";
        }

        private void UsbButton_Click(object sender, RoutedEventArgs e)
        {
            if (VoltageButton.IsChecked == true)
                TestVoltage(5.0, 4.75, 5.25, "линии USB", "5V ± 0,25V");
            else
                OutputTextBlock.Text = "Выбран неправильный метод тестирования.";
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            string userAnswer = (sender as Button).Content.ToString();
            string resultMessage = (userAnswer == correctAnswer) ? "Правильный ответ!" : "Неправильный ответ.";
            resultMessage += $" {explanation}";
            OutputTextBlock.Text = resultMessage;
            HideQuestionAndAnswers();
        }

        private void ShowQuestionAndAnswers()
        {
            QuestionTextBlock.Text = "Правильно ли работает этот элемент?";
            YesButton.Visibility = Visibility.Visible;
            NoButton.Visibility = Visibility.Visible;
        }

        private void HideQuestionAndAnswers()
        {
            QuestionTextBlock.Text = string.Empty;
            YesButton.Visibility = Visibility.Collapsed;
            NoButton.Visibility = Visibility.Collapsed;
            OscilloscopeImage.Visibility = Visibility.Collapsed;
        }
    }
}
