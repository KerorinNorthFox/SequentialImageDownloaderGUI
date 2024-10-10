using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MangaDownloader.ViewModels
{
    /// <summary>
    /// URLリスト表示エリアのViewModel
    /// </summary>
    public partial class UrlListViewModel : ViewModelBase
    {
        public UrlListViewModel() {
            if (Design.IsDesignMode)
            {
                AddUrlToList(new Uri("https://example.com"));
                AddUrlToList(new Uri("https://www.google.com"));
            }

            var isDeleteButtonEnabled = this.WhenAnyValue(
                urlListVm => urlListVm.SelectedIndex,
                selectedIndex => selectedIndex >= 0);

            RemoveUrlCommand = ReactiveCommand.Create(() => _removeUrl(SelectedIndex), isDeleteButtonEnabled);
            ClearUrlListCommand = ReactiveCommand.Create(ClearUrlList);
        }

        /// <summary>
        /// URLリスト
        /// </summary>
        public ObservableCollection<Uri> UrlList { get; set; } = new ObservableCollection<Uri>();

        private int _selectedIndex = -1;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedIndex, value);
        }

        /// <summary>
        /// URLリストをクリアするコマンド
        /// </summary>
        public ICommand ClearUrlListCommand { get; }

        /// <summary>
        /// 選択したURLをURLリストから削除するコマンド
        /// </summary>
        public ICommand RemoveUrlCommand { get; }

        /// <summary>
        /// URLをURLリストに追加する
        /// </summary>
        /// <param name="uri">追加するURL</param>
        public void AddUrlToList(Uri uri)
        {
            UrlList.Add(uri);
        }

        public void ClearUrlList()
        {
            UrlList.Clear();
        }

        public void ClearUrlList(List<int> exceptIndex)
        {
            for (int i = UrlList.Count - 1; i >= 0; i--)
            {
                if (!exceptIndex.Contains(i))
                {
                    _removeUrl(i);
                }
            }
        }

        private void _removeUrl(int index)
        {
            if (index < 0)
            {
                return;
            }
            UrlList.RemoveAt(index);
        }
    }
}
