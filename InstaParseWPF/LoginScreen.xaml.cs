using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InstaParseWPF.InstagramTracker;

namespace InstaParseWPF
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }
        private void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            Login.userLogin = txtUsername.Text;
            // This is bad practice!!!!!!! But now it is not critically.
            Login.userPassword = txtPassword.Text;
            try
            {
                var result = Task.Run(Login.MainAsync).GetAwaiter().GetResult();
                if (result)
                {
                    MessageBox.Show("Вход выполнен успешно.");
                }
                else
                {
                    MessageBox.Show("Что то пошло не так.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
