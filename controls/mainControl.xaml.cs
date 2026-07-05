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
    /// Логика взаимодействия для mainControl.xaml
    /// </summary>
    public partial class mainControl : UserControl
    {
        public mainControl()
        {
            InitializeComponent();
        }

        private void mainTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (mainTextBox.Text == "Поиск")
                mainTextBox.Text = "";
        }

        private void mainTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(mainTextBox.Text))
                mainTextBox.Text = "Поиск"; 
        }


    }
}
