using InstaParseWPF.pages;
using InstaParseWPF.Pages;
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

namespace InstaParseWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new MainPage();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void GridHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void boardButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MainPage());
        }

        private void mediaButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Media());
        }

        private void followersButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Followers());
        }

        private void massmediaButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MassMedia());
        }

        private void storiesButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Story());
        }
    }
}
