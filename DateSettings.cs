using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateShopperWPF
{
    class DateSettings
    {
        public DateTime Start { get; } = DateTime.Today;
        public DateTime End { get; } = DateTime.Today.AddDays(1);
        public int Step { get; }

        public DateSettings(in DateTime start, in DateTime end, int parseStep = 1)
        {
            Start = start;
            End = end;
            Step = parseStep;
        }

        public static DateTime GetDateTime(string writeLine = "Введите дату в формате 'ГГГГ-ММ-ДД': ")
        {
            string input;
            bool isparsed;
            DateTime date;
            do
            {
                Console.WriteLine(writeLine);
                input = Console.ReadLine();
                isparsed = DateTime.TryParse(input, out date);
            } while (!isparsed);
            return date;
        }

        public static int GetInt(string writeLine = "Введите целое число: ")
        {
            string input;
            bool isparse;
            int step;
            do
            {
                Console.WriteLine(writeLine);
                input = Console.ReadLine();
                isparse = int.TryParse(input, out step);
            } while (!isparse);
            return step;
        }
    }
}
