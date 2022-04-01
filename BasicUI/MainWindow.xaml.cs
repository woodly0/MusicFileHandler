using BasicUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows;
using System.Windows.Media;
//using system.windows.media.imaging;
//using system.windows.navigation;
//using system.windows.controls;
//using system.windows.data;
//using system.windows.documents;
//using system.windows.input;

namespace BasicUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PlaylistModel playList = new PlaylistModel()
        {
            Directory = Directory.GetCurrentDirectory(),
            Name = "myPlaylist"
        };
        //public ObservableCollection<SongModel> playList = new ObservableCollection<SongModel>();

        public MainWindow()
        {
            InitializeComponent();
            playList.Songs = new ObservableCollection<SongModel>();
            this.DataContext = playList;

            //this.dataGrid1.ItemsSource = playList.Songs;
        }


        private void dataGrid1_Drop(object sender, DragEventArgs e)
        {
            if (this.checkOverwright.IsChecked == true)
            {
                playList.Songs.Clear();
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                playList.Name = Utility.ExtractFileName(files, playList.Name);
                Utility.ReceiveFiles(files, playList.Songs);
            }
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            for (int i = dataGrid1.SelectedItems.Count - 1; i >= 0; i--)
            {
                playList.Songs.Remove((SongModel)dataGrid1.SelectedItems[i]);
            }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            if (playList.Songs.Count > 0)
            {
                playList.Songs.Clear();
            }
            playList.Name = "";
        }

        private void buttonBrows_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                playList.Directory = dialog.FileName;
            }
        }

        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!playList.ValidateName())
            {
                txtName.Background = Brushes.PaleGoldenrod;
                return;
            }
            else
            {
                txtName.Background = Brushes.White;
            }

            if (!playList.ValidateDirectory())
            {
                txtDirectory.Background = Brushes.PaleGoldenrod;
                return;
            }
            else
            {
                txtDirectory.Background = Brushes.Transparent;
            }

            if (!playList.ValidateSongs())
            {
                dataGrid1.Background = Brushes.PaleGoldenrod;
                return;
            }
            else
            {
                dataGrid1.Background = Brushes.White;
            }

            if (playList.CopyList())  // Action!
            {
                MessageBox.Show("Alle files have been copied.","My Nigga", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }







        //private void btnChangeUser_Click(object sender, RoutedEventArgs e)
        //{
        //    if (lbUsers.SelectedItem != null)
        //        (lbUsers.SelectedItem as User).Name = "Random Name";
        //}

        //private void buttonAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    Utility.GetDummySong(playList);
        //    //dataGrid1.Items.Refresh();
        //}



    }
}
