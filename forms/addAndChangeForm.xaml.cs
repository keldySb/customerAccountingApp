using finalExam_diplom_.classes.databaseTables;
using finalExam_diplom_.controls;
using System.Windows;
using System.Windows.Controls;

namespace finalExam_diplom_.forms
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class addAndChangeForm : Window
    {
        public addAndChangeForm(UserControl control)
        {
            InitializeComponent();

            useControl.Content = control;
        }

    }
}
