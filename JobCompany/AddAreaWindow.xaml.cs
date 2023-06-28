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

namespace JobCompany
{
    /// <summary>
    /// Interaction logic for AddAreaWindow.xaml
    /// </summary>
    public partial class AddAreaWindow : Window
    {
        //Создание атрибутов бд для заполнения данными
        private Area _currentArea = new Area();
        private Picket _currentPicket = new Picket();
        private HistoryChange _currentHistoryChange = new HistoryChange();

        public AddAreaWindow()
        {
            InitializeComponent();
        }

        //При фокусе на втором тектсбоксе данные удаляются, для интуитивно понятного интерфейса
        private void OptionalNumberText_GotFocus(object sender, RoutedEventArgs e)
        {
            OptionalNumberText.Clear();
            OptionalNumberText.Foreground = Brushes.Black;
        }

        //Сохранения в бд
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            //Создание элемента StringBuilder для отслеживания ошибок
            StringBuilder errors = new StringBuilder();

            //Если не выбрано хотя бы одно число и не выбран вес для площадки
            //Выведется соответствующая ошибка
            if (String.IsNullOrWhiteSpace(FirstNumberText.Text))
                errors.AppendLine("Введите первое число");
            if (String.IsNullOrWhiteSpace(FirstNumberText.Text) && String.IsNullOrWhiteSpace(OptionalNumberText.Text))
                errors.AppendLine("Введите второе число");
            if (String.IsNullOrWhiteSpace(WeightBox.Text))
                errors.AppendLine("Введите вес груза");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }    

            //Добавление новой записи площадки в базу данных
            if (_currentArea.Id == 0)
                CoalCompanyEntities.GetContext().Area.Add(_currentArea);

            //Переменная для хранения в себе Int значения выбранного второ числа
            int secondNum;

            //Если вторая строчка выбираемого числа пустая или хранит в себе выбранную автоматически строчку
            //То в переменную secondNum записывается значение первой строчки и создается формат одного числа
            //Иначе в переменную secondNum записывается значение второй строчки и формат двух чисел
            if (OptionalNumberText.Text == "необяз." || String.IsNullOrWhiteSpace(OptionalNumberText.Text))
            {
                OptionalNumberText.Text = FirstNumberText.Text;
                _currentArea.NumberArea = String.Format("{0}", FirstNumberText.Text);
                secondNum = Convert.ToInt32(FirstNumberText.Text);
            } else
            {
                _currentArea.NumberArea = String.Format("{0}-{1}", FirstNumberText.Text, OptionalNumberText.Text);
                secondNum = Convert.ToInt32(OptionalNumberText.Text);
            }

            //Перебор значений для добавления нескольки записей в таблицу
            for (int i = Convert.ToInt32(FirstNumberText.Text); i <= secondNum; i++)
            {
                //Данные записываются в строки таблицы бд Пикет
                _currentPicket.NumberAreaId = _currentArea.Id;
                _currentPicket.NumberPicket = i;
                _currentPicket.Weight = Convert.ToInt32(WeightBox.Text);
                _currentPicket.DateAdd = DateTime.Now;

                //Данные записываются в строки таблицы бд История изменения
                _currentHistoryChange.NumberAreaId = _currentArea.Id;
                _currentHistoryChange.NumberPicket = i;
                _currentHistoryChange.Weight = Convert.ToInt32(WeightBox.Text);
                _currentHistoryChange.DateChange = DateTime.Now;
                _currentHistoryChange.ChangeInfo = "Добавление площадки, добавление пикета";

                //Данные добавляются
                CoalCompanyEntities.GetContext().Picket.Add(_currentPicket);
                CoalCompanyEntities.GetContext().HistoryChange.Add(_currentHistoryChange);

                //Если ошибок не возникло данные сохраняются
                try
                {
                    CoalCompanyEntities.GetContext().SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            MessageBox.Show("Информация сохранена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
