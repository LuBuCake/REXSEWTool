/*
This file is part of RESIDENT EVIL XSEW Tool.

RESIDENT EVIL XSEW Tool is free software: you can redistribute it
and/or modify it under the terms of the GNU General Public License
as published by the Free Software Foundation, either version 3 of
the License, or (at your option) any later version.

RESIDENT EVIL XSEW Tool is distributed in the hope that it will
be useful, but WITHOUT ANY WARRANTY; without even the implied
warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with RESIDENT EVIL XSEW Tool. If not, see <https://www.gnu.org/licenses/>6.
*/

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

        public static int[] AdaptationTable = {
            230, 230, 230, 230, 307, 409, 512, 614,
            768, 614, 512, 409, 307, 230, 230, 230
        };

        public static int[] AdaptCoeff1 = { 256, 512, 0, 192, 240, 460, 392 };
        public static int[] AdaptCoeff2 = { 0, -256, 0, 64, 0, -208, -232 };

        private static int[] RemapSoundData(int[] SoundData, int TargetSampleQuantity)
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

        public static void ChoosePredictor(int[] SoundData, ref int sample_index, ref int predictor, ref int delta)
        {
            int IDELTA_COUNT = 24;

            int bestpred = 0;
            int bestdelta = 0;
            int deltasum;

            for (int bpred = 0; bpred < 7; bpred++)
            {
                deltasum = 0;

                for (int k = sample_index; k < sample_index + IDELTA_COUNT; k++)
                {
                    deltasum += Math.Abs(SoundData[k] - (SoundData[k - 1] * AdaptCoeff1[bpred] + SoundData[k - 2] * AdaptCoeff2[bpred] >> 8));
                }

                deltasum /= (4 * IDELTA_COUNT);

                if (bpred == 0 || deltasum < bestdelta)
                {
                    bestpred = bpred;
                    bestdelta = deltasum;
                }

                if (deltasum == 0)
                {
                    bestpred = bpred;
                    bestdelta = 16;
                    break;
                }
            }

            if (bestdelta < 16)
                bestdelta = 16;

            predictor = bestpred;
            delta = bestdelta;
        }

        // DECODE

        private static int IMA_MS_ExpandNibble(byte nibble, int nibble_shift, ref int predictor, ref int sample1, ref int sample2, ref int coeff1, ref int coeff2, ref int delta)
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

        public static int[] DecodeMS_IMA(int[] BlockHeader, byte[] BlockData)
        {
            int[] result = new int[(BlockData.Length * 2) + 2];
            int result_index = 0;

            int predictor = Clamp(BlockHeader[0], 0, 6);
            int delta = BlockHeader[1];
            int sample1 = BlockHeader[2];
            int sample2 = BlockHeader[3];

            result[result_index] = sample2; result_index++;
            result[result_index] = sample1; result_index++;

            int coeff1 = AdaptCoeff1[predictor];
            int coeff2 = AdaptCoeff2[predictor];

            for (int i = 0; i < BlockData.Length; i++)
            {
                result[result_index] = IMA_MS_ExpandNibble(BlockData[i], 4, ref predictor, ref sample1, ref sample2, ref coeff1, ref coeff2, ref delta); result_index++;
                result[result_index] = IMA_MS_ExpandNibble(BlockData[i], 0, ref predictor, ref sample1, ref sample2, ref coeff1, ref coeff2, ref delta); result_index++;
            }

            return result;
        }

        // ENCODE

        private static int IMA_MS_SimplifyNibble(int sample, ref int sample1, ref int sample2, ref int coeff1, ref int coeff2, ref int delta)
        {
            int predictor, sample_nibble;

            predictor = ((sample1 * coeff1) + (sample2 * coeff2)) >> 8;
            sample_nibble = Clamp((sample - predictor) / delta, -8, 6);

            predictor += sample_nibble * delta;
            predictor = Clamp(predictor, -32768, 32767);

            if (sample_nibble < 0)
                sample_nibble += 16;

            sample2 = sample1;
            sample1 = predictor;

            delta = (AdaptationTable[sample_nibble] * delta) >> 8;

            if (delta < 16)
                delta = 16;

            return sample_nibble;
        }

        private static int[] GenerateBlockHeader(int[] SoundData, ref int predictor, ref int delta, ref int coeff1, ref int coeff2, ref int sample1, ref int sample2, ref int sample_index)
        {
            sample2 = SoundData[sample_index]; sample_index++;
            sample1 = SoundData[sample_index]; sample_index++;

            ChoosePredictor(SoundData, ref sample_index, ref predictor, ref delta);

            coeff1 = AdaptCoeff1[predictor];
            coeff2 = AdaptCoeff2[predictor];

            int[] BlockHeader = new int[4];
            BlockHeader[0] = predictor;
            BlockHeader[1] = delta;
            BlockHeader[2] = sample1;
            BlockHeader[3] = sample2;

            return BlockHeader;
        }

        private static int[] GenerateBlockData(int[] SoundData, ref int coeff1, ref int coeff2, ref int sample1, ref int sample2, ref int delta, ref int sample_index)
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

        public static int[][][] EncodeMS_IMA(int[] SoundData)
        {
            int BlockQuantity = (int)Math.Ceiling((double)SoundData.Length / ((63 * 2) + 2));
            SoundData = RemapSoundData(SoundData, BlockQuantity * ((63 * 2) + 2));

            int[][][] Result = new int[2][][];
            Result[0] = new int[BlockQuantity][]; // Block Header
            Result[1] = new int[BlockQuantity][]; // Block Data

            int predictor = 0;
            int delta = 0;
            int coeff1 = 0;
            int coeff2 = 0;
            int sample1 = 0;
            int sample2 = 0;

            int sample_index = 0;

            for (int i = 0; i < BlockQuantity; i++)
            {
                Result[0][i] = GenerateBlockHeader(SoundData, ref predictor, ref delta, ref coeff1, ref coeff2, ref sample1, ref sample2, ref sample_index);
                Result[1][i] = GenerateBlockData(SoundData, ref coeff1, ref coeff2, ref sample1, ref sample2, ref delta, ref sample_index);
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
                int[] Decoded = XSEWHelper.DecodeMS_IMA(BlockHeader[i], BlockData[i]);

                Decoded.CopyTo(DecodedSamples, index);
                index += Decoded.Length;
            }
        }

        public bool CheckXSEW()
        {
            if (ChunkID == "RIFF" && Format == "WAVE")
            {
                return AudioFormat == 2 && BlockAlign == 70 && NumChannels == 1;
            }

            return false;
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
        public uint SampleRate;
        public uint ByteRate;
        public ushort BlockAlign = 70;
        public ushort BitsPerSample = 4;

        // ADPCM Extra data
        public ushort ExtraDataSize = 32;
        public byte[] ExtraData = new byte[] { 0x80, 0, 7, 0, 0, 1, 0, 0, 0, 2, 0, 0xFF, 0, 0, 0, 0, 0xC0, 0, 0x40, 0, 0xF0, 0, 0, 0, 0xCC, 1, 0x30, 0xFF, 0x88, 1, 0x18, 0xFF };

        // DATA sub-chunk
        public string Subchunck2ID = "data";
        public uint Subchunk2Size;
        public int[][][] Subchunk2Data;

        // SMPL sub-chunk
        public byte[] SmplData;

        public XSEWWriter(string FilePath, int BlockQuantity, uint SampleRate, int[][][] BlockData, int Mode = 1)
        {
            WriteNewXSEW(FilePath, BlockQuantity, SampleRate, BlockData, Mode);
        }

        public XSEWWriter(string FilePath, byte[] XSEWRIFFData, byte[] XSEWSmplData, byte[] XSEWSoundData, int Mode = 1)
        {
            WriteExistentXSEW(FilePath, XSEWRIFFData, XSEWSmplData, XSEWSoundData, Mode);
        }

        private void WriteNewXSEW(string FilePath, int BlockQuantity, uint SampleRate, int[][][] BlockData, int Mode)
        {
            FileStream FS = new FileStream(FilePath, FileMode.Create);
            BinaryWriter BW = new BinaryWriter(FS);

            // Getting Data

            ByteRate = SampleRate * NumChannels * BitsPerSample / 8;

            Subchunk2Size = (uint)(BlockQuantity * BlockAlign);
            Subchunk2Data = BlockData;

            SmplData = XSEWHelper.XSEWFooterSmpl();

            ChunkSize = (uint)(4 + (8 + Subchunk1Size) + 8 + Subchunk2Size + SmplData.Length);

            // Writing

            // RIFF Chunk
            BW.Write(ChunkID.ToCharArray());
            BW.Write(ChunkSize);
            BW.Write(Format.ToCharArray());

            // FMT sub-chunk
            BW.Write(Subchunck1ID.ToCharArray());
            BW.Write(Subchunk1Size);
            BW.Write(AudioFormat);
            BW.Write(NumChannels);
            BW.Write(SampleRate);
            BW.Write(ByteRate);
            BW.Write(BlockAlign);
            BW.Write(BitsPerSample);

            // Extra data
            BW.Write(ExtraDataSize);
            BW.Write(ExtraData);

            // DATA sub-chunk
            BW.Write(Subchunck2ID.ToCharArray());
            BW.Write(Subchunk2Size);

            for (int i = 0; i < BlockQuantity; i++)
            {
                // Header
                BW.Write((byte)BlockData[0][i][0]);
                BW.Write((short)BlockData[0][i][1]);
                BW.Write((short)BlockData[0][i][2]);
                BW.Write((short)BlockData[0][i][3]);

                // Data
                for (int s = 0; s < BlockData[1][i].Length; s++)
                {
                    BW.Write((byte)BlockData[1][i][s]);
                }
            }

            BW.Write(SmplData);

            if (Mode > 1)
                BW.Write(XSEWHelper.XSEWFooterTimeVer());

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
