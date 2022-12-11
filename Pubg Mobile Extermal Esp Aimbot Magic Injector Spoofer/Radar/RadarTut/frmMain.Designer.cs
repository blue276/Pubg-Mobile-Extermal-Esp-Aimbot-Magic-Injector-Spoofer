namespace RadarTut
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.redrawTimer = new System.Windows.Forms.Timer(this.components);
            this.settingsBox = new System.Windows.Forms.GroupBox();
            this.tbrYaw = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.tbrZoom = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.settingsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbrYaw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbrZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // redrawTimer
            // 
            this.redrawTimer.Enabled = true;
            this.redrawTimer.Interval = 250;
            this.redrawTimer.Tick += new System.EventHandler(this.redrawTimer_Tick);
            // 
            // settingsBox
            // 
            this.settingsBox.Controls.Add(this.button1);
            this.settingsBox.Controls.Add(this.tbrYaw);
            this.settingsBox.Controls.Add(this.label2);
            this.settingsBox.Controls.Add(this.tbrZoom);
            this.settingsBox.Controls.Add(this.label1);
            this.settingsBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.settingsBox.Location = new System.Drawing.Point(0, 316);
            this.settingsBox.Name = "settingsBox";
            this.settingsBox.Size = new System.Drawing.Size(284, 168);
            this.settingsBox.TabIndex = 0;
            this.settingsBox.TabStop = false;
            this.settingsBox.Text = "Settings";
            // 
            // tbrYaw
            // 
            this.tbrYaw.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrYaw.Location = new System.Drawing.Point(3, 87);
            this.tbrYaw.Maximum = 360;
            this.tbrYaw.Name = "tbrYaw";
            this.tbrYaw.Size = new System.Drawing.Size(278, 45);
            this.tbrYaw.TabIndex = 3;
            this.tbrYaw.ValueChanged += new System.EventHandler(this.tbrYaw_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(3, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "LocalPlayers\' yaw";
            // 
            // tbrZoom
            // 
            this.tbrZoom.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrZoom.Location = new System.Drawing.Point(3, 29);
            this.tbrZoom.Maximum = 100;
            this.tbrZoom.Name = "tbrZoom";
            this.tbrZoom.Size = new System.Drawing.Size(278, 45);
            this.tbrZoom.TabIndex = 1;
            this.tbrZoom.Value = 50;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Zoom";
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(3, 132);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(278, 33);
            this.button1.TabIndex = 4;
            this.button1.Text = "Randomize";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 484);
            this.Controls.Add(this.settingsBox);
            this.Name = "frmMain";
            this.Text = "RadarTut";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMain_Paint);
            this.settingsBox.ResumeLayout(false);
            this.settingsBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbrYaw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbrZoom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer redrawTimer;
        private System.Windows.Forms.GroupBox settingsBox;
        private System.Windows.Forms.TrackBar tbrYaw;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar tbrZoom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}

s