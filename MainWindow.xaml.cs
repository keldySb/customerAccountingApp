using finalExam_diplom_.controls;
using System.Windows;

namespace finalExam_diplom_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void employeeButton_Click(object sender, RoutedEventArgs e)
        {
            employeeControl ec = new employeeControl();
            useControl.Content = ec;
            headerTextBox.Text = "Список сотрудников";

            ec.setTask += setTask;
        }

        void setTask()
        {
            setBorder.Visibility = Visibility.Visible;
            mainControl mc = new mainControl();
            setTaskOrEmpControl.Content = mc;
            mc.statusBorder.Visibility = Visibility.Collapsed;
            mc.id.Visibility = Visibility.Collapsed;
            mc.fio.Visibility = Visibility.Collapsed;
            mc.status.Visibility = Visibility.Collapsed;
            mc.created_time.Visibility = Visibility.Collapsed;
        }

        private void mainButton_Click(object sender, RoutedEventArgs e)
        {
            mainControl mc = new mainControl();
            useControl.Content = mc;
            headerTextBox.Text = "Заявки клиентов";
        }

        private void reportButton_Click(object sender, RoutedEventArgs e)
        {
            reportControl rc = new reportControl();
            useControl.Content = rc;
            headerTextBox.Text = "Отчетность";
        }
    }
}