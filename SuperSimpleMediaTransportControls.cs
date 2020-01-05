using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace SimpleM
{
    public sealed class SuperSimpleMediaTransportControls : MediaTransportControls
    {
        public event EventHandler<EventArgs> ReducePlaybackRated;
        public event EventHandler<EventArgs> InitPlaybackRated;
        public event EventHandler<EventArgs> AddPlaybackRated;

        public SuperSimpleMediaTransportControls()
        {
            this.DefaultStyleKey = typeof(SuperSimpleMediaTransportControls);
        }

        protected override void OnApplyTemplate()
        {
            // *****************
            // This is where you would get your custom button and create an event handler for its click method.
            Button ReducePlaybackRateButton = GetTemplateChild("ReducePlaybackRateButton") as Button;
            ReducePlaybackRateButton.Click += ReducePlaybackRateButton_Click;

            Button InitPlaybackRateButton = GetTemplateChild("InitPlaybackRateButton") as Button;
            InitPlaybackRateButton.Click += InitPlaybackRateButton_Click;

            Button AddPlaybackRateButton = GetTemplateChild("AddPlaybackRateButton") as Button;
            AddPlaybackRateButton.Click += AddPlaybackRateButton_Click;

            base.OnApplyTemplate();
        }

        private void ReducePlaybackRateButton_Click(object sender, RoutedEventArgs e) {
            ReducePlaybackRated?.Invoke(this, EventArgs.Empty);
        }
        private void InitPlaybackRateButton_Click(object sender, RoutedEventArgs e)
        {
            InitPlaybackRated?.Invoke(this, EventArgs.Empty);
        }
        private void AddPlaybackRateButton_Click(object sender, RoutedEventArgs e)
        {
            AddPlaybackRated?.Invoke(this, EventArgs.Empty);
        }



    }
}
