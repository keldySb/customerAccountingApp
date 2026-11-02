using finalExam_diplom_.classes;
using finalExam_diplom_.classes.databaseTables;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Windows;
using System.Windows.Controls;

namespace finalExam_diplom_.controls
{
    public partial class reportControl : UserControl
    {
        private int time;

        public ISeries[] Series { get; set; } = [];
        public ISeries[] SeriesPriorities { get; set; } = [];

        public reportControl()
        {
            InitializeComponent();
            LoadDiagramData();
            LoadReason();
            LoadInformationData();

            PeriodComboBox.SelectedIndex = 0;
        }

        private async void LoadReason()
        {
            DictionaryClass dictionary = new DictionaryClass();

            ReasonComboBox.ItemsSource = await dictionary.GetReason();
            ReasonComboBox.SelectedValuePath = "Id";
            ReasonComboBox.DisplayMemberPath = "Name";
        }

        private async void LoadCategory()
        {
            DictionaryClass dictionary = new DictionaryClass();

            CategoryComboBox.ItemsSource = await dictionary.GetCategory(Convert.ToInt32(ReasonComboBox.SelectedValue));
            CategoryComboBox.SelectedValuePath = "Id";
            CategoryComboBox.DisplayMemberPath = "Name";
        }

        private async void LoadDiagramData()
        {
            DiagramsClass count = new DiagramsClass();

            List<DiagramTable> table = await count.GetCountAsync(time, ReasonComboBox.Text, CategoryComboBox.Text);

            List<DiagramTable> tablePriorities = await count.GetCountPrioritiesAsync(ReasonComboBox.Text, CategoryComboBox.Text);

            if (table.Count == 0)
            {
                ZeroRequestsBorder.Visibility = Visibility.Visible;
                PrintButton.IsEnabled = false;
            }                
            else
            {
                ZeroRequestsBorder.Visibility = Visibility.Collapsed;
                PrintButton.IsEnabled = true;
            }

            PieData[] data = new PieData[table.Count];

            for (int i = 0; i < table.Count; i++)
            {
                data[i] = new PieData(table[i].Name ?? "", table[i].Count);
            }

            pieChartRequests.Series = data.Select(item =>
                new PieSeries<double>
                {
                    Name = item.Name,
                    Values = item.Values,
                    Fill = new SolidColorPaint(
                        item.Name == "Новая" ? SKColors.Orange :
                        item.Name == "Направлено в УК" ? SKColors.Blue :
                        item.Name == "Получен ответ от УК" ? SKColors.Purple :
                        item.Name == "Выполнено" ? SKColors.YellowGreen :
                        item.Name == "Заявка отменена клиентом" ? SKColors.BlueViolet :
                        SKColors.Gray),
                }).ToArray();

            PieData[] dataPriorities = new PieData[tablePriorities.Count];

            for (int i = 0; i < tablePriorities.Count; i++)
            {
                dataPriorities[i] = new PieData(tablePriorities[i].Name ?? "", tablePriorities[i].Count);
            }

            pieChartPriorities.Series = dataPriorities.Select(item =>
                new PieSeries<double>
                {
                    Name = item.Name,
                    Values = item.Values,
                    Fill = new SolidColorPaint(
                        item.Name == "Высокий" ? SKColors.Yellow :
                        item.Name == "Средний" ? SKColors.Red :
                        item.Name == "Низкий" ? SKColors.Green : 
                        SKColors.Blue),
                }).ToArray();
        }

        private StatisticsClass statistic = new StatisticsClass();

        private async void LoadInformationData()
        {
            StatisticsClass currentStatistic = new StatisticsClass();

            DiagramsClass countRequest = new DiagramsClass();

            List<DiagramTable> table = await countRequest.GetCountAsync(time, ReasonComboBox.Text, CategoryComboBox.Text);

            List<DiagramTable> tablePriorities = await countRequest.GetCountPrioritiesAsync(ReasonComboBox.Text, CategoryComboBox.Text);
            

            for (int i = 0; i < table.Count; i++)
            {
                currentStatistic.Total += table[i].Count;
                if (table[i].Name == "Новая")
                    currentStatistic.New = table[i].Count;

                if (table[i].Name == "Направлено в УК")
                    currentStatistic.SentUK = table[i].Count;

                if (table[i].Name == "Получен ответ от УК")
                    currentStatistic.GetAnswerFromUK = table[i].Count;

                if (table[i].Name == "Выполнено")
                    currentStatistic.Finished = table[i].Count;

                if (table[i].Name == "Заявка отменена клиентом")
                    currentStatistic.Cancelled = table[i].Count;
            }

            CountAllRequests.Text = currentStatistic.Total.ToString();
            CountNewRequests.Text = currentStatistic.New.ToString();
            CountWorkRequests.Text = (currentStatistic.SentUK + currentStatistic.GetAnswerFromUK).ToString();
            CountFinishRequests.Text = currentStatistic.Finished.ToString();
            CountCancelRequests.Text = currentStatistic.Cancelled.ToString();   

            for (int i = 0; i < tablePriorities.Count; i++)
            {
                if (tablePriorities[i].Name == "Низкий")
                    currentStatistic.LowPriority = tablePriorities[i].Count;

                if (tablePriorities[i].Name == "Средний")
                    currentStatistic.MiddlePriority = tablePriorities[i].Count;

                if (tablePriorities[i].Name == "Высокий")
                    currentStatistic.HighPriority = tablePriorities[i].Count;
            }

            if (statistic.Total > 0)
            {
                currentStatistic.HighPriorityPercent = currentStatistic.HighPriority * 100.0 / currentStatistic.Total;
                currentStatistic.NewPercent = currentStatistic.New * 100.0 / currentStatistic.Total;
                currentStatistic.WorkPercent = (currentStatistic.SentUK + currentStatistic.GetAnswerFromUK) * 100.0 / currentStatistic.Total;
                currentStatistic.FinishedPercent = currentStatistic.Finished * 100.0 / currentStatistic.Total;
            }

            informationTextBlock.Text = $"{PeriodComboBox.Text} в системе было зарегистрировано {currentStatistic.Total} заявок: \n\n" +
                $"1. Распределение заявок по статусам:\n" +
                $"     Новые: {currentStatistic.New} \n" +
                $"     Направлено в УК: {currentStatistic.SentUK} \n" +
                $"     Получен ответ от УК: {currentStatistic.GetAnswerFromUK} \n" +
                $"     Выполнено: {currentStatistic.Finished} \n" +
                $"     Заявка отменена клиентом: {currentStatistic.Cancelled} \n\n" +
                $"2. Распределение заявок по приоритетам:\n" +
                $"     Высокий — {currentStatistic.HighPriority}\n" +
                $"     Средний — {currentStatistic.MiddlePriority}\n" +
                $"     Низкий — {currentStatistic.LowPriority}\n\n" +
                $"3. Дополнительные показатели:\n" +
                $"     Доля новых заявок, — {Math.Round(currentStatistic.NewPercent)}%.\n" +
                $"     Доля завершённых заявок — {Math.Round(currentStatistic.FinishedPercent)}%.\n" +
                $"     Доля заявок, находящихся в работе, — {Math.Round(currentStatistic.WorkPercent)}%.\n" +
                $"     Доля заявок с высоким приоритетом — {Math.Round(currentStatistic.HighPriorityPercent)}%.";
            statistic = currentStatistic;
        }

        public class PieData
        {
            public string Name { get; set; }
            public double[] Values { get; set; }

            public PieData(string name, double value)
            {
                Name = name;
                Values = new double[] { value };
            }
        }

        private void UpdateSetting_Click(object sender, RoutedEventArgs e)
        {
            LoadDiagramData();
            LoadInformationData();
        }

        private void ReasonComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CategoryComboBox.IsEnabled = true;
            LoadCategory();
        }

        private void BackDefaultSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            PeriodComboBox.SelectedIndex = 0;
            ReasonComboBox.SelectedIndex = -1;
            CategoryComboBox.SelectedIndex = -1;
            CategoryComboBox.IsEnabled = false;
            LoadDiagramData();
            LoadInformationData();
        }

        private void PeriodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            time = PeriodComboBox.SelectedIndex;
            LoadDiagramData();
            LoadInformationData();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser user = new CurrentUser();
            DateTime currentDate = DateTime.Now;

            string templatePath = @"C:\gitProjects\customerAccountingApp\requestsPattern.docx";

            string outputPath = @"C:\Reports\Отчёт.docx";

            Dictionary<string, string> items = new Dictionary<string, string>
            {
                { "<COUNTNEW>", CountNewRequests.Text },
                { "<COUNTWORK>", CountWorkRequests.Text },
                { "<COUNTFINISH>", CountFinishRequests.Text },
                { "<COUNTCANCEL>", CountCancelRequests.Text },

                { "<FINISHPERCENT>", Math.Round(statistic.FinishedPercent).ToString() },
                { "<WORKPERCENT>", Math.Round(statistic.WorkPercent).ToString() },
                { "<NEWPERCENT>", Math.Round(statistic.NewPercent).ToString() },
                { "<CANCELPERCENT>", Math.Round(statistic.CancelledPercent).ToString() },

                { "<COUNTLOW>", statistic.LowPriority.ToString() },
                { "<COUNTMIDDLE>", statistic.MiddlePriority.ToString() },
                { "<COUNTHIGH>", statistic.HighPriority.ToString() },

                { "<CURRENTDATE>", currentDate.ToString() }
            };

            MakeWordFile wordFile = new MakeWordFile(templatePath);

            bool result = wordFile.Process(statistic, items, outputPath);

            if (result)
            {
                MessageBox.Show("Отчёт сохранён");
            }
            else
            {
                MessageBox.Show("Не удалось сохранить отчёт");
            }
        }
    }
}
