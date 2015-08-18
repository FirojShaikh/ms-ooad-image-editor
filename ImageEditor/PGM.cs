using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace ImageEditor
{
    public class PGM:Image
    {
        private string [,] pixels;

        /// <summary>
        /// Open PGM file and initialize object with provided specification and data
        /// </summary>
        /// <param name="filePath"></param>
        public override void open(string filePath)
        {
            using (var reader=new StreamReader(filePath))
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

                        this.pixels = new string[rows, columns];

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

        /// <summary>
        /// Save PGM file with current state of data. File name will be pre-fixed by Unix style timestamp
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>File Name</returns>
        public override string save(string fileName)
        {
            fileName = string.Format("{0}-{1}.pgm", (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds, fileName);

            var filePath = string.Format("{0}\\{1}", ConfigurationManager.AppSettings["PGMFileDirectory"],fileName);

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
                        lineBuffer.Append(this.pixels[i, j]);
                    }
                    writer.WriteLine(lineBuffer.ToString());
                }
            }

            return fileName;
        }

        // create PGM image object to rotate
        private Image createRotateImage()
        {
            var rotateImage = new PGM();

            rotateImage.magicNumber = this.magicNumber;
            rotateImage.depth = this.depth;

            // rotating image will switch number of columns to number of rows and number of rows to number of columns
            rotateImage.rows = this.columns;
            rotateImage.columns = this.rows;

            rotateImage.pixels = new string[columns, rows];

            return rotateImage;
        }

        // creates PGM image object to flip
        private Image createFlippedImage()
        {
            var flippedImage = new PGM();

            flippedImage.magicNumber = this.magicNumber;
            flippedImage.depth = this.depth;

            // flipping image will keep number of columns and number of rows as is.
            flippedImage.rows = this.rows;
            flippedImage.columns = this.columns;

            flippedImage.pixels = new string[rows, columns];

            return flippedImage;
        }

        // rotate image in 90 degrees
        private PGM rotate()
        {
            var rotateImage = createRotateImage() as PGM;
            
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
        /// Rotate image right
        /// </summary>
        /// <returns></returns>
        public override Image rotateRight()
        {
            return rotate();
        }

        /// <summary>
        /// Rotate image left
        /// </summary>
        /// <returns></returns>
        public override Image rotateLeft()
        {
            return rotate()
                    .rotate()
                    .rotate();
        }

        /// <summary>
        /// flip image vertically
        /// </summary>
        /// <returns></returns>
        public override Image flipVertical()
        {
            var flippedImage = createFlippedImage() as PGM;

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
        /// Flip image horizontally
        /// </summary>
        /// <returns></returns>
        public override Image flipHorizontal()
        {
            var flippedImage = createFlippedImage() as PGM;

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
    }
}
