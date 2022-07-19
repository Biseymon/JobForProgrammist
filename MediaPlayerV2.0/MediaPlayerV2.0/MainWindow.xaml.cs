using Microsoft.Win32;
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
using System.Windows.Threading;

namespace MediaPlayerV2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isPlay = true;
        private bool _isDrag = false;
        private bool _isEnter = false;
        private double _tempSliderValue = 0;
        


        public MainWindow()
        {
            InitializeComponent();

            // запуск таймера для обновления данных 60 раз в секунду
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 17);
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Обновление данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            lbl1.Content = mediaElement.NaturalDuration.ToString();

            if (mediaElement.NaturalDuration.HasTimeSpan)
                if (!_isDrag)
                {
                    timeSlider.Value = mediaElement.Position / mediaElement.NaturalDuration.TimeSpan;
                    lbl2.Content = mediaElement.Position.ToString();
                }
                else
                {
                    _isDrag = false;
                    mediaElement.Position = mediaElement.NaturalDuration.TimeSpan * _tempSliderValue;
                }
            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfiledialog = new OpenFileDialog();
            openfiledialog.Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV"; ;
            if (openfiledialog.ShowDialog() == true)
            {      
                mediaElement.Source = new Uri(openfiledialog.FileName);
                lblName.Content = "Открыт файл:" + openfiledialog.FileName;
                    
                _isPlay = true;
                btnPlay.Content = "Pause";
                mediaElement.Play();
            }
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlay)
            {
                _isPlay = false;
                btnPlay.Content = "Play";
                mediaElement.Pause();
            }
            else
            {
                _isPlay = true;
                btnPlay.Content = "Pause";
                mediaElement.Play();
            }
            
        }

        private void timeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            if (Mouse.LeftButton == MouseButtonState.Pressed && _isEnter)
            {
                _isDrag = true;
                _tempSliderValue = timeSlider.Value;
            }
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Position += new TimeSpan(0, 0, 10);
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Position -= new TimeSpan(0, 0, 10);
        }

        private void timeSlider_MouseEnter(object sender, MouseEventArgs e)
        {
            _isEnter = true;
        }

        private void timeSlider_MouseLeave(object sender, MouseEventArgs e)
        {
            _isEnter = false;
        }
    }
}
