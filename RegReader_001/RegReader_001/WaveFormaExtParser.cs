using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RegReader_001
{
    class WaveFormaExtParser
    {
        private static byte[] TakeBytesRef(byte[] array, ref int offset, int length)
        {
            byte[] temp = array.Skip(offset).Take(length)/*.Reverse()*/.ToArray();
            offset += length;
            return temp;
        }

        
        private static byte GetByte(Int32 num, int byteindex)
        {
            byte b = (byte)((num >> byteindex) & 0xFF);
            return b; 
        }

        public static object ParseDataToStruct(ref ViewModels.WaveFormatExtensible wave) 
        { 
            // get raw data
            byte[] raw = wave.raw;

            int index = 0;

            index += 8;     // unknown header
            index += 2;     // wFormat flag, @todo: check this!

            // parse raw data into member variables
            wave.numOfChannels  = BitConverter.ToInt16(TakeBytesRef(raw, ref index, 2), 0);
            wave.samplingRate   = BitConverter.ToInt32(TakeBytesRef(raw, ref index, 4), 0);
            wave.avgBytesPerSec = BitConverter.ToInt32(TakeBytesRef(raw, ref index, 4), 0);
            wave.blockAlign     = BitConverter.ToInt16(TakeBytesRef(raw, ref index, 2), 0);
            wave.bitsPerSample  = BitConverter.ToInt16(TakeBytesRef(raw, ref index, 2), 0);
            wave.additionalSize = BitConverter.ToInt16(TakeBytesRef(raw, ref index, 2), 0);
            wave.reserved       = BitConverter.ToInt16(TakeBytesRef(raw, ref index, 2), 0);
            wave.channelMask    = BitConverter.ToInt32(TakeBytesRef(raw, ref index, 4), 0);

            return null;
        }

        public static object GenerateByteStream(ViewModels.WaveFormatExtensible wave)
        {
            // get raw data
            byte[] raw = wave.raw;

            int index = 0;

            // Unknown header
            raw[index++] = 0x41;
            raw[index++] = 0x00;
            raw[index++] = 0x1c;
            raw[index++] = 0xfc;
            raw[index++] = 0x01;
            raw[index++] = 0x00;
            raw[index++] = 0x00;
            raw[index++] = 0x00;

            // wFormat
            raw[index++] = 0xfe;
            raw[index++] = 0xff;

            // parse raw data into member variables

            raw[index++] = GetByte(wave.numOfChannels, 0); raw[index++] = GetByte(wave.numOfChannels, 1);

            wave.numOfChannels = BitConverter.ToInt16(TakeBytesRef(raw, ref index, 2), 0);
            wave.samplingRate = BitConverter.ToInt32(TakeBytesRef(raw, ref index, 4), 0);
            wave.avgBytesPerSec = BitConverter.ToInt32(TakeBytesRef(raw, ref index, 4), 0);
            wave.blockAlign = BitConverter.ToInt16(TakeBytesRef(raw, ref index, 2), 0);
            wave.bitsPerSample = BitConverter.ToInt16(TakeBytesRef(raw, ref index, 2), 0);
            wave.additionalSize = BitConverter.ToInt16(TakeBytesRef(raw, ref index, 2), 0);
            wave.reserved = BitConverter.ToInt16(TakeBytesRef(raw, ref index, 2), 0);
            wave.channelMask = BitConverter.ToInt32(TakeBytesRef(raw, ref index, 4), 0);

            return raw;
        }
    }
}
