namespace LiveSplit.UI.Components
{
    partial class IPSplitterComponentSettings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.chkShowInfo = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkIgnorePauseResume = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblListenerStatus = new System.Windows.Forms.Label();
            this.pnlShow = new System.Windows.Forms.Panel();
            this.chkShowServerClients = new System.Windows.Forms.CheckBox();
            this.chkShowServerPort = new System.Windows.Forms.CheckBox();
            this.chkShowServerStatus = new System.Windows.Forms.CheckBox();
            this.chkShowLogout = new System.Windows.Forms.CheckBox();
            this.chkShowDeaths = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlShow.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.chkShowInfo, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.chkIgnorePauseResume, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblListenerStatus, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.pnlShow, 1, 4);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 161F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(594, 338);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // chkShowInfo
            // 
            this.chkShowInfo.AutoSize = true;
            this.chkShowInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkShowInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowInfo.Location = new System.Drawing.Point(108, 104);
            this.chkShowInfo.Name = "chkShowInfo";
            this.chkShowInfo.Size = new System.Drawing.Size(142, 26);
            this.chkShowInfo.TabIndex = 20;
            this.chkShowInfo.Text = "Show Information:";
            this.chkShowInfo.UseVisualStyleBackColor = true;
            this.chkShowInfo.CheckedChanged += new System.EventHandler(this.chkShowInfo_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 32);
            this.label3.TabIndex = 16;
            this.label3.Text = "Options:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 32);
            this.label1.TabIndex = 11;
            this.label1.Text = "Port:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkIgnorePauseResume
            // 
            this.chkIgnorePauseResume.AutoSize = true;
            this.chkIgnorePauseResume.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkIgnorePauseResume.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIgnorePauseResume.Location = new System.Drawing.Point(108, 72);
            this.chkIgnorePauseResume.Name = "chkIgnorePauseResume";
            this.chkIgnorePauseResume.Size = new System.Drawing.Size(178, 26);
            this.chkIgnorePauseResume.TabIndex = 14;
            this.chkIgnorePauseResume.Text = "Ignore Pause / Resume";
            this.chkIgnorePauseResume.UseVisualStyleBackColor = true;
            this.chkIgnorePauseResume.CheckedChanged += new System.EventHandler(this.chkIgnorePauseResume_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.22594F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85.77406F));
            this.tableLayoutPanel1.Controls.Add(this.txtPort, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(108, 8);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(478, 26);
            this.tableLayoutPanel1.TabIndex = 22;
            // 
            // txtPort
            // 
            this.txtPort.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort.Location = new System.Drawing.Point(3, 3);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(55, 22);
            this.txtPort.TabIndex = 7;
            this.txtPort.Text = "0";
            this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(70, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(405, 26);
            this.label2.TabIndex = 21;
            this.label2.Text = "(Port Change Requires LiveSplit Restart)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblListenerStatus
            // 
            this.lblListenerStatus.AutoSize = true;
            this.lblListenerStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblListenerStatus.Location = new System.Drawing.Point(108, 297);
            this.lblListenerStatus.Margin = new System.Windows.Forms.Padding(3);
            this.lblListenerStatus.Name = "lblListenerStatus";
            this.lblListenerStatus.Size = new System.Drawing.Size(478, 33);
            this.lblListenerStatus.TabIndex = 30;
            this.lblListenerStatus.Text = "[Server Status]";
            // 
            // pnlShow
            // 
            this.pnlShow.Controls.Add(this.chkShowServerClients);
            this.pnlShow.Controls.Add(this.chkShowServerPort);
            this.pnlShow.Controls.Add(this.chkShowServerStatus);
            this.pnlShow.Controls.Add(this.chkShowLogout);
            this.pnlShow.Controls.Add(this.chkShowDeaths);
            this.pnlShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlShow.Location = new System.Drawing.Point(108, 136);
            this.pnlShow.Name = "pnlShow";
            this.pnlShow.Size = new System.Drawing.Size(478, 155);
            this.pnlShow.TabIndex = 31;
            // 
            // chkShowServerClients
            // 
            this.chkShowServerClients.AutoSize = true;
            this.chkShowServerClients.Location = new System.Drawing.Point(18, 113);
            this.chkShowServerClients.Name = "chkShowServerClients";
            this.chkShowServerClients.Size = new System.Drawing.Size(137, 21);
            this.chkShowServerClients.TabIndex = 4;
            this.chkShowServerClients.Text = "TCP Client Count";
            this.chkShowServerClients.UseVisualStyleBackColor = true;
            this.chkShowServerClients.CheckedChanged += new System.EventHandler(this.chkShowServerClients_CheckedChanged);
            // 
            // chkShowServerPort
            // 
            this.chkShowServerPort.AutoSize = true;
            this.chkShowServerPort.Location = new System.Drawing.Point(18, 86);
            this.chkShowServerPort.Name = "chkShowServerPort";
            this.chkShowServerPort.Size = new System.Drawing.Size(133, 21);
            this.chkShowServerPort.TabIndex = 3;
            this.chkShowServerPort.Text = "TCP Server Port";
            this.chkShowServerPort.UseVisualStyleBackColor = true;
            this.chkShowServerPort.CheckedChanged += new System.EventHandler(this.chkShowServerPort_CheckedChanged);
            // 
            // chkShowServerStatus
            // 
            this.chkShowServerStatus.AutoSize = true;
            this.chkShowServerStatus.Location = new System.Drawing.Point(18, 59);
            this.chkShowServerStatus.Name = "chkShowServerStatus";
            this.chkShowServerStatus.Size = new System.Drawing.Size(147, 21);
            this.chkShowServerStatus.TabIndex = 2;
            this.chkShowServerStatus.Text = "TCP Server Status";
            this.chkShowServerStatus.UseVisualStyleBackColor = true;
            this.chkShowServerStatus.CheckedChanged += new System.EventHandler(this.chkShowServerStatus_CheckedChanged);
            // 
            // chkShowLogout
            // 
            this.chkShowLogout.AutoSize = true;
            this.chkShowLogout.Location = new System.Drawing.Point(18, 31);
            this.chkShowLogout.Name = "chkShowLogout";
            this.chkShowLogout.Size = new System.Drawing.Size(115, 21);
            this.chkShowLogout.TabIndex = 1;
            this.chkShowLogout.Text = "Logout Count";
            this.chkShowLogout.UseVisualStyleBackColor = true;
            this.chkShowLogout.CheckedChanged += new System.EventHandler(this.chkShowLogout_CheckedChanged);
            // 
            // chkShowDeaths
            // 
            this.chkShowDeaths.AutoSize = true;
            this.chkShowDeaths.Location = new System.Drawing.Point(18, 3);
            this.chkShowDeaths.Name = "chkShowDeaths";
            this.chkShowDeaths.Size = new System.Drawing.Size(109, 21);
            this.chkShowDeaths.TabIndex = 0;
            this.chkShowDeaths.Text = "Death Count";
            this.chkShowDeaths.UseVisualStyleBackColor = true;
            this.chkShowDeaths.CheckedChanged += new System.EventHandler(this.chkShowDeaths_CheckedChanged);
            // 
            // IPSplitterComponentSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "IPSplitterComponentSettings";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.Size = new System.Drawing.Size(612, 385);
            this.Load += new System.EventHandler(this.TextComponentSettings_Load);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.pnlShow.ResumeLayout(false);
            this.pnlShow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkIgnorePauseResume;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkShowInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblListenerStatus;
        private System.Windows.Forms.Panel pnlShow;
        private System.Windows.Forms.CheckBox chkShowServerPort;
        private System.Windows.Forms.CheckBox chkShowServerStatus;
        private System.Windows.Forms.CheckBox chkShowLogout;
        private System.Windows.Forms.CheckBox chkShowDeaths;
        private System.Windows.Forms.CheckBox chkShowServerClients;
    }
}
