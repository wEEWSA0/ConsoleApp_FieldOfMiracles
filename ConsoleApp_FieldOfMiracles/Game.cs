using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_FieldOfMiracles
{
    public class Game
    {
        static Random random = new Random();
        #region Structs
        struct Drum
        {
            public int position;
            public string[] steps;

            public string GetStep(int pos)
            {
                while (pos >= steps.Length)
                {
                    pos -= steps.Length;
                }
                return steps[pos];
            }

            public void NextPosition()
            {
                position++;
                if (position >= steps.Length)
                {
                    position = 0;
                }
            }
        }

        struct Player
        {
            public string name;
            public int score;
        }

        struct Word
        {
            public string word;
            public char[] letters;
            public string fullWord;
        }
        #endregion
        #region Initialize structs
        static Player[] CreateRandomPlayers(int playersCount)
        {
            Player[] players = new Player[playersCount];

            for (int i = 0; i < playersCount; i++)
            {
                players[i] = CreateRandomPlayer();
            }

            return players;
        }

        static Player CreateRandomPlayer()
        {
            Player player = new Player();

            player.name = "" + Convert.ToChar(random.Next('А', 'Я'));
            for (int i = 0; i < random.Next(3, 7); i++)
            {
                char randChar = Convert.ToChar(random.Next('а', 'я'));
                player.name += randChar;
            }
            return player;
        }

        static Word CreateWord(int num)
        {
            Word newWord = new Word();

            newWord.fullWord = QuestionsManager.LoadAnswer(num);
            int count = newWord.fullWord.ToCharArray().Length;
            newWord.word = new String('#', count);

            string uniqueChars = newWord.fullWord.ToLower();

            for (int i = 1; i < uniqueChars.Length;)
            {
                if (uniqueChars.Split(uniqueChars[i]).Length == 2)
                {
                    i++;
                }
                else
                {
                    uniqueChars = uniqueChars.Remove(i, 1);
                }
            }

            if (uniqueChars.Replace(uniqueChars[0].ToString(), "").Length != uniqueChars.Length - 1)
            {
                uniqueChars = uniqueChars.Remove(0, 1);
            }

            newWord.letters = uniqueChars.ToCharArray();
            return newWord;
        }

        #endregion
        #region Interface Methods
        static void ShowScoreMenu(int round, Player[] players)
        {
            Console.WriteLine($"         Раунд {round}\n");
            Console.WriteLine("Игроки:" + "     Текущий счет:");
            
            for (int i = 0; i < players.Length; i++)
            {
                Console.WriteLine("{0, -12}{1, -6}", players[i].name, players[i].score);
            }

            Console.WriteLine("\nЛюбая клавиша для старта раунда");
        }

        static void ShowDrum(Drum drum)
        {
            Console.WriteLine(new String(' ', 25) + drum.GetStep(drum.position));
            Console.WriteLine(new String(' ', 20) + drum.GetStep(drum.position + 1) + new string(' ', 3) + "^" + new string(' ', 3) + drum.GetStep(drum.position + 7));
            Console.WriteLine();
            Console.WriteLine(new String(' ', 18) + drum.GetStep(drum.position + 2) + new string(' ', 11) + drum.GetStep(drum.position + 6));
            Console.WriteLine();
            Console.WriteLine(new String(' ', 20) + drum.GetStep(drum.position + 3) + new string(' ', 7) + drum.GetStep(drum.position + 5));
            Console.WriteLine(new String(' ', 25) + drum.GetStep(drum.position + 4));
        }

        static void ShowGameBar(Word word, string question)
        {
            Console.Clear();
            Console.WriteLine("\n" + new String(' ', 21) + "Поле чудес\n");
            Console.WriteLine(new String(' ', 5) + SystemInput.AddWordWrapForString(question, Console.BufferWidth-10, 5) + "\n\n");
        }
        #endregion
        async Task<Drum> RotatingDrum(int power, float drag, int stopSpeed, Drum drum, Word word, string question)
        {
            int startPower = (int)(random.Next(118, 128) / 100f * power);
            do
            {
                drum.NextPosition();
                ShowGameBar(word, question);
                ShowDrum(drum);
                await Task.Delay((int)(startPower - power)/3);
                power = (int)(power * (1 - drag));
            } while (power > stopSpeed);

            return drum;
        }

        static Word GetNewScoreboardValue(Word word, string inputStr, int letterNum)
        {
            #region Letters ReCreate
            int offset = 0;
            char[] oldLetters = new char[word.letters.Length];
            for (int g = 0; g < word.letters.Length; g++)
            {
                oldLetters[g] = word.letters[g];
            }
            word.letters = new char[oldLetters.Length - 1];
            for (int h = 0; h < word.letters.Length; h++)
            {
                if (h == letterNum)
                {
                    offset++;
                }
                word.letters[h] = oldLetters[h + offset];
            }
            #endregion

            for (int j = 0; j < word.fullWord.Length; j++)
            {
                if (inputStr == word.fullWord[j].ToString().ToLower())
                {
                    word.word = word.word.Substring(0, j) + inputStr + word.word.Substring(j + 1, word.word.Length - j - 1);
                }
            }

            return word;
        }

        static int IsWonOnCurrentMotion(string inputStr, ref Word word)
        {
            int isWon = 0;
            if (inputStr.Length != 1)
            {
                inputStr = SystemInput.GetNormalTextSugestion(inputStr);

                if (word.fullWord == inputStr)
                {
                    Console.WriteLine("Вы правильно отгадали! Правильным ответом было слово " + word.fullWord);
                    isWon = 1;
                }
                else
                {
                    Console.WriteLine("Вы не отгадали! К сожалению, игра окончена. Правильным ответом было слово " + word.fullWord);
                    isWon = -1;
                }
            }
            else
            {
                inputStr = inputStr.ToLower();

                bool isGuessTrue = false;
                for (int i = 0; i < word.letters.Length; i++)
                {
                    if (inputStr == word.letters[i].ToString().ToLower())
                    {
                        Console.WriteLine("Вы отгадали букву!");
                        isGuessTrue = true;

                        word = GetNewScoreboardValue(word, inputStr, i);

                        if (word.letters.Length == 0)
                        {
                            Console.WriteLine("Вы отгадали слово целиком! Правильным ответом было слово " + word.fullWord);
                            isWon = 1;
                        }
                        else
                        {
                            Console.WriteLine("Слово: " + word.word);
                            isWon = 0;
                        }
                    }
                }

                if (!isGuessTrue)
                {
                    Console.WriteLine("Вы не отгадали букву!");
                    Console.WriteLine("Слово: " + word.word);
                }

            }
            return isWon;
        }

        async Task Round(Drum drum, Word word, string question)
        {
            int roundCount = 0;
            int points = 0;

            while (true) {
                roundCount++;
                ShowGameBar(word, question);
                #region Drum Spinning
                ShowDrum(drum);
                Console.WriteLine(SystemInput.InputButton("\n" + new String(' ', 19) + "Крутить барабан"));

                drum = await RotatingDrum(random.Next(3000, 3400), random.Next(150, 220) / 1000f, random.Next(300, 450), drum, word, question);

                points += int.Parse(drum.GetStep(drum.position));
                Console.WriteLine("\n" + new String(' ', 15) + "Вы получаете " + drum.GetStep(drum.position) + " баллов");
                #endregion
                string inputStr = SystemInput.InputString(new String(' ', 12) + "Введите букву или слово целиком");
                int isWon = IsWonOnCurrentMotion(inputStr, ref word);

                Console.ReadKey();

                if (isWon == 0)
                {
                    ShowGameBar(word, question);
                    Console.WriteLine("\n" + new String(' ', 21) + "Ход Якубовича\n");

                    await Task.Delay(random.Next(1500, 3000));

                    if (random.Next(0, 100) < 20 * (roundCount - 1))
                    {
                        Console.WriteLine("Якубович решает сказать слово целиком!");
                        await Task.Delay(random.Next(500, 1000));
                        Console.WriteLine(word.fullWord);
                        await Task.Delay(random.Next(400, 700));
                        Console.WriteLine("Это правильный ответ! Якубович побеждает, вы проигрываете!");
                        break;
                    }
                    else
                    {
                        if (random.Next(0, 100) < 40 + (roundCount * 10))
                        {
                            Console.WriteLine("Якубович решает сказать букву");

                            await Task.Delay(random.Next(500, 1000));

                            string randTrueLetter = "" + word.letters[random.Next(0, word.letters.Length - 1)];

                            randTrueLetter = randTrueLetter.ToLower();

                            for (int i = 0; i < word.letters.Length; i++)
                            {
                                if (randTrueLetter == word.letters[i].ToString().ToLower())
                                {
                                    word = GetNewScoreboardValue(word, randTrueLetter, i);
                                }
                            }
                            
                            Console.WriteLine(randTrueLetter);
                            await Task.Delay(random.Next(400, 700));
                            Console.WriteLine("В слове есть такая буква! Слово: " + word.word);

                            if (word.letters.Length == 0)
                            {
                                Console.WriteLine("Слово полностью раскрыто! Якубович побеждает, вы проигрываете!");
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Якубович решает сказать букву");
                            await Task.Delay(random.Next(500, 1000));
                            char randFalseLetter = (char)random.Next('а', 'я');

                            while (true)
                            {
                                bool isFalseLetterNum = true;
                                for (int i = 0; i < word.letters.Length; i++) {
                                    if (randFalseLetter == word.letters[i])
                                    {
                                        isFalseLetterNum = false;
                                    }
                                }

                                if (isFalseLetterNum)
                                {
                                    break;
                                }
                                else
                                {
                                    randFalseLetter = (char)random.Next('а', 'я');
                                }
                            }

                            Console.WriteLine(randFalseLetter);
                            await Task.Delay(random.Next(400, 700));
                            Console.WriteLine("В слове нет такой буквы! Слово: " + word.word);
                        }
                    }
                    Console.WriteLine("Игра продолжается!");
                    Console.ReadKey();
                }
                else
                {
                    if (isWon == 1)
                    {
                        Console.WriteLine("\nЯкубович повержен!");
                        Console.Write("\nВы заканчиваете игру со счетом " + points + "\n");
                    }
                    else
                    {
                        Console.WriteLine("\nЯкубович побеждает, вы проигрываете!");
                        Console.Write("\nВаш счет аннулируется (" + points + " -> 0)\n");
                    }
                    break;
                }
            }
        }

        async public Task MainGame(string[] args)
        {
            Console.Clear();
            Console.WriteLine("       Поле чудес\n");

            Player[] players = CreateRandomPlayers(3);
            Drum drum = new Drum();

            int questionNum = random.Next(0, QuestionsManager.LoadAnswersCount());
            string infoQuestion = QuestionsManager.LoadQuestionInfo(questionNum);
            Word word = CreateWord(questionNum);

            drum.steps = new string[] { " 0 ", "300", "600", "400", "200", "500", "100", "800" };
            int round = 1;

            Round(drum, word, infoQuestion).Wait();
            
            Console.WriteLine("\nПосле нажатия любой клавиши, приложение закроется");
            Console.ReadKey();
        }
    }
}
