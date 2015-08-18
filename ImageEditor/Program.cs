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

            ConsoleKeyInfo cki;
            var file = string.Empty;
            do
            {
                DisplayPPMMenu();

                cki = Console.ReadKey(false);
                switch (cki.KeyChar.ToString())
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

            Console.WriteLine("\nPress the Escape (Esc) key to quit: \n");
        }
    }
}
