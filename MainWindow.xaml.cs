using finalExam_diplom_.controls;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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