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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var login = UsernameTextBox.Text;
            var pass = PasswordTextBox.Password;
            var email = EmailTextBox.Text;
            var context = new AppDbContext();
            var user_exists = context.Users.FirstOrDefault(x => x.Login
            == login);
            if (user_exists is not null)
            {
                MessageBox.Show("Такой пользователь уже зарегистрирован!");
                return;
            }
        
            var user = new User { Login = login, Password = pass , Email= email};
            context.Users.Add(user);
            context.SaveChanges();
            MessageBox.Show("Регистрация выполнена успешно!");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

