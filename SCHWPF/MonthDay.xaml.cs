﻿using System;
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

namespace WpfScheduler
{
    /// <summary>
    /// Interaction logic for MonthDay.xaml
    /// </summary>
    public partial class MonthDay : UserControl
    {
        internal event EventHandler<Event> OnEventDoubleClick;
        internal event EventHandler<DateTime> OnScheduleDoubleClick;

        private bool _currentMonth;
        private DateTime _date;

        public MonthDay(DateTime dt, bool currentMonth)
        {
            InitializeComponent();
            _currentMonth = currentMonth;
            _date = dt.Date;

            if (dt.Day == 1) lbDay.Content = dt.ToString("m");
            else lbDay.Content = dt.Day;

            if (dt.Date == DateTime.Now.Date)
            {
                globalGrid.BorderBrush = Brushes.Black;
                globalGrid.Background = Brushes.LightGray;
            }
            if (!currentMonth && dt.Date != DateTime.Now.Date)
            {
                lbDay.Foreground = Brushes.LightGray;
            }
        }

        public List<Event> Events
        {
            set
            {
                ucEvents.Children.Clear();
                foreach (Event e in value.OrderBy(e => e.Start))
                {
                    EventUserControl wE = new EventUserControl(e, false);
                    wE.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

                    if (!_currentMonth && e.Start.Date != DateTime.Now.Date)
                    {
                        wE.Opacity = 0.5;
                    }

                    if (e.Start.Date < _date)
                    {
                        wE.BorderElement.Margin = new System.Windows.Thickness { Left = 0 };
                        wE.BorderElement.BorderThickness = new System.Windows.Thickness { Left = 0 };
                    }

                    if (e.End.Date > _date)
                    {
                        wE.BorderElement.Margin = new System.Windows.Thickness { Right = 0 };
                        wE.BorderElement.BorderThickness = new System.Windows.Thickness { Right = 0 };
                    }

                    wE.MouseDoubleClick += ((object sender, MouseButtonEventArgs ea) =>
                    {
                        ea.Handled = true;
                        OnEventDoubleClick(sender, wE.Event);
                    });

                    ucEvents.Children.Add(wE);
                }
            }
        }

        private void ucEvents_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                OnScheduleDoubleClick(sender, _date);
            }
        }

        private void globalGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                OnScheduleDoubleClick(sender, _date);
            }
        }
    }
}
