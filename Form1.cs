using System;
using System.Drawing;
using System.Windows.Forms;
using LibVLCSharp.Shared;

namespace SatkomStreamService
{
    public partial class Form1 : Form
    {
        private const string Password = "1234"; // Замените на ваш пароль
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private bool _isFullScreen = false; // Изменено на false, чтобы изначально было в оконном режиме

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Normal; // Устанавливаем оконный режим
            this.FormBorderStyle = FormBorderStyle.Sizable; // Сizable для отображения рамки
            this.TopMost = false; // Окно не всегда сверху
            this.KeyPreview = true; // Разрешаем форме обрабатывать события клавиатуры перед передачей их контролам
            this.KeyDown += Form1_KeyDown; // Добавляем обработчик события нажатия клавиш
            this.MouseMove += Form1_MouseMove; // Добавляем обработчик события движения мыши
            InitializeVLC();
        }

        private void InitializeVLC()
        {
            Core.Initialize(); // Инициализация LibVLC
            _libVLC = new LibVLC(); // Инициализация _libVLC
            _mediaPlayer = new MediaPlayer(_libVLC);
            videoView1.MediaPlayer = _mediaPlayer; // Установите MediaPlayer в videoView
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Подключение к RTMP потоку
            var streamUrl = "rtmp://5.182.67.245/live/mystream"; // Замените на ваш поток
            var media = new Media(_libVLC, streamUrl, FromType.FromLocation);
            _mediaPlayer.Play(media); // Начинаем воспроизведение
        }

        private void EnterFullScreen()
        {
            _isFullScreen = true;
            this.FormBorderStyle = FormBorderStyle.None; // Убираем рамки
            this.WindowState = FormWindowState.Maximized; // Переход в полноэкранный режим
            this.TopMost = true; // Окно всегда сверху
        }

        private void ExitFullScreen()
        {
            _isFullScreen = false;
            this.FormBorderStyle = FormBorderStyle.Sizable; // Возвращаем рамки
            this.WindowState = FormWindowState.Normal; // Возвращаем оконный режим
            this.TopMost = false; // Окно не всегда сверху
        }

        private void ToggleFullScreen()
        {
            if (_isFullScreen)
            {
                ExitFullScreen(); // Если в полноэкранном режиме, выходим
            }
            else
            {
                EnterFullScreen(); // Иначе, переходим в полноэкранный режим
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Проверяем, нажаты ли Alt и Enter одновременно
            if (e.Alt && e.KeyCode == Keys.Enter)
            {
                ToggleFullScreen(); // Переключаем полноэкранный режим
                e.Handled = true; // Предотвращаем дальнейшую обработку события
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            // Логика для управления полноэкранным режимом по движению мыши, если нужно
        }

        protected override void WndProc(ref Message m)
        {
            // Игнорируем сообщения о сворачивании окна
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MINIMIZE = 0xF020;

            // Блокируем сворачивание окна
            if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt32() & 0xFFF0) == SC_MINIMIZE)
            {
                return; // Игнорируем попытку сворачивания окна
            }

            base.WndProc(ref m);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // Отменяем закрытие
                RequestPasswordAndClose(); // Запрашиваем пароль
            }
            else
            {
                ReleaseResources();
                base.OnFormClosing(e);
            }
        }

        private void RequestPasswordAndClose()
        {
            // Создаем текстовое поле для ввода пароля
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("Введите пароль для закрытия приложения:", "Закрытие приложения", "", -1, -1);

            // Проверяем, введен ли пароль
            if (userInput == Password)
            {
                ReleaseResources();
                Environment.Exit(0); // Завершение всей службы
            }
            else
            {
                // Если пароль неверный, отображаем сообщение
                MessageBox.Show("Неверный пароль. Попробуйте снова.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // Действие по клику на videoView, если нужно
        }
    }
}
