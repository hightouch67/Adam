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
using WpfScheduler;

namespace CMWPF.UserControls
{
    /// <summary>
    /// Logique d'interaction pour MyScheduler.xaml
    /// </summary>
    public partial class MySchedulerUC : UserControl
    {

        ObservableCollection<Event> eventCollection;
        CollectionSaver collsave;
        public int currentUser;
        public MySchedulerUC()
        {
            InitializeComponent();
            eventCollection = new ObservableCollection<Event>();
            collsave = new CollectionSaver();
            setDayToIcon();
            this.DataContext = this;
        }

        private void AddEventToCollection()
        {
            Event currentEvent = new Event();
            currentEvent.Subject = txtbkeyword.Text;
            //string dateTime = dtDay + " " + tpHour;
            string dateTime = dtDay.SelectedDate.Value.ToShortDateString() + " " + tpHour.Text;
            DateTime dt = Convert.ToDateTime(dateTime);
            string dateTime2 = dtDay.SelectedDate.Value.ToShortDateString() + " " + tpHour.Text;
            DateTime dt2 = Convert.ToDateTime(dateTime2);
            var test = cmbduring.Text;
            switch (cmbduring.Text)
            {
                case "15 minutes":
                    currentEvent.End = dt2.AddMinutes(15);
                    break;
                case "30 minutes":
                    currentEvent.End = dt2.AddMinutes(30);
                    break;
                case "45 minutes":
                    currentEvent.End = dt2.AddMinutes(45);
                    break;
                case "1 heure":
                    currentEvent.End = dt2.AddHours(1);
                    break;
                case "2 heures":
                    currentEvent.End = dt2.AddHours(2);
                    break;
                case "3 heures":
                    currentEvent.End = dt2.AddHours(3);
                    break;
                case "4 heures":
                    currentEvent.End = dt2.AddHours(4);
                    break;
                case "8 heures":
                    currentEvent.End = dt2.AddHours(8);
                    break;
            }
            var converter = new BrushConverter();
            if ((Brush)converter.ConvertFromString(colorPicker.SelectedColor.Value.ToString()) != null)
            {
                var brush = (Brush)converter.ConvertFromString(colorPicker.SelectedColor.Value.ToString());
                currentEvent.Color = brush;
            }
            currentEvent.Start = dt;
            //currentEvent.End = dt2;
            eventCollection.Add(currentEvent);
            collsave.SaveCollectionToFile(eventCollection, @"c:\CM\Patients\" + currentUser.ToString() + "\\Events.json");
        }

        public void LoadEventToCalendar()
        {
            ReadEvent reader = new ReadEvent();
            eventCollection.Clear();
            scheduler.Events.Clear();
            eventCollection = reader.ReadEventByPatientId(currentUser);
            foreach (Event e in eventCollection)
            {
                if (ConfirmEventPopup.Visibility == Visibility.Visible)
                {
                    //ConfirmEventPopup.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Event eventToAdd = new Event();
                    eventToAdd = e;
                    if (e.End < DateTime.Now)
                    {
                        ConfirmEventPopup.Visibility = Visibility.Visible;
                        txtEvent.Text = e.Subject;
                        txtDate.Text = e.Start.ToLongDateString();
                        btnAcceptEvent.Tag = e.Id;
                        btnDeclineEvent.Tag = e.Id;
                        btnAcceptStep.Tag = e.Id;
                        btnDeclineStep.Tag = e.Id;
                    }
                    scheduler.AddEvent(eventToAdd);
                }
            }
        }

        private void setDayToIcon()
        {
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            txtDateOfDayIcon.Text = dt.ToShortDateString();
         }

        private void btnview_Click(object sender, RoutedEventArgs e)
        {
            Step1.Visibility = Visibility.Collapsed;
            ViewGrid.Visibility = Visibility.Visible;
            LoadEventToCalendar();
        }

        private void btnbacktostep1_Click(object sender, RoutedEventArgs e)
        {
            Step1.Visibility = Visibility.Visible;
            ViewGrid.Visibility = Visibility.Collapsed;
        }

        private void btncalbyday_Click(object sender, RoutedEventArgs e)
        {
            ViewGrid.Visibility = Visibility.Collapsed;
            CalendarGrid.Visibility = Visibility.Visible;
            scheduler.Mode = WpfScheduler.Mode.Day;
            scheduler.SelectedDate = DateTime.Now;
        }

        private void btncalbyweek_Click(object sender, RoutedEventArgs e)
        {
            ViewGrid.Visibility = Visibility.Collapsed;
            CalendarGrid.Visibility = Visibility.Visible;
            scheduler.Mode = WpfScheduler.Mode.Week;
            scheduler.SelectedDate = DateTime.Now;
        }

        private void btncalbymonth_Click(object sender, RoutedEventArgs e)
        {
            ViewGrid.Visibility = Visibility.Collapsed;
            CalendarGrid.Visibility = Visibility.Visible;
            scheduler.Mode = WpfScheduler.Mode.Month;
            scheduler.SelectedDate = DateTime.Now;
        }

        private void btnbacktostep1_Click_1(object sender, RoutedEventArgs e)
        {
            Step1.Visibility = Visibility.Visible;
            ViewGrid.Visibility = Visibility.Collapsed;
            CalendarGrid.Visibility = Visibility.Collapsed;
        }

        private void btnbacktostep2_Click(object sender, RoutedEventArgs e)
        {
            Step1.Visibility = Visibility.Collapsed;
            ViewGrid.Visibility = Visibility.Visible;
            CalendarGrid.Visibility = Visibility.Collapsed;
        }

        private void btnedit_Click(object sender, RoutedEventArgs e)
        {
            Step1.Visibility = Visibility.Collapsed;
            EditGrid.Visibility = Visibility.Visible;
        }

        private void btnAddNewEvent_Click(object sender, RoutedEventArgs e)
        {
            EditGrid.Visibility = Visibility.Collapsed;
            AddEventGrid.Visibility = Visibility.Visible;
        }

        private void btnSaveNewEvent_Click(object sender, RoutedEventArgs e)
        {
            AddEventToCollection();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Event ev = new Event();
            ev = eventCollection.Where(x => x.Id.ToString() == btn.Tag.ToString()).FirstOrDefault();
            ev.IsDone = true;
            ev.IsAccepted = true;
            ConfirmEventPopup.Visibility = Visibility.Collapsed;
            collsave.SaveCollectionToFile(eventCollection, @"c:\CM\Patients\" + currentUser.ToString() + "\\Events.json");
            //LoadEventToCalendar();
        }

        private void btnDeclineEvent_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Event ev = new Event();
            ev = eventCollection.Where(x => x.Id.ToString() == btn.Tag.ToString()).FirstOrDefault();
            ev.IsDone = true;
            ev.IsCanceled = true;
            ConfirmEventPopup.Visibility = Visibility.Collapsed;
            collsave.SaveCollectionToFile(eventCollection, @"c:\CM\Patients\" + currentUser.ToString() + "\\Events.json");
        }

        private void btnAcceptStep_Click(object sender, RoutedEventArgs e)
        {
            ChooseStep.Visibility = Visibility.Collapsed;
            ConfirmStep.Visibility = Visibility.Visible;
        }

        private void btnDeclineStep_Click(object sender, RoutedEventArgs e)
        {
            ChooseStep.Visibility = Visibility.Collapsed;
            ConfirmStep.Visibility = Visibility.Visible;

        }

        private void btnbackto_Click(object sender, RoutedEventArgs e)
        {
            Step1.Visibility = Visibility.Collapsed;
            ViewGrid.Visibility = Visibility.Collapsed;
            CalendarGrid.Visibility = Visibility.Collapsed;
            AddEventGrid.Visibility = Visibility.Collapsed;
            EditGrid.Visibility = Visibility.Visible;
        }

        private void btnbacktostep1a_Click(object sender, RoutedEventArgs e)
        {
            Step1.Visibility = Visibility.Visible;
            ViewGrid.Visibility = Visibility.Collapsed;
            CalendarGrid.Visibility = Visibility.Collapsed;
            AddEventGrid.Visibility = Visibility.Collapsed;
            EditGrid.Visibility = Visibility.Collapsed;
        }

        private void btnfind_Click(object sender, RoutedEventArgs e)
        {
            SearchGrid.Visibility = Visibility.Visible;
            Step1.Visibility = Visibility.Collapsed;
            ViewGrid.Visibility = Visibility.Collapsed;
            EventList.ItemsSource = eventCollection;

        }

        private void btnpreviousevent_Click(object sender, RoutedEventArgs e)
        {
            EventList.Focus();
            if (EventList.SelectedIndex > 0)
            {
                EventList.SelectedIndex = EventList.SelectedIndex - 1;
                EventList.SelectedItem = EventList.SelectedIndex;
                EventList.ScrollIntoView(EventList.SelectedItem);
            }
        }

        private void btnnextevent_Click(object sender, RoutedEventArgs e)
        {
            EventList.Focus();
            if (EventList.SelectedIndex < EventList.Items.Count - 1)
            {
                EventList.SelectedIndex = EventList.SelectedIndex + 1;
                EventList.SelectedItem = EventList.SelectedIndex;
                EventList.ScrollIntoView(EventList.SelectedItem);
            }
        }

        private void txtbsearchevent_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBlock txtbsearch = sender as TextBlock;
           
        }
    }
}
