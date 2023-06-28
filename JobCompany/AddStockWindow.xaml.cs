using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddStockWindow.xaml
    /// </summary>
    public partial class AddStockWindow : Window
    {
        //Создание атрибутов бд для заполнения данными
        private HistoryChange _currentHistoryChange = new HistoryChange();
        private Stock _currentStock = new Stock();

        public AddStockWindow()
        {
            InitializeComponent();

            //ComboBox принимает значения из таблицы базы данных Area(Площадь)
            SelectAreaCbox.ItemsSource = CoalCompanyEntities.GetContext().Area.ToList();
        }

        //Сохранения в бд
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            //Создание элемента StringBuilder для отслеживания ошибок
            StringBuilder errors = new StringBuilder();

            //Если нет названия склада и не выбрана необходимая площадка для склада
            //Выведется соответствующая ошибка
            if (String.IsNullOrWhiteSpace(StockNameBox.Text))
                errors.AppendLine("Введите название склада");
            if (SelectAreaCbox.SelectedItem == null)
                errors.AppendLine("Выберете площадку");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                //Имя склада принимает введенные значения и записывется в таблицу склада и таблицу истории
                _currentStock.NumberStock = StockNameBox.Text;
                _currentHistoryChange.NumberStock = StockNameBox.Text;

                //Если выбранный элемент ComboBox передается как экземпляр класса таблицы базы данных selectedArea
                if (SelectAreaCbox.SelectedValue is Area selectedArea)
                {
                    //Проверка на соответсвие номера площадки(Id) с площадкой(Id) в таблице пикета. Связь 1:M
                    var currentNumberPicket = CoalCompanyEntities.GetContext().Picket.FirstOrDefault(p => p.NumberAreaId == selectedArea.Id);
                    //Если соответсвие нашлось
                    if (currentNumberPicket != null)
                    {
                        //Значение площадок записывается в список, для проверки одна ли площадка или их больше
                        string[] numberChecking = selectedArea.NumberArea.Split('-');
                        int secondNum;

                        //Если площадка одна, то ее значение идет в secondNum
                        //Если больше, то в значение secondNum идет наибольшее значение площадки
                        if (numberChecking.Length == 1) 
                        {
                            secondNum = Convert.ToInt32(selectedArea.NumberArea.Split('-')[0]);
                        } else
                        {
                            secondNum = Convert.ToInt32(selectedArea.NumberArea.Split('-')[1]);
                        }

                        //Перебор значений для добавления нескольки записей в таблицу
                        for (int i = Convert.ToInt32(selectedArea.NumberArea.Split('-')[0]); i <= secondNum; i++)
                        {
                            //Данные записываются в таблицу бд Склад
                            _currentStock.NumberAreaId = selectedArea.Id;
                            _currentStock.NumberPicket = i;
                            _currentStock.Weight = currentNumberPicket.Weight;
                            _currentStock.DateAdd = DateTime.Now;

                            //Данные записываются в таблицу бд История изменений
                            _currentHistoryChange.NumberAreaId = selectedArea.Id;
                            _currentHistoryChange.NumberPicket = i;
                            _currentHistoryChange.Weight = currentNumberPicket.Weight;
                            _currentHistoryChange.DateChange = DateTime.Now;
                            _currentHistoryChange.ChangeInfo = "Склад разбит на площадку и пикеты";

                            //Данные добавляются
                            CoalCompanyEntities.GetContext().Stock.Add(_currentStock);
                            CoalCompanyEntities.GetContext().HistoryChange.Add(_currentHistoryChange);

                            //Если ошибок не возникло данные сохраняются
                            try
                            {
                                CoalCompanyEntities.GetContext().SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), "ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                        MessageBox.Show("информация сохранена", "успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
            }
        }
    }
}
