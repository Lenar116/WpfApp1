using Microsoft.EntityFrameworkCore;
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
using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        AppDbContext context;
        User user { get; set; }
        double c = 0;
        public Window4(User u)
        {
            InitializeComponent();
            context = new AppDbContext();
            user = context.Users.Include(x => x.CartItems).Where(x => x.Id == u.Id).FirstOrDefault();
            c = 0;
            UpdateList();
        }

        private void UpdateList()
        {
            List<CartItem> cartItems = context.CartItems.Include(x => x.Product).Where(x => user.CartItems.Contains(x)).ToList();
            bool e = false;
            sp_cart.Children.Clear();
            c = 0;
            foreach (CartItem cartItem in cartItems)
            {
                c += Convert.ToDouble(cartItem.Product.Price * cartItem.Amount);
                
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });
                grid.Margin = new Thickness(2.5);

                Image image = new Image();
                try
                {
                    image.Source = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory, "Pictures", cartItem.Product.Photo)));
                }
                catch
                {
                    e = true;
                }
                image.SetValue(Grid.ColumnProperty, 0);
                image.Margin = new Thickness(5);

                TextBlock textBlock = new TextBlock();
                textBlock.Text = cartItem.Product.Name;
                textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock.VerticalAlignment = VerticalAlignment.Top;
                textBlock.FontSize = 20;
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.SetValue(Grid.ColumnProperty, 1);

                var converter = new BrushConverter();

                Button decButton = new Button();
                decButton.Content = "-";
                decButton.Foreground = Brushes.Black;
                decButton.Background = (Brush)converter.ConvertFrom("#F8F8FF");
                decButton.FontWeight = FontWeights.Bold;
                decButton.FontSize = 20;
                decButton.Name = $"dec_{cartItem.Id}";
                decButton.Click += new RoutedEventHandler(Minus);
                decButton.Margin = new Thickness(5);
                decButton.SetValue(Grid.ColumnProperty, 2);

                TextBlock counter = new TextBlock();
                counter.Text = cartItem.Amount.ToString();
                counter.HorizontalAlignment = HorizontalAlignment.Center;
                counter.VerticalAlignment = VerticalAlignment.Center;
                counter.FontSize = 20;
                counter.SetValue(Grid.ColumnProperty, 3);

                Button incButton = new Button();
                incButton.Content = "+";
                incButton.Foreground = Brushes.Black;
                incButton.Background = (Brush)converter.ConvertFrom("#F8F8FF");
                incButton.FontWeight = FontWeights.Bold;
                incButton.FontSize = 20;
                incButton.Name = $"inc_{cartItem.Id}";
                incButton.Click += new RoutedEventHandler(Plus);
                incButton.Margin = new Thickness(5);
                incButton.SetValue(Grid.ColumnProperty, 4);

                grid.Children.Add(image);
                grid.Children.Add(textBlock);
                grid.Children.Add(decButton);
                grid.Children.Add(counter);
                grid.Children.Add(incButton);

                sp_cart.Children.Add(grid);
            }
            if (e) MessageBox.Show("Ошибка");
        }

        private void Plus (object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(((FrameworkElement)sender).Name.Split("_")[1]);
            context.CartItems.Where(x => x.Id == id).FirstOrDefault().Amount++;
            context.SaveChanges();
            UpdateList();
        }

        private void Minus(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(((FrameworkElement)sender).Name.Split("_")[1]);
            context.CartItems.Where(x => x.Id == id).FirstOrDefault().Amount--;
            if (context.CartItems.Where(x => x.Id == id).FirstOrDefault().Amount == 0) context.CartItems.Remove(context.CartItems.Where(x => x.Id == id).FirstOrDefault());
            context.SaveChanges();
            UpdateList();
        }

        private void b_catalog_Click(object sender, RoutedEventArgs e)
        {
            Window6 window6 = new Window6(user);
            window6.Show();
            this.Close();
        }

        private void b_order_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Заказ успешно оформлен! К оплате: {c} руб.");
            context.Users.Include(x => x.CartItems).Where(x => x.Id == user.Id).FirstOrDefault().CartItems.Clear();
            context.SaveChanges();
            UpdateList();
        }
    }
}
