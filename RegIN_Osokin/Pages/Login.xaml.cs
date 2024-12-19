using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RegIN_Osokin.Classes;

namespace RegIN_Osokin.Pages
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        int CountSetPassword = 2;
        bool IsCapture = false;

        public Login()
        {
            InitializeComponent();

            MainWindow.mainWindow.UserLogin.HandlerCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.UserLogin.HandlerInCorrectLogin += InCorrectLogin;
            Capture.HandlerCorrectCapture += CorrectCapture;
        }

        public void CorrectLogin()
        {
            if (TbLogin.Text != MainWindow.mainWindow.UserLogin.Login)
            {
                SetNotification($"Hi, {MainWindow.mainWindow.UserLogin.Name}", Brushes.Black);
                UpdateImage(MainWindow.mainWindow.UserLogin.Image);
            }
        }
        public void InCorrectLogin()
        {
            if (!string.IsNullOrEmpty(LNameUser.Content.ToString()))
            {
                SetNotification("Login is incorrect", Brushes.Red);
                UpdateImage(new byte[] { });
            }
        }
        public void CorrectCapture()
        {
            Capture.IsEnabled = false;
            IsCapture = true;
        }

        public void SetPassword()
        {
            if (MainWindow.mainWindow.UserLogin.Password != String.Empty)
            {
                if (IsCapture)
                {
                    if (MainWindow.mainWindow.UserLogin.Password == TbPassword.Password)
                    {
                        if (string.IsNullOrEmpty(MainWindow.mainWindow.UserLogin.PinCode))
                        {
                            MainWindow.mainWindow.OpenPage(new Confirmation(Confirmation.TypeConfirmation.Login));
                        }
                        else
                        {
                            MainWindow.mainWindow.OpenPage(new PinCode());
                        }
                    }
                    else
                    {
                        if (CountSetPassword > 0)
                        {
                            SetNotification($"Password is incorrect, {CountSetPassword} attempts left", Brushes.Red);
                            CountSetPassword--;
                        }
                        else
                        {
                            Thread TBlockAuthorization = new Thread(BlockAuthorization);
                            TBlockAuthorization.Start();
                            SendMail.SendMessage("An attempt was made to log into your account.", MainWindow.mainWindow.UserLogin.Login);
                        }
                    }
                }
                else
                {
                    SetNotification("Enter capture", Brushes.Red);
                }
            }
        }
        public void BlockAuthorization()
        {
            DateTime StartBlock = DateTime.Now.AddMinutes(3);
            Dispatcher.Invoke(() =>
            {
                TbLogin.IsEnabled = false;
                TbPassword.IsEnabled = false;
                Capture.IsEnabled = false;
            });

            while (DateTime.Now < StartBlock)
            {
                TimeSpan remainingTime = StartBlock - DateTime.Now;
                string timeRemaining = $"{remainingTime.Minutes:00}:{remainingTime.Seconds:00}";
                Dispatcher.Invoke(() => SetNotification($"Reauthorization available in: {timeRemaining}", Brushes.Red));
                Thread.Sleep(1000);
            }
            Dispatcher.Invoke(() =>
            {
                SetNotification($"Hi, {MainWindow.mainWindow.UserLogin.Name}", Brushes.Black);
                TbLogin.IsEnabled = true;
                TbPassword.IsEnabled = true;
                Capture.IsEnabled = true;
                Capture.CreateCapture();
                IsCapture = false;
                CountSetPassword = 2;
            });
        }

        private void UpdateImage(byte[] imageData)
        {
            try
            {
                BitmapImage biImg = new BitmapImage();
                if (imageData.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        biImg.BeginInit();
                        biImg.StreamSource = ms;
                        biImg.EndInit();
                    }
                }
                else
                {
                    biImg = new BitmapImage(new Uri("pack://application:,,,/Images/ic-user.png"));
                }

                AnimateImage(biImg);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void AnimateImage(ImageSource imgSrc)
        {
            DoubleAnimation startAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.6)
            };

            startAnimation.Completed += (sender, e) =>
            {
                IUser.Source = imgSrc;
                DoubleAnimation endAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1.2)
                };
                IUser.BeginAnimation(Image.OpacityProperty, endAnimation);
            };
            IUser.BeginAnimation(Image.OpacityProperty, startAnimation);
        }

        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LNameUser.Content = Message;
            LNameUser.Foreground = _Color;
        }

        private void SetLogin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MainWindow.mainWindow.UserLogin.GetUserLogin(TbLogin.Text);

                if (TbPassword.Password.Length > 0)
                {
                    SetPassword();
                }
            }
        }

        private void SetPassword(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SetPassword();
        }

        private void RecoveryPassword(object sender, MouseButtonEventArgs e) => MainWindow.mainWindow.OpenPage(new Recovery());

        private void OpenRegin(object sender, MouseButtonEventArgs e) => MainWindow.mainWindow.OpenPage(new Regin());

        private void SetLogin(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.UserLogin.GetUserLogin(TbLogin.Text);

            if (TbPassword.Password.Length > 0)
            {
                SetPassword();
            }
        }
    }
}
