using finalExam_diplom_.classes;
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

            if (control is clientsChangeDataControl actionControl)
            {
                actionControl.CloseUC += CloseUC;
            }

            if (control is employeeOpenDataControl actionEmpControl)
            {
                actionEmpControl.CloseUC += CloseUC;
            }
        }

        void CloseUC()
        {
            useControl.Visibility = Visibility.Collapsed;
            this.Hide();
        }
    }
}
