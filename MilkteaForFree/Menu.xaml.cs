using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MilkteaForFree
{
    public partial class Menu : Window
    {
        public ObservableCollection<Tea> Teas { get; set; }
        public ObservableCollection<CartItem> CartItems { get; set; }
        public ObservableCollection<Order> Orders { get; set; }
        public User CurrentUser { get; set; }
        private decimal totalAmount = 0;

        public Menu()
        {
            InitializeComponent();
            CurrentUser = new User
            {
                Username = "JohnDoe",
                Email = "johndoe@example.com",
                Role = "Admin"
            };

            // Display user information
            UsernameTextBlock.Text = CurrentUser.Username;
            EmailTextBlock.Text = CurrentUser.Email;
            RoleTextBlock.Text = CurrentUser.Role;

            // Initialize the Teas collection
            Teas = new ObservableCollection<Tea>
            {
                new Tea { Name = "Classic Milk Tea", Price = 3.50m, ImagePath = "E:\\3W616\\Session06-Database\\MilkteaForFree\\MilkteaForFree\\Images\\classic_milk_tea.png" },
                new Tea { Name = "Taro Milk Tea", Price = 4.00m, ImagePath = "Images/taro_milk_tea.png" },
                new Tea { Name = "Matcha Milk Tea", Price = 4.50m, ImagePath = "Images/matcha_milk_tea.png" }
            };

            // Initialize the CartItems collection
            CartItems = new ObservableCollection<CartItem>();
            TeaListView.ItemsSource = Teas;
            CartListView.ItemsSource = CartItems;

            // Initialize the Orders collection
            Orders = new ObservableCollection<Order>
            {
                new Order { OrderID = 1, OrderDate = DateTime.Now.AddDays(-1), ItemsSummary = "Classic Milk Tea, Taro Milk Tea", TotalAmount = 7.50m },
                new Order { OrderID = 2, OrderDate = DateTime.Now.AddDays(-2), ItemsSummary = "Matcha Milk Tea", TotalAmount = 4.50m }
            };
            OrderHistoryListView.ItemsSource = Orders;
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            Tea selectedTea = (Tea)((FrameworkElement)sender).DataContext;
            var cartItem = CartItems.FirstOrDefault(item => item.Name == selectedTea.Name);
            if (cartItem == null)
            {
                CartItems.Add(new CartItem { Name = selectedTea.Name, Quantity = 1, TotalPrice = selectedTea.Price });
            }
            else
            {
                cartItem.Quantity++;
                cartItem.TotalPrice += selectedTea.Price;
            }
            totalAmount += selectedTea.Price;
            TotalTextBlock.Text = totalAmount.ToString("C");
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Total amount to be paid: {totalAmount:C}", "Checkout", MessageBoxButton.OK, MessageBoxImage.Information);
            CartItems.Clear();
            totalAmount = 0;
            TotalTextBlock.Text = totalAmount.ToString("C");
        }

        private void AddMilkTeaButton_Click(object sender, RoutedEventArgs e)
        {
            string name = MilkTeaNameTextBox.Text;
            if (!decimal.TryParse(MilkTeaPriceTextBox.Text, out decimal price))
            {
                MessageBox.Show("Please enter a valid price.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string imagePath = MilkTeaImagePathTextBox.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(imagePath))
            {
                MessageBox.Show("Please fill in all fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Teas.Add(new Tea { Name = name, Price = price, ImagePath = imagePath });
            MessageBox.Show($"'{name}' has been added to the menu!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Clear input fields after adding the tea
            MilkTeaNameTextBox.Clear();
            MilkTeaPriceTextBox.Clear();
            MilkTeaImagePathTextBox.Clear();
        }

        private void SearchOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? fromDate = FromDatePicker.SelectedDate;
            DateTime? toDate = ToDatePicker.SelectedDate;

            if (fromDate == null || toDate == null)
            {
                MessageBox.Show("Please select both From and To dates.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var filteredOrders = Orders.Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate).ToList();
            OrderHistoryListView.ItemsSource = filteredOrders;

            if (!filteredOrders.Any())
            {
                MessageBox.Show("No orders found in the selected date range.", "No Results", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
