﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Snake
{
    public class SnakeGame
    {
        private readonly int width;
        private readonly int height;

        private Direction currentDirection = Direction.Right;

        public SnakeGame(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.SnakeBody = new LinkedList<Point>(
                new[]
                {
                    new Point() { X = 1, Y = 1 },
                    new Point() { X = 2, Y = 1 },
                    new Point() { X = 3, Y = 1 },
                    new Point() { X = 3, Y = 1 },
                });

            this.GenerateApple();
        }

        public LinkedList<Point> SnakeBody { get; }

        public Point Apple { get; private set; }

        public int Score { get; private set; }

        public GameState Move(Direction direction)
        {
            Point head = this.SnakeBody.Last.Value;
            switch (direction)
            {
                case Direction.Left:
                    if (this.currentDirection != Direction.Right)
                    {
                        this.currentDirection = direction;
                    }

                    break;

                case Direction.Right:
                    if (this.currentDirection != Direction.Left)
                    {
                        this.currentDirection = direction;
                    }

                    break;

                case Direction.Up:
                    if (this.currentDirection != Direction.Down)
                    {
                        this.currentDirection = direction;
                    }

                    break;

                case Direction.Down:
                    if (this.currentDirection != Direction.Up)
                    {
                        this.currentDirection = direction;
                    }

                    break;

                default:
                    break;
            }

            switch (this.currentDirection)
            {
                case Direction.Left:
                    if (this.currentDirection != Direction.Right)
                    {
                        if (head.X - 1 >= 0)
                        {
                            head.X--;
                        }
                        else
                        {
                            head.X = this.width - 1;
                        }
                    }

                    break;

                case Direction.Right:
                    if (this.currentDirection != Direction.Left)
                    {
                        if (head.X + 1 < this.width)
                        {
                            head.X++;
                        }
                        else
                        {
                            head.X = 0;
                        }
                    }

                    break;

                case Direction.Up:
                    if (this.currentDirection != Direction.Down)
                    {
                        if (head.Y - 1 >= 0)
                        {
                            head.Y--;
                        }
                        else
                        {
                            head.Y = this.height - 1;
                        }
                    }

                    break;

                case Direction.Down:
                    if (this.currentDirection != Direction.Up)
                    {
                        if (head.Y + 1 < this.height)
                        {
                            head.Y++;
                        }
                        else
                        {
                            head.Y = 0;
                        }
                    }

                    break;

                default:
                    break;
            }

            if (head == this.Apple)
            {
                this.Score++;
                this.GenerateApple();
            }
            else
            {
                foreach (var item in this.SnakeBody)
                {
                    if (head == item)
                    {
                        return GameState.Lose;
                    }
                }

                this.SnakeBody.RemoveFirst();
            }

            this.SnakeBody.AddLast(head);

            return GameState.None;
        }

        private void GenerateApple()
        {
            var snake = this.SnakeBody.ToArray();
            Point newApple;
            do
            {
                newApple.X = Random.Shared.Next(this.width);
                newApple.Y = Random.Shared.Next(this.height);
            }
            while (IsCollide(newApple));

            this.Apple = newApple;

            bool IsCollide(Point point)
            {
                for (int i = 0; i < snake.Length; i++)
                {
                    if (point == snake[i])
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
