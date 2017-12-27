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
namespace Program
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public object User;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void watermarkedLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            watermarkedLogin.Visibility = System.Windows.Visibility.Collapsed;
            textBoxLogin.Visibility = System.Windows.Visibility.Visible;
            textBoxLogin.Focus();
        }

        private void textBoxLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(textBoxLogin.Text))
            {
                textBoxLogin.Visibility = System.Windows.Visibility.Collapsed;
                watermarkedLogin.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void watermarkedPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            watermarkedPassword.Visibility = System.Windows.Visibility.Collapsed;
            textBoxPassword.Visibility = System.Windows.Visibility.Visible;
            textBoxPassword.Focus();
        }

        private void textBoxPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxPassword.Password))
            {
                textBoxPassword.Visibility = System.Windows.Visibility.Collapsed;
                watermarkedPassword.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void buttonEnter_Click(object sender, RoutedEventArgs e)
        {            
            if ((textBoxLogin.Text == "") || (textBoxPassword.Password == ""))
            {
                MessageBox.Show("Необходимо заполнить оба поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string role = Login.Start(textBoxLogin.Text, textBoxPassword.Password);
                switch (role)
                {                    
                    case "admin":
                        AdminWindow adMan = new AdminWindow();
                        this.Close();
                        adMan.Show();
                        break;
                    case "hr":
                        HRWindow hrMan = new HRWindow();
                        this.Close();
                        hrMan.Show();
                        break;
                    case "journalman":
                        JournalManWindow joMan = new JournalManWindow();
                        this.Close();
                        joMan.Show();
                        break;
                    case "storekeeper":
                        StorekeeperWindow storeMan = new StorekeeperWindow();
                        this.Close();
                        storeMan.Show();
                        break;
                    case "workman":
                        CompletedWorkManWindow workMan = new CompletedWorkManWindow();
                        this.Close();
                        workMan.Show();
                        break;
                    case "<ERROR>":
                        textBoxLogin.Clear();
                        textBoxPassword.Clear();
                        MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case null:
                        MessageBox.Show("Нет соединения с сервером", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    default:
                        MessageBox.Show("Неизвестная ошибка");
                        break;

                }
                
            }
        }
    }
}
 