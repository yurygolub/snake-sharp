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

        private const int BackgroundColor = (32 << 16) | (32 << 8) | 32;
        private const int SnakeColor = (128 << 16) | 128;
        private const int AppleColor = 255 << 16;

        private readonly int pointWidth;
        private readonly int pointHeight;

        private readonly int width;
        private readonly int height;

        private readonly SnakeGame snakeGame = new (FieldWidth, FieldHeight);
        private readonly WriteableBitmap writeableBitmap;
        private readonly Timer timer;

        private Direction direction;

        public MainWindow()
        {
            this.InitializeComponent();

            this.width = (int)this.Width;
            this.height = (int)this.Height;

            this.Width = this.width + 15;
            this.Height = this.height + 35;

            this.image.Width = this.width;
            this.image.Height = this.height;

            this.pointWidth = this.width / FieldWidth;
            this.pointHeight = this.height / FieldHeight;

            this.writeableBitmap = new (this.width, this.height, 96, 96, PixelFormats.Bgr24, null);

            this.image.Source = this.writeableBitmap;

            this.Clear();

            this.timer = new Timer(this.TimerCallback, null, 0, 100);
        }

        private void TimerCallback(object obj)
        {
            this.snakeGame.Move(this.direction);
            this.Dispatcher.Invoke(this.Draw);
        }

        private void Draw()
        {
            var snake = this.snakeGame.SnakeBody;
            var tail = snake.First.Value;
            var head = snake.Last.Value;
            var apple = this.snakeGame.Apple;

            try
            {
                this.writeableBitmap.Lock();

                IntPtr backBufferPtr = this.writeableBitmap.BackBuffer;
                int stride = this.writeableBitmap.BackBufferStride;

                for (int i = apple.Y * this.pointHeight; i < (apple.Y * this.pointHeight) + this.pointHeight; i++)
                {
                    for (int j = apple.X * this.pointWidth; j < (apple.X * this.pointWidth) + this.pointWidth; j++)
                    {
                        IntPtr resultPtr = backBufferPtr;

                        resultPtr += i * stride;
                        resultPtr += j * 3;

                        unsafe
                        {
                            *(int*)resultPtr = AppleColor;
                        }
                    }
                }

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

                this.writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, this.width, this.height));
            }
            finally
            {
                this.writeableBitmap.Unlock();
            }
        }

        private void Clear()
        {
            try
            {
                this.writeableBitmap.Lock();

                IntPtr backBufferPtr = this.writeableBitmap.BackBuffer;
                int stride = this.writeableBitmap.BackBufferStride;

                for (int i = 0; i < this.height; i++)
                {
                    for (int j = 0; j < this.width; j++)
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

                this.writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, this.width, this.height));
            }
            finally
            {
                this.writeableBitmap.Unlock();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Environment.Exit(0);
            }

            this.direction = e.Key switch
            {
                Key.Left => Direction.Left,
                Key.Right => Direction.Right,
                Key.Up => Direction.Up,
                Key.Down => Direction.Down,
                _ => this.direction,
            };
        }
    }
}
