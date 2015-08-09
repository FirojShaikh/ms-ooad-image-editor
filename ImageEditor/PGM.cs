using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace ImageEditor
{
    public class PGM
    {
        private string magicNumber;
        private int columns;
        private int rows;
        private int colorDepth;
        private string [,] pixels;

        public void open(string filePath)
        {
            using (var reader=new StreamReader(filePath))
            {
                int lineCount = 1;

                while (reader.Peek() >= 0)
                {
                    
                    var line = reader.ReadLine();

                    if (line.StartsWith("#"))
                        continue;

                    if (1 == lineCount)
                    {
                        this.magicNumber = line;
                        lineCount++;
                        continue;
                    }

                    if (2 == lineCount)
                    {
                        var columsAndRows = line.Split(null);

                        int charCount = 1;
                        foreach (var specification in columsAndRows)
                        {
                            if (specification.Equals(' ') || specification.Equals('\t'))
                                continue;

                            if (string.IsNullOrWhiteSpace(specification))
                                continue;

                            if (1 == charCount)
                            {
                                this.columns = Convert.ToInt32(specification);
                                charCount++;
                                continue;
                            }

                            if (2 == charCount)
                            {
                                this.rows = Convert.ToInt32(specification);
                                charCount++;
                                continue;
                            }
                        }

                        this.pixels = new string[rows, columns];

                        lineCount++;
                        continue;
                    }

                    if (3 == lineCount)
                    {
                        var colorDepthArray = line.Split(null);

                        foreach (var ccolorDepth in colorDepthArray)
                        {
                            if (string.IsNullOrWhiteSpace(ccolorDepth))
                                continue;

                            this.colorDepth = Convert.ToInt32(ccolorDepth);
                        }

                        lineCount++;
                        continue;
                    }
                    

                    var pixelArray = line.Split(null);
                    int pixelCount = 0;

                    foreach (var pixel in pixelArray)
                    {
                        if (string.IsNullOrWhiteSpace(pixel))
                            continue;

                        this.pixels[lineCount - 4, pixelCount++] = pixel;
                    }

                    lineCount++;
                }
            }
        }

        public string save(string fileName)
        {
            fileName = string.Format("{0}-{1}.pgm", (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds, fileName);

            var filePath = string.Format("{0}\\{1}", ConfigurationManager.AppSettings["PGMFileDirectory"],fileName);

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine(this.magicNumber);
                writer.WriteLine(string.Format("{0} {1}", this.columns, this.rows));
                writer.WriteLine(this.colorDepth);

                var lineBuffer = new StringBuilder();

                for (int i = 0; i < this.rows; i++)
                {
                    lineBuffer.Clear();
                    for (int j = 0; j < this.columns; j++)
                    {
                        if (j > 0)
                        {
                            lineBuffer.Append("\t");
                        }
                        lineBuffer.Append(this.pixels[i, j]);
                    }
                    writer.WriteLine(lineBuffer.ToString());
                }
            }

            return fileName;
        }

        private PGM createRotateImage()
        {
            var rotateImage = new PGM();

            rotateImage.magicNumber = this.magicNumber;
            rotateImage.colorDepth = this.colorDepth;

            rotateImage.rows = this.columns;
            rotateImage.columns = this.rows;

            rotateImage.pixels = new string[columns, rows];

            return rotateImage;
        }

        private PGM createFlippedImage()
        {
            var flippedImage = new PGM();

            flippedImage.magicNumber = this.magicNumber;
            flippedImage.colorDepth = this.colorDepth;

            flippedImage.rows = this.rows;
            flippedImage.columns = this.columns;

            flippedImage.pixels = new string[rows, columns];

            return flippedImage;
        }

        private PGM rotate()
        {
            var rotateImage = createRotateImage();
            
            int k = 0;
            int l = 0;

            for (int i = 0; i < this.columns; i++)
            {
                for (int j = this.rows - 1; j >= 0; j--)
                {
                    rotateImage.pixels[k, l] = this.pixels[j, i];
                    l++;
                }
                k++;
                l = 0;
            }

            return rotateImage;
        }

        public PGM rotateRight()
        {
            return rotate();
        }

        public PGM rotateLeft()
        {
            return rotate()
                    .rotate()
                    .rotate();
        }

        public PGM flipVertical()
        {
            var flippedImage = createFlippedImage();

            int k = 0;
            int l = 0;

            for (int i = 0; i < this.rows; i++)
            {
                for (int j = this.columns - 1; j >= 0; j--)
                {
                    flippedImage.pixels[k, l] = this.pixels[i, j];
                    l++;
                }
                k++;
                l = 0;
            }

            return flippedImage;
        }

        public PGM flipHorizontal()
        {
            var flippedImage = createFlippedImage();

            int k = 0;
            int l = 0;

            for (int i = this.rows-1; i >= 0; i--)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    flippedImage.pixels[k, l] = this.pixels[i, j];
                    l++;
                }
                k++;
                l = 0;
            }

            return flippedImage;
        }
    }
}
