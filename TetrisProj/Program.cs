using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Drawing;
using System.Xml.Schema;
using System.Globalization;
using System.Data;
using System.ComponentModel;

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

        public static void ClearSpecificConsoleArea(int Index0X, int Index0Y, int Width, int Hight)
        {
            for (int y = Index0Y; y < Index0Y+Hight; y++)
                for (int x = Index0X; x < Index0X+Width; x++)
                    One(' ', x, y);
            
        }
    }

    public class Shape
    {
        public Shape()
        {
            SetShape();
            ShapePattern = 'x';
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
            Draw.Few($"[{(Score).ToString()}]", 35, 9);

            DrawNextShape();
        }

        public void MainLoop()
        {
            bool HasTouchedGround;

            while(_pressedkey.Key != ConsoleKey.Escape)
            {
                switch(_pressedkey.Key)
                {
                    case ConsoleKey.A: Rotate(Dir.LEFT); break;
                    case ConsoleKey.D: Rotate(Dir.RIGHT); break;
                    case ConsoleKey.Spacebar: Mirror(); break;
                    case ConsoleKey.LeftArrow: Move(Dir.LEFT, 0); break;
                    case ConsoleKey.RightArrow: Move(Dir.RIGHT, 0); break;
                    case ConsoleKey.Q: Move(Dir.LEFT, -1); break;
                    case ConsoleKey.E: Move(Dir.RIGHT, 1); break;
                    case ConsoleKey.DownArrow: RushDown(); break;
                    default: break;
                }

                HasTouchedGround = checkBlockSurroundings();

                if (!HasTouchedGround)
                    DrawNextGameState();
                else
                {
                    CheckReduce();
                    switchShapes();
                    DrawNextGameState();

                    HasTouchedGround = false;
                }
                

                Thread.Sleep(500);
                
            }
        }

        private void switchShapes()
        {
            PresentShape = new Shape_1();
            NextShape = randomShape();
            
            Draw.ClearSpecificConsoleArea(29, 14, 9, 5);
            DrawNextShape();
        }

        private void CheckReduce()
        {
            bool Reduce = false;
            int j;
         
            for (int i = 19; i>=0; i--)
            {
                for (j = 1; j < 27; j++)
                    if(BusySlots[i,j] == false)
                        break;

                if (j == 27)
                {
                    LayerToReduce[i] = true;
                    Reduce = true;
                }
            }

            if (Reduce)
                ReduceAndMergeLayers();
            
        }

        private void ReduceAndMergeLayers()
        {
            

            for (int i = 19; i >= 0; i--)
            {
                if (LayerToReduce[i] == true)
                {
                    Score += 5;

                    for (int R=19; R>=0; R--)
                        for (int C = 1; C < 27; C++)                       
                            BusySlots[R + 1, C] = BusySlots[R, C];

                    Refresh();               
                }
            }
        }

        private void Refresh()
        {
            for (int i = 1; i < 27; i++)
                for (int j = 0; j < 20; j++)
                {
                    if (BusySlots[j, i] == false)
                        Draw.One(' ', i, j);
                    else
                        Draw.One('x', i, j);
                }
        }

        private bool checkBlockSurroundings()
        {
            for(int i=0; i<PresentShape.Coords.Length; i++)
            {
                if(PresentShape.Coords[i].Imaginary >= 0 )
                    if (PresentShape.Coords[i].Imaginary + 1 >= 20 || BusySlots[(int)PresentShape.Coords[i].Imaginary+1,(int)PresentShape.Coords[i].Real] == true)
                    {
                        for (int b = 0; b < PresentShape.Coords.Length; b++)
                            BusySlots[(int)PresentShape.Coords[b].Imaginary,(int)PresentShape.Coords[b].Real] = true;

                        Console.Beep(200,500);
                        Draw.Few($"[{(++Score).ToString()}]", 35, 9);
                        return true;
                    }
            }

            return false;
        }

        private void DrawNextGameState()
        {

            for(int i=0; i<PresentShape.Coords.Length; i++)
            {
                if(PresentShape.Coords[i].Imaginary + 1 >= 0)
                {
                    if(PresentShape.Coords[i].Imaginary == -1)
                    {
                        PresentShape.Coords[i] = new Complex(PresentShape.Coords[i].Real, PresentShape.Coords[i].Imaginary + 1);
                        Draw.One(PresentShape.ShapePattern, (int)PresentShape.Coords[i].Real, (int)PresentShape.Coords[i].Imaginary);
                    }
                    else
                    {                        
                        Draw.One(' ', (int)PresentShape.Coords[i].Real, (int)PresentShape.Coords[i].Imaginary); 
                        PresentShape.Coords[i] = new Complex(PresentShape.Coords[i].Real, PresentShape.Coords[i].Imaginary + 1);
                    }
                    
                }
                else
                {
                    PresentShape.Coords[i] = new Complex(PresentShape.Coords[i].Real, PresentShape.Coords[i].Imaginary + 1);
                }
            }

            for (int i = 0; i < PresentShape.Coords.Length; i++)          
                if (PresentShape.Coords[i].Imaginary >= 0)               
                    Draw.One(PresentShape.ShapePattern, (int)PresentShape.Coords[i].Real, (int)PresentShape.Coords[i].Imaginary);
           
        }

        private void Move(Dir Direction, int Dash)
        {
            int Mover = (Direction == Dir.RIGHT) ? (1+Dash) : (-1+Dash);
            bool WallBeside = false;

            for(int i=0; i<PresentShape.Coords.Length; i++)          
                if(PresentShape.Coords[i].Real + Mover == 27 || PresentShape.Coords[i].Real + Mover == 0)
                {
                    WallBeside = true;
                    break;
                }

            if(!WallBeside)
            {
                for (int i = 0; i < PresentShape.Coords.Length; i++)
                {
                    if (PresentShape.Coords[i].Imaginary >= 0)
                    {                                               
                         Draw.One(' ', (int)PresentShape.Coords[i].Real, (int)PresentShape.Coords[i].Imaginary);
                         PresentShape.Coords[i] = new Complex(PresentShape.Coords[i].Real+Mover, PresentShape.Coords[i].Imaginary);                        
                    }
                    else
                    {
                        PresentShape.Coords[i] = new Complex(PresentShape.Coords[i].Real+Mover, PresentShape.Coords[i].Imaginary);
                    }
                }

                for (int i = 0; i < PresentShape.Coords.Length; i++)
                    if (PresentShape.Coords[i].Imaginary >= 0)
                        Draw.One(PresentShape.ShapePattern, (int)PresentShape.Coords[i].Real, (int)PresentShape.Coords[i].Imaginary);
            }


            _pressedkey = new ConsoleKeyInfo();
        }

        private void Rotate(Dir Direction)
        {
            double rotator = (Direction == Dir.RIGHT) ? 3 : 1;
            int corrector = (Direction == Dir.RIGHT) ? -1 : 1;

            int MaxImagineryValue = -10;
            Complex CenterOfRotation = new Complex(0, 0);
            Complex ShiftVector;

            for (int i = 0; i < PresentShape.Coords.Length; i++)
                if (PresentShape.Coords[i].Imaginary > MaxImagineryValue)
                {
                    MaxImagineryValue = (int)PresentShape.Coords[i].Imaginary;
                    CenterOfRotation = PresentShape.Coords[i];
                }

            for (int i = 0; i < PresentShape.Coords.Length; i++)
            {
                Draw.One(' ', (int)PresentShape.Coords[i].Real, (int)PresentShape.Coords[i].Imaginary);

                ShiftVector = Complex.Multiply(Complex.Add(CenterOfRotation, Complex.Negate(PresentShape.Coords[i])), Complex.Pow(Complex.ImaginaryOne,rotator));
                PresentShape.Coords[i] = Complex.Add(ShiftVector, Complex.Add(CenterOfRotation, new Complex(corrector, -1)));
            }

            _pressedkey = new ConsoleKeyInfo();
        }

        private void Mirror()
        {
            int maxImagineryValue = -1;
            int minImaginaryValue = 25;

            for (int i = 0; i < PresentShape.Coords.Length; i++)
            {
                if (maxImagineryValue < PresentShape.Coords[i].Imaginary)
                    maxImagineryValue = (int)PresentShape.Coords[i].Imaginary;

                else if (minImaginaryValue > PresentShape.Coords[i].Imaginary)
                    minImaginaryValue = (int)PresentShape.Coords[i].Imaginary;
                
            }


            for (int i = 0; i < PresentShape.Coords.Length; i++)
            {
                if (PresentShape.Coords[i].Imaginary >= 0)
                    Draw.One(' ', (int)PresentShape.Coords[i].Real, (int)PresentShape.Coords[i].Imaginary);

                PresentShape.Coords[i] = Complex.Conjugate(PresentShape.Coords[i]);
                PresentShape.Coords[i] = Complex.Add(PresentShape.Coords[i], (minImaginaryValue + maxImagineryValue + 1) * Complex.ImaginaryOne);
            }

            for(int i = 0; i < PresentShape.Coords.Length; i++)
            {
                if (PresentShape.Coords[i].Imaginary >= 0)
                    Draw.One(PresentShape.ShapePattern, (int)PresentShape.Coords[i].Real, (int)PresentShape.Coords[i].Imaginary);
            }

            _pressedkey = new ConsoleKeyInfo();
        }

        private void RushDown()
        {
            for (int i = 0; i < PresentShape.Coords.Length; i++)
            {
                if (PresentShape.Coords[i].Imaginary >= 0)
                    Draw.One(' ', (int)PresentShape.Coords[i].Real, (int)PresentShape.Coords[i].Imaginary);
            }

            while (!checkBlockSurroundings())
            {
                for(int i=0; i<PresentShape.Coords.Length; i++)
                {
                    PresentShape.Coords[i] = Complex.Add(PresentShape.Coords[i], Complex.ImaginaryOne);
                }
            }

            for (int i = 0; i < PresentShape.Coords.Length; i++)
            {
                if (PresentShape.Coords[i].Imaginary >= 0)
                    Draw.One(PresentShape.ShapePattern, (int)PresentShape.Coords[i].Real, (int)PresentShape.Coords[i].Imaginary);
            }

            _pressedkey = new ConsoleKeyInfo();
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
        private bool[,] BusySlots = new bool[25,27];
        private bool[] LayerToReduce = new bool[20];

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

            Thread t = new Thread(new ThreadStart(Game.MainLoop));

            t.Start();

            ConsoleKeyInfo pressedkey;
            Console.CursorVisible = false;

            do
            {
                pressedkey = Console.ReadKey(true);
                Game._pressedkey = pressedkey;

            } while (pressedkey.Key != ConsoleKey.Escape);

            t.Join();



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
