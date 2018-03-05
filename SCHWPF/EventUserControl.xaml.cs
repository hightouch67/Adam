using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfScheduler
{
    /// <summary>
    /// Interaction logic for EventUserControl.xaml
    /// </summary>
    public partial class EventUserControl : UserControl
    {
        Event _e;

        public EventUserControl(Event e, bool showTime)
        {
            InitializeComponent();

            _e = e;

            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.Subject = e.Subject;
            this.BorderElement.Background = e.Color;
            if (!showTime || e.AllDay)
            {
                //this.DisplayDateText.Visibility = System.Windows.Visibility.Hidden;
                //this.DisplayDateText.Height = 0;
                //this.DisplayDateText.Text = String.Format("{0} - {1}", e.Start.ToShortDateString(), e.End.ToShortDateString());
                //this.DisplayDateLongText.Text = e.Start.ToLongDateString();
                this.DisplayDescriptionText.Text = e.Subject.ToString();
            }
            else
            {
                //this.DisplayDateText.Text = String.Format("{0} - {1}", e.Start.ToString("HH:mm"), e.End.ToString("HH:mm"));
                //this.DisplayDateLongText.Text = e.Start.ToLongDateString();
                this.DisplayDescriptionText.Text = e.Subject.ToString();
                if (e.End < DateTime.Now)
                {
                    this.DisplayDescriptionText.TextDecorations = TextDecorations.Strikethrough;
                    this.Opacity = 0.5;

                    if (e.IsDone == true)
                    {
                        chkAccepted.Visibility = Visibility.Visible;
                    }
                    if (e.IsCanceled == true)
                    {
                        chkDeclined.Visibility = Visibility.Visible;
                    }
                    DropShadowEffect myDropShadowEffect = new DropShadowEffect();
                    Color myShadowColor = new Color();
                    myShadowColor.ScA = 1;
                    myShadowColor.ScB = 0;
                    myShadowColor.ScG = 0;
                    myShadowColor.ScR = 0;
                    myDropShadowEffect.Color = myShadowColor;
                    myDropShadowEffect.Direction = 320;
                    myDropShadowEffect.ShadowDepth = 0;
                    myDropShadowEffect.Opacity = 0;
                    myDropShadowEffect.BlurRadius = 0;
                    this.Effect = null;
                    BorderElement.Effect = null;
                }
            }
            //ToolTip t = new System.Windows.Controls.ToolTip();
            //t.SetValue(Control.StyleProperty, "ToolTip");
            //t.ToolTip = this.DisplayText.Text + Environment.NewLine + String.Format("{0} - {1}", e.Start.ToString("HH:mm"), e.End.ToString("HH:mm"));
            this.BorderElement.ToolTip = this.DisplayText.Text
                                         + Environment.NewLine
                                         + String.Format("{0} {1} {2} {3} ", "De", e.Start.ToString("HH:mm"), "heures à", e.End.ToString("HH:mm"))
                                         + Environment.NewLine
                                         ;
        }

        public Event Event
        {
            get { return _e; }
        }

        #region Subject
        public static readonly DependencyProperty SubjectProperty =
            DependencyProperty.Register("Subject", typeof(string), typeof(EventUserControl),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(AdjustSubject)));

        public string Subject
        {
            get { return (string)GetValue(SubjectProperty); }
            set { SetValue(SubjectProperty, value); }
        }

        private static void AdjustSubject(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as EventUserControl).DisplayText.Text = (string)e.NewValue;
        }
        #endregion
    }



}
