using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace RegIN_Osokin.Pages
{
    /// <summary>
    /// Логика взаимодействия для PinCode.xaml
    /// </summary>
    public partial class PinCode : Page
    {
        private string userLogin = MainWindow.mainWindow.UserLogin.Login;

        public PinCode()
        {
            InitializeComponent();
        }

        public void SetPinCode()
        {
            if (IsValidPinCode(TbPinCode.Text))
            {
                SetNotification("", Brushes.Black);
                MainWindow.mainWindow.UserLogin.GetPinCode(TbPinCode.Text);
            }
            else
            {
                SetNotification("Pin code must be 4 digits.", Brushes.Red);
            }
            OnPinCode();
            MessageBox.Show("Пин-код подтвержден.");
        }
        void OnPinCode()
        {
            if (IsFormValid())
            {
                MainWindow.mainWindow.UserLogin.PinCode = TbPinCode.Text;
                MainWindow.mainWindow.OpenPage(new Login());
            }
        }
        private bool IsValidPinCode(string pinCode) =>
            new Regex(@"^\d{4}$").IsMatch(pinCode);
        private bool IsFormValid() =>
            !string.IsNullOrEmpty(TbPinCode.Text) && IsValidPinCode(TbPinCode.Text);

        private void OpenLogin(object sender, MouseButtonEventArgs e) =>
            MainWindow.mainWindow.OpenPage(new Login());
        private void SetPinCode(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SetPinCode();
        }

        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LPinCode.Content = Message;
            LPinCode.Foreground = _Color;
        }
    }
}
