namespace SubGadgetDownloaderGUI
{
    partial class downloaderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(downloaderForm));
            this.progressBarTest = new System.Windows.Forms.ProgressBar();
            this.lblCurrentTrack = new System.Windows.Forms.Label();
            this.lstTracks = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.lblSaveTo = new System.Windows.Forms.Label();
            this.lblTrackCountStatus = new System.Windows.Forms.Label();
            this.btnCancelDownload = new System.Windows.Forms.Button();
            this.lnkSaveTo = new System.Windows.Forms.LinkLabel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBarTest
            // 
            this.progressBarTest.Location = new System.Drawing.Point(12, 24);
            this.progressBarTest.Name = "progressBarTest";
            this.progressBarTest.Size = new System.Drawing.Size(347, 23);
            this.progressBarTest.TabIndex = 0;
            // 
            // lblCurrentTrack
            // 
            this.lblCurrentTrack.AutoSize = true;
            this.lblCurrentTrack.Location = new System.Drawing.Point(10, 8);
            this.lblCurrentTrack.Name = "lblCurrentTrack";
            this.lblCurrentTrack.Size = new System.Drawing.Size(0, 13);
            this.lblCurrentTrack.TabIndex = 2;
            // 
            // lstTracks
            // 
            this.lstTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lstTracks.FullRowSelect = true;
            this.lstTracks.GridLines = true;
            this.lstTracks.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstTracks.Location = new System.Drawing.Point(12, 53);
            this.lstTracks.Name = "lstTracks";
            this.lstTracks.Size = new System.Drawing.Size(403, 189);
            this.lstTracks.TabIndex = 7;
            this.lstTracks.UseCompatibleStateImageBehavior = false;
            this.lstTracks.View = System.Windows.Forms.View.Details;
            this.lstTracks.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.lstTracks_ColumnWidthChanging);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Track";
            this.columnHeader1.Width = 271;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Status";
            this.columnHeader2.Width = 105;
            // 
            // lblSaveTo
            // 
            this.lblSaveTo.AutoSize = true;
            this.lblSaveTo.Location = new System.Drawing.Point(7, 244);
            this.lblSaveTo.Name = "lblSaveTo";
            this.lblSaveTo.Size = new System.Drawing.Size(52, 13);
            this.lblSaveTo.TabIndex = 8;
            this.lblSaveTo.Text = "Saving To:";
            // 
            // lblTrackCountStatus
            // 
            this.lblTrackCountStatus.AutoSize = true;
            this.lblTrackCountStatus.Location = new System.Drawing.Point(375, 7);
            this.lblTrackCountStatus.Name = "lblTrackCountStatus";
            this.lblTrackCountStatus.Size = new System.Drawing.Size(0, 13);
            this.lblTrackCountStatus.TabIndex = 10;
            // 
            // btnCancelDownload
            // 
            this.btnCancelDownload.Location = new System.Drawing.Point(365, 24);
            this.btnCancelDownload.Name = "btnCancelDownload";
            this.btnCancelDownload.Size = new System.Drawing.Size(50, 23);
            this.btnCancelDownload.TabIndex = 11;
            this.btnCancelDownload.Text = "Cancel";
            this.btnCancelDownload.UseVisualStyleBackColor = true;
            this.btnCancelDownload.Click += new System.EventHandler(this.btnCancelDownload_Click);
            // 
            // lnkSaveTo
            // 
            this.lnkSaveTo.AutoSize = true;
            this.lnkSaveTo.Location = new System.Drawing.Point(59, 244);
            this.lnkSaveTo.Name = "lnkSaveTo";
            this.lnkSaveTo.Size = new System.Drawing.Size(0, 13);
            this.lnkSaveTo.TabIndex = 12;
            this.lnkSaveTo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSaveTo_LinkClicked);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(321, 244);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(94, 23);
            this.btnRemove.TabIndex = 13;
            this.btnRemove.Text = "Remove\\Restore";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // downloaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 269);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.lnkSaveTo);
            this.Controls.Add(this.btnCancelDownload);
            this.Controls.Add(this.lblTrackCountStatus);
            this.Controls.Add(this.lblSaveTo);
            this.Controls.Add(this.lstTracks);
            this.Controls.Add(this.lblCurrentTrack);
            this.Controls.Add(this.progressBarTest);
            this.Font = new System.Drawing.Font("Calibri", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "downloaderForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SubGadgetDownloader GUI v1.1";
            this.Load += new System.EventHandler(this.downloaderForm_Load);
            this.Shown += new System.EventHandler(this.downloaderForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.downloaderForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarTest;
        private System.Windows.Forms.Label lblCurrentTrack;
        private System.Windows.Forms.ListView lstTracks;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label lblSaveTo;
        private System.Windows.Forms.Label lblTrackCountStatus;
        private System.Windows.Forms.Button btnCancelDownload;
        private System.Windows.Forms.LinkLabel lnkSaveTo;
        private System.Windows.Forms.Button btnRemove;
    }
}

