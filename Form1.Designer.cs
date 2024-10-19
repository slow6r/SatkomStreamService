namespace SatkomStreamService
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            videoView1 = new LibVLCSharp.WinForms.VideoView();
            ((System.ComponentModel.ISupportInitialize)videoView1).BeginInit();
            SuspendLayout();
            // 
            // videoView1
            // 
            videoView1.BackColor = Color.Black;
            videoView1.MediaPlayer = null;
            videoView1.Name = "videoView1";
            videoView1.TabIndex = 0;
            videoView1.Dock = DockStyle.Fill;
            videoView1.Text = "videoView1";
            videoView1.Click += videoView1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(videoView1);
            Name = "Form1";
            Text = "Satkom Stream Service";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)videoView1).EndInit();
            ResumeLayout(false);
        }


        private LibVLCSharp.WinForms.VideoView videoView1;
    }
}
