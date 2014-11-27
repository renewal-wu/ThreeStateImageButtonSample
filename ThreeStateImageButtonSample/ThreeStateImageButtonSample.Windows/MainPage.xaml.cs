using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ThreeStateImageButtonSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void ThreeStateImageButton_OnThreeStateImageButtonStateChanged(ThreeStateImageButtonState currentState)
        {
            CurrentStateTextBlock.Text = "CurrentState: " + currentState.ToString();
        }

        private void ThreeStateImageButton_Click(object sender, RoutedEventArgs e)
        {
            ClickStateTextBlock.Text = "Clicked at: " + DateTime.Now.ToString();
        }

        private void EnableSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (ThreeStateImageButtonSample != null)
            {
                ThreeStateImageButtonSample.IsEnabled = EnableSwitch.IsOn;
            }
        }

        private void SelectedSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (ThreeStateImageButtonSample != null)
            {
                ThreeStateImageButtonSample.IsSelected = SelectedSwitch.IsOn;
            }
        }
    }
}
