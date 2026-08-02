using finalExam_diplom_.classes;
using finalExam_diplom_.classes.databaseTables;
using finalExam_diplom_.forms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace finalExam_diplom_.controls
{
    /// <summary>
    /// Логика взаимодействия для employeeControl.xaml
    /// </summary>

    public partial class EmployeeControl : UserControl
    {
        public Action? SetTask;
        private DispatcherTimer timer;
        public EmployeeControl()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 30);
            timer.Start();
        }

        private void ShowDataGridInformation()
        {
            GetEmployeeTable table = new GetEmployeeTable();

            List<EmployeesTable> employee = table.GetEmployee(employeeTextBox.Text);
            employeeDataGrid.ItemsSource = employee;
        }
        public void RefreshData()
        {
            ShowDataGridInformation();
        }

        private void OpenApplication_Click(object sender, RoutedEventArgs e)
        {
            var empData = employeeDataGrid.SelectedItem as EmployeesTable;

            employeeOpenDataControl control = new employeeOpenDataControl(empData);
            addAndChangeForm form = new addAndChangeForm(control);
            form.ShowDialog();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            ShowDataGridInformation();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ShowDataGridInformation();
        }

        private void employeeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowDataGridInformation();
        }

        private void SetTaskButton_Click(object sender, RoutedEventArgs e)
        {
            SetTask();
        }
    }
}
