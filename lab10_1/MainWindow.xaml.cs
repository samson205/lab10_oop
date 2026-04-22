using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace lab10_1
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Product> Products { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Products = new ObservableCollection<Product>()
            {
                new Product { Name="Samsung TV", Category="Телевизор", Price=800, Count=5 },
                new Product { Name="LG Fridge", Category="Холодильник", Price=1200, Count=3 },
                new Product { Name="Philips Vacuum", Category="Пылесос", Price=250, Count=10 }
            };

            PriceGrid.ItemsSource = Products;
            FillListBox();
            boxCategory.SelectedIndex = 0;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Products.Add(new Product
            {
                Name = "Новый товар",
                Category = "Категория",
                Price = 0,
                Count = 0
            });
            FillListBox();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (PriceGrid.SelectedItem is Product selected)
            {
                Products.Remove(selected);
            }
            FillListBox();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            saveFileDialog.FileName = "PriceList.txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("Прайс-лист");
                sb.AppendLine($"Дата: {DateTime.Now}");
                sb.AppendLine();
                sb.AppendLine(
                        $"{"Название",-20} " +
                        $"{"Категория",-20} " +
                        $"{"Цена",-10} " +
                        $"{"Количество",-5}");
                sb.AppendLine("----------------------------------------------------------------");

                foreach (var product in Products)
                {
                    sb.AppendLine(
                        $"{product.Name,-20} " +
                        $"{product.Category,-20} " +
                        $"{product.Price,-10} " +
                        $"{product.Count,-5}");
                }

                File.WriteAllText(saveFileDialog.FileName, sb.ToString(), Encoding.UTF8);

                MessageBox.Show("Прайс-лист успешно сохранен!",
                                "Готово",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        private void boxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (boxCategory.SelectedItem == null)
                return;

            string? selectedCategory = boxCategory.SelectedItem.ToString();

            if (selectedCategory == "Все категории")
            {
                PriceGrid.ItemsSource = null;
                PriceGrid.ItemsSource = Products;
                return;
            }
            var sortedProducts = Products.Where(x => x.Category == selectedCategory).ToList();

            PriceGrid.ItemsSource = null;
            PriceGrid.ItemsSource = sortedProducts;
            FillListBox();
        }

        private void PriceGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            FillListBox();
        }

        private void FillListBox()
        {
            var categories = Products
                .Select(x => x.Category)
                .Distinct()
                .ToList();
            categories.Insert(0, "Все категории");
            boxCategory.ItemsSource = categories;
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
    }
}