namespace Snake
{
    public struct Point
    {
        public int X;
        public int Y;

        public static bool operator ==(Point left, Point right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(Point left, Point right) => !(left == right);

        public override bool Equals(object obj) => obj is Point point && this == point;

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
    }
}
