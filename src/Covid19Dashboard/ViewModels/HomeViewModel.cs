﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Helpers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Core;

namespace Covid19Dashboard.ViewModels
{
    public class HomeViewModel : ObservableObject
    {
        private List<EpidemicIndicator> epidemicIndicators;

        private string dailyConfirmedNewCase;
        private string dailyConfirmedNewCaseLastUpdate;

        public List<EpidemicIndicator> EpidemicIndicators
        {
            get { return epidemicIndicators; }
            set { SetProperty(ref epidemicIndicators, value); }
        }

        public string DailyConfirmedNewCase
        {
            get { return dailyConfirmedNewCase; }
            set { SetProperty(ref dailyConfirmedNewCase, value); }
        }

        public string DailyConfirmedNewCaseLastUpdate
        {
            get { return dailyConfirmedNewCaseLastUpdate; }
            set { SetProperty(ref dailyConfirmedNewCaseLastUpdate, value); }
        }

        public HomeViewModel()
        {
            EpidemicIndicators = new List<EpidemicIndicator>();

            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                EpidemicIndicators = await GetEpidemicIndicators();

                if (EpidemicIndicators.Count > 0)
                {
                    DailyConfirmedNewCase = GetDailyConfirmedNewCasesValue();
                    DailyConfirmedNewCaseLastUpdate = GetDailyConfirmedNewCasesLastUpdate();
                }
            });
        }

        public async Task<List<EpidemicIndicator>> GetEpidemicIndicators()
        {
            // Download the summary of indicators for monitoring the COVID-19 epidemic csv file (national).
            Uri source = new Uri("https://www.data.gouv.fr/fr/datasets/r/f335f9ea-86e3-4ffa-9684-93c009d5e617");

            StorageFile destinationFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(Guid.NewGuid() + ".csv", CreationCollisionOption.GenerateUniqueName);

            DownloadOperation download = new BackgroundDownloader().CreateDownload(source, destinationFile);
            await download.StartAsync();

            List<EpidemicIndicator> epidemicIndicators = await CsvHelper.ToObject<List<EpidemicIndicator>>(destinationFile);
            return epidemicIndicators.OrderByDescending(x => x.Date).ToList();
        }

        private string GetDailyConfirmedNewCasesValue()
        {
            if (EpidemicIndicators != null && EpidemicIndicators.Count > 0)
            {
                int? hospitalization = EpidemicIndicators.FirstOrDefault(x => x.DailyConfirmedNewCases.HasValue).DailyConfirmedNewCases;

                return hospitalization.HasValue ? hospitalization.ToString() : "";
            }

            return "";
        }

        private string GetDailyConfirmedNewCasesLastUpdate()
        {
            if (EpidemicIndicators != null && EpidemicIndicators.Count > 0)
                return EpidemicIndicators.First(x => x.DailyConfirmedNewCases.HasValue).Date.ToShortDateString();

            return "";
        }
    }
}
