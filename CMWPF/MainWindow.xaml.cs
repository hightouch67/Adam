using CMWPF.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
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

namespace CMWPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum State
        {
            Idle = 0,
            Accepting = 1,
            Off = 2,
        }

        private State RecogState = State.Off;
        private SpeechRecognitionEngine recognizer;
        private SpeechSynthesizer synthesizer = null;
        private int Hypothesized = 0;
        private int Recognized = 0;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void UserLoginUC_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            int p;
            p = loggerUC.currentlog;
            if (p != 0)
            {
                MyProfilUC.currentUser = p;
                currentName = loggerUC.currentname;
                MyProfilUC.LoadCurrentPatient(p);
                MySchedulerUC.currentUser = p;
                MySchedulerUC.LoadEventToCalendar();
                MyCareUC.currentUser = p;
                ReadAloud("Content de vous revoir " + currentName);

            }
        }

        string currentName;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //initialize recognizer and synthesizer
            InitializeRecognizerSynthesizer();

            //if input device found then proceed
            if (SelectInputDevice())
            {
                LoadDictationGrammar();
                ButtonStart.IsEnabled = true;
                ReadAloud("We we we négro bien ou quoi ? Tu peux commencer a ouvrir ta gueule !" + currentName);
            }
        }

        /// <summary>
        /// initialize recognizer and synthesizer along with their events
        /// /// </summary>
        private void InitializeRecognizerSynthesizer()
        {
            var selectedRecognizer = (from e in SpeechRecognitionEngine.InstalledRecognizers()
                                      where e.Culture.Equals(Thread.CurrentThread.CurrentCulture)
                                      select e).FirstOrDefault();
            recognizer = new SpeechRecognitionEngine(selectedRecognizer);
            recognizer.AudioStateChanged += new EventHandler<AudioStateChangedEventArgs>(recognizer_AudioStateChanged);
            recognizer.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(recognizer_SpeechHypothesized);
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

            synthesizer = new SpeechSynthesizer();
        }

        /// <summary>
        /// select input device if available
        /// </summary>
        /// <returns></returns>
        private bool SelectInputDevice()
        {
            bool proceedLoading = true;
            //if OS is above XP
            if (IsOscompatible())
            {
                try
                {
                    recognizer.SetInputToDefaultAudioDevice();
                }
                catch
                {
                    proceedLoading = false; //no audio input device
                }
            }
            //if OS is XP or below 
            else
                ThreadPool.QueueUserWorkItem(InitSpeechRecogniser);
            return proceedLoading;
        }

        /// <summary>
        /// Findout if OS is compatible. 
        /// </summary>
        /// <returns>true if greater than XP otherwise false</returns>
        private bool IsOscompatible()
        {
            OperatingSystem osInfo = Environment.OSVersion;
            if (osInfo.Version > new Version("6.0"))
                return true;
            else
                return false;
        }

        private void InitSpeechRecogniser(object o)
        {
            recognizer.SetInputToDefaultAudioDevice();
        }

        /// <summary>
        /// Load grammars, one for command and other for dictation
        /// </summary>
        private void LoadDictationGrammar()
        {
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(new Choices("Fin de la dictée"));
            grammarBuilder.Append(new Choices("Mon Calendrier","Calendrier", "mon calendrier", "calendrier", "calendriers", "Mon calendrier", "le calendrier", "de calendrier"));
            grammarBuilder.Append(new Choices("Mes Soins"));
            grammarBuilder.Append(new Choices("A faire"));
            grammarBuilder.Append(new Choices("Mon Journal", "Mon journal", "mon journal", "Journal", "journal", "Le journal", "le journal", "au journal", "en journal"));
            grammarBuilder.Append(new Choices("Renseignements personnels"));
            grammarBuilder.Append(new Choices("Mon Carnet", "Mon carnet", "mon carnet", "Carnet", "carnet", "Le carnet", "le Carnet", "de carnet", "au carnet", "en carnet"));
            Grammar commandGrammar = new Grammar(grammarBuilder);
            commandGrammar.Name = "main command grammar";
            recognizer.LoadGrammar(commandGrammar);

            DictationGrammar dictationGrammar = new DictationGrammar();
            dictationGrammar.Name = "Dictée";
            recognizer.LoadGrammar(dictationGrammar);
        }

        #region Recognizer events
        private void recognizer_AudioStateChanged(object sender, AudioStateChangedEventArgs e)
        {
            switch (e.AudioState)
            {
                case AudioState.Speech:
                    LabelStatus.Content = "Listening";
                    break;
                case AudioState.Silence:
                    LabelStatus.Content = "Idle";
                    break;
                case AudioState.Stopped:
                    LabelStatus.Content = "Stopped";
                    break;
            }
        }

        private void recognizer_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            Hypothesized++;
            LabelHypothesized.Content = "Hypothesized: " + Hypothesized.ToString();
        }
        private void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Recognized++;
            LabelRecognized.Content = "Recognized: " + Recognized.ToString();

            if (RecogState == State.Off)
                return;
            float accuracy = (float)e.Result.Confidence;
            string phrase = e.Result.Text;
            {
                if (phrase == "End Dictate")
                {
                    RecogState = State.Off;
                    recognizer.RecognizeAsyncStop();
                    ReadAloud("Dictation Ended");
                    return;
                }
                if (phrase == "Mon Calendrier" || phrase == "Calendrier" || phrase == "mon calendrier" || phrase == "calendrier" || phrase == "calendriers" || phrase == "Mon calendrier" || phrase == "le calendrier" || phrase == "de calendrier")
                {
                    RecogState = State.Off;
                    recognizer.RecognizeAsyncStop();
                    ReadAloud("Vous êtes sur votre calendrier");
                    TabControl.SelectedIndex = 0;
                    return;
                }
                if (phrase == "Mes Soins")
                {
                    RecogState = State.Off;
                    recognizer.RecognizeAsyncStop();
                    ReadAloud("Vous êtes sur Réeducations et Soins");
                    TabControl.SelectedIndex = 1;
                    return;
                }
                if (phrase == "A faire")
                {
                    RecogState = State.Off;
                    recognizer.RecognizeAsyncStop();
                    ReadAloud("Vous êtes sur la liste des choses à faire");
                    TabControl.SelectedIndex = 2;
                    return;
                }
                if (phrase == "Mon Journal" || phrase == "Mon journal" || phrase == "mon journal" || phrase == "Journal" || phrase == "journal" || phrase == "Le journal" || phrase == "le journal" || phrase == "au journal" || phrase == "en journal")
                {
                    RecogState = State.Off;
                    recognizer.RecognizeAsyncStop();
                    ReadAloud("Vous êtes sur votre journal");
                    TabControl.SelectedIndex = 3;
                    return;
                }
                if (phrase == "Renseignements personnels")
                {
                    RecogState = State.Off;
                    recognizer.RecognizeAsyncStop();
                    ReadAloud("Vous êtes sur le calendrier");
                    TabControl.SelectedIndex = 4;
                    return;
                }
                if (phrase == "Mon Carnet" || phrase == "Mon carnet" || phrase == "mon carnet" || phrase == "Carnet" || phrase == "carnet" || phrase == "Le carnet" || phrase == "le Carnet" || phrase == "de carnet" || phrase == "au carnet" || phrase == "en carnet")
                {
                    RecogState = State.Off;
                    recognizer.RecognizeAsyncStop();
                    ReadAloud("Vous êtes sur votre carnet");
                    TabControl.SelectedIndex = 5;
                    return;
                }
                ReadAloud("Prononcez plus clairement les mots. Merci.");
                
                //TextBox1.AppendText(" " + e.Result.Text);
            }
        }
        #endregion

        /// <summary>
        /// pause recognition and speak the text sent
        /// </summary>
        /// <param name="speakText"></param>
        public void ReadAloud(string speakText)
        {
            try
            {
                //recognizer.RecognizeAsyncCancel();
                synthesizer.SpeakAsync(speakText);
            }
            catch { }
        }


        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary myResourceDictionary = new ResourceDictionary();
            ResourceDictionary myResourceDictionary2 = new ResourceDictionary();
            ResourceDictionary myResourceDictionary3 = new ResourceDictionary();
            myResourceDictionary2.Source = new Uri(@"pack://application:,,,/CMWPF;component/Ressources/" + "Style" + ".xaml");
            myResourceDictionary3.Source = new Uri(@"pack://application:,,,/CMWPF;component/Converters/" + "Converters" + ".xaml");
            switch (RecogState)
            {
                case State.Off:
                    myResourceDictionary.Source = new Uri(@"pack://application:,,,/CMWPF;component/Ressources/" + "TextDictionnaryEN" + ".xaml");
                    //Application.Current.Resources.MergedDictionaries.Remove()
                    Application.Current.Resources.MergedDictionaries.Clear();
                    Application.Current.Resources.MergedDictionaries.Add(myResourceDictionary);
                    Application.Current.Resources.MergedDictionaries.Add(myResourceDictionary2);
                    Application.Current.Resources.MergedDictionaries.Add(myResourceDictionary3);

                    //RecogState = State.Accepting;
                    //ButtonStart.Content = "Stop";
                    //recognizer.RecognizeAsync(RecognizeMode.Multiple);
                    break;
                case State.Accepting:
                    myResourceDictionary.Source = new Uri(@"pack://application:,,,/CMWPF;component/Ressources/" + "TextDictionnaryFR" + ".xaml");
                    Application.Current.Resources.MergedDictionaries.Clear();
                    Application.Current.Resources.MergedDictionaries.Add(myResourceDictionary);
                    Application.Current.Resources.MergedDictionaries.Add(myResourceDictionary2);
                    Application.Current.Resources.MergedDictionaries.Add(myResourceDictionary3);
                    //RecogState = State.Off;
                    //ButtonStart.Content = "Start";
                    //recognizer.RecognizeAsyncStop();
                    break;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
