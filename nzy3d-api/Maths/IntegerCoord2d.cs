namespace nzy3D.Maths
{

    public class IntegerCoord2d
    {
        public int x;

        public int y;
        public IntegerCoord2d()
        {
            x = 0;
            y = 0;
        }

        public IntegerCoord2d(int xx, int yy)
        {
            x = xx;
            y = yy;
        }

        public override string ToString()
        {
            return ("(IntegerCoord2d) x=" + x + " y=" + y);
        }

    }

}

