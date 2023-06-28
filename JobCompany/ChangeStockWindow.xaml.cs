using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

namespace JobCompany
{
    /// <summary>
    /// Interaction logic for ChangeStockWindow.xaml
    /// </summary>
    public partial class ChangeStockWindow : Window
    {
        //Создание атрибутов базы данных для заполнения строк данными
        private Stock _currentStock = new Stock();
        private HistoryChange _currentHistoryChange = new HistoryChange();

        //Класс инициализации принимает переданные данные из бд и записывает из в selectedStock
        public ChangeStockWindow(Stock selectedStock)
        {
            InitializeComponent();
            
            //Обработчик ошибки. Если переданные данные есть, то их значения передаются в атрибут созданный раннее
            if (selectedStock != null)
                _currentStock = selectedStock;

            DataContext = _currentStock;
        }

        private void SaveNewNameBtn_Click(object sender, RoutedEventArgs e)
        {
            //Создается элемент StringBuilder для отслеживания неккоретных действий пользователя
            StringBuilder errors = new StringBuilder();
            //Если пользователь не ввел новое название склада, то выведется ошибка
            if (String.IsNullOrWhiteSpace(StockTextBox.Text))
                errors.AppendLine("Введите название склада");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    //Если не произошло ошибок на раннем этапе, то значение названия склада выбранного атрибута склада
                    //принимает тоназвание, которое ввел пользователь
                    _currentStock.NumberStock = StockTextBox.Text;

                    _currentHistoryChange.NumberStock = StockTextBox.Text;
                    _currentHistoryChange.NumberAreaId = _currentStock.NumberAreaId;
                    _currentHistoryChange.NumberPicket = _currentStock.NumberPicket;
                    _currentHistoryChange.Weight = _currentStock.Weight;
                    _currentHistoryChange.DateChange = DateTime.Now;
                    _currentHistoryChange.ChangeInfo = "Изменение названия склада";

                    CoalCompanyEntities.GetContext().HistoryChange.Add(_currentHistoryChange);
                    CoalCompanyEntities.GetContext().Stock.AddOrUpdate(_currentStock);

                    CoalCompanyEntities.GetContext().SaveChanges();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
