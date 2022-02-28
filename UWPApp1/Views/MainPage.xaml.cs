using System;

using UWPApp1.ViewModels;

using Windows.UI.Xaml.Controls;

namespace UWPApp1.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();

        }
    }
}
