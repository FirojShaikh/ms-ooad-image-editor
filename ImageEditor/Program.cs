using System;
using System.Configuration;

namespace ImageEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            var pgm = new PGM();

            pgm.open(string.Format("{0}\\{1}", ConfigurationManager.AppSettings["PGMFileDirectory"], ConfigurationManager.AppSettings["SourceFile"]));

            ConsoleKeyInfo cki;
            var file = string.Empty;
            do
            {
                DisplayMenu();

                cki = Console.ReadKey(false);
                switch (cki.KeyChar.ToString())
                {
                    case "1":
                           file= pgm
                                    .rotateRight()
                                    .save("RightRotated");

                        Console.WriteLine(string.Format("{0} PGM file created with Right rotation.",file));
                        break;
                    case "2":
                        file = pgm
                            .rotateLeft()
                            .save("LeftRotated");

                        Console.WriteLine(string.Format("{0} PGM file created with Left rotation.",file));
                        break;
                    case "3":
                        file = pgm
                                .flipVertical()
                                .save("VerticalFlipped");

                        Console.WriteLine(string.Format("{0} PGM file created by filipping vertically.",file));
                        break;
                    case "4":
                        file=pgm
                                .flipHorizontal()
                                .save("HorizontalFlipped");

                        Console.WriteLine(string.Format("{0} PGM file created by flipping horizontally.",file));
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

            Console.ReadKey();
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("\n\nPGM ImageEditor Menu:\n");
            Console.WriteLine("\t1. Rotate Right:\n");
            Console.WriteLine("\t2. Rotate Left:\n");
            Console.WriteLine("\t3. Flip Vertical:\n");
            Console.WriteLine("\t4. Flip Horizontal:\n");

            Console.WriteLine("\nPress the Escape (Esc) key to quit: \n");
        }
    }
}
