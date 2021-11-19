using System;

namespace SharpSweeper
{
    class Game
    {
        // "Global" ints holding basic info about playing grid
        private static int XSize = 0;
        private static int YSize = 0;
        private static int MineNum = 0;
        public static void StartGame(int width, int height, int numberOfMines)
        {
            // Initialises basic grid info to input parameters and creates a new visible grid and mine information grid which get passed to the function play game
            XSize = width;
            YSize = height;
            MineNum = numberOfMines;
            PlayGame(Grid.GenerateMineGrid(XSize, YSize, MineNum), Grid.GenerateShownGrid(XSize, YSize));
            Console.Write("Press [enter] to return to main menu:");
            Console.Read();
        }
        private static void PlayGame(int[,] MineInformation, char[,] ShownGrid)
        {
            // Main method for running game
            string InputPrompt = "Enter Square to begin or type [Exit] at anytime to quit to main menu:";
            bool HitMine = false;
            bool GameOver = false;
            bool EndGame = false;
            // Keeps game running until game over conditon
            while (!GameOver)
            {
                // Runs TakeTurn Function
                TakeTurn();
                if (EndGame)
                {
                    // Game over condition for manual exit
                    GameEnded();
                }
                else if (HitMine)
                {
                    // Game over condition for hitting a mine
                    GameLost();
                }
                else if (SquaresLeft() == MineNum)
                {
                    // Game over condition for clearing all non mine squares
                    GameWon();
                }
                int SquaresLeft()
                {
                    // Simple function to count how many spaces are left uncleared
                    int Count = 0;
                    foreach (char c in ShownGrid)
                    {
                        if (c == '█')
                        {
                            Count++;
                        }
                    }
                    return Count;
                }
            }
            void TakeTurn()
            {
                int[] SelectedCoord = new int[2];
                bool ValidInput = false;
                // Gets input for square to be cleared
                do
                {
                    Console.Clear();
                    Console.WriteLine(InputPrompt);   
                    Grid.PrintGrid(ShownGrid);
                    // Sets SelectedCoord to the square coordinates in form LetterNumber converted to a zero based coordinate system
                    SelectedCoord = ConvertToCoordinates(Console.ReadLine().ToUpper());
                } while (!ValidInput);
                InputPrompt = "Select square to reveal:";
                // Gets value of minegrid at SelectedCoord
                char SquareContents = GetSquareContents(SelectedCoord[0], SelectedCoord[1]);
                if (EndGame)
                {
                    // Condition for manual exit
                    return;
                }
                else if (SquareContents == '*')
                {
                    // Condition for hitting a mine
                    HitMine = true;
                    return;
                }
                else if (SquareContents == '0')
                {
                    // Clears all squares around 0 space
                    HandleZero(SelectedCoord[0], SelectedCoord[1]);
                }
                else
                {
                    // Displays number from mineinformation grid to visible grid
                    UpdateVisibleGrid(SelectedCoord[0], SelectedCoord[1]);
                    // Autoreveals all 0's in a 3x3 area centred arround selected space
                    RevealZeroes(SelectedCoord[0], SelectedCoord[1]);
                }

                int[] ConvertToCoordinates(string input)
                {
                    // Converts input in form LetterNumber to a zero based coordinate system
                    int[] Coords = new int[2];
                    if (input == "EXIT")
                    {
                        // Condition for manually exiting game
                        ValidInput = true;
                        EndGame = true;
                        return Coords;
                    }
                    else if (input == "SV_CHEATS 1")
                    {
                        // Easter egg condition that reveals mines on the visible grid
                        Console.Clear();
                        Console.WriteLine("EASTER EGG SV_Cheats 1: Mines Revealed on grid\nPress [enter] to continue");
                        Console.Read();
                        Grid.RevealMines(ShownGrid, MineInformation);
                        ValidInput = true;
                        return Coords;
                    }
                    else if (input.Length < 2)
                    {
                        // Condition if input is less than 2 chars long
                        ValidInput = false;
                        InputPrompt = "Location not valid - Enter in form YX:";
                        return Coords;
                    }
                    else if (!Int32.TryParse(input.Substring(1, input.Length - 1), out int Y) || (int)input[0] - 65 < 0)
                    {
                        // Condition for if either 2 letters are inputed or input is in form NumberLetter
                        InputPrompt = "Location not valid - Enter in form YX:";
                        ValidInput = false;
                        return Coords;
                    }
                    else if ((Y - 1 < 0 || Y - 1 >= XSize) || ((int)input[0] - 65 < 0 || (int)input[0] - 65 >= YSize))
                    {
                        // Condition for valid input that is outside of playing area
                        InputPrompt = "Location not on grid:";
                        ValidInput = false;
                        return Coords;
                    }
                    else
                    {
                        // Condition for valid input that is within playing area
                        ValidInput = true;
                        Coords[1] = (int)input[0] - 65;
                        Coords[0] = Y - 1;
                        return Coords;
                    }
                }
                void RevealZeroes(int X, int Y)
                {
                    // Reveals all 0's in a 3x3 area centred arround X,Y
                    int x, y;
                    for (int Yoffset = 0; Yoffset < 3; Yoffset++)
                    {
                        for (int Xoffset = 0; Xoffset < 3; Xoffset++)
                        {
                            x = (X - 1) + Xoffset;
                            y = (Y - 1) + Yoffset;
                            if ((-1 < x && x < XSize) && (-1 < y && y < YSize))
                            {
                                if (MineInformation[x, y] == 0)
                                {
                                    HandleZero(x, y);
                                }
                            }
                        }
                    }
                }
                void HandleZero(int X, int Y)
                {
                    // Clears all squares in a 3x3 area centred on X,Y
                    int x, y;
                    for (int Yoffset = 0; Yoffset < 3; Yoffset++)
                    {
                        for (int Xoffset = 0; Xoffset < 3; Xoffset++)
                        {
                            x = (X - 1) + Xoffset;
                            y = (Y - 1) + Yoffset;
                            if ((-1 < x && x < XSize) && (-1 < y && y < YSize))
                            {
                                UpdateVisibleGrid(x, y);
                            }
                        }
                    }
                }
                char GetSquareContents(int X, int Y) => (char)(MineInformation[X,Y] + 48); // Gets the value stored in mine information grid at X,Y as a Char
                void UpdateVisibleGrid(int X, int Y) => ShownGrid[X , Y] = (char)(MineInformation[X, Y] + 48); // Sets value of visible grid at X,Y to the Char Value of the mine information grid at X,Y
            }
            void GameLost()
            {
                // Executes when game is lost
                GameOver = true;
                Console.Clear();
                Console.WriteLine("You hit a mine!");
                // Adds location of mines to visible grid
                ShowMines();
            }
            void GameWon()
            {
                // Executes when game is won
                GameOver = true;
                Console.Clear();
                Console.WriteLine("Congratulations all mines found!");
                // Adds location of mines to visible grid
                ShowMines();
            }
            void GameEnded()
            {
                // Executes when game is manually exited
                GameOver = true;
                Console.Clear();
                Console.WriteLine("Game ended, revealing mines");
                // Adds location of mines to visible grid
                ShowMines();
            }
            void ShowMines() => Grid.PrintGrid(Grid.RevealMines(ShownGrid, MineInformation));
        }
        public static void PeterMode()
        {
            // Easter egg
            // Funny family man
            Console.WriteLine("EASTER EGG Peter mode: Press [enter] to continue");
            Console.ReadLine();
            PlayGame(Grid.GenerateMineGrid(XSize = 40, YSize = 19,MineNum = 69), Special()) ;
            char[,] Special()
            {
                // Just dont even bother looking at this dumb shit
                char[,] grid = new char[40, 19];
                int count = 0;
                string Pter = "░░░░░░░░░░░░░░░░▄▄▄███████▄▄░░░░░░░░░░░░░░░░░░░░░░░░░░▄███████████████▄░░░░░░░░░░░░░░░░░░░░░░█▀▀▀▄░░░░█████▀▀███▄░░░░░░░░░░░░░░░░░░░█░░▄░░█▄▄█░░░░▀▄▄█████░░░░░░░░░░░░░░░▄▀▀▀▄▄▀▀▀▀▀▀▄░░▀░░█▀▀▀░▀██░░░░░░░░░░░░▄▀░░░░░█░░░░░░▀▄▄▄▄█░░░░░░░▀▄░░░░░░░░░░▄▀░░░░░▄█▀▄▄▄▄░░░░░▄░░░░░░░░░█░░░░░░░░░░█░░░░░▄█▀▄▄▄▄▄▄▄▄▄▀▀░░░░░░░░░▀▄░░░░░░░░█░░░░░░▀█▄░░░░░░░░░░░░░░░░░░░░░█░░░░░░░░█░░░░░░░░▀▄░░░░░░░░░░░░░░░░░░░░█░░░░░░░░█░░░░░░░░▄█░░░░░░░░░░░░░░░░░░░░█░░░░░░░░█░░░░░░▄▀░░░░░░░░▀▄░░░░░░░░░░░░█░░░░░░░░█░░░░░░▀▄░░░▄░░░░░█░░░░░░░░░░░░█░░░░░░░░█░░░░░░░░▀▀▀▀▀█▄▄▀░░░░░░░░░░░░░█░░░░░░░░█░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▄██░░░░░░░█▄░░░░░░░░░░░░░░░░░░░░░░░░░░▄▀▀▄▀▄▄░░░░▄█▀▄░░░░░░░░░░░░░░░░░░░░░░▄▄▀░░▄▀░░░░▄░▀░░█░▀▄▄░░░░░░░░░░░░░░░▄▄▄▀░░░▄▀░░░░░▀░░░░░░▀▄░░▀▄░░░░░░░░▄▄▄▀▀░░░░▄▀▀░░░░░░░";
                for (int y = 0; y < 19; y++)
                {
                    for (int x = 0; x < 40; x++)
                    {
                        grid[x, y] = Pter[count];
                        count++;
                    }
                }
                return grid;
            }
        }
    }
    class Grid
    {
        // Class containing all functions related to a grid
        public static int[,] GenerateMineGrid(int X, int Y, int Mines)
        {
            // Generates a 2D int array of size X,Y with Mines number of spaces set to -6 and every other space set to how many -6 squares are nearby
            int[,] grid = new int[X, Y];
            PopulateMines();
            AddNumbers();
            bool ValidLocaion;
            void PopulateMines()
            {
                // Adds Mines number of mines to grid
                Random Rnd = new Random();
                ValidLocaion = false;
                for (int i = 0; i < Mines; i++)
                {
                    ValidLocaion = false;
                    int RndX;
                    int RndY;
                    while (!ValidLocaion)
                    {
                        RndX = Rnd.Next(X);
                        RndY = Rnd.Next(Y);
                        if (grid[RndX,RndY] == 0)
                        {
                            grid[RndX, RndY] = -6;
                            // the reason mines are set to -6 is due to how the mine information is shown on the visible grid
                            // when a square is revealed the grid location is set to the mine information for that location +48 to convert from an int to the UTF-8 equivelent char for that int
                            // as mines are set to -6 they get converted to UTF-8 42 which is an asterisk so mines get displayed as '*'
                            ValidLocaion = true;
                        }
                        else
                        {
                            ValidLocaion = false;
                        }
                    }
                }
            }
            void AddNumbers()
            {
                // Adds numbers showing how many mines are nearby to the grid
                // This function hurts my eyes
                for (int j = 0; j < Y; j++)
                {
                    for (int i = 0; i < X; i++)
                    {
                        if (grid[i,j] == -6)
                        {
                            for (int l = 0; l < 3; l++)
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    int CheckX = (i - 1) + k;
                                    int CheckY = (j - 1) + l;
                                    if ((CheckX >= 0 && CheckX < X) && (CheckY >= 0 && CheckY < Y)) 
                                    {
                                        if (grid[CheckX, CheckY] != -6)
                                        {
                                            // Im sorry for this abomination of if and for statements
                                            grid[CheckX, CheckY] = grid[CheckX, CheckY] + 1;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return grid;
        }
        public static char[,] GenerateShownGrid(int X, int Y)
        {
            // Creates a new 2D char grid of size X,Y with all spaces set to '█'
            char[,] grid = new char[X, Y];
            for (int y = 0; y < Y; y++)
            {
                for (int x = 0; x < X; x++)
                {
                    grid[x, y] = '█';
                }
            }
            return grid;
        }
        public static void PrintGrid(char[,] grid)
        {
            // Prints Grid to console
            PrintFirstLine();
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                PrintLine(y);
            }

            void PrintFirstLine()
            {
                // Prints the column numbers to grid
                Console.Write("    ");
                for (int x = 1; x < grid.GetLength(0) + 1; x++)
                {
                    Console.Write(x + " ");
                    if (x < 10)
                    {
                        Console.Write(" ");
                    }
                }
                Console.Write("\n");
            }
            void PrintLine(int Y)
            {
                // Prints row Y from the grid
                Console.Write(" ");
                Console.Write((char)(Y + 65) + " ");
                for (int X = 0; X < grid.GetLength(0); X++)
                {
                    Console.Write(" " + grid[X, Y] + " ");
                }
                Console.Write("\n");
            }
        }
        public static char[,] RevealMines(char[,] grid, int[,] minegrid)
        {
            // Outputs a grid that is the same as the input grid with the locations of mines revealed
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if (minegrid[x, y] == -6)
                    {
                        grid[x, y] = (char)(minegrid[x, y] + 48);
                    }
                }
            }
            return grid;
        }
    }
}
