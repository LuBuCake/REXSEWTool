namespace REXSEWTool
{
    partial class Main
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.CopyrightLabel = new System.Windows.Forms.Label();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.XSEWTab = new System.Windows.Forms.TabPage();
            this.ToWAVGPBox = new System.Windows.Forms.GroupBox();
            this.ToWAVOpenFilesButton = new System.Windows.Forms.Button();
            this.ToWAVFilePathBox = new System.Windows.Forms.TextBox();
            this.ToWAVButton = new System.Windows.Forms.Button();
            this.ToXSEWGPBox = new System.Windows.Forms.GroupBox();
            this.ToXSEWREV = new System.Windows.Forms.RadioButton();
            this.ToXSEWRE6 = new System.Windows.Forms.RadioButton();
            this.ToXSEWOpenFilesButton = new System.Windows.Forms.Button();
            this.ToXSEWFilePathBox = new System.Windows.Forms.TextBox();
            this.ToXSEWButton = new System.Windows.Forms.Button();
            this.SPACTab = new System.Windows.Forms.TabPage();
            this.SPACToolsGPBox = new System.Windows.Forms.GroupBox();
            this.SPACToolsReplaceGPBox = new System.Windows.Forms.GroupBox();
            this.ReplaceXSEWButton = new System.Windows.Forms.Button();
            this.SPACSoundsComboBox = new System.Windows.Forms.ComboBox();
            this.ReplaceLabel = new System.Windows.Forms.Label();
            this.WithLabel = new System.Windows.Forms.Label();
            this.SPACToolsExtractGPBox = new System.Windows.Forms.GroupBox();
            this.SPACExtractButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SPACNameTextBox = new System.Windows.Forms.TextBox();
            this.OpenSPACButton = new System.Windows.Forms.Button();
            this.SaveSPACButton = new System.Windows.Forms.Button();
            this.MainTabControl.SuspendLayout();
            this.XSEWTab.SuspendLayout();
            this.ToWAVGPBox.SuspendLayout();
            this.ToXSEWGPBox.SuspendLayout();
            this.SPACTab.SuspendLayout();
            this.SPACToolsGPBox.SuspendLayout();
            this.SPACToolsReplaceGPBox.SuspendLayout();
            this.SPACToolsExtractGPBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CopyrightLabel
            // 
            this.CopyrightLabel.AutoSize = true;
            this.CopyrightLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.CopyrightLabel.Location = new System.Drawing.Point(202, 216);
            this.CopyrightLabel.Name = "CopyrightLabel";
            this.CopyrightLabel.Size = new System.Drawing.Size(175, 15);
            this.CopyrightLabel.TabIndex = 3;
            this.CopyrightLabel.Text = "Copyright © 2020 by LuBuCake";
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.XSEWTab);
            this.MainTabControl.Controls.Add(this.SPACTab);
            this.MainTabControl.Location = new System.Drawing.Point(12, 12);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(365, 201);
            this.MainTabControl.TabIndex = 4;
            // 
            // XSEWTab
            // 
            this.XSEWTab.Controls.Add(this.ToWAVGPBox);
            this.XSEWTab.Controls.Add(this.ToXSEWGPBox);
            this.XSEWTab.Location = new System.Drawing.Point(4, 24);
            this.XSEWTab.Name = "XSEWTab";
            this.XSEWTab.Padding = new System.Windows.Forms.Padding(3);
            this.XSEWTab.Size = new System.Drawing.Size(357, 173);
            this.XSEWTab.TabIndex = 0;
            this.XSEWTab.Text = "XSEW Converter";
            this.XSEWTab.UseVisualStyleBackColor = true;
            // 
            // ToWAVGPBox
            // 
            this.ToWAVGPBox.Controls.Add(this.ToWAVOpenFilesButton);
            this.ToWAVGPBox.Controls.Add(this.ToWAVFilePathBox);
            this.ToWAVGPBox.Controls.Add(this.ToWAVButton);
            this.ToWAVGPBox.Location = new System.Drawing.Point(6, 6);
            this.ToWAVGPBox.Name = "ToWAVGPBox";
            this.ToWAVGPBox.Size = new System.Drawing.Size(345, 78);
            this.ToWAVGPBox.TabIndex = 5;
            this.ToWAVGPBox.TabStop = false;
            this.ToWAVGPBox.Text = "Convert to WAV (16 bit signed PCM)";
            // 
            // ToWAVOpenFilesButton
            // 
            this.ToWAVOpenFilesButton.Location = new System.Drawing.Point(299, 18);
            this.ToWAVOpenFilesButton.Name = "ToWAVOpenFilesButton";
            this.ToWAVOpenFilesButton.Size = new System.Drawing.Size(25, 23);
            this.ToWAVOpenFilesButton.TabIndex = 1;
            this.ToWAVOpenFilesButton.Text = "...";
            this.ToWAVOpenFilesButton.UseVisualStyleBackColor = true;
            this.ToWAVOpenFilesButton.Click += new System.EventHandler(this.ToWAVOpenFilesButton_Click);
            // 
            // ToWAVFilePathBox
            // 
            this.ToWAVFilePathBox.AllowDrop = true;
            this.ToWAVFilePathBox.Location = new System.Drawing.Point(22, 18);
            this.ToWAVFilePathBox.Name = "ToWAVFilePathBox";
            this.ToWAVFilePathBox.Size = new System.Drawing.Size(271, 23);
            this.ToWAVFilePathBox.TabIndex = 0;
            // 
            // ToWAVButton
            // 
            this.ToWAVButton.Location = new System.Drawing.Point(237, 47);
            this.ToWAVButton.Name = "ToWAVButton";
            this.ToWAVButton.Size = new System.Drawing.Size(87, 23);
            this.ToWAVButton.TabIndex = 0;
            this.ToWAVButton.Text = "Convert";
            this.ToWAVButton.UseVisualStyleBackColor = true;
            this.ToWAVButton.Click += new System.EventHandler(this.ToWAVButton_Click);
            // 
            // ToXSEWGPBox
            // 
            this.ToXSEWGPBox.Controls.Add(this.ToXSEWREV);
            this.ToXSEWGPBox.Controls.Add(this.ToXSEWRE6);
            this.ToXSEWGPBox.Controls.Add(this.ToXSEWOpenFilesButton);
            this.ToXSEWGPBox.Controls.Add(this.ToXSEWFilePathBox);
            this.ToXSEWGPBox.Controls.Add(this.ToXSEWButton);
            this.ToXSEWGPBox.Location = new System.Drawing.Point(6, 90);
            this.ToXSEWGPBox.Name = "ToXSEWGPBox";
            this.ToXSEWGPBox.Size = new System.Drawing.Size(345, 77);
            this.ToXSEWGPBox.TabIndex = 4;
            this.ToXSEWGPBox.TabStop = false;
            this.ToXSEWGPBox.Text = "Convert to XSEW (Microsoft ADPCM)";
            // 
            // ToXSEWREV
            // 
            this.ToXSEWREV.AutoSize = true;
            this.ToXSEWREV.Location = new System.Drawing.Point(73, 49);
            this.ToXSEWREV.Name = "ToXSEWREV";
            this.ToXSEWREV.Size = new System.Drawing.Size(72, 19);
            this.ToXSEWREV.TabIndex = 3;
            this.ToXSEWREV.Text = "REV 1 / 2";
            this.ToXSEWREV.UseVisualStyleBackColor = true;
            // 
            // ToXSEWRE6
            // 
            this.ToXSEWRE6.AutoSize = true;
            this.ToXSEWRE6.Checked = true;
            this.ToXSEWRE6.Location = new System.Drawing.Point(22, 49);
            this.ToXSEWRE6.Name = "ToXSEWRE6";
            this.ToXSEWRE6.Size = new System.Drawing.Size(45, 19);
            this.ToXSEWRE6.TabIndex = 2;
            this.ToXSEWRE6.TabStop = true;
            this.ToXSEWRE6.Text = "RE6";
            this.ToXSEWRE6.UseVisualStyleBackColor = true;
            // 
            // ToXSEWOpenFilesButton
            // 
            this.ToXSEWOpenFilesButton.Location = new System.Drawing.Point(299, 18);
            this.ToXSEWOpenFilesButton.Name = "ToXSEWOpenFilesButton";
            this.ToXSEWOpenFilesButton.Size = new System.Drawing.Size(25, 23);
            this.ToXSEWOpenFilesButton.TabIndex = 1;
            this.ToXSEWOpenFilesButton.Text = "...";
            this.ToXSEWOpenFilesButton.UseVisualStyleBackColor = true;
            this.ToXSEWOpenFilesButton.Click += new System.EventHandler(this.ToXSEWOpenFilesButton_Click);
            // 
            // ToXSEWFilePathBox
            // 
            this.ToXSEWFilePathBox.AllowDrop = true;
            this.ToXSEWFilePathBox.Location = new System.Drawing.Point(22, 18);
            this.ToXSEWFilePathBox.Name = "ToXSEWFilePathBox";
            this.ToXSEWFilePathBox.Size = new System.Drawing.Size(271, 23);
            this.ToXSEWFilePathBox.TabIndex = 0;
            // 
            // ToXSEWButton
            // 
            this.ToXSEWButton.Location = new System.Drawing.Point(237, 47);
            this.ToXSEWButton.Name = "ToXSEWButton";
            this.ToXSEWButton.Size = new System.Drawing.Size(87, 23);
            this.ToXSEWButton.TabIndex = 0;
            this.ToXSEWButton.Text = "Convert";
            this.ToXSEWButton.UseVisualStyleBackColor = true;
            this.ToXSEWButton.Click += new System.EventHandler(this.ToXSEWButton_Click);
            // 
            // SPACTab
            // 
            this.SPACTab.Controls.Add(this.SPACToolsGPBox);
            this.SPACTab.Controls.Add(this.groupBox1);
            this.SPACTab.Location = new System.Drawing.Point(4, 24);
            this.SPACTab.Name = "SPACTab";
            this.SPACTab.Padding = new System.Windows.Forms.Padding(3);
            this.SPACTab.Size = new System.Drawing.Size(357, 173);
            this.SPACTab.TabIndex = 1;
            this.SPACTab.Text = "RE6 SPC Container";
            this.SPACTab.UseVisualStyleBackColor = true;
            // 
            // SPACToolsGPBox
            // 
            this.SPACToolsGPBox.Controls.Add(this.SPACToolsReplaceGPBox);
            this.SPACToolsGPBox.Controls.Add(this.SPACToolsExtractGPBox);
            this.SPACToolsGPBox.Location = new System.Drawing.Point(6, 65);
            this.SPACToolsGPBox.Name = "SPACToolsGPBox";
            this.SPACToolsGPBox.Size = new System.Drawing.Size(345, 102);
            this.SPACToolsGPBox.TabIndex = 8;
            this.SPACToolsGPBox.TabStop = false;
            this.SPACToolsGPBox.Text = "Tools";
            // 
            // SPACToolsReplaceGPBox
            // 
            this.SPACToolsReplaceGPBox.Controls.Add(this.ReplaceXSEWButton);
            this.SPACToolsReplaceGPBox.Controls.Add(this.SPACSoundsComboBox);
            this.SPACToolsReplaceGPBox.Controls.Add(this.ReplaceLabel);
            this.SPACToolsReplaceGPBox.Controls.Add(this.WithLabel);
            this.SPACToolsReplaceGPBox.Location = new System.Drawing.Point(156, 22);
            this.SPACToolsReplaceGPBox.Name = "SPACToolsReplaceGPBox";
            this.SPACToolsReplaceGPBox.Size = new System.Drawing.Size(183, 74);
            this.SPACToolsReplaceGPBox.TabIndex = 1;
            this.SPACToolsReplaceGPBox.TabStop = false;
            this.SPACToolsReplaceGPBox.Text = "Replace";
            // 
            // ReplaceXSEWButton
            // 
            this.ReplaceXSEWButton.Location = new System.Drawing.Point(92, 44);
            this.ReplaceXSEWButton.Name = "ReplaceXSEWButton";
            this.ReplaceXSEWButton.Size = new System.Drawing.Size(75, 23);
            this.ReplaceXSEWButton.TabIndex = 5;
            this.ReplaceXSEWButton.Text = "Browse";
            this.ReplaceXSEWButton.UseVisualStyleBackColor = true;
            this.ReplaceXSEWButton.Click += new System.EventHandler(this.ReplaceXSEWButton_Click);
            // 
            // SPACSoundsComboBox
            // 
            this.SPACSoundsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SPACSoundsComboBox.FormattingEnabled = true;
            this.SPACSoundsComboBox.Location = new System.Drawing.Point(67, 17);
            this.SPACSoundsComboBox.Name = "SPACSoundsComboBox";
            this.SPACSoundsComboBox.Size = new System.Drawing.Size(99, 23);
            this.SPACSoundsComboBox.TabIndex = 2;
            // 
            // ReplaceLabel
            // 
            this.ReplaceLabel.AutoSize = true;
            this.ReplaceLabel.Location = new System.Drawing.Point(12, 20);
            this.ReplaceLabel.Name = "ReplaceLabel";
            this.ReplaceLabel.Size = new System.Drawing.Size(51, 15);
            this.ReplaceLabel.TabIndex = 3;
            this.ReplaceLabel.Text = "Replace:";
            // 
            // WithLabel
            // 
            this.WithLabel.AutoSize = true;
            this.WithLabel.Location = new System.Drawing.Point(12, 49);
            this.WithLabel.Name = "WithLabel";
            this.WithLabel.Size = new System.Drawing.Size(36, 15);
            this.WithLabel.TabIndex = 4;
            this.WithLabel.Text = "With:";
            // 
            // SPACToolsExtractGPBox
            // 
            this.SPACToolsExtractGPBox.Controls.Add(this.SPACExtractButton);
            this.SPACToolsExtractGPBox.Location = new System.Drawing.Point(6, 22);
            this.SPACToolsExtractGPBox.Name = "SPACToolsExtractGPBox";
            this.SPACToolsExtractGPBox.Size = new System.Drawing.Size(144, 74);
            this.SPACToolsExtractGPBox.TabIndex = 0;
            this.SPACToolsExtractGPBox.TabStop = false;
            this.SPACToolsExtractGPBox.Text = "Extract";
            // 
            // SPACExtractButton
            // 
            this.SPACExtractButton.Location = new System.Drawing.Point(36, 30);
            this.SPACExtractButton.Name = "SPACExtractButton";
            this.SPACExtractButton.Size = new System.Drawing.Size(75, 23);
            this.SPACExtractButton.TabIndex = 3;
            this.SPACExtractButton.Text = "Extract";
            this.SPACExtractButton.UseVisualStyleBackColor = true;
            this.SPACExtractButton.Click += new System.EventHandler(this.SPACExtractButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SPACNameTextBox);
            this.groupBox1.Controls.Add(this.OpenSPACButton);
            this.groupBox1.Controls.Add(this.SaveSPACButton);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 53);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SPC File";
            // 
            // SPACNameTextBox
            // 
            this.SPACNameTextBox.Enabled = false;
            this.SPACNameTextBox.Location = new System.Drawing.Point(183, 19);
            this.SPACNameTextBox.Name = "SPACNameTextBox";
            this.SPACNameTextBox.ReadOnly = true;
            this.SPACNameTextBox.Size = new System.Drawing.Size(141, 23);
            this.SPACNameTextBox.TabIndex = 2;
            // 
            // OpenSPACButton
            // 
            this.OpenSPACButton.Location = new System.Drawing.Point(21, 19);
            this.OpenSPACButton.Name = "OpenSPACButton";
            this.OpenSPACButton.Size = new System.Drawing.Size(75, 23);
            this.OpenSPACButton.TabIndex = 0;
            this.OpenSPACButton.Text = "Open";
            this.OpenSPACButton.UseVisualStyleBackColor = true;
            this.OpenSPACButton.Click += new System.EventHandler(this.OpenSPACButton_Click);
            // 
            // SaveSPACButton
            // 
            this.SaveSPACButton.Location = new System.Drawing.Point(102, 19);
            this.SaveSPACButton.Name = "SaveSPACButton";
            this.SaveSPACButton.Size = new System.Drawing.Size(75, 23);
            this.SaveSPACButton.TabIndex = 1;
            this.SaveSPACButton.Text = "Save";
            this.SaveSPACButton.UseVisualStyleBackColor = true;
            this.SaveSPACButton.Click += new System.EventHandler(this.SaveSPACButton_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 237);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.CopyrightLabel);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RESIDENT EVIL XSEW Tool";
            this.Load += new System.EventHandler(this.Main_Load);
            this.MainTabControl.ResumeLayout(false);
            this.XSEWTab.ResumeLayout(false);
            this.ToWAVGPBox.ResumeLayout(false);
            this.ToWAVGPBox.PerformLayout();
            this.ToXSEWGPBox.ResumeLayout(false);
            this.ToXSEWGPBox.PerformLayout();
            this.SPACTab.ResumeLayout(false);
            this.SPACToolsGPBox.ResumeLayout(false);
            this.SPACToolsReplaceGPBox.ResumeLayout(false);
            this.SPACToolsReplaceGPBox.PerformLayout();
            this.SPACToolsExtractGPBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CopyrightLabel;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage XSEWTab;
        private System.Windows.Forms.TabPage SPACTab;
        private System.Windows.Forms.GroupBox SPACToolsGPBox;
        private System.Windows.Forms.GroupBox SPACToolsReplaceGPBox;
        private System.Windows.Forms.Button ReplaceXSEWButton;
        private System.Windows.Forms.ComboBox SPACSoundsComboBox;
        private System.Windows.Forms.Label ReplaceLabel;
        private System.Windows.Forms.Label WithLabel;
        private System.Windows.Forms.GroupBox SPACToolsExtractGPBox;
        private System.Windows.Forms.Button SPACExtractButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox SPACNameTextBox;
        private System.Windows.Forms.Button OpenSPACButton;
        private System.Windows.Forms.Button SaveSPACButton;
        private System.Windows.Forms.GroupBox ToXSEWGPBox;
        private System.Windows.Forms.RadioButton ToXSEWREV;
        private System.Windows.Forms.RadioButton ToXSEWRE6;
        private System.Windows.Forms.Button ToXSEWOpenFilesButton;
        private System.Windows.Forms.TextBox ToXSEWFilePathBox;
        private System.Windows.Forms.Button ToXSEWButton;
        private System.Windows.Forms.GroupBox ToWAVGPBox;
        private System.Windows.Forms.Button ToWAVOpenFilesButton;
        private System.Windows.Forms.TextBox ToWAVFilePathBox;
        private System.Windows.Forms.Button ToWAVButton;
    }
}

