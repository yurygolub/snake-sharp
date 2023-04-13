using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Snake;

namespace SnakeWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int FieldWidth = 10;
        private const int FieldHeight = 10;

        private const int BackgroundColor = 0;
        private const int SnakeColor = 255 << 16;

        private readonly int pointWidth;
        private readonly int pointHeight;

        private readonly SnakeGame snakeGame = new (FieldWidth, FieldHeight);
        private readonly WriteableBitmap writeableBitmap;

        private readonly Timer timer;

        private Direction direction;

        public MainWindow()
        {
            this.InitializeComponent();

            this.image.Width = this.Width;
            this.image.Height = this.Height;

            this.pointWidth = (int)this.Width / FieldWidth;
            this.pointHeight = (int)this.Height / FieldHeight;

            this.writeableBitmap = new ((int)this.Width, (int)this.Height, 96, 96, PixelFormats.Bgr24, null);

            this.image.Source = this.writeableBitmap;

            this.timer = new Timer(
                (o) =>
                {
                    Thread.Sleep(100);
                    this.snakeGame.Move(this.direction);
                    this.Dispatcher.Invoke(() => this.Draw());
                },
                null,
                0,
                100);
        }

        private void Draw()
        {
            var snake = this.snakeGame.SnakeBody;
            var tail = snake.First.Value;
            var head = snake.Last.Value;

            try
            {
                this.writeableBitmap.Lock();

                IntPtr backBufferPtr = this.writeableBitmap.BackBuffer;
                int stride = this.writeableBitmap.BackBufferStride;

                for (int i = tail.Y * this.pointHeight; i < (tail.Y * this.pointHeight) + this.pointHeight; i++)
                {
                    for (int j = tail.X * this.pointWidth; j < (tail.X * this.pointWidth) + this.pointWidth; j++)
                    {
                        IntPtr resultPtr = backBufferPtr;

                        resultPtr += i * stride;
                        resultPtr += j * 3;

                        unsafe
                        {
                            *(int*)resultPtr = BackgroundColor;
                        }
                    }
                }

                for (int i = head.Y * this.pointHeight; i < (head.Y * this.pointHeight) + this.pointHeight; i++)
                {
                    for (int j = head.X * this.pointWidth; j < (head.X * this.pointWidth) + this.pointWidth; j++)
                    {
                        IntPtr resultPtr = backBufferPtr;

                        resultPtr += i * stride;
                        resultPtr += j * 3;

                        unsafe
                        {
                            *(int*)resultPtr = SnakeColor;
                        }
                    }
                }

                this.writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, (int)this.Width, (int)this.Height));
            }
            finally
            {
                this.writeableBitmap.Unlock();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            this.direction = e.Key switch
            {
                Key.Left => Direction.Left,
                Key.Right => Direction.Right,
                Key.Up => Direction.Up,
                Key.Down => Direction.Down,
            };
        }
    }
}
