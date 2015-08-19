using System;
using System.Configuration;

namespace ImageEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessMainMenu();

            Console.ReadKey();
        }

        private static void ProcessPGMMenu()
        {
            var pgm = new PGM();

            pgm.open(string.Format("{0}\\{1}", ConfigurationManager.AppSettings["PGMFileDirectory"], ConfigurationManager.AppSettings["PGMSourceFile"]));

            ConsoleKeyInfo cki;
            var file = string.Empty;
            do
            {
                DisplayPGMMenu();

                cki = Console.ReadKey(false);
                switch (cki.KeyChar.ToString())
                {
                    case "1":
                        file = pgm
                                 .rotateRight()
                                 .save("PGM-RightRotated");

                        Console.WriteLine(string.Format("{0} PGM file created with Right rotation.", file));
                        break;
                    case "2":
                        file = pgm
                            .rotateLeft()
                            .save("PGM-LeftRotated");

                        Console.WriteLine(string.Format("{0} PGM file created with Left rotation.", file));
                        break;
                    case "3":
                        file = pgm
                                .flipVertical()
                                .save("PGM-VerticalFlipped");

                        Console.WriteLine(string.Format("{0} PGM file created by filipping vertically.", file));
                        break;
                    case "4":
                        file = pgm
                                .flipHorizontal()
                                .save("PGM-HorizontalFlipped");

                        Console.WriteLine(string.Format("{0} PGM file created by flipping horizontally.", file));
                        break;
                    default:
                        if (cki.Key == ConsoleKey.Escape)
                        {
                            Console.WriteLine("\nReturning back to main menu.");
                        }
                        else
                        {
                            Console.WriteLine("Unkonwn menu option entered.");
                        }
                        break;
                }
            } while (cki.Key != ConsoleKey.Escape);
        }

        private static void ProcessPPMMenu()
        {
            var ppm = new PPM();

            ppm.open(string.Format("{0}\\{1}", ConfigurationManager.AppSettings["PPMFileDirectory"], ConfigurationManager.AppSettings["PPMSourceFile"]));

            var selectedOption = string.Empty;
            var file = string.Empty;
            do
            {
                DisplayPPMMenu();

                selectedOption = Console.ReadLine();

                switch (selectedOption)
                {
                    case "1":
                        file = ppm
                                 .rotateRight()
                                 .save("PPM-RightRotated");

                        Console.WriteLine(string.Format("{0} PPM file created with Right rotation.", file));
                        break;
                    case "2":
                        file = ppm
                            .rotateLeft()
                            .save("PPM-LeftRotated");

                        Console.WriteLine(string.Format("{0} PPM file created with Left rotation.", file));
                        break;
                    case "3":
                        file = ppm
                                .flipVertical()
                                .save("PPM-VerticalFlipped");

                        Console.WriteLine(string.Format("{0} PPM file created by filipping vertically.", file));
                        break;
                    case "4":
                        file = ppm
                                .flipHorizontal()
                                .save("PPM-HorizontalFlipped");

                        Console.WriteLine(string.Format("{0} PPM file created by flipping horizontally.", file));
                        break;
                    case "5":
                        file = ppm
                                .negateRed()
                                .save("PPM-NegateRed");

                        Console.WriteLine(string.Format("{0} PPM file created by negating red.", file));
                        break;
                    case "6":
                        file = ppm
                                .negateGreen()
                                .save("PPM-NegateGreen");

                        Console.WriteLine(string.Format("{0} PPM file created by negating green.", file));
                        break;
                    case "7":
                        file = ppm
                                .negateBlue()
                                .save("PPM-NegateBlue");

                        Console.WriteLine(string.Format("{0} PPM file created by negating blue.", file));
                        break;
                    case "8":
                        file = ppm
                                .GreyScale()
                                .save("PPM-GreyScale");

                        Console.WriteLine(string.Format("{0} PPM file created by scaling upto grey.", file));
                        break;
                    case "9":
                        file = ppm
                                .flattenRed()
                                .save("PPM-FlattenRed");

                        Console.WriteLine(string.Format("{0} PPM file created by flatten red.", file));
                        break;
                    case "10":
                        file = ppm
                                .flattenGreen()
                                .save("PPM-flattenGreen");

                        Console.WriteLine(string.Format("{0} PPM file created by flatten green.", file));
                        break;
                    case "11":
                        file = ppm
                                .flattenBlue()
                                .save("PPM-FlattenBlue");

                        Console.WriteLine(string.Format("{0} PPM file created by flatten blue.", file));
                        break;
                    case "12":
                        file = ppm
                                .convertToPGM()
                                .save("PPM-To-PGM-ConvertedFile");

                        Console.WriteLine(string.Format("{0} PPM file  converted into PGM file.", file));
                        break;
                    case "13":
                        file = ppm
                                .horizontalBlur()
                                .save("PPM-HorizontalBlur");

                        Console.WriteLine(string.Format("{0} PPM file  horizontally blured.", file));
                        break;
                    case "14":
                        file = ppm
                                .extremeContrast()
                                .save("PPM-ExtremeContrast");

                        Console.WriteLine(string.Format("{0} PPM file  extremely contrasted.", file));
                        break;
                    case "15":
                        file = ppm
                                .randomNoise()
                                .save("PPM-RandomeNoise");

                        Console.WriteLine(string.Format("{0} PPM file  with random noise.", file));
                        break;
                    default:
                        if (selectedOption == "X")
                        {
                            Console.WriteLine("\nReturning back to main menu.");
                        }
                        else
                        {
                            Console.WriteLine("Unkonwn menu option entered.");
                        }
                        break;
                }
            } while (selectedOption != "X");
        }

        private static void ProcessMainMenu()
        {
            ConsoleKeyInfo cki;
            var file = string.Empty;
            do
            {
                DisplayMainMenu();

                cki = Console.ReadKey(false);
                switch (cki.KeyChar.ToString())
                {
                    case "1":
                        ProcessPGMMenu();
                        break;
                    case "2":
                        ProcessPPMMenu();
                        break;
                    default:
                        if (cki.Key == ConsoleKey.Escape)
                        {
                            Console.WriteLine("Thank you for using utility.");
                        }
                        else
                        {
                            Console.WriteLine("Unkonwn menu option entered.");
                        }
                        break;
                }
            } while (cki.Key != ConsoleKey.Escape);
        }

        private static void DisplayMainMenu()
        {
            Console.WriteLine("\n\n ImageEditor Menu:\n");
            Console.WriteLine("\t1. PGM:\n");
            Console.WriteLine("\t2. PPM:\n");

            Console.WriteLine("\nPress the Escape (Esc) key to quit: \n");
        }

        private static void DisplayPGMMenu()
        {
            Console.WriteLine("\n\nPGM ImageEditor Menu:\n");
            Console.WriteLine("\t1. Rotate Right:\n");
            Console.WriteLine("\t2. Rotate Left:\n");
            Console.WriteLine("\t3. Flip Vertical:\n");
            Console.WriteLine("\t4. Flip Horizontal:\n");

            Console.WriteLine("\nPress the Escape (Esc) key to quit: \n");
        }

        private static void DisplayPPMMenu()
        {
            Console.WriteLine("\n\nPPM ImageEditor Menu:\n");
            Console.WriteLine("\t1. Rotate Right:\n");
            Console.WriteLine("\t2. Rotate Left:\n");
            Console.WriteLine("\t3. Flip Vertical:\n");
            Console.WriteLine("\t4. Flip Horizontal:\n");
            Console.WriteLine("\t5. Negate Red:\n");
            Console.WriteLine("\t6. Negate Green:\n");
            Console.WriteLine("\t7. Negate Blue:\n");
            Console.WriteLine("\t8. Grey Scale:\n");
            Console.WriteLine("\t9. Flatten Red:\n");
            Console.WriteLine("\t10. Flatten Green:\n");
            Console.WriteLine("\t11. Flatten Blue:\n");
            Console.WriteLine("\t12. Convert to PGM:\n");
            Console.WriteLine("\t13. Horizontal Blur:\n");
            Console.WriteLine("\t14. Extreme Contrast:\n");
            Console.WriteLine("\t15. Randome Noise:\n");

            Console.WriteLine("\nPress \"X\" to quit: \n");
        }
    }
}
