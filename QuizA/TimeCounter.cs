using InputSimulatorStandard;
using InputSimulatorStandard.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace QuizA
{
    public class TimeCounter
    {
        static public Timer MyTimer;
        static public int MilSec { get; set; }
        static public int Sec { get; set; }
        static public int Min { get; set; }
        static public bool IsActive { get; set; } = false;
        static public int QuestionLength { get; set; } = 20;
        static public int QuestLen { get; set; }
        static public bool Tracker { get; set; } = false;
        static public int AnswerDuration { get; set; } = 0;



        static public void PressEnterKey()
        {
            var simulator = new InputSimulator();
            simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            //Console.WriteLine("\n Enter key called!!");
            //MyTimer.Interval = 10;
            //MyTimer.Start();
        }


        //public static int QuestionDurations(int nOfQuest)
        //{
        //    int questLen;

        //    if (nOfQuest <= 10)
        //    {

        //        questLen = 8;
        //    }
        //    else if (11 < nOfQuest && nOfQuest <= 20)
        //    {
        //        questLen = 15;
        //    }
        //    else if (21 < nOfQuest && nOfQuest <= 50)
        //    {
        //        questLen = 20;
        //    }
        //    else
        //    {
        //        questLen = 25;
        //    }

        //    return questLen + 10;
        //}


        public static int TotalQuizTime(int quizCount = 0)
        {

            int questLen;

            if (quizCount <= 10)
            {

                questLen = 4;
            }
            else if (11 < quizCount && quizCount <= 15)
            {
                questLen = 7;
            }

            else if (15 < quizCount && quizCount <= 20)
            {
                questLen = 10;
            }
            else if (21 < quizCount && quizCount <= 50)
            {
                questLen = 12;
            }
            else
            {
                questLen = 15;
            }

            return (questLen) * quizCount;
        }


        public static void stopTimer()
        {
            //\n //TimeCounter.QuestLen * Quizes.Count
            if (Sec >= QuestLen)
            {
                Tracker = !Tracker;


                Console.ForegroundColor = ConsoleColor.DarkRed;

                Console.WriteLine($"\n \t\t\t\t Quiz has Ended. Time spent is: {QuestLen} seconds!");

                Console.Write("\n \t\t\t\t Time UP, NOTE: Further responses provided will be ignored and not recorded!!");

                Console.WriteLine($"\n \t\t\t\t You can complete the quiz but the questions after this prompt will not be considered as valid inputs. " +
                    $"It just enables you to see the questions and their options");

                Console.ResetColor();

                IsActive = !IsActive;

                Console.CursorLeft = QuestionLength + 38;

                MyTimer.Stop();


                PressEnterKey();
            }
        }


        public static void haltTime()
        {
            //\n //TimeCounter.QuestLen * Quizes.Count
            Tracker = !Tracker;

            Console.WriteLine($" Timer is stopped because time is up!");

            Console.Write(" Time UP!");

            IsActive = !IsActive;

            Console.CursorLeft = QuestionLength + 38;

            MyTimer.Stop();

        }

        public static void resetTimer()
        {
            MilSec = 0;
            Sec = 0;
            Min = 0;
            IsActive = false;


        }



        public static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!IsActive)
            {
                MilSec++; //since the default value is 10milseconds, it means when milSec = 100, then 1 second has passed
                if (MilSec >= 100)
                {
                    Sec++;  //if milSec >= 100, then it means it is 1 second already and should reset milSec back to 0
                    MilSec = 0;

                    if (Sec >= 60) //This means 1 min has passed, hence reset sec = 0
                    {
                        Min++;
                        Sec = 0;

                    }
                }
                string timeDisplay = String.Format("{0:00}", MilSec) + " millisecs" + " : " + String.Format("{0:00}", Sec) + " Secs " + " : " + String.Format("{0:00}", Min) + " Min ";  //) + " : " + String.Format("{0:00}", Min
                Console.CursorLeft = QuestionLength + 45;
                Console.Write("Time: {0}  :", timeDisplay);
                stopTimer();

            }
        }








    }


}