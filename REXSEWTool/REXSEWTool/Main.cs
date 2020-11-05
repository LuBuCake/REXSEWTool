using System;
using System.IO;
using System.Windows.Forms;

namespace REXSEWTool
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        SPAC WorkingSPAC;

        private string[] ToWAVFiles;
        private string[] ToWAVSafeFiles;
        private string[] ToXSEWFiles;
        private string[] ToXSEWSafeFiles;

        private string SPACFilePath;

        private void Main_Load(object sender, EventArgs e)
        {
            SPACFilePath = "";
            ToWAVFiles = new string[0];
            ToWAVSafeFiles = new string[0];
            ToXSEWFiles = new string[0];
            ToXSEWSafeFiles = new string[0];

            SPACNameTextBox.Text = "No SPC file opened";
        }

        // SPAC Tools

        private bool CheckSPAC()
        {
            if (WorkingSPAC == null)
            {
                MessageBox.Show("No SPC container loaded.", "Ops!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return false;
            }

            return true;
        }

        private void OpenSPACButton_Click(object sender, EventArgs e)
        {
            string SPACNameSafe;

            using (OpenFileDialog OpenFile = new OpenFileDialog())
            {
                OpenFile.Filter = "RE6 SPC files (*.spc)|*.spc";
                OpenFile.Title = "Select a valid SPC Container file";

                if (OpenFile.ShowDialog() == DialogResult.OK)
                {
                    SPACFilePath = OpenFile.FileName;
                    WorkingSPAC = new SPAC(SPACFilePath);

                    if (!WorkingSPAC.CheckSPAC())
                    {
                        MessageBox.Show("This is not a valid SPC file.", "Ops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        SPACFilePath = "";
                        WorkingSPAC = null;
                        return;
                    }

                    SPACNameSafe = OpenFile.SafeFileName;

                    if (SPACNameSafe.Contains(".spc"))
                        SPACNameSafe = SPACNameSafe.Replace(".spc", "");

                    SPACNameTextBox.Text = SPACNameSafe;

                    SPACSoundsComboBox.Items.Clear();

                    for (int i = 0; i < WorkingSPAC.NumSounds; i++)
                    {
                        SPACSoundsComboBox.Items.Add("SOUND #: " + i.ToString());
                    }

                    SPACSoundsComboBox.Items.Add("MULTIPLE");
                    SPACSoundsComboBox.SelectedIndex = 0;
                }
            }
        }

        private void SaveSPACButton_Click(object sender, EventArgs e)
        {
            if (!CheckSPAC())
                return;

            WorkingSPAC.SaveSPAC(SPACFilePath);
            MessageBox.Show("SPC file saved!.", "Yay!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void SPACExtractButton_Click(object sender, EventArgs e)
        {
            if (!CheckSPAC())
                return;

            using (FolderBrowserDialog Directory = new FolderBrowserDialog())
            {
                if (Directory.ShowDialog() == DialogResult.OK)
                {
                    WorkingSPAC.ExtractXSEW(Directory.SelectedPath);
                    MessageBox.Show("XSEW files generated!", "Yay!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        private void ReplaceXSEWButton_Click(object sender, EventArgs e)
        {
            if (!CheckSPAC())
                return;

            XSEWReader XSEWFile;
            string filename;

            if (SPACSoundsComboBox.SelectedIndex == WorkingSPAC.NumSounds)
            {
                using (OpenFileDialog OpenFile = new OpenFileDialog())
                {
                    OpenFile.Filter = "XSEW files (*.xsew)|*.xsew";
                    OpenFile.Title = "Select one or more XSEW file with the index you want to replace on its name";
                    OpenFile.Multiselect = true;

                    if (OpenFile.ShowDialog() == DialogResult.OK)
                    {
                        for (int i = 0; i < OpenFile.FileNames.Length; i++)
                        {
                            filename = OpenFile.SafeFileNames[i];

                            if (filename.Contains(".xsew"))
                                filename = filename.Replace(".xsew", "");

                            try
                            {
                                if (int.Parse(filename) >= 0 && int.Parse(filename) <= (WorkingSPAC.NumSounds - 1))
                                {
                                    XSEWFile = new XSEWReader(OpenFile.FileNames[i]);
                                    WorkingSPAC.ReplaceXSEW(int.Parse(filename), XSEWFile.RIFFData, XSEWFile.SmplData, XSEWFile.Subchunk2Data);
                                }
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Something wrong during index conversion, check if your file is a valid one and if it has the correct index on its name.", "Ops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        MessageBox.Show("XSEW data replaced!", "Yay!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }

                return;
            }

            using (OpenFileDialog OpenFile = new OpenFileDialog())
            {
                OpenFile.Filter = "XSEW files (*.xsew)|*.xsew";
                OpenFile.Title = "Select a valid XSEW file";

                if (OpenFile.ShowDialog() == DialogResult.OK)
                {
                    XSEWFile = new XSEWReader(OpenFile.FileName);

                    if (!XSEWFile.CheckXSEW())
                    {
                        MessageBox.Show(OpenFile.FileName + " is not a valid XSEW file.", "Ops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    WorkingSPAC.ReplaceXSEW(SPACSoundsComboBox.SelectedIndex, XSEWFile.RIFFData, XSEWFile.SmplData, XSEWFile.Subchunk2Data);
                    MessageBox.Show("XSEW data replaced!", "Yay!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        // XSEW Tools

        private void ToWAVOpenFilesButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OpenFiles = new OpenFileDialog())
            {
                OpenFiles.Filter = "XSEW files (*.xsew)|*.xsew";
                OpenFiles.Title = "Select one or more XSEW files";
                OpenFiles.Multiselect = true;

                if (OpenFiles.ShowDialog() == DialogResult.OK)
                {
                    ToWAVFilePathBox.Text = "";

                    ToWAVFiles = new string[OpenFiles.FileNames.Length];
                    ToWAVSafeFiles = new string[OpenFiles.FileNames.Length];

                    for (int i = 0; i < OpenFiles.FileNames.Length; i++)
                    {
                        ToWAVFiles[i] = OpenFiles.FileNames[i];
                        ToWAVSafeFiles[i] = OpenFiles.SafeFileNames[i];
                        ToWAVFilePathBox.Text += ToWAVSafeFiles[i] + " ";
                    }
                }
            }
        }

        private void ToWAVButton_Click(object sender, EventArgs e)
        {
            XSEWReader XSEWFile;
            WAVEWriter WAVFile;
            string newfilename;

            if (ToWAVFiles.Length <= 0)
            {
                MessageBox.Show("No files to convert.", "Ops!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            using (FolderBrowserDialog Directory = new FolderBrowserDialog())
            {
                if (Directory.ShowDialog() == DialogResult.OK)
                {
                    foreach (string xsewfile in ToWAVFiles)
                    {
                        if (!File.Exists(xsewfile))
                        {
                            MessageBox.Show("File(s) is/are no longer present.", "Ops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            ToWAVFiles = new string[0];
                            ToWAVFilePathBox.Text = "";
                            return;
                        }

                        newfilename = ToWAVSafeFiles[Array.IndexOf(ToWAVFiles, xsewfile)];

                        if (newfilename.Contains(".xsew"))
                            newfilename = newfilename.Replace(".xsew", ".wav");

                        XSEWFile = new XSEWReader(xsewfile, true);

                        int[][] DecodedData = new int[1][];
                        DecodedData[0] = XSEWFile.DecodedSamples;

                        WAVFile = new WAVEWriter(Directory.SelectedPath + "/" + newfilename, XSEWFile.NumChannels, XSEWFile.SampleRate, 16, XSEWFile.DecodedSamples.Length, DecodedData);
                    }

                    MessageBox.Show("WAVE files generated!", "Yay!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    ToWAVFiles = new string[0];
                    ToWAVFilePathBox.Text = "";
                }
            }
        }

        private void ToXSEWOpenFilesButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OpenFiles = new OpenFileDialog())
            {
                OpenFiles.Filter = "XSEW Files (*.xsew)|*.xsew";
                OpenFiles.Title = "Select one or more XSEW files";
                OpenFiles.Multiselect = true;

                if (OpenFiles.ShowDialog() == DialogResult.OK)
                {
                    ToXSEWFilePathBox.Text = "";

                    ToXSEWFiles = new string[OpenFiles.FileNames.Length];
                    ToXSEWSafeFiles = new string[OpenFiles.FileNames.Length];

                    for (int i = 0; i < OpenFiles.FileNames.Length; i++)
                    {
                        ToXSEWFiles[i] = OpenFiles.FileNames[i];
                        ToXSEWSafeFiles[i] = OpenFiles.SafeFileNames[i];
                        ToXSEWFilePathBox.Text += ToXSEWSafeFiles[i] + " ";
                    }
                }
            }
        }

        private void ToXSEWButton_Click(object sender, EventArgs e)
        {
            XSEWWriter XSEWFile;
            string newfilename;

            if (ToXSEWFiles.Length <= 0)
            {
                MessageBox.Show("No files to convert.", "Ops!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            using (FolderBrowserDialog Directory = new FolderBrowserDialog())
            {
                if (Directory.ShowDialog() == DialogResult.OK)
                {
                    foreach (string soundfile in ToXSEWFiles)
                    {
                        if (!File.Exists(soundfile))
                        {
                            MessageBox.Show("File(s) is/are no longer present.", "Ops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            ToXSEWFiles = new string[0];
                            ToXSEWFilePathBox.Text = "";
                            return;
                        }

                        newfilename = ToXSEWSafeFiles[Array.IndexOf(ToXSEWFiles, soundfile)];

                        XSEWReader SoundFile = new XSEWReader(soundfile, true);

                        int mode;

                        if (ToXSEWRE6.Checked)
                            mode = 1;
                        else
                            mode = 2;

                        if (!SoundFile.FooterFixed)
                            SoundFile.FixFooter();

                        XSEWFile = new XSEWWriter(Directory.SelectedPath + "/" + newfilename, SoundFile.RIFFData, SoundFile.SmplData, SoundFile.Subchunk2Data, mode);
                    }

                    MessageBox.Show("XSEW file(s) generated!", "Yay!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    ToXSEWFiles = new string[0];
                    ToXSEWFilePathBox.Text = "";
                }
            }
        }
    }
}
