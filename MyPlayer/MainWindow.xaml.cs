using System;
using System.Collections.Generic;
using System.Data.Common;
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

namespace MyPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();

        TimeSpan ts = new TimeSpan();
        TimeSpan tsProgress = new TimeSpan();
        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = TimeSpan.FromSeconds(1);
        }

        private void Button_ClickOpen(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Filter = "Video |*.avi|Video |*.mp4|Video |*.mpeg|Audio |*.mp3"; // Filter files by extension
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    mediaElement.Source = new Uri(dlg.FileName);
                    lbPlaylist.Items.Add(dlg.FileName);
                    lbPlaylist.Focus();
                    lbPlaylist.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exeption");    
            }
            
        }
        private void Button_ClickPlay(object sender, RoutedEventArgs e)
        {
            if (mediaElement.LoadedBehavior == MediaState.Pause)
                timer.Start();                   // чтобы продолжалось время когда снимаем с паузы
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.Play();
            mediaElement.LoadedBehavior = MediaState.Play;
            sliderProgress.Value = 0;
        }
        private void timerTick(object sender, EventArgs e)
        {
            tsProgress = mediaElement.Position;
            txtStartTime.Text = String.Format("{0:00}:{1:00}:{2:00}", tsProgress.Hours,
                tsProgress.Minutes, tsProgress.Seconds);
            sliderProgress.Value = mediaElement.Position.TotalSeconds; // движение слайдера
        }
        private void Button_ClickStop(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.Pause();
            mediaElement.LoadedBehavior = MediaState.Pause;
        }

        private void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement.Volume = sliderVolume.Value;
        }

        private void sliderProgress_LostMouseCapture_1(object sender, MouseEventArgs e)
        {
            TimeSpan time = new TimeSpan(0, 0, Convert.ToInt32(Math.Round(sliderProgress.Value))); //отлавливаем позицию на которую нужно перемотать трек
            mediaElement.Position = time; //устанавливаем новую позицию для трека
        }

        private void sliderProgress_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TimeSpan time = new TimeSpan(0, 0, Convert.ToInt32(Math.Round(sliderProgress.Value))); //отлавливаем позицию на которую нужно перемотать трек
            mediaElement.Position = time;
        }

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            sliderProgress.Value = 0;
        }

        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            sliderProgress.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            ts = mediaElement.NaturalDuration.TimeSpan;
            txtAllTime.Text = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            timer.Start();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            var index = (int)lbPlaylist.SelectedIndex;
            if ((index + 1) < lbPlaylist.Items.Count)
            {
            mediaElement.Source = new Uri(lbPlaylist.Items[index+1].ToString());
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.Play();
            mediaElement.LoadedBehavior = MediaState.Play;
            sliderProgress.Value = 0;
            lbPlaylist.Focus();
            lbPlaylist.SelectedIndex = index+1;
            }
        }

        private void miPlayItem_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            mediaElement.Source = new Uri(lbPlaylist.SelectedItem.ToString());
            if (mediaElement.LoadedBehavior == MediaState.Pause)
                timer.Start();                   // чтобы продолжалось время когда снимаем с паузы
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.Play();
            mediaElement.LoadedBehavior = MediaState.Play;
            sliderProgress.Value = 0;
        }

        private void miAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Filter = "Video |*.avi|Video |*.mp4|Video |*.mpeg|Audio |*.mp3"; // Filter files by extension
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    mediaElement.Source = new Uri(dlg.FileName);
                    lbPlaylist.Items.Add(dlg.FileName);
                    lbPlaylist.Focus();
                    lbPlaylist.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exeption");
            }
        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {
            var item = lbPlaylist.SelectedItem;
            if (item != null)
            {
                this.lbPlaylist.Items.Remove(item);
            }
        }

        private void miClear_Click(object sender, RoutedEventArgs e)
        {
            if (this.lbPlaylist.Items.Count > 0)
            {
                lbPlaylist.Items.Clear();
            }
        }

        private void lbPlaylist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
            mediaElement.Source = new Uri(lbPlaylist.SelectedItem.ToString());
            if (mediaElement.LoadedBehavior == MediaState.Pause)
                timer.Start();                   // чтобы продолжалось время когда снимаем с паузы
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.Play();
            mediaElement.LoadedBehavior = MediaState.Play;
            sliderProgress.Value = 0;
        } 
    }
}
