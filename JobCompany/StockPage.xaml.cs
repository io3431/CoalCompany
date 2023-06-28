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

namespace JobCompany
{
    /// <summary>
    /// Interaction logic for StockPage.xaml
    /// </summary>
    public partial class StockPage : Page
    {
        public StockPage()
        {
            InitializeComponent();

            //К DataGrid в качестве ресурса добавляется таблица базы данных Stock(Склад)
            StockDGrid.ItemsSource = CoalCompanyEntities.GetContext().Stock.ToList();
        }

        //Реализация обработчика событий кнопки, для добавления нового склада
        //Открывается окно AddStockWindow с отцентрованным положением на экране
        //Чтобы у пользователя не было возможности закрыть главное окно и сломать приложение, использую метод ShowDialog()
        private void AddStockBtn_Click(object sender, RoutedEventArgs e)
        {
            AddStockWindow addStock = new AddStockWindow();
            addStock.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addStock.ShowDialog();
        }

        //Реализация обработчика событий кнопки, для изменения названия склада
        private void ChangeStock_Click(object sender, RoutedEventArgs e)
        {
            //Объект передает значения выбранного склада как значения из таблицы Stock базы данных
            ChangeStockWindow changeStock = new ChangeStockWindow((sender as Button).DataContext as Stock);
            changeStock.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            changeStock.ShowDialog();
        }

        //Реализация кнопки удаления элементов
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            //Переменная принимающая выбранные значения таблици Stock
            var StocksForRemoving = StockDGrid.SelectedItems.Cast<Stock>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {StocksForRemoving.Count()} элементов?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                //Осуществление безопасного удаления строк при помощи try-catch
                try
                {
                    //Класс базы данных принимает контекст таблицы Stock и при помощи метода
                    //RemoveRange передаем переменную с выбранными строками таблицы и удаляем
                    CoalCompanyEntities.GetContext().Stock.RemoveRange(StocksForRemoving);
                    //Сохраниние в базе данных
                    CoalCompanyEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    //В случае какой-либо ошибки пользователь узнает о ней и данные не будут сохраняться
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Обработчик события кнопки, который позволяет обновить DataGrid с подлюченной базой данных
        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            StockDGrid.ItemsSource = CoalCompanyEntities.GetContext().Stock.ToList();
        }
    }
}
