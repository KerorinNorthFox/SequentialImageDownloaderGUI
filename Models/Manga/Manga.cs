using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;

namespace MangaDownloader.Models
{
    public class Manga
    {
        public string? Title { get; set; }

        public Uri Uri { get; }

        public string? Author { get; set; }

        public string? AuthorCircleName { get; set; }

        public Dictionary<int, Page> Pages { get; private set; } = new Dictionary<int, Page>();

        /// <summary>
        /// DL進捗状況
        /// </summary>
        public DownloadState State { get; private set; } = DownloadState.NotDownloaded;

        public Manga(Uri uri)
        {
            Uri = uri;
        }

        public void AddPageByIndex(int index, Uri uri, Bitmap image)
        {
            Pages.Add(index, new Page(uri, image));
        }

        public void ChangeDownloadState(DownloadState state)
        {
            State = state;
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