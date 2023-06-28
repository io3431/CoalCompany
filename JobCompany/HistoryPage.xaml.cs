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
    /// Interaction logic for HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {
        public HistoryPage()
        {
            InitializeComponent();
            HistoryDGrid.ItemsSource = CoalCompanyEntities.GetContext().HistoryChange.ToList();
        }

        private void AllTimeBtn_Click(object sender, RoutedEventArgs e)
        {
            HistoryDGrid.ItemsSource = CoalCompanyEntities.GetContext().HistoryChange.ToList();
        }

        private void DataSelectedPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            HistoryDGrid.ItemsSource = CoalCompanyEntities.GetContext().HistoryChange.Where(p => p.DateChange == DataSelectedPicker.SelectedDate).ToList();
        }
    }
}
