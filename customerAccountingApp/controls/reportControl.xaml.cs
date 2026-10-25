using finalExam_diplom_.classes;
using finalExam_diplom_.classes.databaseTables;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace finalExam_diplom_.controls
{
    public partial class reportControl : UserControl
    {

        public ISeries[] Series { get; set; } = Array.Empty<ISeries>();
        public ISeries[] SeriesPriorities { get; set; } = Array.Empty<ISeries>();

        public reportControl()
        {
            InitializeComponent();
            LoadDiagramData();
            LoadReason();
            LoadInformationData();

            PeriodComboBox.SelectedIndex = 0;
        }

        private async void LoadDiagramData()
        {
            DiagramsClass count = new DiagramsClass();

            List<DiagramTable> table = await count.GetCountAsync(ReasonComboBox.Text, CategoryComboBox.Text);

            List<DiagramTable> tablePriorities = await count.GetCountPrioritiesAsync(ReasonComboBox.Text, CategoryComboBox.Text);

            if (table.Count == 0)
            {
                pieChartRequests.Visibility = Visibility.Hidden;
                pieChartPriorities.Visibility = Visibility.Hidden;
                ZeroRequestsTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                pieChartRequests.Visibility = Visibility.Visible;
                pieChartPriorities.Visibility = Visibility.Visible;
                ZeroRequestsTextBlock.Visibility = Visibility.Collapsed;
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
                        item.Name == "В работе" ? SKColors.Blue :
                        item.Name == "Завершено" ? SKColors.Purple :
                        item.Name == "Отменено" ? SKColors.YellowGreen :
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

        private async void LoadInformationData()
        {
            DiagramsClass countRequest = new DiagramsClass();

            List<DiagramTable> table = await countRequest.GetCountAsync(ReasonComboBox.Text, CategoryComboBox.Text);

            if (table.Count > 0)
            {
                for (int i = 0; i < table.Count; i++)
                {
                    if (table[i].Name == "Новая")
                        CountAllRequests.Text = table[i].Count.ToString();

                    if (table[i].Name == "В работе")
                        CountWorkRequests.Text = table[i].Count.ToString();

                    if (table[i].Name == "Завершено")
                        CountFinishRequests.Text = table[i].Count.ToString();

                    if (table[i].Name == "Отменено")
                        CountCancelRequests.Text = table[i].Count.ToString();
                }
            }
            else
            {
                CountAllRequests.Text = table.Count.ToString();
                CountWorkRequests.Text = table.Count.ToString();
                CountFinishRequests.Text = table.Count.ToString();
                CountCancelRequests.Text = table.Count.ToString();
            }

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
            CategoryComboBox.IsEnabled = false;
            LoadDiagramData();
            LoadInformationData();
            LoadReason();
            LoadCategory();
        }
    }
}
