using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_FieldOfMiracles
{
    static internal class SystemInput
    {
        #region System Input Methods
        public static int InputIntInBorder(string message, int min, int max)
        {
            bool isMistakes;
            int result;

            do
            {
                Console.WriteLine(message);
                isMistakes = !int.TryParse(Console.ReadLine(), out result);
                if (!isMistakes)
                {
                    if (result > max || result < min)
                    {
                        isMistakes = true;
                        Console.WriteLine("Введенное число содержит ошибку! Введен неверный номер вопроса.");
                    }
                }
                else
                {
                    Console.WriteLine("Введенное число содержит ошибку!");
                }
            } while (isMistakes);
            return result;
        }

        public static int InputChar(string message)
        {
            bool isMistakes;
            char result;

            do
            {
                Console.WriteLine(message);
                isMistakes = !char.TryParse(Console.ReadKey().KeyChar.ToString(), out result);
            } while (isMistakes);
            return result;
        }

        public static int InputInt(string message)
        {
            bool isMistakes;
            int result;

            do
            {
                Console.WriteLine(message);
                isMistakes = !int.TryParse(Console.ReadLine(), out result);
            } while (isMistakes);
            return result;
        }

        public static ConsoleKey InputButton(string message)
        {
            ConsoleKey result;
            Console.WriteLine(message);
            result = Console.ReadKey().Key;
            return result;
        }

        public static string InputString(string message)
        {
            bool isMistakes;
            string result;

            do
            {
                isMistakes = false;
                Console.WriteLine(message);
                result = Console.ReadLine();

                if (result == "" || result == " ")
                {
                    isMistakes = true;
                }
            } while (isMistakes);
            return result;
        }

        public static bool InputBool(string message)
        {
            bool isMistakes;
            bool result;

            do
            {
                Console.WriteLine(message);
                isMistakes = !bool.TryParse(Console.ReadLine(), out result);
            } while (isMistakes);
            return result;
        }
        #endregion
        #region System Comparison Methods
        public static bool isBetween(int num, int min, int max)
        {
            if (num >= min && num <= max)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region System Console Buffer
        public static string AddWordWrapForString(string input, int bufferWidth)
        {
            string newString = "";
            string[] delInput = input.Split(' ');

            string parString = "";
            for (int i = 0; i < delInput.Length; i++)
            {
                if (parString != "")
                {
                    parString += " ";
                }
                parString += delInput[i];
                
                if (parString.Length > bufferWidth)
                {
                    newString += "\n";
                    parString = delInput[i];
                }

                if (parString != delInput[i])
                {
                    newString += " ";
                }
                newString += delInput[i];
            }

            return newString;
        }

        public static string AddWordWrapForString(string input, int bufferWidth, int indent)
        {
            string newString = "";
            string[] delInput = input.Split(' ');

            string parString = "";
            for (int i = 0; i < delInput.Length; i++)
            {
                if (parString != "")
                {
                    parString += " ";
                }
                parString += delInput[i];

                if (parString.Length > bufferWidth)
                {
                    newString += "\n" + new String(' ', indent);
                    parString = new String(' ', indent) + delInput[i];
                }

                if (parString != delInput[i])
                {
                    newString += " ";
                }
                newString += delInput[i];
            }

            return newString;
        }
        #endregion
        #region Text Filter
        public static string GetNormalTextSugestion(string input)
        {
            string newString = "";
            string[] proposals = input.ToLower().Split('.', '!', '?');

            if (proposals[proposals.Length - 1] == "")
            {
                string[] prp = new string[proposals.Length];

                for (int i = 0; i < prp.Length; i++)
                {
                    prp[i] = proposals[i];
                }

                proposals = new string[proposals.Length-1];

                for (int i = 0; i < proposals.Length; i++)
                {
                    proposals[i] = prp[i];
                }
            }

            string marks = input.ToLower();

            for (int i = 0; i < proposals.Length; i++)
            {
                marks = marks.Replace(proposals[i], "");
            }

            for (int i = 0; i < proposals.Length; i++)
            {
                proposals[i] = proposals[i][0].ToString().ToUpper()[0] + proposals[i].Substring(1);

                if (marks.Length != 0)
                {
                    proposals[i] += marks[i];
                }
                newString += proposals[i];
            }
            
            return newString;
        }
        #endregion
    }
}
