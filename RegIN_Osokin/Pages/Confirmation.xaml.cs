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

namespace RegIN_Osokin.Pages
{
    /// <summary>
    /// Логика взаимодействия для Confirmation.xaml
    /// </summary>
    public partial class Confirmation : Page
    {
        public enum TypeConfirmation
        {
            Login,
            Regin
        }
        TypeConfirmation ThisTypeConfirmation;
        public int Code = 0;
        public Confirmation(TypeConfirmation TypeConfirmation)
        {
            InitializeComponent();
            ThisTypeConfirmation = TypeConfirmation;
            SendMailCode();
        }
        public void SendMailCode()
        {
            Code = new Random().Next(100000, 999999);
            Classes.SendMail.SendMessage($"Login code: {Code}", MainWindow.mainWindow.UserLogin.Login);
            TimerSendMailCode();
        }
        public async void TimerSendMailCode()
        {
            for (int i = 0; i < 60; i++)
            {
                LTimer.Content = $"A second message can be sent after {60 - i} seconds";
                await Task.Delay(1000);
            }

            BSendMessage.IsEnabled = true;
            LTimer.Content = "";
        }
        private void SendMail(object sender, RoutedEventArgs e) => SendMailCode();
        private void SetCode(object sender, KeyEventArgs e)
        {
            if (TbCode.Text.Length == 6)
                SetCode();

        }
        private void SetCode(object sender, RoutedEventArgs e) => SetCode();
        void SetCode()
        {
            if (TbCode.Text == Code.ToString() && TbCode.IsEnabled == true)
            {
                TbCode.IsEnabled = false;

                string message = ThisTypeConfirmation == TypeConfirmation.Login
                    ? "Авторизация пользователя успешно подтверждена."
                    : "Регистрация пользователя успешно подтверждена.";
                MessageBox.Show(message);

                if (ThisTypeConfirmation == TypeConfirmation.Regin)
                {
                    MainWindow.mainWindow.UserLogin.SetUser();
                }
                else
                {
                    MainWindow.mainWindow.OpenPage(new PinCode());
                }
            }
        }
        private void OpenLogin(object sender, MouseButtonEventArgs e) => MainWindow.mainWindow.OpenPage(new Login());
    }
}
