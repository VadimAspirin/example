using System;
using System.Data;
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
using System.Windows.Shapes;
using System.Drawing;

namespace Program
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        Admin admin = new Admin();
        DataRowView row;
        List<string> searchBy = new List<string>();

        public AdminWindow()
        {
            InitializeComponent();
        }

        public void addToSearchList()
        {
            searchBy.Clear();
            for (int i = 0; i < admin.Tables.Tables["users"].Columns.Count; i++)
            {
                searchBy.Add(admin.Tables.Tables["users"].Columns[i].Caption);
            }
        }

        public void Update()
        {
            addToSearchList();
            comboBoxSearchBy.DataContext = searchBy;
            comboBoxTypeOfAcc.DataContext = admin.Tables.Tables["roles"].DefaultView;
            dataGridAccounts.DataContext = admin.Tables.Tables["users"].DefaultView;
            dataGridOffices.DataContext = admin.Tables.Tables["s_office"].DefaultView;
            dataGridChangeProductCount.DataContext = admin.Tables.Tables["s_product"].DefaultView;
            dataGridDeleteVisits.DataContext = admin.Tables.Tables["s_journal_visit"].DefaultView;
            dataGridDeleteCompletedWork.DataContext = admin.Tables.Tables["s_completed_work"].DefaultView;
            dataGridDeleteSolvedProblems.DataContext = admin.Tables.Tables["s_solved_problems"].DefaultView;
        }

        private void buttonAddOffice_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxLogin.Text == "" || textBoxPassword.Text == "" || comboBoxTypeOfAcc.Text == "")
            {
                MessageBox.Show("Все поля должны быть заполнены", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (admin.AddUser(textBoxLogin.Text, textBoxPassword.Text, comboBoxTypeOfAcc.Text) == true)
                {
                    MessageBox.Show("Учётная запись успешно создана", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Учётная запись не была создана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                admin.UpdateTables();
                Update();
                textBoxLogin.Text = null;
                textBoxPassword.Text = null;
                comboBoxTypeOfAcc.SelectedIndex = -1;
            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            Update();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (admin.DelUser(dataGridAccounts.SelectedValue.ToString()) == true)
                {
                    MessageBox.Show("Учётная запись успешно удалена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Учётная запись не была удалена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                admin.UpdateTables();
                Update();
            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show("Необходимо выбрать учётную запись для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void buttonAddAddress_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxAddress.Text == "")
            {
                MessageBox.Show("Все поля должны быть заполнены", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (admin.AddOffice(textBoxAddress.Text) == true)
                {
                    MessageBox.Show("Офис успешно добавлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Офис не был добавлен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                admin.UpdateTables();
                Update();
                textBoxAddress.Text = null;
            }
        }

        private void buttonDeleteAddress_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (admin.DelOffice(dataGridOffices.SelectedValue.ToString()) == true)
                {
                    MessageBox.Show("Офис успешно удалён", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Офис не была удалён", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                admin.UpdateTables();
                Update();
            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show("Необходимо выбрать офис для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            admin.UpdateTables();
            Update();
            if (!admin.Connection)
            {
                MessageBox.Show("Соединение с сервером не установлено", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void textBoxSearchAccounts_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxSearch.Text != "" && comboBoxSearchBy.Text != "")
            {
                admin.Tables.Tables["users"].DefaultView.RowFilter = "[" + comboBoxSearchBy.Text + "] like '%" + textBoxSearch.Text + "%'";
            }
            if (textBoxSearch.Text == "" || comboBoxSearchBy.Text == "")
            {
                admin.Tables.Tables["users"].DefaultView.RowFilter = "";
            }
            Update();
        }

        private void buttonChangeAddress_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (textBoxNewAddress.Text == "")
                {
                    MessageBox.Show("Все поля должны быть заполнены", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    if (admin.ChangeOffice(dataGridOffices.SelectedValue.ToString(), textBoxNewAddress.Text) == true)
                    {
                        MessageBox.Show("Офис успешно изменён", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Офис не был изменён", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    admin.UpdateTables();
                    Update();
                    textBoxNewAddress.Text = null;
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Необходимо выбрать офис", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void dataGridOffices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                row = (DataRowView)dataGridOffices.SelectedItems[0];
                textBoxNewAddress.Text = Convert.ToString(row["Адрес"]);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Необходимо выбрать офис", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void buttonDeleteSolvedProblems_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (admin.DelSolvedProblems(dataGridDeleteSolvedProblems.SelectedValue.ToString()) == true)
                {
                    MessageBox.Show("Решённая проблема успешно удалена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Решённая проблема не была удалена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                admin.UpdateTables();
                Update();
            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show("Необходимо выбрать решённую проблему для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void buttonDeleteCompletedWork_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (admin.DelCompletedWork(dataGridDeleteCompletedWork.SelectedValue.ToString()) == true)
                {
                    MessageBox.Show("Выполненная работа успешно удалена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Выполненная работа не была удалена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                admin.UpdateTables();
                Update();
            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show("Необходимо выбрать выполненную работу для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void buttonDeleteVisit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                row = (DataRowView)dataGridDeleteVisits.SelectedItems[0];
                if (admin.DelJournalEntry(Convert.ToString(row["Дата"]), Convert.ToString(row["Ид-ор работника"])) == true)
                {
                    MessageBox.Show("Смена успешно удалена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Смена не была удалена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                admin.UpdateTables();
                Update();
            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show("Необходимо выбрать смену для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void buttonChangeProductCount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(textBoxChangeProductCount.Text != "")
                {
                    if (admin.ChangeProductCount(dataGridChangeProductCount.SelectedValue.ToString(), textBoxChangeProductCount.Text) == true)
                    {
                        MessageBox.Show("Офис успешно удалён", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Офис не была удалён", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    admin.UpdateTables();
                    Update();
                    textBoxChangeProductCount.Text = null;
                }
                else
                {
                    MessageBox.Show("Все поля должны быть заполнены", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                
            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show("Необходимо выбрать офис для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void textBoxChangeProductCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }
    }
}
