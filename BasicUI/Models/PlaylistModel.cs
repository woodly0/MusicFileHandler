using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicUI.Models
{
    public class PlaylistModel : INotifyPropertyChanged
    {

        private string _name;
        public string Name
        {
            get { return this._name; }
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                    this.NotifyPropertyChanged("Name");
                }
            }
        }
        private bool NameIsValid;

        private string _directory;
        public string Directory
        {
            get { return this._directory; }
            set
            {
                if (this._directory != value)
                {
                    this._directory = value;
                    this.NotifyPropertyChanged("Directory");
                }
            }
        }
        private bool DirectoryIsValid;
        public ObservableCollection<SongModel> Songs { get; set; }

        private bool SongsIsValid;


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }


        public bool ValidateName()
        {
            Name = Utility.CleanString(Name);
            NameIsValid = Utility.StringIsValid(Name, 3, 20);
            return NameIsValid;
        }


        public bool ValidateDirectory()
        {
            try
            {
                Utility.CreateDir(Directory + "\\" + Name);
                DirectoryIsValid = true;
            }
            catch (Exception ex)
            {
                DirectoryIsValid = false;
            }
            return DirectoryIsValid;
        }


        public bool ValidateSongs()
        {
            List<SongModel> distinctSongs = Songs
                                  .GroupBy(s => s.FileName)
                                  .Select(g => g.First())
                                  .ToList();
            Songs.Clear();
            foreach (var song in distinctSongs) { Songs.Add(song); }   // thats probably not the best way to remove duplicates

            if (Songs.Count > 1)
            {
                SongsIsValid = true;
            }
            else
            {
                SongsIsValid = false;
            }
            return SongsIsValid;
        }


        public bool CopyList()
        {
            if (NameIsValid && DirectoryIsValid && SongsIsValid)
            {
                var target = Directory + "\\" + Name;
                foreach (var song in Songs)
                {
                    Utility.CopyFile(song.FilePath + "\\" + song.FileName, target + "\\" + song.FileName);
                }
                return true;
            }
            return false;
        }


    }
}
