using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finalExam_diplom_.classes
{
    public class StatisticsClass
    {
        public int Total { get; set; }

        // Статусы
        public int New { get; set; }
        public int SentUK { get; set; }
        public int GetAnswerFromUK { get; set; }
        public int InWork { get; set; }
        public int Finished { get; set; }

        // Приоритеты
        public int NonePriority { get; set; }
        public int LowPriority { get; set; }
        public int MiddlePriority { get; set; }
        public int HighPriority { get; set; }

        // Проценты
        public double NewPercent { get; set; }
        public double WorkPercent { get; set; }
        public double UKPercent { get; set; }
        public double FinishedPercent { get; set; }
        public double HighPriorityPercent { get; set; }
    }
}
