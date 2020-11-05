using System;
using System.IO;

namespace REXSEWTool
{
    public static class XSEWHelper
    {
        public static byte[] XSEWFooterTimeVer() // REV Games
        {
            DateTime Time = DateTime.UtcNow;

            short Year = (short)Time.Year;
            byte Month = (byte)Time.Month;
            byte Day = (byte)Time.Day;

            byte Hour = (byte)Time.Hour;
            byte Minute = (byte)Time.Minute;
            byte Second = (byte)Time.Second;

            return new byte[] { 0x74, 0x49, 0x4D, 0x45, 8, 0, 0, 0, BitConverter.GetBytes(Year)[0], BitConverter.GetBytes(Year)[1], Month, Day, Hour, Minute, Second, 0, 0x76, 0x65, 0x72, 0x2E, 4, 0, 0, 0, 1, 0, 0, 0 };
        }

        public static byte[] XSEWFooterSmpl()
        {
            byte[] Result = { 0x73, 0x6D, 0x70, 0x6C, 0x3C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00 };

            return Result;
        }

        // CODEC

        // Ref Page https://wiki.multimedia.cx/index.php/Microsoft_ADPCM

        private static int Clamp(int val, int min, int max)
        {
            if (val < min) { return min; }
            if (val > max) { return max; }
            return val;
        }

        public static int Sign_Extend(int Value, int Bits)
        {
            int shift = 8 * sizeof(int) - Bits;
            int v = Value << shift;
            return v >> shift;
        }

        public static int[] AdaptationTable = {
            230, 230, 230, 230, 307, 409, 512, 614,
            768, 614, 512, 409, 307, 230, 230, 230
        };

        public static int[] AdaptCoeff1 = { 256, 512, 0, 192, 240, 460, 392 };
        public static int[] AdaptCoeff2 = { 0, -256, 0, 64, 0, -208, -232 };

        // DECODE

        public static int IMA_MS_ExpandNibble(byte nibble, int nibble_shift, ref int predictor, ref int sample1, ref int sample2, ref int coeff1, ref int coeff2, ref int delta)
        {
            int sample_nibble = nibble >> nibble_shift & 0xF;
            int signed_nibble = (sample_nibble & 8) == 8 ? sample_nibble - 16 : sample_nibble;

            predictor = (((sample1 * coeff1) + (sample2 * coeff2)) >> 8) + (signed_nibble * delta);
            predictor = Clamp(predictor, -32768, 32767);

            sample2 = sample1;
            sample1 = predictor;

            delta = (AdaptationTable[sample_nibble] * delta) >> 8;

            if (delta < 16)
                delta = 16;

            return sample1;
        }

        public static int[] DecodeBlock(int[] BlockHeader, byte[] BlockData)
        {
            int[] result = new int[(BlockData.Length * 2) + 2];
            int result_index = 0;

            int predictor = Clamp(BlockHeader[0], 0, 6);
            int delta = Sign_Extend(BlockHeader[1], 16);
            int sample1 = Sign_Extend(BlockHeader[2], 16);
            int sample2 = Sign_Extend(BlockHeader[3], 16);

            result[result_index] = sample2;
            result_index++;
            result[result_index] = sample1;
            result_index++;

            int coeff1 = AdaptCoeff1[predictor];
            int coeff2 = AdaptCoeff2[predictor];

            for (int i = 0; i < BlockData.Length; i++)
            {
                result[result_index] = IMA_MS_ExpandNibble(BlockData[i], 4, ref predictor, ref sample1, ref sample2, ref coeff1, ref coeff2, ref delta);
                result_index++;
                result[result_index] = IMA_MS_ExpandNibble(BlockData[i], 0, ref predictor, ref sample1, ref sample2, ref coeff1, ref coeff2, ref delta);
                result_index++;
            }

            return result;
        }

        // ENCODE

        public static int IMA_MS_SimplifyNibble(int sample, ref int sample1, ref int sample2, ref int coeff1, ref int coeff2, ref int delta)
        {
            int predictor, nibble, bias;

            predictor = ((sample1 * coeff1) + (sample2 * coeff2)) >> 8;
            nibble = sample - predictor;

            if (nibble >= 0)
                bias = delta / 2;
            else
                bias = delta / 2;

            nibble = (nibble + bias) / delta;
            nibble &= 0x0F;

            predictor += ((nibble & 0x08) == 8 ? (nibble - 16) : nibble) * delta;

            sample2 = sample1;
            sample1 = Clamp(predictor, -32768, 32767);

            delta = (AdaptationTable[nibble] * delta) >> 8;
            if (delta < 16)
                delta = 16;

            return nibble;
        }

        public static int[] GenerateBlockHeader(int[] SoundData, ref int predictor, ref int delta, ref int sample1, ref int sample2, ref int sample_index)
        {
            // Update next samples and apply them into the header

            sample1 = SoundData[sample_index]; sample_index++;
            sample2 = SoundData[sample_index]; sample_index++;

            int[] BlockHeader = new int[4];
            BlockHeader[0] = predictor;
            BlockHeader[1] = delta;
            BlockHeader[2] = sample2;
            BlockHeader[3] = sample1;

            return BlockHeader;
        }

        public static int[] GenerateBlockData(int[] SoundData, ref int predictor, ref int coeff1, ref int coeff2, ref int sample1, ref int sample2, ref int delta, ref int sample_index)
        {
            int[] BlockData = new int[63];

            int nibble;

            for (int i = 0; i < 63; i++)
            {
                nibble = IMA_MS_SimplifyNibble(SoundData[sample_index], ref sample1, ref sample2, ref coeff1, ref coeff2, ref delta) << 4; sample_index++;
                nibble |= IMA_MS_SimplifyNibble(SoundData[sample_index], ref sample1, ref sample2, ref coeff1, ref coeff2, ref delta); sample_index++;
                BlockData[i] = nibble;
            }

            return BlockData;
        }

        public static int[] RemapSoundData(int[] SoundData, int TargetSampleQuantity)
        {
            int[] newsounddata = new int[TargetSampleQuantity];

            int index = 0;
            SoundData.CopyTo(newsounddata, index);
            index += SoundData.Length;

            if (SoundData.Length < newsounddata.Length)
            {
                for (int i = index; i < newsounddata.Length; i++)
                {
                    newsounddata[i] = 0;
                }
            }

            return newsounddata;
        }

        public static int[][][] EncodeMS_IMA(int[] SoundData)
        {
            int BlockQuantity = (int)Math.Ceiling((double)SoundData.Length / ((63 * 2) + 2));
            SoundData = RemapSoundData(SoundData, BlockQuantity * ((63 * 2) + 2));

            int[][][] Result = new int[2][][];
            Result[0] = new int[BlockQuantity][]; // Block Header
            Result[1] = new int[BlockQuantity][]; // Block Data

            // Master indexer
            int sample_index = 0;

            int predictor = 0;
            int delta = 16;
            int coeff1 = AdaptCoeff1[predictor];
            int coeff2 = AdaptCoeff2[predictor];
            int sample1 = 0;
            int sample2 = 0;

            for (int i = 0; i < BlockQuantity; i++)
            {
                Result[0][i] = GenerateBlockHeader(SoundData, ref predictor, ref delta, ref sample1, ref sample2, ref sample_index);
                Result[1][i] = GenerateBlockData(SoundData, ref predictor, ref coeff1, ref coeff2, ref sample1, ref sample2, ref delta, ref sample_index);
            }

            return Result;
        }
    }

    public class XSEWReader
    {
        FileStream FS;
        BinaryReader BR;

        // RIFF Chunk
        public string ChunkID;          // 4 Bytes raw string 'RIFF'
        public uint ChunkSize;          // unasigned int, should equal to total filelength - 8
        public string Format;           // 4 Bytes raw string 'WAVE'

        public byte[] RIFFData;         // Packed RIFF and FMT chunk

        // FMT sub-chunk
        public string Subchunck1ID;     // 4 Bytes raw string 'fmt '
        public uint Subchunk1Size;      // 4 Bytes MTF 2.0 = 50
        public ushort AudioFormat;      // 2 Bytes 2 = Microsoft ADPCM
        public ushort NumChannels;      // 2 Bytes Mono = 1, Stereo = 2, etc...
        public uint SampleRate;         // 4 Bytes 8000, 44100, etc...
        public uint ByteRate;           // 4 Bytes SampleRate * NumChannels * BitsPerSample / 8
        public ushort BlockAlign;       // 2 Bytes NumChannels * BitsPerSample / 8
        public ushort BitsPerSample;    // 2 Bytes 8 bits = 8, 16 bits = 16, etc...

        // ADPCM Extra data
        public ushort ExtraDataSize;    // 4 Bytes MTF 2.0 = 32
        public byte[] ExtraData;        // ExtraDataSize MTF 2.0 = { 80 00 07 00 00 01 00 00 00 02 00 FF 00 00 00 00 C0 00 40 00 F0 00 00 00 CC 01 30 FF 88 01 18 FF }

        // DATA sub-chunk
        public string Subchunck2ID;     // 4 Bytes raw string 'data'
        public uint Subchunk2Size;      // 4 Bytes NumSamples * NumChannels * BitsPerSample / 8
        public byte[] Subchunk2Data;    // Var array containing the raw sample data

        // BLOCK Data
        public int NumBlocks;
        public int[][] BlockHeader;
        public byte[][] BlockData;

        // FOOTER sub-chunk
        public string FooterID;
        public bool FooterFixed;

        // SMPL/FACT sub-chunk
        public byte[] SmplData;
        public byte[] FactData;

        // DECODED
        public int[] DecodedSamples;

        public XSEWReader(string FilePath, bool Decode = false)
        {
            FS = new FileStream(FilePath, FileMode.Open);
            BR = new BinaryReader(FS);

            // RIFF Reading

            RIFFData = new byte[78];
            RIFFData = BR.ReadBytes(78);

            BR.BaseStream.Position -= 78;

            for (int i = 0; i < 4; i++)
            {
                ChunkID += (char)BR.ReadByte();
            }

            ChunkSize = BR.ReadUInt32();

            for (int i = 0; i < 4; i++)
            {
                Format += (char)BR.ReadByte();
            }

            // FMT Reading

            for (int i = 0; i < 4; i++)
            {
                Subchunck1ID = Subchunck1ID + (char)BR.ReadByte();
            }

            Subchunk1Size = BR.ReadUInt32();
            AudioFormat = BR.ReadUInt16();
            NumChannels = BR.ReadUInt16();
            SampleRate = BR.ReadUInt32();
            ByteRate = BR.ReadUInt32();
            BlockAlign = BR.ReadUInt16();
            BitsPerSample = BR.ReadUInt16();

            if (!CheckXSEW())
                return;

            // Extra

            ExtraDataSize = BR.ReadUInt16();
            ExtraData = new byte[ExtraDataSize];
            ExtraData = BR.ReadBytes(ExtraDataSize);

            // Data

            for (int i = 0; i < 4; i++)
            {
                Subchunck2ID += (char)BR.ReadByte();
            }

            Subchunk2Size = BR.ReadUInt32();
            Subchunk2Data = new byte[(int)Subchunk2Size];
            Subchunk2Data = BR.ReadBytes((int)Subchunk2Size);

            BR.BaseStream.Position -= Subchunk2Size;

            NumBlocks = (int)(Subchunk2Size / BlockAlign);
            BlockHeader = new int[NumBlocks][];
            BlockData = new byte[NumBlocks][];

            for (int i = 0; i < NumBlocks; i++)
            {
                BlockHeader[i] = new int[4];
                BlockHeader[i][0] = BR.ReadByte();
                BlockHeader[i][1] = BR.ReadInt16();
                BlockHeader[i][2] = BR.ReadInt16();
                BlockHeader[i][3] = BR.ReadInt16();

                BlockData[i] = new byte[BlockAlign - 7];
                BlockData[i] = BR.ReadBytes(BlockAlign - 7);
            }

            for (int i = 0; i < 4; i++)
            {
                FooterID += (char)BR.ReadByte();
            }

            BR.BaseStream.Position -= 4;

            if (FooterID == "smpl")
            {
                SmplData = new byte[68];
                SmplData = BR.ReadBytes(68);

                FooterFixed = true;
            }
            else if (FooterID == "fact")
            {
                FactData = new byte[12];
                FactData = BR.ReadBytes(12);

                FooterFixed = false;
            }

            FS.Dispose();
            BR.Dispose();

            if (Decode)
                DecodeSamples();
        }

        public void FixFooter()
        {
            SmplData = XSEWHelper.XSEWFooterSmpl();

            ChunkSize = 0x46 + Subchunk2Size + (uint)SmplData.Length;

            byte[] NewSize = BitConverter.GetBytes(ChunkSize);

            RIFFData[4] = NewSize[0];
            RIFFData[5] = NewSize[1];
            RIFFData[6] = NewSize[2];
            RIFFData[7] = NewSize[3];
        }

        private void DecodeSamples()
        {
            DecodedSamples = new int[((BlockAlign - 5) * NumBlocks) * 2];

            int index = 0;

            for (int i = 0; i < NumBlocks; i++)
            {
                int[] Decoded = XSEWHelper.DecodeBlock(BlockHeader[i], BlockData[i]);

                Decoded.CopyTo(DecodedSamples, index);
                index += Decoded.Length;
            }
        }

        public bool CheckXSEW()
        {
            return ChunkID == "RIFF" && Format == "WAVE" && AudioFormat == 2;
        }
    }

    public class XSEWWriter
    {
        // RIFF Chunk
        public string ChunkID = "RIFF";
        public uint ChunkSize;
        public string Format = "WAVE";

        // FMT sub-chunk
        public string Subchunck1ID = "fmt ";
        public uint Subchunk1Size = 50;
        public ushort AudioFormat = 2;
        public ushort NumChannels = 1;
        public uint SampleRate;         // 4 Bytes 8000, 44100, etc...
        public uint ByteRate;           // 4 Bytes SampleRate * NumChannels * BitsPerSample / 8
        public ushort BlockAlign = 70;
        public ushort BitsPerSample = 4;

        // ADPCM Extra data
        public uint ExtraDataSize = 32;
        public byte[] ExtraData = new byte[] { 0x80, 0, 7, 0, 0, 1, 0, 0, 0, 2, 0, 0xFF, 0, 0, 0, 0, 0xC0, 0, 0x40, 0, 0xF0, 0, 0, 0, 0xCC, 1, 0x30, 0xFF, 0x88, 1, 0x18, 0xFF };

        // DATA sub-chunk
        public string Subchunck2ID;     // 4 Bytes raw string 'data'
        public uint Subchunk2Size;      // 4 Bytes NumSamples * NumChannels * BitsPerSample / 8
        public int[] Subchunk2Data;     // Var array containing the raw sample data

        public XSEWWriter(string FilePath, byte[] XSEWRIFFData, byte[] XSEWSmplData, byte[] XSEWSoundData, int Mode = 1)
        {
            WriteExistentXSEW(FilePath, XSEWRIFFData, XSEWSmplData, XSEWSoundData, Mode);
        }

        private void WriteNewXSEW(string FilePath, uint SampleRate, int[] SoundData)
        {
            FileStream FS = new FileStream(FilePath, FileMode.Create);
            BinaryWriter BW = new BinaryWriter(FS);

            int Samples = SoundData.Length;
            ByteRate = SampleRate * NumChannels * BitsPerSample / 8;

            Subchunk2Data = SoundData;


            FS.Dispose();
            BW.Dispose();
        }

        private void WriteExistentXSEW(string FilePath, byte[] XSEWRIFFData, byte[] XSEWSmplData, byte[] XSEWSoundData, int Mode)
        {
            FileStream FS = new FileStream(FilePath, FileMode.Create);
            BinaryWriter BW = new BinaryWriter(FS);

            BW.Write(XSEWRIFFData);
            BW.Write(XSEWSoundData);
            BW.Write(XSEWSmplData);

            if (Mode > 1)
                BW.Write(XSEWHelper.XSEWFooterTimeVer());

            FS.Dispose();
            BW.Dispose();
        }
    }
}
