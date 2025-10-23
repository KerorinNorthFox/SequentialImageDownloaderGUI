using Avalonia.Media.Imaging;
using ReactiveUI;
using System;
using System.Collections.Generic;

namespace MangaDownloader.Models
{
    public class Manga : ReactiveObject, IDisposable
    {
        public string Title { get; set; } = string.Empty;

        public Uri Uri { get; }

        public string? Author { get; set; }

        public string? AuthorCircleName { get; set; }

        public Dictionary<int, Page> Pages { get; private set; } = new Dictionary<int, Page>();

        private DownloadStatus _state = DownloadStatus.Pending;

        private bool _disposed = false;

        public Manga(Uri uri)
        {
            Uri = uri;
        }

        public DownloadStatus State
        {
            get => _state;
            set => this.RaiseAndSetIfChanged(ref _state, value);
        }

        public void AddPageByIndex(int index, Uri uri, Bitmap image)
        {
            Pages.Add(index, new Page(index, uri, image));
        }

        public void ChangeDownloadState(DownloadStatus state)
        {
            State = state;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 全てのPageのBitmapを解放
                    foreach (var page in Pages.Values)
                    {
                        page?.Dispose();
                    }
                    Pages.Clear();
                }
                _disposed = true;
            }
        }

        ~Manga()
        {
            Dispose(false);
        }
    }
}

// メモ
// サムネを決めたかのbooleanメンバ変数を実装(これをもとにdiscordにアップロードできるか判定)
// ダウンロード完了したかのメンバ変数も必要 (TaskManageViewでDownloadボタンを推したときに、
// このメンバ変数でダウンロードするかを判定)

// ->もしサムネ用クラスを実装する場合
//   サムネに使用する画像のインデックスメンバ変数を実装
//   範囲を決めてトリミングした領域のbitmapを保持するメンバ変数の実装