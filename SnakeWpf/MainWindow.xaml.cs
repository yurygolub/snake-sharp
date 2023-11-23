using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
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

        private const int ImageWidth = 600;
        private const int ImageHeight = 600;

        private const int BackgroundColor = (32 << 16) | (32 << 8) | 32;
        private const int SnakeColor = (128 << 16) | 128;
        private const int HeadColor = (128 << 16) | (64 << 8);
        private const int AppleColor = (32 << 16) | (196 << 8) | 32;

        private readonly int pointWidth;
        private readonly int pointHeight;

        private readonly WriteableBitmap writeableBitmap = new (ImageWidth, ImageHeight, 96, 96, PixelFormats.Bgr24, null);
        private readonly DispatcherTimer timer = new ();

        private SnakeGame snakeGame;
        private Direction direction;
        private bool isStarted;

        public MainWindow()
        {
            this.InitializeComponent();

            this.Width = ImageWidth + 15;
            this.Height = ImageHeight + 35;

            this.image.Width = ImageWidth;
            this.image.Height = ImageHeight;

            this.pointWidth = ImageWidth / FieldWidth;
            this.pointHeight = ImageHeight / FieldHeight;

            this.image.Source = this.writeableBitmap;

            this.timer.Tick += this.Update;
            this.timer.Interval = TimeSpan.FromMilliseconds(300);

            MessageBox.Show("Press Enter to start game");
        }

        private void Update(object sender, EventArgs eventArgs)
        {
            GameState state = this.snakeGame.Move(this.direction);
            switch (state)
            {
                case GameState.None:
                    break;

                case GameState.Win:
                    this.timer.Stop();
                    MessageBox.Show($"You won! Score: {this.snakeGame.Score}");
                    this.StartGame();
                    break;

                case GameState.Lose:
                    this.timer.Stop();
                    MessageBox.Show($"You lose! Score: {this.snakeGame.Score}");
                    this.StartGame();
                    break;

                default:
                    break;
            }

            this.Draw();
        }

        private void Draw()
        {
            var apple = this.snakeGame.Apple;

            try
            {
                this.writeableBitmap.Lock();

                IntPtr backBufferPtr = this.writeableBitmap.BackBuffer;
                int stride = this.writeableBitmap.BackBufferStride;

                for (int i = 0; i < ImageHeight; i++)
                {
                    for (int j = 0; j < ImageWidth; j++)
                    {
                        IntPtr resultPtr = backBufferPtr;

                        resultPtr += i * stride;
                        resultPtr += j * 3;

                        unsafe
                        {
                            //*(int*)resultPtr = BackgroundColor;
                            *(byte*)(resultPtr + 0) = 32;
                            *(byte*)(resultPtr + 1) = 32;
                            *(byte*)(resultPtr + 2) = 32;
                        }
                    }
                }

                for (int i = apple.Y * this.pointHeight; i < (apple.Y * this.pointHeight) + this.pointHeight; i++)
                {
                    for (int j = apple.X * this.pointWidth; j < (apple.X * this.pointWidth) + this.pointWidth; j++)
                    {
                        IntPtr resultPtr = backBufferPtr;

                        resultPtr += i * stride;
                        resultPtr += j * 3;

                        unsafe
                        {
                            //*(int*)resultPtr = AppleColor;
                            *(byte*)(resultPtr + 0) = 32;
                            *(byte*)(resultPtr + 1) = 196;
                            *(byte*)(resultPtr + 2) = 32;
                        }
                    }
                }

                foreach (var item in this.snakeGame)
                {
                    for (int i = item.Y * this.pointHeight; i < (item.Y * this.pointHeight) + this.pointHeight; i++)
                    {
                        for (int j = item.X * this.pointWidth; j < (item.X * this.pointWidth) + this.pointWidth; j++)
                        {
                            IntPtr resultPtr = backBufferPtr;

                            resultPtr += i * stride;
                            resultPtr += j * 3;

                            unsafe
                            {
                                //*(int*)resultPtr = SnakeColor;
                                *(byte*)(resultPtr + 0) = 128;
                                *(byte*)(resultPtr + 1) = 0;
                                *(byte*)(resultPtr + 2) = 128;
                            }
                        }
                    }
                }

                for (int i = this.snakeGame.Head.Y * this.pointHeight; i < (this.snakeGame.Head.Y * this.pointHeight) + this.pointHeight; i++)
                {
                    for (int j = this.snakeGame.Head.X * this.pointWidth; j < (this.snakeGame.Head.X * this.pointWidth) + this.pointWidth; j++)
                    {
                        IntPtr resultPtr = backBufferPtr;

                        resultPtr += i * stride;
                        resultPtr += j * 3;

                        unsafe
                        {
                            //*(int*)resultPtr = HeadColor;
                            *(byte*)(resultPtr + 0) = 0;
                            *(byte*)(resultPtr + 1) = 64;
                            *(byte*)(resultPtr + 2) = 128;
                        }
                    }
                }

                this.writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, ImageWidth, ImageHeight));
            }
            finally
            {
                this.writeableBitmap.Unlock();
            }
        }

        private void StartGame()
        {
            this.snakeGame = new SnakeGame(FieldWidth, FieldHeight);
            this.direction = default;
            this.Draw();
            this.timer.Start();
            this.isStarted = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!this.isStarted && e.Key == Key.Enter)
            {
                this.StartGame();
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

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
