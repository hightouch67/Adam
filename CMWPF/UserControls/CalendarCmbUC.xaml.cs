using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace CMWPF.UserControls
{
    /// <summary>
    /// Logique d'interaction pour CalendarCmbUC.xaml
    /// </summary>
    public partial class CalendarCmbUC : UserControl
    {
        int[] Days = new int[31];
        string[] monthNames;
        List<int> years = new List<int>();
        public CalendarCmbUC()
        {
            InitializeComponent();
            initializeLists();
            LoadMonthsCombos();
            LoadYearCombo();
            initalizeDaysArray();
            LoadDaysCombo();
        }

        public bool CmbAreEnabled
        {
            get { return (bool)GetValue(CmbAreEnabledProperty); }
            set { SetValue(CmbAreEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CmbAreEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CmbAreEnabledProperty =
            DependencyProperty.Register("CmbAreEnabled", typeof(bool), typeof(CalendarCmbUC), new PropertyMetadata(false));

        public void initializeLists()
        {
            monthNames = DateTimeFormatInfo.CurrentInfo.MonthNames;
            years = Enumerable.Range(DateTime.Now.Year - 99, 100).ToList();
        }
        public void initalizeDaysArray()
        {
            int month = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(cmdMonths.SelectedValue.ToString()) + 1;
            Days = new int[DateTime.DaysInMonth(Convert.ToInt32(cmbYear.SelectedValue), month)] ;
            for (int i = 0; i < Days.Count(); i++)
            {
                Days[i] = i + 1;
            } 
        }
        public void LoadDaysCombo()
        {
            cmbDays.ItemsSource = Days;
            cmbDays.SelectedValue = DateTime.Now.Day;
        }
        public void LoadYearCombo()
        {
            cmbYear.ItemsSource = years;
            cmbYear.SelectedValue = DateTime.Now.Year;
        }
        public void LoadMonthsCombos()
        {

            cmdMonths.ItemsSource = monthNames;
            cmdMonths.SelectedValue = DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Now.Month);
        }

        private void cmdMonths_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmdMonths.SelectedValue != null)
            {
                initalizeDaysArray();
                LoadDaysCombo();
            }
        }

        private void cmbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbYear.SelectedValue != null)
            {
                initalizeDaysArray();
                LoadDaysCombo();
            }
        }

    }
}
