using ReactiveUI;
using System;
using System.Windows.Input;

namespace MangaDownloader.ViewModels
{
    public partial class InputUrlViewModel : ViewModelBase
    {
        private string _inputUrlText = "";

        private string _errorMessage = "";

        private bool _isDownloading = false;

        /// <summary>
        /// URL入力時に実行する外部アクション
        /// </summary>
        private Action<Uri> _addUrlAction;

        public InputUrlViewModel(Action<Uri> addUrlAction)
        {
            _addUrlAction = addUrlAction;

            var canExecuteCommand = this.WhenAnyValue(x => x.IsDownloading, isDownloading => !isDownloading);

            AddUrlCommand = ReactiveCommand.Create(addUrl, canExecuteCommand);
            ClearInputUrlCommand = ReactiveCommand.Create(ClearInputUrl);
        }

        /// <summary>
        /// URLリストに追加するUrlのテキスト
        /// </summary>
        public string InputUrlText
        {
            get => _inputUrlText;
            set => this.RaiseAndSetIfChanged(ref _inputUrlText, value);
        }

        /// <summary>
        /// URLリストに追加するときのURLバリデーションのエラー文
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public bool IsDownloading
        {
            get => _isDownloading;
            set => this.RaiseAndSetIfChanged(ref _isDownloading, value);
        }

        /// <summary>
        /// URL追加ボタンのコマンド
        /// </summary>
        public ICommand AddUrlCommand { get; }

        /// <summary>
        /// テキストボックスからURLを削除するコマンド
        /// </summary>
        public ICommand ClearInputUrlCommand { get; }

        private void addUrl()
        {
            Uri inputUri;
            try
            {
                inputUri = new Uri(InputUrlText);
            }
            catch (UriFormatException)
            {
                ErrorMessage = "Invalid URL";
                return;
            }

            _addUrlAction.Invoke(inputUri);
            ClearInputUrl();
        }

        public void ClearInputUrl()
        {
            clearInputUrl();
            clearErrorMessage();
        }

        private void clearInputUrl()
        {
            InputUrlText = "";
        }

        private void clearErrorMessage()
        {
            ErrorMessage = "";
        }

    }
}
