using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace ImageEditor
{
    public class PPM : Image
    {
        private Color[,] pixels;

        // create PPM image object to rotate
        private Image createRotateImage()
        {
            var rotateImage = new PPM();

            rotateImage.magicNumber = this.magicNumber;
            rotateImage.depth = this.depth;

            // rotating image will switch number of columns to number of rows and number of rows to number of columns
            rotateImage.rows = this.columns;
            rotateImage.columns = this.rows;

            rotateImage.pixels = new Color[columns, rows];

            return rotateImage;
        }

        private Image createImage()
        {
            var image = new PPM();

            image.magicNumber = this.magicNumber;
            image.depth = this.depth;

            // flipping image will keep number of columns and number of rows as is.
            image.rows = this.rows;
            image.columns = this.columns;

            image.pixels = new Color[rows, columns];

            return image;
        }

        // creates PPM image object to flip
        private Image createFlippedImage()
        {
            var flippedImage = new PPM();

            flippedImage.magicNumber = this.magicNumber;
            flippedImage.depth = this.depth;

            // flipping image will keep number of columns and number of rows as is.
            flippedImage.rows = this.rows;
            flippedImage.columns = this.columns;

            flippedImage.pixels = new Color[rows, columns];

            return flippedImage;
        }

        // rotate image in 90 degrees
        private PPM rotate()
        {
            var rotateImage = createRotateImage() as PPM;

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

        public override Image flipHorizontal()
        {
            var flippedImage = createFlippedImage() as PPM;

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

        public override Image flipVertical()
        {
            var flippedImage = createFlippedImage() as PPM;

            int k = 0;
            int l = 0;

            for (int i = this.rows - 1; i >= 0; i--)
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

        public override void open(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                int lineCount = 1;

                while (reader.Peek() >= 0)
                {

                    var line = reader.ReadLine();

                    // Ignore comments
                    if (line.StartsWith("#"))
                        continue;

                    // First line have PGM file MagicNumber
                    if (1 == lineCount)
                    {
                        this.magicNumber = line;
                        lineCount++;
                        continue;
                    }

                    // Second line have PGM file MagicNumber
                    if (2 == lineCount)
                    {
                        var columnsAndRows = line.Split(null);

                        int numberCount = 1;
                        foreach (var specification in columnsAndRows)
                        {
                            if (specification.Equals(' ') || specification.Equals('\t'))
                                continue;

                            if (string.IsNullOrWhiteSpace(specification))
                                continue;

                            // First Number indicates number of columns
                            if (1 == numberCount)
                            {
                                this.columns = Convert.ToInt32(specification);
                                numberCount++;
                                continue;
                            }

                            // Second number indicates number of rows
                            if (2 == numberCount)
                            {
                                this.rows = Convert.ToInt32(specification);
                                numberCount++;
                                continue;
                            }
                        }

                        this.pixels = new Color[rows, columns];

                        lineCount++;
                        continue;
                    }

                    // Line 3 indicates colorDepth
                    if (3 == lineCount)
                    {
                        var colorDepthArray = line.Split(null);

                        foreach (var ccolorDepth in colorDepthArray)
                        {
                            if (string.IsNullOrWhiteSpace(ccolorDepth))
                                continue;

                            this.depth = Convert.ToInt32(ccolorDepth);
                        }

                        lineCount++;
                        continue;
                    }

                    // string.Split(null) splits string by white space.
                    var colorArray = line.Split(null);
                    int pixelCount = 0;
                    int colorCount = 0;
                    int red = 0;
                    int green = 0;
                    int blue = 0;
                    foreach (var color in colorArray)
                    {
                        if (string.IsNullOrWhiteSpace(color))
                            continue;

                        if (colorCount == 2)
                        {
                            blue = Convert.ToInt32(color);
                            this.pixels[lineCount - 4, pixelCount++] = new Color(red, green, blue);
                            red = 0;
                            green = 0;
                            blue = 0;

                            colorCount = 0;

                            continue;
                        }

                        if (colorCount == 1)
                        {
                            green = Convert.ToInt32(color);
                            colorCount++;
                            continue;
                        }

                        if (colorCount == 0)
                        {
                            red = Convert.ToInt32(color);
                            colorCount++;
                            continue;
                        }
                    }

                    lineCount++;
                }
            }
        }

        public Image flattenGreen()
        {
            throw new NotImplementedException();
        }

        public Image flattenRed()
        {
            throw new NotImplementedException();
        }

        public Image GreyScale()
        {
            throw new NotImplementedException();
        }

        public Image negateBlue()
        {
            throw new NotImplementedException();
        }

        public Image negateGreen()
        {
            throw new NotImplementedException();
        }

        public Image negateRed()
        {
            throw new NotImplementedException();
        }

        public Image randomNoise()
        {
            throw new NotImplementedException();
        }

        public Image extremeContrast()
        {
            throw new NotImplementedException();
        }

        public Image horizontalBlur()
        {
            throw new NotImplementedException();
        }

        public override Image rotateLeft()
        {
            return rotate()
                    .rotate()
                    .rotate();
        }

        public override Image rotateRight()
        {
            return rotate();
        }

        public override string save(string fileName)
        {
            fileName = string.Format("{0}-{1}.ppm", (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds, fileName);

            var filePath = string.Format("{0}\\{1}", ConfigurationManager.AppSettings["PPMFileDirectory"], fileName);

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine(this.magicNumber);
                writer.WriteLine(string.Format("{0} {1}", this.columns, this.rows));
                writer.WriteLine(this.depth);

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
                        lineBuffer.Append(this.pixels[i, j].ToString());
                    }
                    writer.WriteLine(lineBuffer.ToString());
                }
            }

            return fileName;
        }

        public Image flattenBlue()
        {
            var flattenImage = createImage() as PPM;

            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    flattenImage.pixels[i, j] = this.pixels[i, j];
                    flattenImage.pixels[i, j].Blue = 0;
                }
            }

            return flattenImage;
        }

        public Image convertToPGM()
        {
            var pgmPixels = new string[this.rows,this.columns];

            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    pgmPixels[i, j] = this.pixels[i, j].GetPGMCompatibleColor().ToString();
                }
            }

            return new PGM(this.rows, this.columns, this.depth, pgmPixels);
        }
    }
}
