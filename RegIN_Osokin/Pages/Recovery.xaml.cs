using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace RegIN_Osokin.Pages
{
    /// <summary>
    /// Логика взаимодействия для Recovery.xaml
    /// </summary>
    public partial class Recovery : Page
    {
        string OldLogin;
        bool IsCapture = false;
        public Recovery()
        {
            InitializeComponent();
            MainWindow.mainWindow.UserLogin.HandlerInCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.UserLogin.HandlerInCorrectLogin += InCorrectLogin;
            Capture.HandlerCorrectCapture += CorrectCapture;
        }
        private void CorrectLogin()
        {
            if (OldLogin != TbLogin.Text)
            {
                SetNotification("Hi, " + MainWindow.mainWindow.UserLogin.Name, Brushes.Black);

                try
                {
                    BitmapImage bilImg = new BitmapImage();
                    MemoryStream ms = new MemoryStream(MainWindow.mainWindow.UserLogin.Image);
                    bilImg.BeginInit();
                    bilImg.StreamSource = ms;
                    bilImg.EndInit();
                    ImageSource imgSrc = bilImg;

                    DoubleAnimation StartAnimation = new DoubleAnimation();
                    StartAnimation.From = 1;
                    StartAnimation.To = 0;
                    StartAnimation.Duration = TimeSpan.FromSeconds(0.6);
                    StartAnimation.Completed += delegate
                    {
                        IUser.Source = imgSrc;

                        DoubleAnimation EndAnimation = new DoubleAnimation();
                        EndAnimation.From = 0;
                        EndAnimation.To = 1;
                        EndAnimation.Duration = TimeSpan.FromSeconds(1.2);
                        IUser.BeginAnimation(Image.OpacityProperty, EndAnimation);
                    };
                    IUser.BeginAnimation(Image.OpacityProperty, StartAnimation);
                }
                catch (Exception exp)
                {
                    Debug.WriteLine(exp.Message);
                }

                OldLogin = TbLogin.Text;
                SendNewPassword();
            }
        }
        private void InCorrectLogin()
        {
            if (!LNameUser.Content.Equals(""))
            {
                LNameUser.Content = "";

                DoubleAnimation StartAnimation = new DoubleAnimation();
                StartAnimation.From = 1;
                StartAnimation.To = 0;
                StartAnimation.Duration = TimeSpan.FromSeconds(0.6);

                StartAnimation.Completed += delegate
                {
                    IUser.Source = new BitmapImage(new Uri("pack://application:,,,/Images/ic-user.png"));

                    DoubleAnimation EndAnimation = new DoubleAnimation();
                    EndAnimation.From = 0;
                    EndAnimation.To = 1;
                    EndAnimation.Duration = TimeSpan.FromSeconds(1.2);

                    IUser.BeginAnimation(Image.OpacityProperty, EndAnimation);
                };

                IUser.BeginAnimation(Image.OpacityProperty, StartAnimation);
            }

            if (TbLogin.Text.Length > 0)
            {
                SetNotification("Login is incorrect", Brushes.Red);
            }
        }
        private void CorrectCapture()
        {
            Capture.IsEnabled = false;
            IsCapture = true;
            SendNewPassword();
        }
        private void SetLogin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MainWindow.mainWindow.UserLogin.GetUserLogin(TbLogin.Text);
            }
        }

        private void SetLogin(object sender, RoutedEventArgs e) =>
            MainWindow.mainWindow.UserLogin.GetUserLogin(TbLogin.Text);
        public void SendNewPassword()
        {
            if (IsCapture)
            {
                if (MainWindow.mainWindow.UserLogin.Password != String.Empty)
                {
                    DoubleAnimation StartAnimation = new DoubleAnimation();
                    StartAnimation.From = 1;
                    StartAnimation.To = 0;
                    StartAnimation.Duration = TimeSpan.FromSeconds(0.6);

                    StartAnimation.Completed += delegate
                    {
                        IUser.Source = new BitmapImage(new Uri("pack://application:,,,/Images/mail.png"));

                        DoubleAnimation EndAnimation = new DoubleAnimation();
                        EndAnimation.From = 0;
                        EndAnimation.To = 1;
                        EndAnimation.Duration = TimeSpan.FromSeconds(1.2);

                        IUser.BeginAnimation(Image.OpacityProperty, EndAnimation);
                    };

                    IUser.BeginAnimation(Image.OpacityProperty, StartAnimation);
                    SetNotification("An email has been sent to your email..", Brushes.Black);
                    MainWindow.mainWindow.UserLogin.CreateNewPassword();
                }
            }
        }
        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LNameUser.Content = Message;
            LNameUser.Foreground = _Color;
        }
        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Pages.Login());

        }
    }
}
