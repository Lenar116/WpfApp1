using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для Window5.xaml
    /// </summary>
    public partial class Window5 : Window
    {

        public Window5()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Создайте новый экземпляр продукта
            Product newProduct = new Product
            {
                Name = NameTextBox.Text,
                Price = decimal.Parse(PriceTextBox.Text),
                Photo = FileNameTextBox.Text,
            };

            using (var context = new AppDbContext())
            {

                {
                    context.Products.Add(newProduct);
                    context.SaveChanges();

                }
                MessageBox.Show("Продукт успешно добавлен!");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

