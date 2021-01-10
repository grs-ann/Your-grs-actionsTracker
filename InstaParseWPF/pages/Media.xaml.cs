using InstaParseWPF.InstagramTracker;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace InstaParseWPF.Pages
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Media : Page
    {
        public Media()
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
        // start parsing click
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (userAddress != null && folderTextBox != null)
            {
                // For collecting media URI.
                Dictionary<string, List<string>> URI = new Dictionary<string, List<string>>()
                {
                    ["photoURI"] = new List<string>(),
                    ["videoURI"] = new List<string>()
                };
                Parsing p = new Parsing(Login._instaApi);
                var URIcollection = await p.GetMedia(userAddress.Text, URI);
                if (URIcollection != null)
                {
                    await Downloader.DownloadPhotoToFolder(folderTextBox.Text, userAddress.Text, URIcollection);
                    await Downloader.DownloadVideoToFolder(folderTextBox.Text, userAddress.Text, URIcollection);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Fill the all fields!");
            }
        }
    }
}
