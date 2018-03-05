using CMWPF.Classes;
using CMWPF.Classes.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour UserLoginUC.xaml
    /// </summary>
    public partial class UserLoginUC : UserControl
    {
        public ObservableCollection<Patient> patientCollection { get; set; }
        public int currentlog { get; set; }
        public string currentname { get; set; }
        public string currentLastName { get; set; }
        public UserLoginUC()
        {
            InitializeComponent();
            this.DataContext = this;
            patientCollection = new ObservableCollection<Patient>();
            LoadPatients();
        }

        private void LoadPatients()
        {
            ReadPatient PatientReader = new ReadPatient();
            patientCollection = PatientReader.GetAllPatients();
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = sender as ListView;
            Patient logpatient = new Patient();
            logpatient = lv.SelectedItem as Patient;
            txtWelcomeName.Text = logpatient.patientName;
            currentname = logpatient.patientName;
            currentLastName = logpatient.patientLastName;
            currentlog = logpatient.patientId;
            txtbPassOn.Text = logpatient.patientPrint;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtbPass.Text == txtbPassOn.Text)
            {
                this.Visibility = Visibility.Collapsed;
            }
            else
                txtwrongpass.Visibility = Visibility.Visible;
                txtwelcome.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void txtbPass_SelectionChanged(object sender, RoutedEventArgs e)
        {
            txtwrongpass.Visibility = Visibility.Collapsed;
            txtwelcome.Visibility = Visibility.Visible;
        }
    }
}
