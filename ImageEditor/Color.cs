namespace ImageEditor
{
    public class Color
    {
        public int Red { get; private set; }
        public int Green { get; private set; }
        public int Blue { get; private set; }

        public Color(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}", Red, Green, Blue);
        }
    }
}
