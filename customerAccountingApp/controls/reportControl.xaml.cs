using finalExam_diplom_.classes;
using finalExam_diplom_.classes.databaseTables;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WPF;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace finalExam_diplom_.controls
{
    public partial class reportControl : UserControl
    {

        public ISeries[] Series { get; set; } = Array.Empty<ISeries>();

        public reportControl()
        {
            InitializeComponent();
            DataContext = this;
            LoadDiagramData();
            LoadComboBoxData();

            PeriodComboBox.SelectedIndex = 0;
        }

        private async void LoadDiagramData()
        {

            DiagramsClass count = new DiagramsClass();

            List<DiagramTable> table = await count.GetCountAsync();

            if (table.Count == 0)
                return;

            PieData[] data = new PieData[table.Count];

            for (int i = 0; i < table.Count; i++)
            {
                data[i] = new PieData(table[i].Name ?? "", table[i].Count);
            }

            pieChart.Series = data.Select(item =>
                new PieSeries<double>
                {
                    Name = item.Name,
                    Values = item.Values
                }).ToArray();
        }


        private async void LoadComboBoxData()
        {
            DictionaryClass dictionary = new DictionaryClass();

            ReasonComboBox.ItemsSource = await dictionary.GetReason();
            ReasonComboBox.SelectedValuePath = "Id";
            ReasonComboBox.DisplayMemberPath = "Name";

            CategoryComboBox.ItemsSource = await dictionary.GetCategory();
            CategoryComboBox.SelectedValuePath = "Id";
            CategoryComboBox.DisplayMemberPath = "Name";

            DiagramsClass countRequest = new DiagramsClass();

            List<DiagramTable> table = await countRequest.GetCountAsync();

            for (int i = 0; i < table.Count; i++)
            {
                if (table[i].Name == "Новая")
                {
                    CountAllRequests.Text = table[i].Count.ToString();
                    DiagramAllRequestsLabel.Visibility = Visibility.Visible;
                    DiagramAllRequestsLabel.Content = "Все заявки: " + table[i].Count.ToString();
                }
                    
                if (table[i].Name == "В работе")
                {
                    CountWorkRequests.Text = table[i].Count.ToString();
                    DiagramWorkRequestsLabel.Visibility = Visibility.Visible;
                    DiagramWorkRequestsLabel.Content = "В работе: " + table[i].Count.ToString();
                }
                    
                if (table[i].Name == "Завершено")
                {
                    CountFinishRequests.Text = table[i].Count.ToString();
                    DiagramFinishRequestsLabel.Visibility = Visibility.Visible;
                    DiagramFinishRequestsLabel.Content = "Завершенные: " + table[i].Count.ToString();
                }
                    
                if (table[i].Name == "Отменено")
                {
                    CountCancelRequests.Text = table[i].Count.ToString();
                    DiagramCancelRequestsLabel.Visibility = Visibility.Visible;
                    DiagramCancelRequestsLabel.Content = "Отменные: " + table[i].Count.ToString();
                }
                    
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
    }
}
