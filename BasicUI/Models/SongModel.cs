using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicUI.Models
{
    public class SongModel : INotifyPropertyChanged
    {
        public string Artist { get; set; }
        public string Title { get; set; }


        private string _fileName;
        public string FileName {
            get { return this._fileName; }
            set
            {
                if (this._fileName != value)
                {
                    this._fileName = value;
                    this.NotifyPropertyChanged("FileName");
                }
            }
        }


        public string FilePath { get; set; }
        public double Size { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
