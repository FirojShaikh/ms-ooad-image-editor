namespace ImageEditor
{
    public class Color
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public Color()
        {

        }

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

        public int GetPGMCompatibleColor()
        {
            return (this.Red + this.Green + this.Blue) / 3;
        }
    }
}
