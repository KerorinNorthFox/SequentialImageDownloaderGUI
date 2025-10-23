using Avalonia.Controls;
using MangaDownloader.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MangaDownloader.ViewModels
{
    public partial class MangaListViewModel : ViewModelBase
    {
        public MangaListViewModel()
        {
            if (Design.IsDesignMode)
            {
                AddManga(new Uri("https://example.com"));
                AddManga(new Uri("https://www.google.com"));
            }

            var isDeleteButtonEnabled = this.WhenAnyValue( // SelectedIndexが選択されているときにisDeleteButtonEnabledをtrueにする
                selfVm => selfVm.SelectedIndex,
                i => i >= 0);

            RemoveMangaCommand = ReactiveCommand.Create(() => _removeManga(SelectedIndex), isDeleteButtonEnabled);
            ClearMangaListCommand = ReactiveCommand.Create(ClearMangaList);
        }

        /// <summary>
        /// Mangaのリスト
        /// </summary>
        public ObservableCollection<Manga> MangaList { get; private set; } = new ObservableCollection<Manga>();

        private int _selectedIndex = -1;

        /// <summary>
        /// リスト内で選択されたタスクのインデックス
        /// </summary>
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedIndex, value);
        }

        /// <summary>
        /// 選択したURLをURLリストから削除するコマンド
        /// </summary>
        public ICommand RemoveMangaCommand { get; }

        /// <summary>
        /// URLリストをクリアするコマンド
        /// </summary>
        public ICommand ClearMangaListCommand { get; }

        /// <summary>
        /// URLをMangaとしてMangaListに追加する
        /// </summary>
        /// <param name="uri">追加するURL</param>
        public void AddManga(Uri uri)
        {
            MangaList.Add(new Manga(uri));
        }

        public void ClearMangaList()
        {
            MangaList.Clear();
        }

        public void ClearMangaList(List<int> exceptIndex)
        {
            for (int i = MangaList.Count - 1; i >= 0; i--)
            {
                if (!exceptIndex.Contains(i))
                {
                    _removeManga(i);
                }
            }
        }

        private void _removeManga(int index)
        {
            if (index < 0)
            {
                return;
            }
            MangaList.RemoveAt(index);
        }
    }
}