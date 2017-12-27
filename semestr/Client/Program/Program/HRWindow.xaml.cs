using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Program
{    
    /// <summary>
    /// Логика взаимодействия для HR.xaml
    /// </summary>
    public partial class HRWindow : Window
    {
        HumanResources hr = new HumanResources();
        DataRowView row;
        List<string> searchBy = new List<string>();

        public HRWindow()
        {
            InitializeComponent();
        }

        public void addToSearchList()
        {
            searchBy.Clear();
            for (int i = 0; i < hr.Tables.Tables["s_workman"].Columns.Count; i++)
            {
                searchBy.Add(hr.Tables.Tables["s_workman"].Columns[i].Caption);
            }
        }

        public void Update()
        {
            addToSearchList();
            comboBoxSearchBy.DataContext = searchBy;
            dataGridWorkers.DataContext = hr.Tables.Tables["s_workman"].DefaultView;
            comboBoxPost.DataContext = hr.Tables.Tables["s_post_workman"].DefaultView;
            comboBoxManager.DataContext = hr.Tables.Tables["manager"].DefaultView;
            comboBoxOffice.DataContext = hr.Tables.Tables["s_office"].DefaultView;
            comboBoxPostChange.DataContext = hr.Tables.Tables["s_post_workman"].DefaultView;
            comboBoxManagerChange.DataContext = hr.Tables.Tables["manager"].DefaultView;
            comboBoxOfficeChange.DataContext = hr.Tables.Tables["s_office"].DefaultView;
        }
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxFirstName.Text == "" || textBoxLastName.Text == "" || comboBoxPost.Text == "" || comboBoxOffice.Text == "")
            {
                MessageBox.Show("Обязательные поля, помеченные звёздочкой, должны быть заполнены", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (hr.AddWorkman(textBoxLastName.Text, textBoxFirstName.Text, textBoxMidleName.Text, comboBoxPost.SelectedValue.ToString(), comboBoxManager.SelectedValue.ToString(), comboBoxOffice.SelectedValue.ToString()) == true)
                {
                    MessageBox.Show("Работник успешно добавлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Работник не был добавлен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                hr.UpdateTables();
                Update();
                textBoxLastName.Text = null;
                textBoxFirstName.Text = null;
                textBoxMidleName.Text = null;
                comboBoxPost.SelectedIndex = -1;
                comboBoxManager.SelectedIndex = -1;
                comboBoxPost.SelectedIndex = -1;
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {            
            try
            {
                if (hr.DelWorkman(dataGridWorkers.SelectedValue.ToString()) == true)
                {
                    MessageBox.Show("Работник успешно удалён", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Работник не был удалён", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                hr.UpdateTables();
                Update();
            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show("Необходимо выбрать работника для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void windowHR_Initialized(object sender, EventArgs e)
        {
            Update();
        }
        public void LostConnection()
        {
            if (hr.Connection == false)
            {
                MessageBox.Show("Проблемы с соединением", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxSearch.Text != "" && comboBoxSearchBy.Text != "" && comboBoxSearchBy.Text != "Идентификатор")
            {                
                hr.Tables.Tables["s_workman"].DefaultView.RowFilter = "[" + comboBoxSearchBy.Text + "] like '%" + textBoxSearch.Text + "%'";
            }
            if (comboBoxSearchBy.Text == "Идентификатор")
            {
                Int32 number;
                Int32.TryParse(textBoxSearch.Text, out number);
                hr.Tables.Tables["s_workman"].DefaultView.RowFilter = "[" + comboBoxSearchBy.Text + "] = '" + number + "'";
            }
            if (textBoxSearch.Text == "" || comboBoxSearchBy.Text == "")
            {
                hr.Tables.Tables["s_workman"].DefaultView.RowFilter = "";
            }
            Update();
        }

        private void dataGridWorkers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                row = (DataRowView)dataGridWorkers.SelectedItems[0];
                textBoxFirstNameChange.Text = Convert.ToString(row["Имя"]);
                textBoxLastNameChange.Text = Convert.ToString(row["Фамилия"]);
                textBoxMidleNameChange.Text = Convert.ToString(row["Отчество"]);
                comboBoxPostChange.Text = Convert.ToString(row["Должность"]);
                comboBoxManagerChange.Text = Convert.ToString(row["Начальник"]);
                comboBoxOfficeChange.Text = Convert.ToString(row["Офис"]);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Необходимо выбрать товар", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void buttonChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (textBoxFirstNameChange.Text == "" || textBoxLastNameChange.Text == "" || comboBoxPostChange.Text == "" || comboBoxOfficeChange.Text == "")
                {
                    MessageBox.Show("Обязательные поля, помеченные звёздочкой, должны быть заполнены", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    if (hr.ChangeWorkman(dataGridWorkers.SelectedValue.ToString(), textBoxLastNameChange.Text, textBoxFirstNameChange.Text, textBoxMidleNameChange.Text, comboBoxPostChange.SelectedValue.ToString(), comboBoxManagerChange.SelectedValue.ToString(), comboBoxOfficeChange.SelectedValue.ToString()) == true)
                    {
                        MessageBox.Show("Работник успешно изменён", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Работник не был изменён", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    hr.UpdateTables();
                    Update();
                    textBoxLastNameChange.Text = null;
                    textBoxFirstNameChange.Text = null;
                    textBoxMidleNameChange.Text = null;
                    comboBoxPostChange.SelectedIndex = -1;
                    comboBoxManagerChange.SelectedIndex = -1;
                    comboBoxOfficeChange.SelectedIndex = -1;
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Необходимо выбрать работника", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            hr.UpdateTables();
            Update();
            if (!hr.Connection)
            {
                MessageBox.Show("Соединение с сервером не установлено", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
