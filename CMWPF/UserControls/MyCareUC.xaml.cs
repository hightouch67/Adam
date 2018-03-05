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
    /// <summary>
    /// Logique d'interaction pour MyCareUC.xaml
    /// </summary>
    public partial class MyCareUC : UserControl
    {
        public ObservableCollection<Therapist> therapistCollection { get; set; }
        public ObservableCollection<Care> careCollection { get; set; }
        public int currentUser { get; set; }
        public int currentTherapist { get; set; }
        public int currentCare { get; set; }
        public MyCareUC()
        {
            InitializeComponent();
            therapistCollection = new ObservableCollection<Therapist>();
            careCollection = new ObservableCollection<Care>();
            this.DataContext = this;

        }



        public void AddTherapistToTherapistCollection(Therapist therapist)
        {
            therapistCollection.Add(therapist);
        }

        public void AddCareToCareCollection(Care care)
        {
            careCollection.Add(care);
        }
        public void LoadCurrentTherapist(int therapistid)
        {
            ReadTherapist TherapistReader = new ReadTherapist();
            therapistCollection = TherapistReader.GetOneTherapist(therapistid);
            Therapist currentTherapist = new Therapist();
            currentTherapist = therapistCollection.Where(x => x.therapistId == therapistid).FirstOrDefault();
            if (currentTherapist != null)
            {
                txtbName.Text = currentTherapist.therapistName;
                txtbLastName.Text = currentTherapist.therapistLastName;
                txtbAdress.Text = currentTherapist.therapistAdress;
                txtbFunction.Text = currentTherapist.therapistFunction;
                txtbTel.Text = currentTherapist.therapistTel;
                txtbMail.Text = currentTherapist.therapistMail;
                txtbMoreInfo.Text = currentTherapist.therapistMoreInfo;
                currentTherapist.patientsList.Add(currentUser.ToString());
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(currentTherapist.therapistPic);
                src.CacheOption = BitmapCacheOption.None;
                src.EndInit();
                imgPhoto.Source = src;
            }
        }
        private void SaveTherapist()
        {
            Therapist oldtherapist = new Therapist();
            oldtherapist = therapistCollection.Where(x => x.therapistId == currentTherapist).FirstOrDefault();
            if (oldtherapist != null)
            {
                string hashcode = txtbName.Text + txtbLastName.Text;
                int code = hashcode.GetHashCode();
                oldtherapist.therapistId = code;
                oldtherapist.therapistName = txtbName.Text;
                oldtherapist.therapistLastName = txtbLastName.Text;
                oldtherapist.therapistAdress = txtbAdress.Text;
                oldtherapist.therapistFunction = txtbAdress.Text;
                oldtherapist.therapistTel = txtbTel.Text;
                oldtherapist.therapistMail = txtbMail.Text;
                oldtherapist.therapistMoreInfo = txtbMoreInfo.Text;
                oldtherapist.patientsList.Add(currentUser.ToString());
                DateTime compared = new DateTime();
                compared = Convert.ToDateTime("0001-01-01T00:00:00");
                if (oldtherapist.therapistCreationDate == compared)
                {
                    oldtherapist.therapistCreationDate = DateTime.Now;
                }
                oldtherapist.therapistLastModificationDate = DateTime.Now;
                string subPath = @"c:\CM\Patients\" + currentUser.ToString() + "\therapists" + oldtherapist.therapistId;
                bool exists = Directory.Exists(subPath);

                if (!exists)
                    Directory.CreateDirectory(subPath);
                string imgpath = @"c:\CM\Patients\" + currentUser.ToString() + "\\" + oldtherapist.therapistLastName + " " + Guid.NewGuid() + ".png";
                if (imgPhoto.Source != null)
                {
                    SaveToPng(imgPhoto, imgpath);
                }
                oldtherapist.therapistPic = imgpath;
                therapistCollection.Clear();
                AddTherapistToTherapistCollection(oldtherapist);
                SaveCollectionToFile(therapistCollection, @"c:\CM\Patients\" + currentUser.ToString() + "\\" + oldtherapist.therapistId + ".json");
            }
            else
            {
                Therapist newTherapist = new Therapist();
                string hashcode = txtbName.Text + txtbLastName.Text;
                int code = hashcode.GetHashCode();
                newTherapist.therapistId = code;
                newTherapist.therapistName = txtbName.Text;
                newTherapist.therapistLastName = txtbLastName0.Text;
                newTherapist.therapistAdress = txtbAdress.Text;
                newTherapist.therapistFunction = txtbFunction.Text;
                newTherapist.therapistTel = txtbTel.Text;
                newTherapist.therapistMail = txtbMail.Text;
                newTherapist.therapistMoreInfo = txtbMoreInfo.Text;
                //newTherapist.patientsList.Add(currentUser.ToString());e

                DateTime compared = new DateTime();
                compared = Convert.ToDateTime("0001-01-01T00:00:00");
                if (newTherapist.therapistCreationDate == compared)
                {
                    newTherapist.therapistCreationDate = DateTime.Now;
                }
                newTherapist.therapistLastModificationDate = DateTime.Now;
                string subPath = @"c:\CM\Patients\" + currentUser.ToString() + "\\" + newTherapist.therapistId;

                bool exists = Directory.Exists(subPath);

                if (!exists)
                    Directory.CreateDirectory(subPath);

                string imgpath = @"c:\CM\Patients\" + currentUser.ToString() + "\\" + newTherapist.therapistLastName + " " + Guid.NewGuid() + ".png";
                if (imgPhoto.Source != null)
                {
                    SaveToPng(imgPhoto, imgpath);
                }
                newTherapist.therapistPic = imgpath;
                AddTherapistToTherapistCollection(newTherapist);
                SaveCollectionToFile(therapistCollection, @"c:\CM\Patients\" + currentUser.ToString() + "\\therapists.json");
                therapistCollection.Clear();

            }
        }



        private void SaveCare()
        {
            Care oldcare = new Care();
            oldcare = careCollection.Where(x => x.careId == currentCare).FirstOrDefault();
            if (oldcare != null)
            {
                string hashcode = txtbName.Text + txtbLastName.Text;
                int code = hashcode.GetHashCode();
                oldcare.careId = code;
                oldcare.careName = txtbCareName.Text;
                oldcare.careFunction = txtbCareFunction.Text;
                oldcare.carePrescriptor = txtbPrescriptor.Text;
                oldcare.carePoso = txtbPoso.Text;
                oldcare.careDuring = txtbCareDuring.Text;
                oldcare.careEndDate = txtbCareEnd.Text;
                oldcare.careSecondaryEffect = txtbSecondaryEffects.Text;
                oldcare.careComplementaryInfo = txtbComplementaryInfo.Text;
                DateTime compared = new DateTime();
                compared = Convert.ToDateTime("0001-01-01T00:00:00");
                string subPath = @"c:\CM\Patients\" + currentUser.ToString() + "\therapists" + oldcare.careId;
                bool exists = Directory.Exists(subPath);

                if (!exists)
                    Directory.CreateDirectory(subPath);
                string imgpath = @"c:\CM\Patients\" + currentUser.ToString() + "\therapists" + oldcare.careId + " " + Guid.NewGuid() + ".png";
                if (imgPhoto.Source != null)
                {
                    SaveToPng(imgPhoto, imgpath);
                }
                oldcare.carePic1 = imgpath;
                therapistCollection.Clear();
                AddCareToCareCollection(oldcare);
                SaveCollectionToFile(therapistCollection, @"c:\CM\Patients\" + currentUser.ToString() + "\\" + oldcare.careId + ".json");
            }
            else
            {
                Care newcare = new Care();
                string hashcode = txtbName.Text + txtbLastName.Text;
                int code = hashcode.GetHashCode();
                newcare.careId = code;
                newcare.careName = txtbCareName.Text;
                newcare.careFunction = txtbCareFunction.Text;
                newcare.carePrescriptor = txtbPrescriptor.Text;
                newcare.carePoso = txtbPoso.Text;
                newcare.careDuring = txtbCareDuring.Text;
                newcare.careEndDate = txtbCareEnd.Text;
                newcare.careSecondaryEffect = txtbSecondaryEffects.Text;
                newcare.careComplementaryInfo = txtbComplementaryInfo.Text;
                //newTherapist.patientsList.Add(currentUser.ToString());e

                DateTime compared = new DateTime();
                compared = Convert.ToDateTime("0001-01-01T00:00:00");
                //if (newcare.therapistCreationDate == compared)
                //{
                //    newcare.therapistCreationDate = DateTime.Now;
                //}
                //newTherapist.therapistLastModificationDate = DateTime.Now;
                string subPath = @"c:\CM\Patients\" + currentUser.ToString() + "\\" + newcare.careId;

                bool exists = Directory.Exists(subPath);

                if (!exists)
                    Directory.CreateDirectory(subPath);

                string imgpath = @"c:\CM\Patients\" + currentUser.ToString() + "\\" + newcare.careName + " " + Guid.NewGuid() + ".png";
                string imgpath2 = @"c:\CM\Patients\" + currentUser.ToString() + "\\" + newcare.careName + " " + Guid.NewGuid() + ".png";

                if (imgPhoto1.Source != null)
                {
                    SaveToPng(imgPhoto1, imgpath);
                }
                if (imgPhoto2.Source != null)
                {
                    SaveToPng(imgPhoto2, imgpath2);
                }
                newcare.carePic1 = imgpath;
                newcare.carePic2 = imgpath2;
                AddCareToCareCollection(newcare);
                SaveCollectionToFile(careCollection, @"c:\CM\Patients\" + currentUser.ToString() + "\\care.json");
                careCollection.Clear();

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Therapist thera = new Therapist();
            AddTherapistToTherapistCollection(thera);
        }

        private void btnli_Click(object sender, RoutedEventArgs e)
        {
            Choose.Visibility = Visibility.Collapsed;
            StepTherapist.Visibility = Visibility.Visible;
            ReadTherapist readthera = new ReadTherapist();
            therapistCollection = readthera.GetAllTherapists(currentUser);
            therapistlist.ItemsSource = therapistCollection;
            btnnexttherapist_Click(sender, e);
        }

        private void btnbacktostep1_Click(object sender, RoutedEventArgs e)
        {
            Choose.Visibility = Visibility.Visible;
            StepTherapist.Visibility = Visibility.Collapsed;
            StepCare.Visibility = Visibility.Collapsed;
        }

        private void btnlistofthera_Click(object sender, RoutedEventArgs e)
        {
            TherapistList.Visibility = Visibility.Visible;
            StepTherapist.Visibility = Visibility.Collapsed;
        }

        private void btnsavethera_Click(object sender, RoutedEventArgs e)
        {
            Choose.Visibility = Visibility.Collapsed;
            StepTherapist.Visibility = Visibility.Collapsed;
            StepCare.Visibility = Visibility.Collapsed;
            TherapistInfo.Visibility = Visibility.Visible;
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

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            Choose.Visibility = Visibility.Collapsed;
            StepTherapist.Visibility = Visibility.Collapsed;
            StepCare.Visibility = Visibility.Visible;
            CareInfo.Visibility = Visibility.Collapsed;
            CareList.Visibility = Visibility.Collapsed;
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
            SaveTherapist();
            btnModif.IsEnabled = true;
            btnCancel.IsEnabled = false;
            btnSave.IsEnabled = false;
        }

        private void btnprevioustherapist_Click(object sender, RoutedEventArgs e)
        {
            therapistlist.Focus();
            if (therapistlist.SelectedIndex > 0)
            {
                therapistlist.SelectedIndex = therapistlist.SelectedIndex - 1;
                therapistlist.SelectedItem = therapistlist.SelectedIndex;
                therapistlist.ScrollIntoView(therapistlist.SelectedItem);
            }
        }

        private void btnnexttherapist_Click(object sender, RoutedEventArgs e)
        {
            therapistlist.Focus();
            if (therapistlist.SelectedIndex < therapistlist.Items.Count - 1)
            {
                therapistlist.SelectedIndex = therapistlist.SelectedIndex + 1;
                therapistlist.SelectedItem = therapistlist.SelectedIndex;
                therapistlist.ScrollIntoView(therapistlist.SelectedItem);
            }
        }

        private void btns_Click(object sender, RoutedEventArgs e)
        {
            Choose.Visibility = Visibility.Collapsed;
            StepCare.Visibility = Visibility.Visible;
            ReadCare readcare = new ReadCare();

            careCollection = readcare.GetAllCare(currentUser);
            carelist.ItemsSource = careCollection;
            btnnexttherapist_Click(sender, e);
        }

        private void btnsavecare_Click(object sender, RoutedEventArgs e)
        {
            Choose.Visibility = Visibility.Collapsed;
            StepTherapist.Visibility = Visibility.Collapsed;
            StepCare.Visibility = Visibility.Collapsed;
            CareInfo.Visibility = Visibility.Visible;
        }

        private void btnlistofcare_Click(object sender, RoutedEventArgs e)
        {
            CareList.Visibility = Visibility.Visible;
            StepCare.Visibility = Visibility.Collapsed;
        }

        private void btConfirmSaveCare_Click(object sender, RoutedEventArgs e)
        {
            ConfirmEditPopup1.Visibility = Visibility.Collapsed;
            ConfirmCancelPopup1.Visibility = Visibility.Collapsed;
            ConfirmSavePopup1.Visibility = Visibility.Collapsed;
            //SaveCare();
        }

        private void btnSave1_Click(object sender, RoutedEventArgs e)
        {
            ConfirmEditPopup1.Visibility = Visibility.Visible;

        }

        private void btnConfirm1_Click(object sender, RoutedEventArgs e)
        {
            ConfirmEditPopup1.Visibility = Visibility.Collapsed;
            ConfirmCancelPopup1.Visibility = Visibility.Collapsed;
            ConfirmSavePopup1.Visibility = Visibility.Collapsed;
            SaveCare();
            btnModif1.IsEnabled = true;
            btnCancel1.IsEnabled = false;
            btnSave1.IsEnabled = false;
        }

        private void btnpreviouscare_Click(object sender, RoutedEventArgs e)
        {
            carelist.Focus();
            if (carelist.SelectedIndex > 0)
            {
                carelist.SelectedIndex = carelist.SelectedIndex - 1;
                carelist.SelectedItem = carelist.SelectedIndex;
                carelist.ScrollIntoView(carelist.SelectedItem);
            }
        }

        private void btnnextcare_Click(object sender, RoutedEventArgs e)
        {
            carelist.Focus();
            if (carelist.SelectedIndex < carelist.Items.Count - 1)
            {
                carelist.SelectedIndex = carelist.SelectedIndex + 1;
                carelist.SelectedItem = carelist.SelectedIndex;
                carelist.ScrollIntoView(carelist.SelectedItem);
            }
        }

        private void Control_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgPhoto1.Source = new BitmapImage(new Uri(op.FileName));
                ctrl1.Visibility = Visibility.Collapsed;
            }
        }

        private void Control_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgPhoto2.Source = new BitmapImage(new Uri(op.FileName));
                ctrl2.Visibility = Visibility.Collapsed;
            }
        }

        private void btnBackFromCareList_Click(object sender, RoutedEventArgs e)
        {
            Choose.Visibility = Visibility.Collapsed;
            StepTherapist.Visibility = Visibility.Collapsed;
            StepCare.Visibility = Visibility.Visible;
            CareInfo.Visibility = Visibility.Collapsed;
            CareList.Visibility = Visibility.Collapsed;
        }

        private void btnfindcare_Click(object sender, RoutedEventArgs e)
        {
            Choose.Visibility = Visibility.Collapsed;
            StepTherapist.Visibility = Visibility.Collapsed;
            StepCare.Visibility = Visibility.Collapsed;
            CareInfo.Visibility = Visibility.Collapsed;
            FindCare.Visibility = Visibility.Visible;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            ObservableCollection<Care> caresearch = new ObservableCollection<Care>();
            caresearch.Clear();
            carelist1.ItemsSource = caresearch;

            //carelist.Items.Clear();
            Care findcare = new Care();
            string txt = txtbSearchCare.Text;
            findcare = careCollection.Where(x => x.careName == txtbSearchCare.Text).FirstOrDefault();
            if (findcare != null)
            {
                caresearch.Clear();
                caresearch.Add(findcare);
                carelist1.ItemsSource = caresearch;
            }
        }
    }
}
