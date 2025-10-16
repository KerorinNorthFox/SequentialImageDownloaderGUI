using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;

namespace MangaDownloader.Models
{
    public class Manga
    {
        public string MangaTitle { get; private set; } = string.Empty;

        public Uri MangaUri { get; }

        public string MangaAuthor { get; private set; } = string.Empty;

        public string MangaAuthorCircleName { get; private set; } = string.Empty;

        public Manga(Uri uri)
        {
            MangaUri = uri;
        }


        public Dictionary<int, Bitmap> Images { get; private set; } = new Dictionary<int, Bitmap>();

        public void AddImageByIndex(int index, Bitmap image)
        {
            Images.Add(index, image);
        }

        public DownloadState State { get; private set; }

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