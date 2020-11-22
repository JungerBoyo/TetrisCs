using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Drawing;
using System.Xml.Schema;

namespace TetrisProj
{
    static public class Draw
    {
        public static void One(char c, int X, int Y)
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(c);
        }

        public static void Few(string s, int X, int Y)
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(s);
        }
    }

    public class Shape
    {
        public Shape()
        {
            SetShape();
            ShapePattern = '■';
        }

        protected void SetShape() { }

        protected char ShapePattern { get; }
        protected int Center = 19;
        protected Complex[] Coords;
    }

    public class Shape_1 : Shape
    {
        /*
            x
            x
            x
            x
        */

        protected new void SetShape()
        {
            Coords = new Complex[4];

            Coords[0] = new Complex(Center, -1);
            Coords[1] = new Complex(Center, -2);
            Coords[2] = new Complex(Center, -3);
            Coords[3] = new Complex(Center, -4);
        }

    }

    public class Shape_2 : Shape
    {
       /*
         
         x 
         x x
           x
           x
       */

        protected new void SetShape()
        {
            Coords = new Complex[5];

            Coords[0] = new Complex(Center, -1);
            Coords[1] = new Complex(Center, -2);
            Coords[2] = new Complex(Center, -3);
            Coords[3] = new Complex(Center - 1, -3);
            Coords[4] = new Complex(Center - 1, -4);
     
        }

    }

    public class Shape_3 : Shape
    {
        /*
           x
         x x
           x
           x
       */
        protected new void SetShape()
        {
            Coords = new Complex[5];

            Coords[0] = new Complex(Center, -1);
            Coords[1] = new Complex(Center, -2);
            Coords[2] = new Complex(Center, -3);
            Coords[3] = new Complex(Center - 1, -3);
            Coords[4] = new Complex(Center, -4);
        }

    }

    public class Shape_4 : Shape
    {
        /*
           x x
             x
             x
        */
        protected new void SetShape()
        {
            Coords = new Complex[4];

            Coords[0] = new Complex(Center, -1);
            Coords[1] = new Complex(Center, -2);
            Coords[2] = new Complex(Center, -3);
            Coords[3] = new Complex(Center - 1, -3);
        }

    }

    public class Shape_5 : Shape
    {
        /*
          x x
          x x
        */

        protected new void SetShape()
        {
            Coords = new Complex[4];

            Coords[0] = new Complex(Center, -1);
            Coords[1] = new Complex(Center, -2);
            Coords[2] = new Complex(Center - 1, -2);
            Coords[3] = new Complex(Center - 1, -1);
        }

    }

    public class GamePanel
    {
        public GamePanel()
        {
            DrawGamePanel();
        }

        private void DrawGamePanel()
        {
            for (int i = 28; i < 39; i++) 
            {
                Draw.One('─', i, 0);
                Draw.One('─', i, 7);
                Draw.One('─', i, 10);
            }
            
            for (int i = 1; i < 39; i++)
                Draw.One('─', i, 20);

            for (int j = 1; j < 20; j++)
            {
                for (int i = 0; i < 40; i += 39)
                {
                    Draw.One('│', i, j);
                    Draw.One('│', 27, j);
                }
            }

            Draw.One('└', 27, 20);
            Draw.One('│', 0, 0);
            Draw.One('┌', 27, 0);
            Draw.One('┐', 39, 0);
            Draw.One('┘', 39, 20);
            Draw.One('└', 0, 20);

            Draw.Few("N E X T", 30, 12);
            Draw.Few("S H A P E", 29, 13);

            Draw.Few("RECORD ", 28, 8);
            Draw.Few("SCORE ", 28, 9);

            Draw.Few("S P A C E", 29, 2);
            Draw.Few("└ MIRROR", 29, 3);

            Draw.Few("'A'/'D'", 29, 5);
            Draw.Few("└ ROTATE", 29, 6);
        }
    }
    public class Logic
    {

    }

    public class Saving
    {
        public Saving(bool _NewGame)
        {

        }
    }

    public class Menu
    {
        public Menu()
        {
            DrawMenu();
            _NewGame = MenuControl();
        }

        private void DrawMenu()
        {
            Console.SetWindowSize(41, 21);

            for(int i=0; i<40; i++) 
                Draw.One('─', i, 0);

            for (int j = 1; j < 20; j++)
                for (int i = 0; i < 40; i+=39)
                    Draw.One('│', i, j);
                
            for (int i = 1; i < 39; i++)
                Draw.One('─', i, 20);

            Draw.Few("T  E  T  R  I  S", 12, 6);
            Draw.Few("> New Game <", 14, 10);
            Draw.Few("> Load  Game <", 13, 12);

            Draw.One('┌', 0, 0);
            Draw.One('┐', 39, 0);
            Draw.One('┘', 39, 20);
            Draw.One('└', 0, 20);

            Console.SetCursorPosition(27, 12);

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
                        NewGame = false;
                    }
                    else
                    {
                        Console.SetCursorPosition(26, 10);
                        Console.Beep(200, 200);
                        NewGame = true;
                    }
                }
                

            } 
            while (key.Key != ConsoleKey.Enter);

            Console.Clear();

            return NewGame;
        }

        private ConsoleKeyInfo key;
        private bool _NewGame { get; }
    }

    class Program
    {
        static void Main(string[] args)
        {          
            Menu _Menu = new Menu();
            GamePanel _GamePanel = new GamePanel();






            Console.SetCursorPosition(0, 21);
















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
