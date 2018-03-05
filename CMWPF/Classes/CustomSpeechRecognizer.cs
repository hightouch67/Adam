using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CMWPF.Classes
{
    public class CustomSpeechRecognizer
    {
        //Enum to hold the recognizer's state
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

        public void StartRecognizer()
        {
            //initialize recognizer and synthesizer
            InitializeRecognizerSynthesizer();

            //if input device found then proceed
            if (SelectInputDevice())
            {
                LoadDictationGrammar();
                //ButtonStart.IsEnabled = true;
                ReadAloud("Vous pouvez commencer la dictée");
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
                System.Threading.ThreadPool.QueueUserWorkItem(InitSpeechRecogniser);
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
            Grammar commandGrammar = new Grammar(grammarBuilder);
            commandGrammar.Name = "main command grammar";
            recognizer.LoadGrammar(commandGrammar);

            DictationGrammar dictationGrammar = new DictationGrammar();
            dictationGrammar.Name = "dictée";
            recognizer.LoadGrammar(dictationGrammar);
        }

        #region Recognizer events
        private void recognizer_AudioStateChanged(object sender, AudioStateChangedEventArgs e)
        {
            switch (e.AudioState)
            {
                case AudioState.Speech:
                    //LabelStatus.Content = "Listening";
                    break;
                case AudioState.Silence:
                    //LabelStatus.Content = "Idle";
                    break;
                case AudioState.Stopped:
                    //LabelStatus.Content = "Stopped";
                    break;
            }
        }

        private void recognizer_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            Hypothesized++;
            //LabelHypothesized.Content = "Hypothesized: " + Hypothesized.ToString();
        }
        private void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Recognized++;
           // LabelRecognized.Content = "Recognized: " + Recognized.ToString();

            if (RecogState == State.Off)
                return;
            float accuracy = (float)e.Result.Confidence;
            string phrase = e.Result.Text;
            {
                if (phrase == "Fin de la dictée")
                {
                    RecogState = State.Off;
                    recognizer.RecognizeAsyncStop();
                    ReadAloud("Dictée terminée");
                    return;
                }
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
                recognizer.RecognizeAsyncCancel();
                synthesizer.SpeakAsync(speakText);
            }
            catch { }
        }


        //private void ButtonStart_Click()
        //{
        //    switch (RecogState)
        //    {
        //        case State.Off:
        //            RecogState = State.Accepting;
        //           // ButtonStart.Content = "Stop";
        //            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        //            break;
        //        case State.Accepting:
        //            RecogState = State.Off;
        //           // ButtonStart.Content = "Start";
        //            recognizer.RecognizeAsyncStop();
        //            break;
        //    }

        //}
    }
}
