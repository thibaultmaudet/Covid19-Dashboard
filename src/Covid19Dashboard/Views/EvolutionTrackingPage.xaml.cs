﻿using Covid19Dashboard.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Views
{
    public sealed partial class EvolutionTrackingPage : Page
    {
        public EvolutionTrackingViewModel ViewModel { get; } = new EvolutionTrackingViewModel();

        public EvolutionTrackingPage()
        {
            InitializeComponent();
            Loaded += EvolutionTrackingPage_Loaded;
        }

        private async void EvolutionTrackingPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(ListDetailsViewControl.ViewState);
        }
    }
}
