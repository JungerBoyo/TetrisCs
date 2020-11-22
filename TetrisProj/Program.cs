using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Drawing;
using System.Xml.Schema;

namespace TetrisProj
{   

    public class Saving
    {

    }

    public class Menu
    {
        public Menu()
        {
            DrawMenu();
            _NewGame = MenuControl();
        }

        ~Menu() => Console.Clear();

        private void DrawMenu()
        {
            Console.SetWindowSize(41, 21);

            for(int i=0; i<40; i++) { Console.SetCursorPosition(i, 0); Console.Write('_'); }

            for (int j = 1; j < 20; j++)
            {
                for (int i = 0; i < 40; i+=39)
                {
                    Console.SetCursorPosition(i, j); 
                    Console.Write('|');
                }
            }

            for (int i = 1; i < 39; i++) { Console.SetCursorPosition(i, 19); Console.Write('_'); }

            Console.SetCursorPosition(12, 6);
            Console.Write("T  E  T  R  I  S");

            Console.SetCursorPosition(14, 10);
            Console.Write("> New Game <");

            Console.SetCursorPosition(13, 12);
            Console.Write("> Load  Game <");

        }

        private bool MenuControl()
        {
            bool NewGame = false;
               
            do
            {
                key = Console.ReadKey(true);

                if(key.Key == ConsoleKey.W || key.Key == ConsoleKey.S)
                {
                    if(NewGame)
                    {
                        Console.SetCursorPosition(27, 12);
                        Console.Beep(100, 200);
                        NewGame = true;
                    }
                    else
                    {
                        Console.SetCursorPosition(26, 10);
                        Console.Beep(200, 200);
                        NewGame = false;
                    }
                }
                

            } 
            while (key.Key != ConsoleKey.Enter);

            

            return NewGame;
        }


        private ConsoleKeyInfo key;
        private bool _NewGame;
        //"■")
    }

    class Program
    {
        static void Main(string[] args)
        {

            
            Menu _Menu = new Menu();

            char k;

            //while( (k = Console.ReadKey().KeyChar) != 'a');






















            /*
            List<Complex> shape = new List<Complex>();
            Complex tmp;

            shape.Add(new Complex(1 ,1));
            shape.Add(new Complex(2, 1));
            shape.Add(new Complex(2, 2));
            shape.Add(new Complex(2, 3));
            shape.Add(new Complex(3, 3));

            foreach (var coord in shape)
            {
                Console.SetCursorPosition((int)coord.Real,(int)coord.Imaginary);
                Console.Write('x');
            }

            
            Thread.Sleep(1000);
            Console.Clear();

          

            foreach (Complex coord in shape)
            {
                

                tmp = Complex.Multiply(coord,Complex.ImaginaryOne);
                tmp = Complex.Add(tmp, 4 * Complex.One);

                

                Console.SetCursorPosition((int)tmp.Real, (int)tmp.Imaginary);
                Console.Write('x');
            }
            

            Thread.Sleep(1000);
            Console.Clear();


            
            foreach (var coord in shape)
            {
                tmp = Complex.Conjugate(coord);
                tmp = Complex.Add(tmp, 4 * Complex.ImaginaryOne);


                Console.SetCursorPosition((int)tmp.Real, (int)tmp.Imaginary);
                Console.Write('x');
            }
            


            Console.SetCursorPosition(10, 10);
            */
        }
    }
}
