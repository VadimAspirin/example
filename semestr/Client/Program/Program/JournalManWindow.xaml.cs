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
using System.Windows.Shapes;

namespace Program
{
    /// <summary>
    /// Логика взаимодействия для JournalMan.xaml
    /// </summary>
    public partial class JournalManWindow : Window
    {
        JournalMan journalMan = new JournalMan();
        public JournalManWindow()
        {
            InitializeComponent();
            Update();
        }
        public void Update()
        {
            comboBoxWorker.DataContext = journalMan.Tables.Tables["s_workman"].DefaultView;
            dataGridVisits.DataContext = journalMan.Tables.Tables["s_journal_visit"].DefaultView;
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerComing.Text == "" || comboBoxWorker.Text == "")
            {
                MessageBox.Show("Необходимо заполнить все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (journalMan.AddJournalEntry(comboBoxWorker.SelectedValue.ToString(), datePickerComing.Text) == true)
                {
                    MessageBox.Show("Смена добавлена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Смена не была добавлена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                journalMan.UpdateTables();
                Update();
                datePickerComing.Text = null;
                comboBoxWorker.SelectedIndex = -1;
            }
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            journalMan.UpdateTables();
            Update();
            if (!journalMan.Connection)
            {
                MessageBox.Show("Соединение с сервером не установлено", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
