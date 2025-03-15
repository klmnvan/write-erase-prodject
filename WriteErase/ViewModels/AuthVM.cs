using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Linq;
using WriteErase.Helpers;
using WriteErase.Models;

namespace WriteErase.ViewModels
{
    partial class AuthVM : ViewModelBase
    {
        [ObservableProperty]
        private string _userLogin = "loginDEjrm2018"; 
        [ObservableProperty]
        private string _userPassword = "Cpb+Im";

        [ObservableProperty]
        private Canvas? _captcha = null;

        [ObservableProperty]
        private string _tbcaptcha = "";

        [ObservableProperty]
        private bool _isDisabled = true;

        [ObservableProperty]
        public bool isVisibleCaptcha = false;

        string _textCaptcha = "";

        public AuthVM()
        {
            _textCaptcha = "";
            Tbcaptcha = "";
            Captcha = null;
            IsDisabled = true;
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Tick += new EventHandler(EventTimer);
        }

        [RelayCommand]
        public void GoListProduct()
        {
            MainWindowViewModel.Instance.CurrentPage = new ListProductPage(new Models.Order());
        }

        /// <summary>
        /// Метод в котором описан функционал попытки авторизации
        /// </summary>
        [RelayCommand]
        public async void TryAuth()
        {
            if (UserLogin != "" && UserPassword != "")
            {
                if(Captcha == null)
                {
                    if (MainWindowViewModel.Context.Users.Any(it => it.Login == UserLogin))
                    {
                        var user = MainWindowViewModel.Context.Users.First(it => it.Login == UserLogin);
                        if (user.Password == UserPassword)
                        {
                            MainWindowViewModel.Instance.CurrentPage = new ListProductPage(user);
                        }
                        else
                        {
                            MessageBoxManager.GetMessageBoxStandard("Ошибка", "Пароль неверный!", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
                            CreateCaptha();
                        }
                    }
                    else
                    {
                        MessageBoxManager.GetMessageBoxStandard("Ошибка", $"Пользователь с логином {UserLogin} в системе не зарегистрирован!", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
                        CreateCaptha();
                    }
                }
                else
                {
                    User? user = MainWindowViewModel.Context.Users.FirstOrDefault(u => u.Password == UserPassword && u.Login == UserLogin);
                    if (user != null && Tbcaptcha == _textCaptcha) MainWindowViewModel.Instance.CurrentPage = new ListProductPage(user);
                    else
                    {
                        Captcha = null;
                        IsDisabled = false;
                        IsVisibleCaptcha = false;
                        UserLogin = "";
                        UserPassword = "";
                        Tbcaptcha = "";
                        timer.Start(); // запуск таймера
                        await MessageBoxManager.GetMessageBoxStandard("Доступ временно ограничен", "Попробуйте еще раз через 10 секунд", ButtonEnum.Ok).ShowAsync();
                    }
                }

            }
            else MessageBoxManager.GetMessageBoxStandard("Ошибка", "Не все поля заполнены!", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
        }

        /// <summary>
		/// Генерация капчи и отображение
		/// </summary>
		private void CreateCaptha()
        {
            _textCaptcha = "";
            RandomElements rndEl = new RandomElements();
            Random rnd = new Random();
            Canvas canvas = new Canvas()
            {
                Width = 400,
                Height = 100,
                Background = rndEl.GetRandomColor()
            };
            for (int i = 0; i < 4; i++)
            {
                TextBlock text = new TextBlock()
                {
                    Text = rndEl.GetRandomText(),
                    FontSize = 50,
                    Foreground = Brushes.Black,
                    Padding = rndEl.GetRandomThickness(i + 1),
                    FontWeight = rnd.Next(2) == 0 ? FontWeight.Bold : FontWeight.Normal,
                    FontStyle = rnd.Next(2) == 0 ? FontStyle.Italic : FontStyle.Normal
                };
                _textCaptcha += text.Text;
                canvas.Children.Add(text);
            }
            for (int i = 0; i < 10; i++)
            {
                Line line = new Line()
                {
                    StartPoint = new Point(rnd.Next(400), rnd.Next(100)),
                    EndPoint = new Point(rnd.Next(400), rnd.Next(100)),
                    Stroke = rndEl.GetRandomColor(),
                    StrokeThickness = 3
                };
                canvas.Children.Add(line);
            }
            Captcha = canvas;
            IsVisibleCaptcha = true;
        }

        #region [ Таймер ]
        DispatcherTimer timer = new DispatcherTimer();
        /// <summary>
        /// Cобытие, которое происходит после истечения времени таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventTimer(object sender, EventArgs e)
        {
            IsDisabled = true;
            CreateCaptha();
            timer.Stop();
        }
        #endregion
    }
}
