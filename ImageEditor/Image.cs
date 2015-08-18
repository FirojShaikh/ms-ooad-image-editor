namespace ImageEditor
{
    public abstract class Image
    {
        protected string magicNumber;
        protected int columns;
        protected int rows;
        protected int depth;

        public abstract Image flipHorizontal();
        public abstract Image flipVertical();
        public abstract Image rotateLeft();
        public abstract Image rotateRight();
        public abstract string save(string fileName);
        public abstract void open(string filePath);
    }
}
