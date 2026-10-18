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
    /// Логика взаимодействия для reportControl.xaml
    /// </summary>
    public partial class reportControl : UserControl
    {
        public reportControl()
        {
            InitializeComponent();
        }

        public class PieData(string name, double value)
        {
            public string Name { get; set; } = name;
            public double[] Value { get; set; } = [value];
        }

        public class ViewModel
        {
            public PieData[] Data { get; set; } = [
                    new("Mary", 10),
                    new("John", 20),
                    new("Alice", 30),
                    new("Bob", 40),
                    new("Charlie", 50)
                ];
        }
    }
}
