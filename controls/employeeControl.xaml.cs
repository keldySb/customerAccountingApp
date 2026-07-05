using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace finalExam_diplom_.controls
{
    /// <summary>
    /// Логика взаимодействия для employeeControl.xaml
    /// </summary>
    public partial class employeeControl : UserControl
    {
        public employeeControl()
        {
            InitializeComponent();
        }

        private void employeeTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (employeeTextBox.Text == "Поиск")
                employeeTextBox.Text = "";
        }

        private void employeeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(employeeTextBox.Text))
                employeeTextBox.Text = "Поиск";
        }

    }
}
