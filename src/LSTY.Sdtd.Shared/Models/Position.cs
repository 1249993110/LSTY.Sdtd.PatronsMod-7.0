namespace LSTY.Sdtd.Shared.Models
{
    /// <summary>
    /// 坐标
    /// </summary>
    public class Position
    {
        public Position()
        {
        }

        public Position(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Position(float x, float y, float z)
        {
            this.X = (int)x;
            this.Y = (int)y;
            this.Z = (int)z;
        }

        public Position(double x, double y, double z)
        {
            this.X = (int)x;
            this.Y = (int)y;
            this.Z = (int)z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public override string ToString()
        {
            return $"{X} {Y} {Z}";
        }
    }
}