using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.SoundFont;
using NAudio.Wave;
using SoundModem.Base;

namespace SoundModem.Model
{
    public class Model : NotifyPropertyChangedBase
    {

        private int _sampleRate;
        private int _numChannels;
        private WaveOut _waveOut;
        private DBPSK _dbpsk;

        public Model()
        {
            this._sampleRate = 44100;   //44.1 kHz
            this._numChannels = 1;      //mono
            this._dbpsk = new DBPSK();
        }

        public void Beep(int freq)
        {
            //Console.Beep(650, 650);
            CustomBeep(freq);
        }

        public string Encode(string message)
        {
            return _dbpsk.Encode(message);
        }

        public string Decode(string message)
        {
            return _dbpsk.Decode(message);
        }

        private void CustomBeep(int freq)
        {
            if (_waveOut == null)
            {
                var sineWaveProvider = new SineWaveProvider32() {Frequency = freq, Amplitude = 0.25f};
                sineWaveProvider.SetWaveFormat(_sampleRate, _numChannels);
                _waveOut = new WaveOut();
                _waveOut.Init(sineWaveProvider);
                _waveOut.Play();
            }
            else
            {
                _waveOut.Stop();
                _waveOut.Dispose();
                _waveOut = null;
            }
        }

        
    }
}
