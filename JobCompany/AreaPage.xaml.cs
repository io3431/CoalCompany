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
    /// Interaction logic for AreaPage.xaml
    /// </summary>
    public partial class AreaPage : Page
    {
        public AreaPage()
        {
            InitializeComponent();
            //К DataGrid в качестве ресурса добавляется таблица базы данных Picket(Пикет)
            AreaDGrid.ItemsSource = CoalCompanyEntities.GetContext().Picket.ToList();
        }

        //Реализация обработчика событий кнопки, для добавления новых пикетов и площадки
        //Открывается окно AddAreaWindow с отцентрованным положением на экране
        //Чтобы у пользователя не было возможности закрыть главное окно и сломать приложение, использую метод ShowDialog()
        private void AddArea_Click(object sender, RoutedEventArgs e)
        {
            AddAreaWindow addArea = new AddAreaWindow();
            addArea.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addArea.ShowDialog();
        }

        //Реализация обработчика событий кнопки, для изменения названия веса Площадки и Пикета
        private void ChangeWeight_Click(object sender, RoutedEventArgs e)
        {
            //Объект передает значения выбранного пикета как значения из таблицы Picket базы данных
            ChangeWeightWindow changeWeight = new ChangeWeightWindow((sender as Button).DataContext as Picket)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            changeWeight.ShowDialog();
        }

        //Обработчик события кнопки, который позволяет обновить DataGrid с подлюченной базой данных
        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            AreaDGrid.ItemsSource = CoalCompanyEntities.GetContext().Picket.ToList();
        }
    }
}
