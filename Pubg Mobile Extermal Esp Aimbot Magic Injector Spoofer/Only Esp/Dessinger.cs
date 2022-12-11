namespace PUBGMESP
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Btn_Activate = new System.Windows.Forms.Button();
            this.LoopTimer = new System.Windows.Forms.Timer(this.components);
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Btn_Activate
            // 
            this.Btn_Activate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_Activate.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Activate.Location = new System.Drawing.Point(0, 0);
            this.Btn_Activate.Name = "Btn_Activate";
            this.Btn_Activate.Size = new System.Drawing.Size(419, 47);
            this.Btn_Activate.TabIndex = 0;
            this.Btn_Activate.Text = "Inject";
            this.Btn_Activate.UseVisualStyleBackColor = true;
            this.Btn_Activate.Click += new System.EventHandler(this.Btn_Activate_Click);
            // 
            // LoopTimer
            // 
            this.LoopTimer.Interval = 500;
            this.LoopTimer.Tick += new System.EventHandler(this.Loop_Tick);
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Tick += new System.EventHandler(this.Update_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(419, 47);
            this.Controls.Add(this.Btn_Activate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "PUBGM HACK - [ AM7 ]";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Btn_Activate;
        private System.Windows.Forms.Timer LoopTimer;
        private System.Windows.Forms.Timer UpdateTimer;
    }
}

s