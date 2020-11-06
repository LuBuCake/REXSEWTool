using System.IO;

namespace REXSEWTool
{
    public class SPAC
    {
        FileStream FS;
        BinaryReader BR;

        // HEADER Buffer = 0x20 (32)

        public string Format;
        public int Version;
        public int NumSounds;
        public int DataUnknow1;
        public int DataUnknow2;
        public int Meta1Start;
        public int Meta2Start;
        public int SoundDataStart;

        private int XSEWBufferSize = 0x92;
        private int XSEWRIFFBufferSize = 0x4E;
        private int XSEWSmplBufferSize = 0x44;

        public int MetaSize;
        public byte[] Meta;

        public byte[][] XSEWRIFFData;
        public byte[][] XSEWSmplData;
        public byte[][] XSEWSoundData;

        public int[] XSEWSoundDataSize;

        public SPAC(string FilePath)
        {
            LoadSpac(FilePath);
        }

        private void LoadSpac(string FilePath)
        {
            FS = new FileStream(FilePath, FileMode.Open);
            BR = new BinaryReader(FS);

            // HEADER

            for (int i = 0; i < 4; i++)
            {
                Format = Format + (char)BR.ReadByte();
            }

            Version = BR.ReadInt32();
            NumSounds = BR.ReadInt32();
            DataUnknow1 = BR.ReadInt32();
            DataUnknow2 = BR.ReadInt32();
            Meta1Start = BR.ReadInt32();
            Meta2Start = BR.ReadInt32();
            SoundDataStart = BR.ReadInt32();

            long SavePosition = BR.BaseStream.Position;

            if (!CheckSPAC())
                return;

            // DATA

            XSEWRIFFData = new byte[NumSounds][];
            XSEWSmplData = new byte[NumSounds][];
            XSEWSoundData = new byte[NumSounds][];
            XSEWSoundDataSize = new int[NumSounds];

            BR.BaseStream.Position += 0x4A;

            for (int i = 0; i < NumSounds; i++)
            {
                XSEWSoundDataSize[i] = BR.ReadInt32();
                BR.BaseStream.Position -= 4;
                BR.BaseStream.Position += XSEWBufferSize;
            }

            BR.BaseStream.Position = SavePosition;

            for (int i = 0; i < NumSounds; i++)
            {
                // RIFF
                XSEWRIFFData[i] = new byte[XSEWRIFFBufferSize];
                XSEWRIFFData[i] = BR.ReadBytes(XSEWRIFFBufferSize);

                // Smpl
                XSEWSmplData[i] = new byte[XSEWSmplBufferSize];
                XSEWSmplData[i] = BR.ReadBytes(XSEWSmplBufferSize);
            }

            MetaSize = SoundDataStart - ((XSEWBufferSize * NumSounds) + 32);

            Meta = new byte[MetaSize];
            Meta = BR.ReadBytes(MetaSize);

            for (int i = 0; i < NumSounds; i++)
            {
                XSEWSoundData[i] = new byte[XSEWSoundDataSize[i]];
                XSEWSoundData[i] = BR.ReadBytes(XSEWSoundDataSize[i]);
            }

            FS.Dispose();
            BR.Dispose();
        }

        public void SaveSPAC(string FilePath)
        {
            FS = new FileStream(FilePath, FileMode.Create);
            BinaryWriter BW = new BinaryWriter(FS);

            // HEADER

            BW.Write(Format.ToCharArray());
            BW.Write(Version);
            BW.Write(NumSounds);
            BW.Write(DataUnknow1);
            BW.Write(DataUnknow2);
            BW.Write(Meta1Start);
            BW.Write(Meta2Start);
            BW.Write(SoundDataStart);

            for (int i = 0; i < NumSounds; i++)
            {
                BW.Write(XSEWRIFFData[i]);
                BW.Write(XSEWSmplData[i]);
            }

            BW.Write(Meta);

            for (int i = 0; i < NumSounds; i++)
            {
                BW.Write(XSEWSoundData[i]);
            }

            FS.Dispose();
            BW.Dispose();
        }

        public bool CheckSPAC()
        {
            return Format == "SPAC" && Version == 13;
        }

        public void ReplaceXSEW(int Index, byte[] RIFF, byte[] Smpl, byte[] SoundData)
        {
            XSEWRIFFData[Index] = RIFF;
            XSEWSmplData[Index] = Smpl;
            XSEWSoundData[Index] = SoundData;
        }

        public void ExtractXSEW(string Directory)
        {
            XSEWWriter XsewW;
            string filename;

            for (int i = 0; i < NumSounds; i++)
            {
                filename = Directory + "/" + i.ToString() + ".xsew";
                XsewW = new XSEWWriter(filename, XSEWRIFFData[i], XSEWSmplData[i], XSEWSoundData[i]);
            }
        }
    }
}
