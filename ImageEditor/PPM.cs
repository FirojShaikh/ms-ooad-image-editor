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

        // create image
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

        /// <summary>
        /// Open PGM file and initialize object with provided specification and data
        /// </summary>
        /// <param name="filePath"></param>
        public override void open(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                int lineCount = 1;

                while (reader.Peek() >= 0)
                {
                    if (lineCount < 4)
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
                    }
                    else
                    {
                        var tokens = reader.ReadToEnd().Split(null);
                        int colorCount = 0;

                        int red = 0;
                        int green = 0;
                        int blue = 0;
                        int i = 0;
                        int j = 0;

                        foreach (var token in tokens)
                        {
                            if (j == this.columns)
                            {
                                i++;
                                j = 0;
                            }

                            if (string.IsNullOrWhiteSpace(token))
                                continue;

                            if (colorCount == 2)
                            {
                                blue = Convert.ToInt32(token);
                                this.pixels[i, j++] = new Color(red, green, blue);
                                red = 0;
                                green = 0;
                                blue = 0;

                                colorCount = 0;

                                continue;
                            }

                            if (colorCount == 1)
                            {
                                green = Convert.ToInt32(token);
                                colorCount++;
                                continue;
                            }

                            if (colorCount == 0)
                            {
                                red = Convert.ToInt32(token);
                                colorCount++;
                                continue;
                            }
                        }
                    }

                    lineCount++;
                }
            }
        }

        /// <summary>
        /// Save PGM file with current state of data. File name will be pre-fixed by Unix style timestamp
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// flips each row horizontally
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// flips each row vertically
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// rotates image 90 degrees left
        /// </summary>
        /// <returns></returns>
        public override Image rotateLeft()
        {
            return rotate()
                    .rotate()
                    .rotate();
        }

        /// <summary>
        /// rotates image 90 degrees right
        /// </summary>
        /// <returns></returns>
        public override Image rotateRight()
        {
            return rotate();
        }

        /// <summary>
        /// sets the red value to zero
        /// </summary>
        /// <returns></returns>
        public Image flattenRed()
        {
            var flattenImage = createImage() as PPM;

            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    flattenImage.pixels[i, j] = this.pixels[i, j];
                    flattenImage.pixels[i, j].Red = 0;
                }
            }

            return flattenImage;
        }

        /// <summary>
        /// sets the green value to zero
        /// </summary>
        /// <returns></returns>
        public Image flattenGreen()
        {
            var flattenImage = createImage() as PPM;

            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    flattenImage.pixels[i, j] = this.pixels[i, j];
                    flattenImage.pixels[i, j].Green = 0;
                }
            }

            return flattenImage;
        }

        /// <summary>
        /// sets the blue value to zero
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// sets each pixel value to the average of the three RGB values
        /// </summary>
        /// <returns></returns>
        public Image GreyScale()
        {
            var newImage = createImage() as PPM;

            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    int averagePixelValue = Convert.ToInt32(Math.Round((double)((this.pixels[i, j].Red + this.pixels[i, j].Green + this.pixels[i, j].Blue) / 3)));
                    newImage.pixels[i, j] = new Color(averagePixelValue, averagePixelValue, averagePixelValue);
                }
            }

            return newImage;
        }

        /// <summary>
        /// negate the red number of each pixel. The maximum color depth number is useful here. 
        /// If the red were 0, it would become 255; if it were 255 it would become 0. 
        /// If the red were 100, it would become 155.
        /// </summary>
        /// <returns></returns>
        public Image negateRed()
        {
            var negatedImage = createImage() as PPM;

            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    negatedImage.pixels[i, j] = new Color(this.depth - this.pixels[i, j].Red, this.pixels[i, j].Green, this.pixels[i, j].Blue);
                }
            }

            return negatedImage;
        }

        /// <summary>
        /// negate the green number of each pixel. The maximum color depth number is useful here. 
        /// If the green were 0, it would become 255; if it were 255 it would become 0. 
        /// If the green were 100, it would become 155.
        /// </summary>
        /// <returns></returns>
        public Image negateGreen()
        {
            var negatedImage = createImage() as PPM;

            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    negatedImage.pixels[i, j] = new Color(this.pixels[i, j].Red, this.depth - this.pixels[i, j].Green, this.pixels[i, j].Blue);
                }
            }

            return negatedImage;
        }

        /// <summary>
        /// negate the blue number of each pixel. The maximum color depth number is useful here. 
        /// If the blue were 0, it would become 255; if it were 255 it would become 0. 
        /// If the blue were 100, it would become 155.
        /// </summary>
        /// <returns></returns>
        public Image negateBlue()
        {
            var negatedImage = createImage() as PPM;

            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    negatedImage.pixels[i, j] = new Color(this.pixels[i, j].Red, this.pixels[i, j].Green, this.depth - this.pixels[i, j].Blue);
                }
            }

            return negatedImage;
        }

        /// <summary>
        /// aves image as a PGM ﬁle
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// which will take the values of the red numbers of three adjacent pixels and replace them 
        /// with their average - note that this is different from greyscale! it also does the same with 
        /// the greens and the blues of 3 adjacent pixels. Pixels on the edges will have to be handled specially.
        /// </summary>
        /// <returns></returns>
        public Image horizontalBlur()
        {
            var bluredImage = createImage() as PPM;

            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    var red = ((this.pixels[i, j] == null ? 0 : this.pixels[i, j].Red) + (j + 1 >= this.columns ? 0 : this.pixels[i, j + 1].Red) + (j + 2 >= this.columns ? 0 : this.pixels[i, j + 2].Red)) / 3;
                    var green = ((this.pixels[i, j] == null ? 0 : this.pixels[i, j].Green) + (j + 1 >= this.columns ? 0 : this.pixels[i, j + 1].Green) + (j + 2 >= this.columns ? 0 : this.pixels[i, j + 2].Green)) / 3;
                    var blue = ((this.pixels[i, j] == null ? 0 : this.pixels[i, j].Blue) + (j + 1 >= this.columns ? 0 : this.pixels[i, j + 1].Blue) + (j + 2 >= this.columns ? 0 : this.pixels[i, j + 2].Blue)) / 3;

                    bluredImage.pixels[i, j] = new Color(red, green, blue);
                }
            }

            return bluredImage;
        }

        /// <summary>
        /// which will change each color number to either the highest color number possible or to 0. 
        /// This change is based on whether it is greater than the midpoint of the color range, or less. 
        /// If it is greater than half of the color depth, replace it with the colordepth. 
        /// If it is less, replace it with zero.
        /// </summary>
        /// <returns></returns>
        public Image extremeContrast()
        {
            var extremeContrastImage = createImage() as PPM;

            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    var red = this.pixels[i, j].Red > (this.depth / 2) ? this.depth : 0;
                    var green = this.pixels[i, j].Green > (this.depth / 2) ? this.depth : 0;
                    var blue = this.pixels[i, j].Blue > (this.depth / 2) ? this.depth : 0;

                    extremeContrastImage.pixels[i, j] = new Color(red, green, blue);
                }
            }

            return extremeContrastImage;
        }

        // generates random color number
        private int getRandomColorNumber(int color)
        {
            var randomColorNumber = new Random();

            var randomColorFactor = randomColorNumber.Next(color);

            if (color + randomColorFactor <= 255)
                return color + randomColorFactor;

            if (color - randomColorFactor >= 0)
                return color - randomColorFactor;

            return color > (this.depth / 2) ? this.depth : 0;
        }

        /// <summary>
        /// adds a random number to each color number or subtracts a random number. 
        /// It would have a parameter which would represent the size of the random number range; 
        /// i.e. a value of 10 would add or subtract numbers in the range of 0 to 9, 
        /// a value of 50 would add or subtract numbers in the range 0 to 49. Another random number 
        /// would decide whether it was an addition or a subtraction. Remember that the values in the buffer 
        /// should not exceed the colordepth nor get less than 0. You can use the extreme values instead.
        /// </summary>
        /// <returns></returns>
        public Image randomNoise()
        {
            var randomNoiseImage = createImage() as PPM;
            
            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    var red = getRandomColorNumber(this.pixels[i, j].Red);
                    var green = getRandomColorNumber(this.pixels[i, j].Green);
                    var blue = getRandomColorNumber(this.pixels[i, j].Blue);

                    randomNoiseImage.pixels[i, j] = new Color(red, green, blue);
                }
            }

            return randomNoiseImage;
        }
    }
}
