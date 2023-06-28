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
    /// Interaction logic for ChangeWeightWindow.xaml
    /// </summary>
    public partial class ChangeWeightWindow : Window
    {
        //Создание атрибутов базы данных для заполнения строк данными
        private Picket _currentPicket = new Picket();
        private HistoryChange _currentHistoryChange = new HistoryChange();
        
        //Класс инициализации принимает переданные данные из бд и записывает из в selectedPicket
        public ChangeWeightWindow(Picket selectedPicket)
        {
            InitializeComponent();

            //Обработчик ошибки. Если переданные данные есть, то их значения передаются в атрибут созданный раннее
            if (selectedPicket != null)
                _currentPicket = selectedPicket;

            DataContext = _currentPicket;
        }

        private void SaveNewWeightBtn_Click(object sender, RoutedEventArgs e)
        {
            //Создается элемент StringBuilder для отслеживания неккоретных действий пользователя
            StringBuilder errors = new StringBuilder();
            //Если пользователь не ввел данные веса груза, то выведется ошибка
            if (String.IsNullOrWhiteSpace(WeightTextBox.Text))
                errors.AppendLine("Введите название склада");
            
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                try
                {
                    //Если не произошло ошибок на раннем этапе, то значение веса выбранного атрибута пикета
                    //принимает тот вес, который ввел пользователь
                    _currentPicket.Weight = Convert.ToInt32(WeightTextBox.Text);

                    _currentHistoryChange.NumberAreaId = _currentPicket.NumberAreaId;
                    _currentHistoryChange.NumberPicket = _currentPicket.NumberPicket;
                    _currentHistoryChange.Weight = Convert.ToInt32(WeightTextBox.Text);
                    _currentHistoryChange.DateChange = DateTime.Now;
                    _currentHistoryChange.ChangeInfo = "Изменение веса пикета";

                    CoalCompanyEntities.GetContext().HistoryChange.Add(_currentHistoryChange);
                    CoalCompanyEntities.GetContext().Picket.AddOrUpdate(_currentPicket);

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
