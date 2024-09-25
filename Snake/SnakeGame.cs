using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Snake;

public class SnakeGame : IEnumerable<Point>
{
    private readonly int width;
    private readonly int height;

    private readonly LinkedList<Point> snakeBody;

    private Direction currentDirection = Direction.Right;

    public SnakeGame(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.snakeBody = new LinkedList<Point>(
            new[]
            {
                new Point() { X = 1, Y = 1 },
                new Point() { X = 2, Y = 1 },
                new Point() { X = 3, Y = 1 },
                new Point() { X = 4, Y = 1 },
            });

        this.GenerateApple();
    }

    public Point Apple { get; private set; }

    public Point Head => this.snakeBody.Last.Value;

    public int Score { get; private set; }

    public GameState Move(Direction direction)
    {
        Point head = this.snakeBody.Last.Value;
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
                }

                break;

            case Direction.Right:
                if (this.currentDirection != Direction.Left)
                {
                    if (head.X + 1 < this.width)
                    {
                        head.X++;
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
                }

                break;

            case Direction.Down:
                if (this.currentDirection != Direction.Up)
                {
                    if (head.Y + 1 < this.height)
                    {
                        head.Y++;
                    }
                }

                break;

            default:
                break;
        }

        if (head == this.Apple)
        {
            this.Score++;
            this.snakeBody.AddLast(head);

            if (this.snakeBody.Count == this.width * this.height)
            {
                return GameState.Win;
            }

            this.GenerateApple();
        }
        else
        {
            foreach (var item in this.snakeBody)
            {
                if (head == item)
                {
                    return GameState.Lose;
                }
            }

            this.snakeBody.RemoveFirst();
            this.snakeBody.AddLast(head);
        }

        return GameState.None;
    }

    public IEnumerator<Point> GetEnumerator()
    {
        return this.snakeBody.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    private void GenerateApple()
    {
        var snake = this.snakeBody.ToArray();
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
