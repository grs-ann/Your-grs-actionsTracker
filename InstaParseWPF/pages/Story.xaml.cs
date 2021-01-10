using InstaParseWPF.InstagramTracker;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Story.xaml
    /// </summary>
    public partial class Story : Page
    {
        public Story()
        {
            InitializeComponent();
        }

        private void ButtonFolder_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    folderTextBox.Text = fbd.SelectedPath.ToString();
                }
            }

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (userAddress.Text != null)
            {
                var storyURI = new List<string>();
                Parsing p = new Parsing(Login._instaApi);
                Dictionary<string, List<string>> urises = new Dictionary<string, List<string>>()
                {
                    ["images"] = new List<string>(),
                    ["videos"] = new List<string>()
                };
                urises = await p.GetStories(userAddress.Text, urises);
                await Downloader.DownloadStoryFeedToFolder(folderTextBox.Text, userAddress.Text, urises);
            }
        }
    }
}
