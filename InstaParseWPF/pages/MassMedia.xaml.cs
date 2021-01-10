using InstaParseWPF.InstagramTracker;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for MassMedia.xaml
    /// </summary>
    public partial class MassMedia : Page
    {
        public MassMedia()
        {
            InitializeComponent();
        }
        private void ButtonFolder_Click(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Текстовые файлы|*.txt";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    folderTextBox.Text = ofd.FileName;
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string path = folderTextBox.Text;
            if (path != null)
            {
                var userIDs = File.ReadAllLines(path);
                foreach (var userID in userIDs)
                {
                    if (userID != null && folderTextBox != null)
                    {
                        // For collecting media URI.
                        Dictionary<string, List<string>> URI = new Dictionary<string, List<string>>()
                        {
                            ["photoURI"] = new List<string>(),
                            ["videoURI"] = new List<string>()
                        };
                        Parsing p = new Parsing(Login._instaApi);
                        var URIcollection = await p.GetMedia(userID, URI);
                        if (URIcollection != null)
                        {
                            await Downloader.DownloadPhotoToFolder(folderTosave.Text, userID, URIcollection);
                            await Downloader.DownloadVideoToFolder(folderTosave.Text, userID, URIcollection);
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Fill the all fields!");
                    }
                }
            }
        }

        private void buttontoSaveFolder_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    folderTosave.Text = fbd.SelectedPath.ToString();
                }
            }
        }
    }
}
