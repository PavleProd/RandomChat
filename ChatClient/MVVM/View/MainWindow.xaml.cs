using ChatClient.MVVM.View;
using System.Windows;
using System.Windows.Input;

namespace RandomChat
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _mainMenuControl = new MainMenuControl();
            _chatControl = new ChatControl();

            ToMainMenuPage();
        }

        public void ToMainMenuPage()
        {
            PageSource.Content = _mainMenuControl;
        }

        public void ToChatPage()
        {
            PageSource.Content = _chatControl;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private MainMenuControl _mainMenuControl;
        private ChatControl _chatControl;
    }
}