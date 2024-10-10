﻿using MangaDownloader.Models;
using ReactiveUI;
using System;

namespace MangaDownloader.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public PrepareAreaViewModel PrepareAreaViewModel { get; }

        public DownloadingInfoViewModel DownloadingInfoViewModel { get; } = new DownloadingInfoViewModel();


        public MainWindowViewModel()
        {
            var source = new ProgressEventSource().Subscribe(SetMaxProgress, UpdateProgress, ResetProgress);
            PrepareAreaViewModel = new PrepareAreaViewModel(source);
            _currentPage = PrepareAreaViewModel;

            // ダウンロードしているかで画面(CurrentPage)を変更
            this.WhenAnyValue(x => x.PrepareAreaViewModel.IsDownloading)
                .Subscribe(isDownloading => CurrentPage = isDownloading ? DownloadingInfoViewModel : PrepareAreaViewModel);

        }

        private ViewModelBase _currentPage;

        public ViewModelBase CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        private int _maxProgressValue = 1;

        public int MaxProgressValue
        {
            get { return _maxProgressValue; }
            set { this.RaiseAndSetIfChanged(ref _maxProgressValue, value); }
        }

        private int _progressValue = 0;

        public int ProgressValue
        {
            get { return _progressValue; }
            set { this.RaiseAndSetIfChanged(ref _progressValue, value); }
        }

        public void SetMaxProgress(int value)
        {
            this.MaxProgressValue = value;
        }

        public void UpdateProgress(int addition)
        {
            this.ProgressValue += addition;
        }

        public void ResetProgress()
        {
            this.ProgressValue = 0;
        }
    }
}