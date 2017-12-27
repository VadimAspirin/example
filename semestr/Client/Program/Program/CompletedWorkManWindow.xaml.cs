using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Program
{
    /// <summary>
    /// Логика взаимодействия для CompletedWorkManWindow.xaml
    /// </summary>
    public partial class CompletedWorkManWindow : Window
    {
        CompletedWorkMan completedWorkMan = new CompletedWorkMan();

        public CompletedWorkManWindow()
        {
            InitializeComponent();
        }

        public void Update()
        {
            completedWorkMan.UpdateTables();
            if(datePickerWork.Text != "")
            {
                comboBoxWorker.DataContext = completedWorkMan.Tables.Tables["Работник"].DefaultView;
            }
            if(comboBoxWorker.Text != "")
            {
                comboBoxTypyOfWork.DataContext = completedWorkMan.Tables.Tables["Тип работы"].DefaultView;
            }
            dataGridWork.DataContext = completedWorkMan.Tables.Tables["s_completed_work"].DefaultView;            
            comboBoxProduct.DataContext = completedWorkMan.Tables.Tables["s_product"].DefaultView;
            if(datePickerProblem.Text != "")
            {
                comboBoxManager.DataContext = completedWorkMan.Tables.Tables["Менеджер"].DefaultView;
            }            
            dataGridSolvedProblems.DataContext = completedWorkMan.Tables.Tables["s_solved_problems"].DefaultView;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            dataGridWork.DataContext = completedWorkMan.Tables.Tables["s_completed_work"].DefaultView;
            dataGridSolvedProblems.DataContext = completedWorkMan.Tables.Tables["s_solved_problems"].DefaultView;
            comboBoxProduct.DataContext = completedWorkMan.Tables.Tables["s_product"].DefaultView;
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            if(datePickerWork.Text != "" && comboBoxWorker.Text != "" && comboBoxTypyOfWork.Text != "" && comboBoxProduct.Text != "")
            {
                if (completedWorkMan.AddCompletedWork(datePickerWork.Text, comboBoxWorker.SelectedValue.ToString(), comboBoxTypyOfWork.SelectedValue.ToString(), comboBoxProduct.SelectedValue.ToString(), textBoxNumberOfProducts.Text) == true)
                {
                    MessageBox.Show("Выполненная работа добавлена успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Выполненная работа не была добавлена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                datePickerWork.Text = null;
                comboBoxWorker.SelectedIndex = -1;
                comboBoxTypyOfWork.SelectedIndex = -1;
                comboBoxProduct.SelectedIndex = -1;
                textBoxNumberOfProducts.Text = null;
                Update();
            }
            else
            {
                MessageBox.Show("Необходимо заполнить все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(datePickerWork.Text != "")
            {
                if (completedWorkMan.NewDateCompletedWork(datePickerWork.Text))
                {
                    completedWorkMan.UpdateTables();
                    comboBoxWorker.DataContext = completedWorkMan.Tables.Tables["Работник"].DefaultView;
                }
            }
            //запрос на рабочих в эту дату
        }

        private void comboBoxWorker_DropDownClosed(object sender, EventArgs e)
        {
            if(comboBoxWorker.Text != "")
            {
                completedWorkMan.NewIdWorkman(comboBoxWorker.SelectedValue.ToString());
                completedWorkMan.UpdateTables();
                comboBoxTypyOfWork.DataContext = completedWorkMan.Tables.Tables["Тип работы"].DefaultView;
            }
            //посылаю рабочего
        }

        private void buttonProblemsOK_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerProblem.Text != "" && comboBoxManager.Text != "" && textBoxSolvedProblem.Text != "")
            {
                if (completedWorkMan.AddSolvedProblems(datePickerProblem.Text, comboBoxManager.SelectedValue.ToString(), textBoxSolvedProblem.Text) == true)
                {
                    MessageBox.Show("Решённая проблема добавлена успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Решённая проблема не была добавлена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                datePickerProblem.Text = null;
                comboBoxManager.SelectedIndex = -1;
                textBoxSolvedProblem.Text = null;
                Update();
            }
            else
            {
                MessageBox.Show("Необходимо заполнить все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void datePickerProblem_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(datePickerProblem.Text != "")
            {
                if (completedWorkMan.NewDateSolvedProblems(datePickerProblem.Text));
                {
                    completedWorkMan.UpdateTables();            
                    comboBoxManager.DataContext = completedWorkMan.Tables.Tables["Менеджер"].DefaultView;
                }
            }
        }
        public void LostConnection()
        {
            if (completedWorkMan.Connection == false)
            {
                MessageBox.Show("Проблемы с соединением", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            completedWorkMan.UpdateTables();
            Update();
            if (!completedWorkMan.Connection)
            {
                MessageBox.Show("Соединение с сервером не установлено", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
