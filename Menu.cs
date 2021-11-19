using System;

namespace SharpSweeper
{
    // This is basically just a bunch of switch statements
    // I really cant be bothered to comment all of this
    // Esentially what the main, settings and difficulty menus do is ask for the user to pick an option eg start game, change difficulty and acts accordingly
    // If anyone cares that much you can mostly tell whats going on by looking at the dialoge for each option
    class Menu
    {
        private static string InvalidCommand = "Invalid Command:";
        private static int Width = 9;
        private static int Height = 9;
        private static int MineAmount = 10;
        public static void MainMenu()
        {
            // MainMenu class, houses all of the functions and methods related to the main menu
            bool ExitGame = false;
            string MenuPrompt = "Enter Command:";
            while (!ExitGame)
            {
                Console.Clear();
                ShowMenuDialogue();
                switch (Console.ReadLine().ToUpper())
                {
                    case "P":
                        Game.StartGame(Width, Height, MineAmount);
                        break;
                    case "D":
                        DifficultySettings();
                        break;
                    case "S":                     
                        SettingsMenu();
                        break;
                    case "E":
                        ExitGame = true;
                        break;
                    case "HEY LOUIS": // Nyehehe
                        Game.PeterMode();
                        break;
                    default:
                        MenuPrompt = InvalidCommand;
                        break;
                }
            }
            Console.Clear();
            Console.Write("Press [enter] to exit:");
            Console.Read();
            void ShowMenuDialogue()
            {
                Console.Write(" __           _   _  __         _  _  _   _  _ \n" +
                             @"(_  |_|  /\  |_) |_)(_  \    / |_ |_ |_) |_ |_)" + "\n" +
                             @"__) | | /--\ | \ |  __)  \/\/  |_ |_ |   |_ | \" +
                             "\n\nP: Play game\nD: Difficulty\nS: Settings\nE: Exit\n" + MenuPrompt);
            
            }
            void SettingsMenu()
            {
                // Currently does nothing, need to think of ideas to add
                Console.Clear();
                bool ExitSettings = false;
                string SettingsPrompt = "Enter Command:";
                while (!ExitSettings)
                {
                    Console.Write("Work in progress\nE: Return to main menu\n" + SettingsPrompt);
                    switch (Console.ReadLine().ToUpper())
                    {
                        case "E":
                            Console.Clear();
                            ExitSettings = true;
                            break;
                        default:
                            Console.Clear();
                            SettingsPrompt = InvalidCommand;
                            break;
                    }
                }


            }
            void DifficultySettings()
            {
                // Houses all of the methods and functions to change the difficulty
                bool ExitDiifcultySettings = false;
                string DifficultySettingsPrompt = "Enter Command:";
                while (!ExitDiifcultySettings)
                {
                    Console.Clear();
                    Console.Write($"Difficulty settings:\nCurrent size is {Width} by {Height} with {MineAmount} mines\nC: Change Difficulty\nE: Return to main menu\n{DifficultySettingsPrompt}",Width,Height,MineAmount);
                    switch (Console.ReadLine().ToUpper())
                    {
                        case "E":
                            ExitDiifcultySettings = true;
                            break;
                        case "C":
                            ChangeDifficulty();
                            break;
                        default:
                            DifficultySettingsPrompt = InvalidCommand;
                            break;
                    }
                }
                void ChangeDifficulty()
                {
                    bool ValidChoice = false;
                    Console.Clear();
                    string ChangeDifficultyPrompt = "Enter Command:";
                    while (!ValidChoice)
                    {
                        Console.Write("Change difficulty\nB: Beginner (9 by 9 with 10 mines)\nI: Intermediate (16 by 16 with 40 mines)\nE: Expert (30 by 16 with 99 mines)\nEX: Extreme (77 by 26 with 410 mines, the largest grid that displays properly at 1920 by 1080 resolution)\nMX: Max (99 by 26 with 520 mines, the largest supported game)\nC: Custom (Enter custom game menu)\n" + ChangeDifficultyPrompt);
                        switch (Console.ReadLine().ToUpper())
                        {
                            case "B":
                                ValidChoice = true;
                                Width = 9;
                                Height = 9;
                                MineAmount = 10;
                                break;
                            case "I":
                                ValidChoice = true;
                                Width = 16;
                                Height = 16;
                                MineAmount = 40;
                                break;
                            case "E":
                                ValidChoice = true;
                                Width = 30;
                                Height = 16;
                                MineAmount = 99;
                                break;
                            case "EX":
                                ValidChoice = true;
                                Width = 77;
                                Height = 26;
                                MineAmount = 410;
                                break;
                            case "MX":
                                ValidChoice = true;
                                Width = 99;
                                Height = 26;
                                MineAmount = 520;
                                break;
                            case "C":
                                ValidChoice = true;
                                CustomGameMenu();
                                break;
                            case "DEBUG":
                                ValidChoice = true;
                                Debug();
                                break;
                            default:
                                Console.Clear();
                                ChangeDifficultyPrompt = InvalidCommand;
                                break;
                        }
                    }
                    void CustomGameMenu()
                    {
                        // these three functions ask for a user input of the size/number of mines and ensures they are valid
                        Console.Clear();
                        Width = GetWidth();
                        Height = GetHeight();
                        MineAmount = GetMines();
                        int GetWidth()
                        { 
                            bool ValidSetting = false;
                            bool IsNumber = false;
                            while (!ValidSetting)
                            {
                                Console.Write("Input width (must be between 2 and 99 inclusive):");
                                IsNumber = Int32.TryParse(Console.ReadLine(), out int i);
                                if (!IsNumber)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Error: Letter entered");
                                }
                                else if (i < 2 || i > 99)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Error: Number not in valid range");
                                }
                                else
                                {
                                    Console.Clear();
                                    ValidSetting = true;
                                    return i;
                                }
                            }
                            return 9;
                        }
                        int GetHeight()
                        {
                            bool ValidSetting = false;
                            bool IsNumber = false;
                            while (!ValidSetting)
                            {
                                Console.Write("Input height (must be between 2 and 26 inclusive):");
                                IsNumber = Int32.TryParse(Console.ReadLine(), out int i);
                                if (!IsNumber)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Error: Letter entered");
                                }
                                else if (i < 2 || i > 26)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Error: Number not in valid range");
                                }
                                else
                                {
                                    Console.Clear();
                                    ValidSetting = true;
                                    return i;
                                }
                            }
                            return 9;
                        }
                        int GetMines()
                        {
                            bool ValidSetting = false;
                            bool IsNumber = false;
                            int MaxMines = (Width - 1) * (Height - 1);
                            while (!ValidSetting)
                            {
                                Console.Write($"Input number of mines (must be between 1 and {MaxMines} inclusive):");
                                IsNumber = Int32.TryParse(Console.ReadLine(), out int i);
                                if (!IsNumber)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Error: Letter entered");
                                }
                                else if (i < 1 || i > MaxMines)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Error: Number not in valid range");
                                }
                                else
                                {
                                    Console.Clear();
                                    ValidSetting = true;
                                    return i;
                                }
                            }
                            return 10;
                        }
                    }
                    void Debug()
                    {
                        Console.Clear();
                        Console.Write("EASTER EGG HIDDEN MENU:\nCreate a custom game with no limits\nWARNING: there are absolutely no restrictions so you can very easily crash the game\n\n(notes: Values equal to or less than 0 will crash the game\n Same with numbers above the signed 32 bit storage limit (2,147,483,647)\n Width values above 99 will cease to display column numbers correctly\n Height values above 61 will break due to the row letters being set to UTF-8 control characters\n Additionally height numbers above 32 are impossible due to location inputs getting autocapitalised\n Mine values higher than the height x width of the board will fail to load due to lack of avalable spaces for mines)\n\n");
                        Console.Write("Input Width:");
                        Width = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Input height:");
                        Height = Convert.ToInt32(Console.ReadLine());
                        Console.Write($"Input Number of mines: (Grid size is {Width * Height}):");
                        MineAmount = Convert.ToInt32(Console.ReadLine());
                    }
                }
            }

        }
    }
}