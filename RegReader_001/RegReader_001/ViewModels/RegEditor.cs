using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RegReader_001.ViewModels
{
    // data storage class
    class WaveFormatExtensible
    {
        public byte[] raw;                  // raw bytestream read directly from registry
        public Int16 numOfChannels;         // e.g.: stereo:2
        public Int32 samplingRate;          // e.g.: 48000 (48khz)
        public Int32 avgBytesPerSec;        // internal, e.g.: numofchannels * bitspersec * bytespersample
        public Int16 blockAlign;            // internal, ???
        public Int16 bitsPerSample;         // e.g.: 16 bit / 24 bit / 32 bit
        public Int16 additionalSize;        // internal, not important to show it
        public Int16 reserved;              // internal
        public Int32 channelMask;           // ???
        public Guid  subFormat;             // internal
    }

    class RegEditor : INotifyPropertyChanged
    {
        RegReader               _regReader  = new RegReader();
        WaveFormatExtensible    _wave1      = new WaveFormatExtensible();   // 16 bits
        WaveFormatExtensible    _wave2      = new WaveFormatExtensible();   // 32 bits


        public RegEditor()
        {
            _wave1.raw = new byte[48];
            _wave2.raw = new byte[48];

            listChannelNum.Add("Stereo");
            listChannelNum.Add("5.1");
        }

        public void ReadRegistry()
        {
            // Read bytestream from Registry
            _wave1.raw = (byte[]) _regReader.Read("{f19f064d-082c-4e27-bc73-6882a1bb8e4c},0");   // 16 bits
            _wave2.raw = (byte[]) _regReader.Read("{e4870e26-3cc5-4cd2-ba46-ca0a9a70ed04},0");   // 32 bits

            // Parse Bytestream into dataholder class
            WaveFormaExtParser.ParseDataToStruct(ref _wave1);
            WaveFormaExtParser.ParseDataToStruct(ref _wave2);

            // Populate Fields of ViewModel
            ChannelNumber   = _wave1.numOfChannels.ToString();
            SamplingRate    = _wave1.samplingRate.ToString();
            BitsPerSample   = _wave1.bitsPerSample.ToString();
            ChannelMask     = "0x" + _wave1.channelMask.ToString("X");

            ChannelNumber2  = _wave2.numOfChannels.ToString();
            SamplingRate2   = _wave2.samplingRate.ToString();
            BitsPerSample2  = _wave2.bitsPerSample.ToString();
            ChannelMask2    = "0x" + _wave2.channelMask.ToString("X");
        }

        #region Fields

        private ObservableCollection<string> listChannelNum = new ObservableCollection<string>();
        public ObservableCollection<string> ListChannelNum
        {
            get
            {
                return this.listChannelNum;
            }

            set
            {
                if (value != this.listChannelNum)
                {
                    this.listChannelNum = value;
                    NotifyPropertyChanged("ListChannelNum");
                }
            }
        }

        private string channelNumber;
        public string ChannelNumber
        {
            get
            {
                return this.channelNumber;
            }

            set
            {
                if (value != this.channelNumber)
                {
                    this.channelNumber = value;
                    NotifyPropertyChanged("ChannelNumber");
                }
            }
        }

        private string samplingRate;
        public string SamplingRate
        {
            get
            {
                return this.samplingRate;
            }

            set
            {
                if (value != this.samplingRate)
                {
                    this.samplingRate = value;
                    NotifyPropertyChanged("SamplingRate");
                }
            }
        }

        private string bitsPerSample;
        public string BitsPerSample
        {
            get
            {
                return this.bitsPerSample;
            }

            set
            {
                if (value != this.bitsPerSample)
                {
                    this.bitsPerSample = value;
                    NotifyPropertyChanged("BitsPerSample");
                }
            }
        }

        private string channelMask;
        public string ChannelMask
        {
            get
            {
                return this.channelMask;
            }

            set
            {
                if (value != this.channelMask)
                {
                    this.channelMask = value;
                    NotifyPropertyChanged("ChannelMask");
                }
            }
        }

        private string channelNumber2;
        public string ChannelNumber2
        {
            get
            {
                return this.channelNumber2;
            }

            set
            {
                if (value != this.channelNumber2)
                {
                    this.channelNumber2 = value;
                    NotifyPropertyChanged("ChannelNumber2");
                }
            }
        }

        private string samplingRate2;
        public string SamplingRate2
        {
            get
            {
                return this.samplingRate2;
            }

            set
            {
                if (value != this.samplingRate2)
                {
                    this.samplingRate2 = value;
                    NotifyPropertyChanged("SamplingRate2");
                }
            }
        }

        private string bitsPerSample2;
        public string BitsPerSample2
        {
            get
            {
                return this.bitsPerSample2;
            }

            set
            {
                if (value != this.bitsPerSample2)
                {
                    this.bitsPerSample2 = value;
                    NotifyPropertyChanged("BitsPerSample2");
                }
            }
        }

        private string channelMask2;
        public string ChannelMask2
        {
            get
            {
                return this.channelMask2;
            }

            set
            {
                if (value != this.channelMask2)
                {
                    this.channelMask2 = value;
                    NotifyPropertyChanged("ChannelMask2");
                }
            }
        }

        #endregion
        
        #region PropertyChangedInterface

        public event PropertyChangedEventHandler PropertyChanged;
        // This method is called by the Set accessor of each property. 
        // The CallerMemberName attribute that is applied to the optional propertyName 
        // parameter causes the property name of the caller to be substituted as an argument. 
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
