using CMWPF.Classes;
using CMWPF.Classes.Functions;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
    public partial class MyProfilUC : UserControl
    {
        public ObservableCollection<Patient> patientCollection { get; set; }
        public int currentUser { get; set; }
        public MyProfilUC()
        {
            InitializeComponent();
            patientCollection = new ObservableCollection<Patient>();
            this.DataContext = this;
        }

        public void AddPatientToPatientCollection(Patient patient)
        {
            patientCollection.Add(patient);
        }
        public void LoadCurrentPatient(int patientid)
        {
            ReadPatient PatientReader = new ReadPatient();
            patientCollection = PatientReader.GetOnePatient(patientid);
            Patient currentPatient = new Patient();
            currentPatient = patientCollection.Where(x => x.patientId == currentUser).FirstOrDefault();
            if (currentPatient != null)
            { 
            txtbName.Text = currentPatient.patientName;
            txtbLastName.Text = currentPatient.patientLastName;
            txtbBornWhere.Text = currentPatient.patientBornWhere;
            txtbAge.Text = currentPatient.patientAge;
            txtbAdress.Text = currentPatient.patientAdress;
            txtbTel.Text = currentPatient.patientTel;
            txtbMail.Text = currentPatient.patientMail;
            txtbMoreInfo.Text = currentPatient.patientMoreInfo;
            txtbDisease.Text = currentPatient.patientDisease;
            txtbDifficulty.Text = currentPatient.patientDifficulty;

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(currentPatient.patientPic);
            src.CacheOption = BitmapCacheOption.None;
            src.EndInit();
            imgPhoto.Source = src;
            }
        }
        private void SavePatient()
        {
            Patient oldpatient = new Patient();
            oldpatient = patientCollection.Where(x => x.patientId == currentUser).FirstOrDefault();
            if (oldpatient != null)
            {
                string hashcode = txtbName.Text + txtbLastName.Text;
                int code = hashcode.GetHashCode();
                oldpatient.patientId = code;
                oldpatient.patientName = txtbName.Text;
                oldpatient.patientLastName = txtbLastName.Text;
                oldpatient.patientAge = txtbAge.Text;
                oldpatient.patientBornWhere = txtbBornWhere.Text;
                oldpatient.patientAdress = txtbAdress.Text;
                oldpatient.patientTel = txtbTel.Text;
                oldpatient.patientMail = txtbMail.Text;
                oldpatient.patientMoreInfo = txtbMoreInfo.Text;
                oldpatient.patientDisease = txtbDisease.Text;
                oldpatient.patientDifficulty = txtbDifficulty.Text;
                DateTime compared = new DateTime();
                compared = Convert.ToDateTime("0001-01-01T00:00:00");
                if (oldpatient.patientCreationDate == compared)
                {
                    oldpatient.patientCreationDate = DateTime.Now;
                }
                oldpatient.patientLastModificationDate = DateTime.Now;
                string subPath = @"c:\CM\Patients\" + oldpatient.patientId;
                bool exists = Directory.Exists(subPath);

                if (!exists)
                    Directory.CreateDirectory(subPath);
                string imgpath = @"c:\CM\Patients\" + oldpatient.patientId + "\\" + oldpatient.patientLastName + " " + Guid.NewGuid() + ".png";
                if (imgPhoto.Source != null)
                {
                    SaveToPng(imgPhoto, imgpath);
                }
                oldpatient.patientPic = imgpath;
                patientCollection.Clear();
                AddPatientToPatientCollection(oldpatient);
                SaveCollectionToFile(patientCollection, @"c:\CM\Patients\" + oldpatient.patientId + ".json");

            }
            else
            {
                Patient newpatient = new Patient();
                string hashcode = txtbName.Text + txtbLastName.Text;
                int code = hashcode.GetHashCode();
                newpatient.patientId = code;
                newpatient.patientName = txtbName.Text;
                newpatient.patientLastName = txtbLastName.Text;
                newpatient.patientAge = txtbAge.Text;
                newpatient.patientBornWhere = txtbBornWhere.Text;
                newpatient.patientAdress = txtbAdress.Text;
                newpatient.patientTel = txtbTel.Text;
                newpatient.patientMail = txtbMail.Text;
                newpatient.patientMoreInfo = txtbMoreInfo.Text;
                newpatient.patientDisease = txtbDisease.Text;
                newpatient.patientDifficulty = txtbDifficulty.Text;
                DateTime compared = new DateTime();
                compared = Convert.ToDateTime("0001-01-01T00:00:00");
                if (newpatient.patientCreationDate == compared)
                {
                    newpatient.patientCreationDate = DateTime.Now;
                }
                newpatient.patientLastModificationDate = DateTime.Now;
                string subPath = @"c:\CM\Patients\" + newpatient.patientId;

                bool exists = Directory.Exists(subPath);

                if (!exists)
                    Directory.CreateDirectory(subPath);

                string imgpath = @"c:\CM\Patients\" + newpatient.patientId + "\\" + newpatient.patientLastName + " " + Guid.NewGuid() + ".png";
                if (imgPhoto.Source != null)
                {
                    SaveToPng(imgPhoto, imgpath);
                }
                newpatient.patientPic = imgpath;
                patientCollection.Clear();
                AddPatientToPatientCollection(newpatient);
                SaveCollectionToFile(patientCollection, @"c:\CM\Patients\" + newpatient.patientId + ".json");

            }
        }


        void SaveToPng(FrameworkElement visual, string fileName)
        {
            var encoder = new PngBitmapEncoder();
            SaveUsingEncoder(visual, fileName, encoder);
        }

        void SaveUsingEncoder(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            BitmapFrame frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);

            using (var stream = File.Create(fileName))
            {
                encoder.Save(stream);
            }
        }

        private void SaveCollectionToFile(object observablecollection, string path)
        {
            var jsonToOutput = JsonConvert.SerializeObject(observablecollection, Formatting.Indented);

            using (StreamWriter writetext = new StreamWriter(path))
            {
                writetext.WriteLine(jsonToOutput);
            }
            Debug.WriteLine(jsonToOutput);
        }

        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void btnModif_Click(object sender, RoutedEventArgs e)
        {
            ConfirmEditPopup.Visibility = Visibility.Visible;
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            ConfirmEditPopup.Visibility = Visibility.Collapsed;
            ConfirmCancelPopup.Visibility = Visibility.Collapsed;
            ConfirmSavePopup.Visibility = Visibility.Collapsed;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            ConfirmEditPopup.Visibility = Visibility.Collapsed;
            btnModif.IsEnabled = false;
            btnCancel.IsEnabled = true;
            btnSave.IsEnabled = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ConfirmCancelPopup.Visibility = Visibility.Visible;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ConfirmSavePopup.Visibility = Visibility.Visible;
        }

        private void btConfirmCancel_Click(object sender, RoutedEventArgs e)
        {
            ConfirmEditPopup.Visibility = Visibility.Collapsed;
            ConfirmCancelPopup.Visibility = Visibility.Collapsed;
            ConfirmSavePopup.Visibility = Visibility.Collapsed;
            btnModif.IsEnabled = true;
            btnCancel.IsEnabled = false;
            btnSave.IsEnabled = false;
        }

        private void btConfirmSave_Click(object sender, RoutedEventArgs e)
        {
            ConfirmEditPopup.Visibility = Visibility.Collapsed;
            ConfirmCancelPopup.Visibility = Visibility.Collapsed;
            ConfirmSavePopup.Visibility = Visibility.Collapsed;
            SavePatient();
            btnModif.IsEnabled = true;
            btnCancel.IsEnabled = false;
            btnSave.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            homegrid.Visibility = Visibility.Collapsed;
            MyInfo.Visibility = Visibility.Visible;
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            homegrid.Visibility = Visibility.Visible;
            MyInfo.Visibility = Visibility.Collapsed;
        }

        private void btnFriends_Click(object sender, RoutedEventArgs e)
        {
            homegrid.Visibility = Visibility.Collapsed;
            FriendsInfo.Visibility = Visibility.Visible;
        }

        private void btnbacktostep1a_Click(object sender, RoutedEventArgs e)
        {
            homegrid.Visibility = Visibility.Visible;
            FriendsInfo.Visibility = Visibility.Collapsed;
        }
    }
}
