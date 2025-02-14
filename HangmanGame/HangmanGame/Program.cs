using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

namespace HangmanGame
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Words to guess
            string[] wordList =
            {
            "Heizoel","Donau", "Lokomotive", "Recycling", "Weihnachtsmann","Gymnastik", "Rhythmus", "Metapher ", "Einfaltspinsel", "Kernspintomografie",
            "Jackett", "Bredouille", "Zucchini", "Portemonnaie", "Haftpflichtversicherung", "Hollywood", "Fussballweltmeisterschaft", "Wasserverschmutzung",
            "Zukunftsmusik", "Zwiebelsuppe", "Zebra", "Yeti", "Babypuppe", "Quizshow", "Finanzdienstleistungsunternehmen", "Opernhaus", "Wrestling", "Hund",
            "Katze", "Meerschweinchen", "Galgenraten", "Autobahn", "Eichhoernchen", "Chemie", "Biologie", "Auto", "Maschendrahtzaun", "Dumpfbacke", "Terrasse",
            "Quarzuhr", "Lebenswandel", "Schatzi", "Burgverlies", "Salzgrotte", "Intelligenzquotient", "Kopfkino", "Umweltschutzorganisation", "Voodoopuppe"
        };

            // Generate a random word from the wordList with the length of the respective word
            Random randomWord = new Random();
            int randomWordIndex = randomWord.Next(wordList.Length);
            string wordToGuess = wordList[randomWordIndex].ToLower();

            // Create a char array with the length of the word to guess
            char[] word = new char[wordToGuess.Length];

            // A char array to store the already guessed letters. The counter is for increasing the index of the attempts
            char[] guessedLetter = new char[30];
            int letterCount = 0;
            int lifesLeft = 10;

            // Initialize each position in the word with "_"
            for (int i = 0; i < wordToGuess.Length; i++)
            {
                word[i] = '_';
            }
            Console.WriteLine($"The word to guess has {word.Length} letters");

            // Game loop
            while (lifesLeft > 0)
            {
                // If the word is guessed correctly and the player wins
                if (wordToGuess == new string(word))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Great! The word is {char.ToUpper(wordToGuess[0])}{wordToGuess.Substring(1)}");
                    Console.ResetColor();
                    break;
                }
                else
                {
                    // Input a letter or "1" to solve the word
                    char guess = LetterGuessInput();

                    if (guess == '1')
                    {
                        lifesLeft = SolveWord(lifesLeft, wordToGuess);
                        if (lifesLeft == 0)
                        {
                            break;
                        }
                    }
                    else
                    {
                        // Save the guessed letters into the array
                        guessedLetter[letterCount] = guess;
                        letterCount++;

                        // Deduct a life if an incorrect letter was guessed
                        if (!wordToGuess.Contains(guess))
                        {
                            lifesLeft--;
                        }
                        else
                        {
                            // If the letter is correct, update the word array by replacing "_" with the guessed letter
                            for (int i = 0; i < wordToGuess.Length; i++)
                            {
                                if (wordToGuess[i] == guess)
                                {
                                    word[i] = guess;
                                }
                            }
                        }
                    }

                    Console.WriteLine();
                    DrawLifesLeft(lifesLeft);
                    GuessedLetterColor(guessedLetter, wordToGuess);
                    Console.WriteLine($"Word to guess: {new string(word)}");
                    DrawHangman(lifesLeft);
                    DrawLoosingText(lifesLeft);
                    Console.WriteLine();

                    if (wordToGuess != new string(word) && lifesLeft == 0)
                    {
                        Console.WriteLine($"Sorry, you couldn't guess the word after 10 attempts. The word was {char.ToUpper(wordToGuess[0])}{wordToGuess.Substring(1)}");
                    }
                }
            }
        }

        // Method to input a letter or the word to solve the puzzle
        private static char LetterGuessInput()
        {
            Console.WriteLine("Please enter a letter");
            Console.WriteLine("To solve the word, type '1'");
            Console.WriteLine();
            return Console.ReadKey().KeyChar;
        }

        // Method to guess the entire word and win the game
        private static int SolveWord(int lifesLeft, string wordToGuess)
        {
            Console.WriteLine();
            Console.WriteLine("You want to solve. If yes, type the word. If no, type 'n'.");
            string answer = Console.ReadLine().ToLower();

            if (answer == "n")
            {
                // If the player decided not to solve the word, they are returned to the game
            }
            else
            {
                if (answer == wordToGuess)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Great! The word is: {char.ToUpper(wordToGuess[0])}{wordToGuess.Substring(1)}");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Sorry, that is incorrect. The word was: {char.ToUpper(wordToGuess[0])}{wordToGuess.Substring(1)}");
                    Console.ResetColor();
                }
                lifesLeft = 0;
            }
            return lifesLeft;
        }

        // Method to display guessed letters in green if correct and red if incorrect
        private static void GuessedLetterColor(char[] guessedLetters, string wordToGuess)
        {
            Console.Write("Already guessed letters: ");
            foreach (char letter in guessedLetters)
            {
                if (wordToGuess.Contains(letter))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.Write(letter.ToString().ToUpper());

                Console.ResetColor();
            }
            Console.WriteLine(); // New line
        }

        // Method to draw the hangman based on remaining lives
        private static void DrawHangman(int lifesLeft)
        {
            for (int i = 1; i <= 15; i++)
            {
                for (int j = 1; j <= 28; j++)
                {
                    // Border of the playing field
                    bool border = (i == 1 || i == 15 || j == 1 || j == 28);
                    if (border)
                    {
                        Console.Write(".");
                        continue;
                    }

                    // Hangman body parts based on remaining lives
                    bool hasPole = lifesLeft <= 9 && (i >= 3 && i <= 14) && j == 24;
                    bool hasBase = lifesLeft <= 9 && i == 14 && (j >= 21 && j <= 27);
                    bool hasTop = lifesLeft <= 8 && i == 2 && (j >= 15 && j <= 24);
                    bool hasPillar = lifesLeft <= 7 && i == 3 && (j >= 23 && j <= 24);
                    bool hasRope = lifesLeft <= 6 && (i >= 3 && i <= 4) && j == 15;

                    bool hasHead = lifesLeft <= 5 &&
                                  ((i == 5 && j == 15) || (i == 7 && j == 15) || // Head top/bottom
                                   (i == 7 && j == 14) || (i == 6 && j == 16) || // Head side "\"
                                   (i == 6 && j == 14) || (i == 7 && j == 16));  // Head side "/"

                    bool hasBody = lifesLeft <= 4 && (i >= 8 && i <= 10) && j == 15;
                    bool hasRightLeg = lifesLeft <= 3 && ((i == 11 && j == 16) || (i == 12 && j == 17));
                    bool hasLeftLeg = lifesLeft <= 2 && ((i == 11 && j == 14) || (i == 12 && j == 13));
                    bool hasRightArm = lifesLeft <= 1 && ((i == 8 && j == 16) || (i == 9 && j == 17));
                    bool hasLeftArm = lifesLeft <= 0 && ((i == 8 && j == 14) || (i == 9 && j == 13));

                    if (hasPole) Console.Write("|");
                    else if (hasBase) Console.Write("_");
                    else if (hasTop) Console.Write("_");
                    else if (hasRope) Console.Write("|");
                    else if (hasPillar) Console.Write("\\");
                    else if (hasHead) Console.Write("_");
                    else if (hasBody) Console.Write("|");
                    else if (hasRightLeg) Console.Write("\\");
                    else if (hasLeftLeg) Console.Write("/");
                    else if (hasRightArm) Console.Write("\\");
                    else if (hasLeftArm) Console.Write("/");
                    else Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        private static void DrawLoosingText(int lifesLeft)
        {
            for (int i = 1; i <= 15; i++)
            {
                for (int j = 1; j <= 28; j++)
                {
                    // Text "YOU" appears (columns 5 to 7)
                    if (lifesLeft <= 0 && i == 1 && (j == 5 || j == 6 || j == 7))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(j == 5 ? "Y" :
                                      j == 6 ? "O" : "U");
                        Console.ResetColor();
                    }
                    // Text "ARE DEAD" appears directly after "YOU" (columns 8 to 15)
                    else if (lifesLeft <= 0 && i == 2 && (j == 8 || j == 9 || j == 10 || j == 11 || j == 12 || j == 13 || j == 14 || j == 15))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(
                            j == 8 ? "A" :
                            j == 9 ? "R" :
                            j == 10 ? "E " :
                            j == 11 ? "D" :
                            j == 12 ? "E" :
                            j == 13 ? "A" :
                            j == 14 ? "D" : "!"
                        );
                        Console.ResetColor();
                    }
                }
                Console.Write(" ");
            }
        }

        // Method to display the remaining lives in different colors
        private static void DrawLifesLeft(int lifesLeft)
        {
            if (lifesLeft >= 9)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Lifes Left:{lifesLeft}");
                Console.ResetColor();
            }
            else if (lifesLeft >= 6)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"Lifes Left:{lifesLeft}");
                Console.ResetColor();
            }
            else if (lifesLeft >= 4)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"Lifes Left:{lifesLeft}");
                Console.ResetColor();
            }
            else if (lifesLeft >= 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Lifes Left:{lifesLeft}");
                Console.ResetColor();
            }
            else if (lifesLeft >= 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Lifes Left:{lifesLeft}");
                Console.ResetColor();
            }
        }
    }
}