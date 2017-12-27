using System;
using System.Collections.Generic;
using System.Data;
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
using System.Threading;

namespace Program
{
    /// <summary>
    /// Логика взаимодействия для Storekeeper.xaml
    /// </summary>
    public partial class StorekeeperWindow : Window
    {
        Storekeeper storekeeper = new Storekeeper();
        DataRowView row;
        List<string> searchBy = new List<string>();

        public StorekeeperWindow()
        {            
            InitializeComponent();
            Update();
        }
        public void Update()
        {
            addToSearchList();
            comboBoxSearchBy.DataContext = searchBy;
            dataGridProducts.DataContext = storekeeper.Tables.Tables["s_product"].DefaultView;
            dataGridTypeOfProducts.DataContext = storekeeper.Tables.Tables["s_type_product"].DefaultView;
            comboBoxProductType.DataContext = storekeeper.Tables.Tables["s_type_product"].DefaultView;
            comboBoxProductTypeChange.DataContext = storekeeper.Tables.Tables["s_type_product"].DefaultView;
        }
        public void addToSearchList()
        {
            searchBy.Clear();
            for (int i = 0; i < storekeeper.Tables.Tables["s_product"].Columns.Count; i++)
            {
                searchBy.Add(storekeeper.Tables.Tables["s_product"].Columns[i].Caption);
            }
        }
        private void buttonAddProductType_Click(object sender, RoutedEventArgs e) //ГОТОВО
        {
            if (textBoxProductType.Text == "")
            {
                MessageBox.Show("Необходимо заполнить все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (storekeeper.AddTypeProduct(textBoxProductType.Text) == true)
                {
                    MessageBox.Show("Тип товара успешно добавлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Тип товара не был добавлен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                textBoxProductType.Clear();
                storekeeper.UpdateTables();
                Update();
            }
        }

        private void buttonDeleteTypeOfProduct_Click(object sender, RoutedEventArgs e) //ГОТОВО
        {
            try
            {
                if (storekeeper.DelTypeProduct(dataGridTypeOfProducts.SelectedValue.ToString()) == true)
                {
                    MessageBox.Show("Тип товара успешно удалён", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Тип товара не был удалён", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                storekeeper.UpdateTables();
                Update();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Необходимо выбрать тип товара для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }            
        }

        private void buttonChangeProduct_Click(object sender, RoutedEventArgs e) //ГОТОВО
        {
            try
            {
                if (textBoxProductNameChange.Text == "" || textBoxProductPriceChange.Text == "" || comboBoxProductTypeChange.Text == "")
                {
                    MessageBox.Show("Необходимо заполнить все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    if (storekeeper.ChangeProduct(dataGridProducts.SelectedValue.ToString(), textBoxProductNameChange.Text, textBoxProductPriceChange.Text, comboBoxProductTypeChange.SelectedValue.ToString()) == true)
                    {
                        MessageBox.Show("Товар успешно изменён", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Товар не был изменён", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    textBoxProductNameChange.Clear();
                    textBoxProductPriceChange.Clear();
                    comboBoxProductType.SelectedIndex = -1;
                    storekeeper.UpdateTables();
                    Update();
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Необходимо выбрать тип товара для изменения", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void buttonAddProduct_Click(object sender, RoutedEventArgs e) //ГОТОВО
        {
            if (textBoxProductName.Text == "" || textBoxProductPrice.Text == "" || comboBoxProductType.Text == "")
            {
                MessageBox.Show("Необходимо заполнить все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (storekeeper.AddProduct(textBoxProductName.Text, textBoxProductPrice.Text, comboBoxProductType.SelectedValue.ToString()) == true)
                {
                    MessageBox.Show("Товар успешно добавлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Товар не был добавлен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                textBoxProductName.Clear();
                textBoxProductPrice.Clear();
                comboBoxProductType.SelectedIndex = -1;
                storekeeper.UpdateTables();
                Update();
            }            
        }

        private void buttonDeleteProduct_Click(object sender, RoutedEventArgs e) //ГОТОВО
        {
            try
            {
                if (dataGridProducts.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("Необходимо выбрать товар для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    if (storekeeper.DelProduct(dataGridProducts.SelectedValue.ToString()) == true)
                    {
                        MessageBox.Show("Товар успешно удалён", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Товар не был удалён", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    storekeeper.UpdateTables();
                    Update();
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Необходимо выбрать товар для удаления", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void dataGridProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)//ГОТОВО
        {
            try
            {
                row = (DataRowView)dataGridProducts.SelectedItems[0];
                textBoxProductNameChange.Text = Convert.ToString(row["Название"]);
                textBoxProductPriceChange.Text = Convert.ToString(row["Цена"]);
                comboBoxProductTypeChange.Text = Convert.ToString(row["Тип"]);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Необходимо выбрать товар", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxSearch.Text != "" && comboBoxSearchBy.Text != "")
            {
                if (comboBoxSearchBy.Text == "Название" || comboBoxSearchBy.Text == "Тип")
                {
                    storekeeper.Tables.Tables["s_product"].DefaultView.RowFilter = "[" + comboBoxSearchBy.Text + "] like '%" + textBoxSearch.Text + "%'";
                }
                if (comboBoxSearchBy.Text == "Идентификатор" || comboBoxSearchBy.Text == "Количество")
                {
                    Int32 number;
                    Int32.TryParse(textBoxSearch.Text, out number);
                    storekeeper.Tables.Tables["s_product"].DefaultView.RowFilter = "[" + comboBoxSearchBy.Text + "] = '" + number + "'";
                }
                if (comboBoxSearchBy.Text == "Цена")
                {
                    double number;
                    double.TryParse(textBoxSearch.Text, out number);
                    storekeeper.Tables.Tables["s_product"].DefaultView.RowFilter = "[" + comboBoxSearchBy.Text + "] = '" + number + "'";
                }
            }
            if (textBoxSearch.Text == "" || comboBoxSearchBy.Text == "")
            {
                storekeeper.Tables.Tables["s_product"].DefaultView.RowFilter = "";
            }
            Update();
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            storekeeper.UpdateTables();
            Update();
            if (!storekeeper.Connection)
            {
                MessageBox.Show("Соединение с сервером не установлено", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        public void LostConnection()
        {
            if(storekeeper.Connection == false)
            {
                MessageBox.Show("Проблемы с соединением", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}