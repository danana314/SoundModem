using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SoundModem.Model;
using SoundModem.Base;

namespace SoundModemUI
{
    public class ViewModel : NotifyPropertyChangedBase
    {
        private readonly static Model _model = new Model();
        public Model Model
        {
            get
            {
                return ViewModel._model;
            }
        }

        public string UnencodedMessage { get; set; }
        public string EncodedMessage { get; set; }
        public string DecodedMessage { get; set; }
        public int Frequency { get; set; }

        public ICommand BeepCmd { get; set; }
        public ICommand EncodeCmd { get; set; }

        public ViewModel()
        {
            this.BeepCmd = new RelayCommand(p => this.Model.Beep(Frequency));
            this.EncodeCmd = new RelayCommand(p => EncodeMessage());
        }

        private void EncodeMessage()
        {
            EncodedMessage = Model.Encode(UnencodedMessage);
            DecodedMessage = Model.Decode(EncodedMessage);
            NotifyPropertyChanged("EncodedMessage");
            NotifyPropertyChanged("DecodedMessage");
        }
    }
}
