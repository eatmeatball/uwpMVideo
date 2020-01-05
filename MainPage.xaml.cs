using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
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
        public ObservableCollection<VideoFileInfoData> VideoFileInfoList = new ObservableCollection<VideoFileInfoData>();



        public MainPage()
        {
            this.InitializeComponent();
            List<VideoFileInfoData> ss = DataAccess.GetData();
            VideoFileInfoList.Clear();
            foreach (var s in ss)
            {
                VideoFileInfoList.Add(s);

            }
        }



        async private System.Threading.Tasks.Task SetLocalMedia()
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            openPicker.FileTypeFilter.Add(".wmv");
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".wma");
            openPicker.FileTypeFilter.Add(".mp3");


            var file = await openPicker.PickMultipleFilesAsync();

            // mediaPlayer is a MediaPlayerElement defined in XAML
            if (file != null)
            {
                bool first = false;
                foreach (var f in file)
                {

                    if (!first)
                    {
                        VideoFileInfoList.Add(new VideoFileInfoData(f.Name, f.Path)); ;


                        mediaPlayer.Source = MediaSource.CreateFromStorageFile(f);

                        mediaPlayer.MediaPlayer.Play();

                        first = true;
                    }
                }
            }
        }

        async private System.Threading.Tasks.Task SetLocalDirMedia()
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop
            };

            folderPicker.FileTypeFilter.Add(".wmv");
            folderPicker.FileTypeFilter.Add(".mp4");
            folderPicker.FileTypeFilter.Add(".wma");
            folderPicker.FileTypeFilter.Add(".mp3");

            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if (folder != null)
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                speedText.Text = "Picked folder: " + folder.Name;
                IReadOnlyList<StorageFile> fileList = await folder.GetFilesAsync();

                bool first = false;

                foreach (StorageFile file in fileList)
                {

                    VideoFileInfoList.Add(new VideoFileInfoData(file.Name, file.Path));



                    // mediaPlayer is a MediaPlayerElement defined in XAML
                    if (file != null)
                    {

                        if (!first)
                        {

                            mediaPlayer.Source = MediaSource.CreateFromStorageFile(file);

                            mediaPlayer.MediaPlayer.Play();

                            first = true;
                        }

                    }
                }

            }
            else
            {
                UpShowText("Operation cancelled.");
            }

        }

        public void UpShowText(string Text)
        {
            speedText.Text = Text;
        }

        private async void ChooseFileBtn_Click(object sender, RoutedEventArgs e)
        {
            await SetLocalMedia();
        }

        private async void ChooseDirBtn_Click(object sender, RoutedEventArgs e)
        {
            await SetLocalDirMedia();
        }





        async private void MItems_ItemClick(object sender, ItemClickEventArgs e)
        {
            var Data = e.ClickedItem as VideoFileInfoData;
            var FilePath = Data.FilePath;
            // 这个。。。当前路径有点
            /*StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            foreach (var item in Path.GetInvalidPathChars())
            {
                FilePath = FilePath.Replace(item, '_');
            }
            var File = await storageFolder.GetFileAsync(FilePath);*/

            // 全路径读取文件

            foreach (var item in Path.GetInvalidPathChars())
            {
                FilePath = FilePath.Replace(item, '_');
            }

            StorageFile readFile = await StorageFile.GetFileFromPathAsync(FilePath);

            mediaPlayer.Source = MediaSource.CreateFromStorageFile(readFile);

            UpdateTitle(FilePath);
        }

        public class VideoFileInfoData
        {
            public string FileName { get; set; }

            public string FilePath { get; set; }

            public VideoFileInfoData(string Name, string Path)
            {
                this.FileName = Name;
                this.FilePath = Path;
            }

        }

        private int likes = 0;

        private async void CustomMTC_Liked(object sender, EventArgs e)
        {
            var messageDialog = new MessageDialog("You liked this video " + (++likes) + " times.");
            await messageDialog.ShowAsync();
        }

        private void SuperSimpleMediaTransportControls_AddPlaybackRated(object sender, EventArgs e)
        {
            mediaPlayer.MediaPlayer.PlaybackSession.PlaybackRate += 0.1;
            UpShowText("当前播放速度为：" + mediaPlayer.MediaPlayer.PlaybackSession.PlaybackRate);
        }

        private void SuperSimpleMediaTransportControls_InitPlaybackRated(object sender, EventArgs e)
        {
            mediaPlayer.MediaPlayer.PlaybackSession.PlaybackRate = 1;
            UpShowText("当前播放速度为：" + mediaPlayer.MediaPlayer.PlaybackSession.PlaybackRate);
        }

        private void SuperSimpleMediaTransportControls_ReducePlaybackRated(object sender, EventArgs e)
        {
            mediaPlayer.MediaPlayer.PlaybackSession.PlaybackRate -= 0.1;
            UpShowText("当前播放速度为：" + mediaPlayer.MediaPlayer.PlaybackSession.PlaybackRate);

        }


        public void UpdateTitle(string newTitle)
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = false;
            ApplicationView.GetForCurrentView().Title = newTitle;
        }


        async private void TestDelBtn_Click(object sender, RoutedEventArgs e)
        {
            DataAccess.DelAllData();
                
                var messageDialog = new MessageDialog("数据删除完毕");
                await messageDialog.ShowAsync();
            

        }

        async private void TestSelectBtn_Click(object sender, RoutedEventArgs e)
        {

            var text = "";
            List<VideoFileInfoData> ss = DataAccess.GetData();
            VideoFileInfoList.Clear();
            foreach (var s   in ss)
            {
                text += s.FileName;
                VideoFileInfoList.Add(s);

            }
                        
            var messageDialog = new MessageDialog(text);
            await messageDialog.ShowAsync();

        }

        async private void TestInsertBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var VideoFileInfo in VideoFileInfoList)
            {
                DataAccess.AddData(VideoFileInfo.FileName , VideoFileInfo.FilePath);
            }


            var messageDialog = new MessageDialog("数据插入完毕");
            await messageDialog.ShowAsync();
        }
    }

}
