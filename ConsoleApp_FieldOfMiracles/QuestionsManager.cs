using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp_FieldOfMiracles
{
    public class QuestionsManager
    {
        public const string infoFile = "GameData.txt";
        struct Question
        {
            public string info;
            public string answer;
        }

        #region Load Question
        static public int LoadAnswersCount()
        {
            StreamReader reader = new StreamReader(infoFile);
            int length = int.Parse(reader.ReadLine());

            reader.Close();

            return length;
        }

        static public string LoadAnswer(int num)
        {
            Question[] questions = ReadAllQuestions(infoFile);

            if (num < questions.Length)
            {
                return questions[num].answer;
            }
            else
            {
                return "number is too big";
            }
        }

        static public string LoadQuestionInfo(int num)
        {
            Question[] questions = ReadAllQuestions(infoFile);

            if (num < questions.Length)
            {
                return questions[num].info;
            }
            else
            {
                return "number is too big";
            }
        }
        #endregion
        #region File Methods
        static void WriteAllQuestions(string filename, Question[] questions)
        {
            StreamWriter writer = new StreamWriter(filename);
            writer.WriteLine(questions.Length);

            for (int i = 0; i < questions.Length; i++)
            {
                writer.WriteLine(questions[i].info + "~" + questions[i].answer);
            }

            writer.Close();
        }

        static Question[] ReadAllQuestions(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            int length = int.Parse(reader.ReadLine());
            Question[] questions = new Question[length];

            for (int i = 0; i < length; i++)
            {
                string streamLine = reader.ReadLine();
                questions[i].info = streamLine.Split('~')[0];
                questions[i].answer = streamLine.Split('~')[1];
            }

            reader.Close();
            return questions;
        }
        #endregion
        #region Massive Methods
        static void ShowManyQuestions(Question[] questions)
        {
            if (questions.Length != 0)
            {
                for (int i = 0; i < questions.Length; i++)
                {
                    Console.Write(i + ". ");
                    Console.WriteLine(questions[i].info);
                    Console.WriteLine("-" + questions[i].answer);
                    Console.WriteLine("---");
                }
            }
            else
            {
                Console.WriteLine("No questions were founded");
            }
        }

        static Question[] AddNewQuestion(Question[] questions, Question question)
        {
            Question[] newQuestions = new Question[questions.Length + 1];

            for (int i = 0; i < questions.Length; i++)
            {
                newQuestions[i] = questions[i];
            }
            newQuestions[newQuestions.Length-1] = question;

            return newQuestions;
        }

        static Question[] EditQuestionByNum(Question[] questions, int num)
        {
            if (questions.Length != 0 && questions.Length > num)
            {
                Question[] newQuestions = new Question[questions.Length];

                for (int i = 0; i < newQuestions.Length; i++)
                {
                    if (i != num)
                    {
                        newQuestions[i].info = questions[i].info;
                        newQuestions[i].answer = questions[i].answer;
                    }
                    else
                    {
                        newQuestions[i].info = SystemInput.InputString("Введите новое содержание вопроса: ");
                        newQuestions[i].answer = SystemInput.InputString("Введите новое содержание ответа: ");
                    }
                }
                
                return newQuestions;
            }
            else
            {
                Console.WriteLine("No questions were founded");
                return questions;
            }
        }

        static Question[] DeleteQuestionByNum(Question[] questions, int num)
        {
            if (questions.Length != 0 && questions.Length > num)
            {
                if (questions.Length == 1)
                {
                    Console.WriteLine("Удаление не произошло. Нельзя удалить единственный вопрос!");
                    return questions;
                }
                Question[] newQuestions = new Question[questions.Length - 1];

                int g = 0;
                for (int i = 0; i < questions.Length; i++)
                {
                    if (i != num)
                    {
                        newQuestions[g] = questions[i];
                        g++;
                    }
                }

                return newQuestions;
            }
            else
            {
                Console.WriteLine("No questions were founded");
                return questions;
            }
        }
        #endregion
        #region User Methods
        static Question ReadNewQuestion()
        {
            Question question = new Question();

            question.info = SystemInput.InputString("Введите вопрос: ");
            question.answer = SystemInput.InputString("Введите ответ: ");

            return question;
        }

        static void ClearAllQuestions(string infoFile)
        {
            if (File.Exists(infoFile))
            {
                File.Delete(infoFile);
                Console.WriteLine("Данные были стерты");
            }
            else
            {
                Console.WriteLine("Данные не были стерты, так как не были сохранены");
            }
        }
        #endregion
        #region Interface Methods
        static void ShowMenu()
        {
            Console.WriteLine("1. Добавить вопрос");
            Console.WriteLine("2. Редактировать вопрос");
            Console.WriteLine("3. Удалить вопрос");
            Console.WriteLine("9. Стереть все данные");
            Console.WriteLine("0. Вернуться в главное меню");
        }
        #endregion

        static public void MainQuestions(string[] args)
        {
            Console.Clear();
            Question[] questions;
            
            bool isRunning = true;
            do
            {
                Console.WriteLine("Менеджер вопросов\n");
                if (File.Exists(infoFile))
                {
                    questions = ReadAllQuestions(infoFile);
                    ShowManyQuestions(questions);

                    ShowMenu();
                    int action = SystemInput.InputInt("");
                    switch (action)
                    {
                        case 1:
                            {
                                Question newQuestion = ReadNewQuestion();
                                questions = AddNewQuestion(questions, newQuestion);
                                WriteAllQuestions(infoFile, questions);
                                break;
                            }
                        case 2:
                            {
                                int num = SystemInput.InputIntInBorder("Введите номер вопроса", 0, questions.Length-1);
                                questions = EditQuestionByNum(questions, num);
                                WriteAllQuestions(infoFile, questions);
                                break;
                            }
                        case 3:
                            {
                                int num = SystemInput.InputIntInBorder("Введите номер вопроса", 0, questions.Length-1);
                                questions = DeleteQuestionByNum(questions, num);
                                WriteAllQuestions(infoFile, questions);
                                break;
                            }
                        case 9:
                            {
                                ClearAllQuestions(infoFile);
                                break;
                            }
                        case 0:
                            {
                                Console.WriteLine("Программа завершает свою работу");
                                isRunning = false;
                                Program.MainProgram(args);
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Нет пункта меню с номером " + action);
                                break;
                            }
                    }
                }
                else
                {
                    Console.WriteLine("No questions found, so please enter the first question.");
                    questions = new Question[1];
                    questions[0] = ReadNewQuestion();
                    WriteAllQuestions(infoFile, questions);
                }

                Console.WriteLine("");
                Console.WriteLine("Press any button to continue");
                Console.ReadKey();
                Console.Clear();
            } while (isRunning);
        }
    }
}
