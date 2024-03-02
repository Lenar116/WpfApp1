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
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private User loggedInUser;
        public Window2(User user)
        {
            InitializeComponent();
            loggedInUser = user;
            UpdateUserInfo();
        }

        private void UpdateUserInfo()
        {
            UsernameLabel.Text =  loggedInUser.Login;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window6 window6 = new Window6(loggedInUser);
            window6.Show();
            this.Close();
        }
    }
}
