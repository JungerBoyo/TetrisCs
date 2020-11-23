using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Drawing;
using System.Xml.Schema;

namespace TetrisProj
{
    public enum Dir { UP, DOWN, LEFT, RIGHT };

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

        protected virtual void SetShape() { }

        public char ShapePattern;
        protected int Center = 14;
        public Complex[] Coords;
    }
  
    public class Shape_1 : Shape
    {
        /*
            x
            x
            x
            x
        */
   
        protected override void SetShape()
        {
            Coords = new Complex[4];

            Coords[0] = new Complex(Center, -1);
            Coords[1] = new Complex(Center, -2);
            Coords[2] = new Complex(Center, -3);
            Coords[3] = new Complex(Center, -4);

            base.SetShape();
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

        protected override void SetShape()
        {
            Coords = new Complex[5];

            Coords[0] = new Complex(Center, -1);
            Coords[1] = new Complex(Center, -2);
            Coords[2] = new Complex(Center, -3);
            Coords[3] = new Complex(Center - 1, -3);
            Coords[4] = new Complex(Center - 1, -4);
            
            base.SetShape();

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
        protected override void SetShape()
        {
            Coords = new Complex[5];

            Coords[0] = new Complex(Center, -1);
            Coords[1] = new Complex(Center, -2);
            Coords[2] = new Complex(Center, -3);
            Coords[3] = new Complex(Center - 1, -3);
            Coords[4] = new Complex(Center, -4);

            base.SetShape();
        }

    }

    public class Shape_4 : Shape
    {
        /*
           x x
             x
             x
        */
        protected override void SetShape()
        {
            Coords = new Complex[4];

            Coords[0] = new Complex(Center, -1);
            Coords[1] = new Complex(Center, -2);
            Coords[2] = new Complex(Center, -3);
            Coords[3] = new Complex(Center - 1, -3);

            base.SetShape();
        }

    }

    public class Shape_5 : Shape
    {
        /*
          x x
          x x
        */

        protected override void SetShape()
        {
            Coords = new Complex[4];

            Coords[0] = new Complex(Center, -1);
            Coords[1] = new Complex(Center, -2);
            Coords[2] = new Complex(Center - 1, -2);
            Coords[3] = new Complex(Center - 1, -1);

            base.SetShape();
        }

    }

    
    public class Logic
    {
        public Logic()
        {
            PresentShape = randomShape();
            NextShape = randomShape();

            Score = 0;

            DrawNextShape();

            MainLoop();
        }

        private void MainLoop()
        {
            bool HasTouchedGround;

            while(_pressedkey.Key != ConsoleKey.Escape)
            {
                if (_pressedkey.Key == ConsoleKey.A)
                    Rotate(Dir.LEFT);
                else if (_pressedkey.Key == ConsoleKey.D)
                    Rotate(Dir.RIGHT);
                else if (_pressedkey.Key == ConsoleKey.Spacebar)
                    Mirror();
                else if (_pressedkey.Key == ConsoleKey.LeftArrow)
                    Move(Dir.LEFT);
                else if (_pressedkey.Key == ConsoleKey.RightArrow)
                    Move(Dir.RIGHT);
                else if (_pressedkey.Key == ConsoleKey.DownArrow)
                    RushDown();

                //if (_pressedkey.Key != ConsoleKey.Enter) _pressedkey = Enter;

                HasTouchedGround = checkBlockSurroundings();

                if(!HasTouchedGround)
                {
                    DrawNextGameState();
                }
                else
                {
                    ReduceGround();
                    switchShapes();

                    DrawNextGameState();
                }

                
            }
        }

        private void switchShapes()
        {

        }

        private void ReduceGround()
        {

        }

        private bool checkBlockSurroundings()
        {
            return false;
        }

        private void DrawNextGameState()
        {

        }

        private void Move(Dir Direction)
        {

        }

        private void Rotate(Dir Direction)
        {

        }

        private void Mirror()
        {

        }

        private void RushDown()
        {

        }

        private Shape randomShape()
        {
            Random rnd = new Random();

            switch(rnd.Next(1,5))
            {
                case 1: return new Shape_1();
                case 2: return new Shape_2();
                case 3: return new Shape_3();
                case 4: return new Shape_4();
                case 5: return new Shape_5();
            }

            return new Shape();
        }

        private void DrawNextShape()
        {
            for (int i = 0; i < NextShape.Coords.Length; i++)
                Draw.One(
                            NextShape.ShapePattern,
                            (int)NextShape.Coords[i].Real + 19,
                            (int)NextShape.Coords[i].Imaginary + 19
                         );
            
        }

        
        private Shape PresentShape;
        private Shape NextShape;

        public int Score;
        public ConsoleKeyInfo _pressedkey;
    }

    public static class Saving
    {
        static public void LoadGame()
        {
           
        }

        static public void SaveGame()
        {

        }

        static public void NewHighestScore(int Score)
        {

        }

        public static int HighestScore;
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

    public class PauseScreen : Menu
    {

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
        public bool _NewGame { get; }
    }

    class Program
    {
        static void Main(string[] args)
        {          
            Menu _Menu = new Menu();
            GamePanel _GamePanel = new GamePanel();

            if (_Menu._NewGame)
                Saving.LoadGame();

            Logic Game = new Logic();

            ConsoleKeyInfo pressedkey;

            do
            {
                pressedkey = Console.ReadKey(true);
                Game._pressedkey = pressedkey;

            } while (pressedkey.Key != ConsoleKey.Escape);





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
