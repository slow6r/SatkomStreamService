using System;
using System.Drawing;
using System.Windows.Forms;
using LibVLCSharp.Shared;

namespace SatkomStreamService
{
    public partial class Form1 : Form
    {
        private const string Password = "1234"; // �������� �� ��� ������
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private bool _isFullScreen = false; // �������� �� false, ����� ���������� ���� � ������� ������

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Normal; // ������������� ������� �����
            this.FormBorderStyle = FormBorderStyle.Sizable; // �izable ��� ����������� �����
            this.TopMost = false; // ���� �� ������ ������
            this.KeyPreview = true; // ��������� ����� ������������ ������� ���������� ����� ��������� �� ���������
            this.KeyDown += Form1_KeyDown; // ��������� ���������� ������� ������� ������
            this.MouseMove += Form1_MouseMove; // ��������� ���������� ������� �������� ����
            InitializeVLC();
        }

        private void InitializeVLC()
        {
            Core.Initialize(); // ������������� LibVLC
            _libVLC = new LibVLC(); // ������������� _libVLC
            _mediaPlayer = new MediaPlayer(_libVLC);
            videoView1.MediaPlayer = _mediaPlayer; // ���������� MediaPlayer � videoView
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // ����������� � RTMP ������
            var streamUrl = "rtmp://5.182.67.245/live/mystream"; // �������� �� ��� �����
            var media = new Media(_libVLC, streamUrl, FromType.FromLocation);
            _mediaPlayer.Play(media); // �������� ���������������
        }

        private void EnterFullScreen()
        {
            _isFullScreen = true;
            this.FormBorderStyle = FormBorderStyle.None; // ������� �����
            this.WindowState = FormWindowState.Maximized; // ������� � ������������� �����
            this.TopMost = true; // ���� ������ ������
        }

        private void ExitFullScreen()
        {
            _isFullScreen = false;
            this.FormBorderStyle = FormBorderStyle.Sizable; // ���������� �����
            this.WindowState = FormWindowState.Normal; // ���������� ������� �����
            this.TopMost = false; // ���� �� ������ ������
        }

        private void ToggleFullScreen()
        {
            if (_isFullScreen)
            {
                ExitFullScreen(); // ���� � ������������� ������, �������
            }
            else
            {
                EnterFullScreen(); // �����, ��������� � ������������� �����
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // ���������, ������ �� Alt � Enter ������������
            if (e.Alt && e.KeyCode == Keys.Enter)
            {
                ToggleFullScreen(); // ����������� ������������� �����
                e.Handled = true; // ������������� ���������� ��������� �������
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            // ������ ��� ���������� ������������� ������� �� �������� ����, ���� �����
        }

        protected override void WndProc(ref Message m)
        {
            // ���������� ��������� � ������������ ����
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MINIMIZE = 0xF020;

            // ��������� ������������ ����
            if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt32() & 0xFFF0) == SC_MINIMIZE)
            {
                return; // ���������� ������� ������������ ����
            }

            base.WndProc(ref m);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // �������� ��������
                RequestPasswordAndClose(); // ����������� ������
            }
            else
            {
                ReleaseResources();
                base.OnFormClosing(e);
            }
        }

        private void RequestPasswordAndClose()
        {
            // ������� ��������� ���� ��� ����� ������
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("������� ������ ��� �������� ����������:", "�������� ����������", "", -1, -1);

            // ���������, ������ �� ������
            if (userInput == Password)
            {
                ReleaseResources();
                Environment.Exit(0); // ���������� ���� ������
            }
            else
            {
                // ���� ������ ��������, ���������� ���������
                MessageBox.Show("�������� ������. ���������� �����.", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReleaseResources()
        {
            _mediaPlayer?.Stop();
            _mediaPlayer?.Dispose();
            _libVLC?.Dispose();
        }

        private void videoView1_Click(object sender, EventArgs e)
        {
            // �������� �� ����� �� videoView, ���� �����
        }
    }
}
