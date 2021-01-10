using InstaParseWPF.InstagramTracker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InstaParseWPF.pages
{
    /// <summary>
    /// Interaction logic for Followers.xaml
    /// </summary>
    public partial class Followers : Page
    {
        public Followers()
        {
            InitializeComponent();
        }

        // Start button.
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Parsing p = new Parsing(Login._instaApi);
            var usersList = await p.GetUserFollowers(userAddress.Text);
            var nonPrivateFollowers = usersList.Value.Where(x => x.IsPrivate == false).Select(x => x.UserName);
            using (FileStream fstream = new FileStream($@"{folderTextBox.Text}\publicFollowers.txt", FileMode.OpenOrCreate))
            {
                using (StreamWriter swriter = new StreamWriter(fstream, Encoding.UTF8))
                {
                    foreach (var item in nonPrivateFollowers)
                    {
                        await swriter.WriteLineAsync(item);
                    }
                }
            }
        }

        private void ButtonFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                folderTextBox.Text = fbd.SelectedPath.ToString();
            }
        }
    }
}
