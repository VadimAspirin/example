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
    /// Логика взаимодействия для ProductChangeWindow.xaml
    /// </summary>
    public partial class ProductChangeWindow : Window
    {
        string name;
        List<String> productTypes;
        List<string> priceAndType = new List<string>() { "", "" };

        public ProductChangeWindow(string oldName, List<String> prodTypes)
        {            
            InitializeComponent();
            name = oldName;
            productTypes = prodTypes;
            for (int i = 0; i < productTypes.Count(); i++)
            {
                comboBoxProductType.Items.Add(productTypes[i]);
            }
        }

        private void windowProductChange_Initialized(object sender, EventArgs e)
        {
            
        }

        private void buttonChangeProduct_Click(object sender, RoutedEventArgs e)
        {
            if (Storekeeper.ChangeProduct(name, textBoxProductName.Text, textBoxProductPrice.Text, comboBoxProductType.Text) == true)
            {
                MessageBox.Show("Товар успешно изменён", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Товар не был изменён", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
    }
}
