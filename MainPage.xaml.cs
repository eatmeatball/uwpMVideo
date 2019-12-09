using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace SimpleM
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            await SetLocalMedia();
        }

        async private System.Threading.Tasks.Task SetLocalMedia()
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            openPicker.FileTypeFilter.Add(".wmv");
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".wma");
            openPicker.FileTypeFilter.Add(".mp3");

            var file = await openPicker.PickSingleFileAsync();

            // mediaPlayer is a MediaPlayerElement defined in XAML
            if (file != null)
            {
                mediaPlayer.Source = MediaSource.CreateFromStorageFile(file);
                //mediaPlayer.Visibility = Visibility.Visible;
                //chooseFileBtn.Visibility = Visibility.Collapsed;
                mediaPlayer.MediaPlayer.Play();

                //mediaPlayer.MediaPlayer.MediaEnded += new TypedEventHandler<Windows.Media.Playback.MediaPlayer, object>((player, resource) =>
                //{
                //    _ = chooseFileBtn.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                //    {
                //        chooseFileBtn.Visibility = Visibility.Visible;
                //        mediaPlayer.Visibility = Visibility.Collapsed;
                //    });
                //});
            }
        }

        

        private void AddPlaybackRate_Click(object sender, RoutedEventArgs e)
        {

            mediaPlayer.MediaPlayer.PlaybackSession.PlaybackRate += 0.1;
            speedText.Text = "当前播放速度为：" + mediaPlayer.MediaPlayer.PlaybackSession.PlaybackRate; ;
        }

        private void ReducePlaybackRate_Click(object sender, RoutedEventArgs e)
        {

            mediaPlayer.MediaPlayer.PlaybackSession.PlaybackRate -= 0.1;
            speedText.Text = "当前播放速度为：" + mediaPlayer.MediaPlayer.PlaybackSession.PlaybackRate; ;
        }

        private void InitPlaybackRate_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.MediaPlayer.PlaybackSession.PlaybackRate = 1;
            speedText.Text = "当前播放速度为：" + mediaPlayer.MediaPlayer.PlaybackSession.PlaybackRate; ;
        }
    }
}
