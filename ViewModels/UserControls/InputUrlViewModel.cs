using ReactiveUI;
using System;
using System.Windows.Input;

namespace MangaDownloader.ViewModels
{
    public partial class InputUrlViewModel : ViewModelBase
    {
        /// <summary>
        /// URL入力時に実行する外部アクション
        /// </summary>
        private Action<Uri> _addUrlAction;

        public InputUrlViewModel(Action<Uri> addUrlAction)
        {
            _addUrlAction = addUrlAction;

            AddUrlCommand = ReactiveCommand.Create(AddUrl);
            ClearInputUrlCommand = ReactiveCommand.Create(ClearInputUrl);
        }

        private string _errorMessage = "";

        /// <summary>
        /// URLリストに追加するときのURLバリデーションのエラー文
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        private string _inputUrlText = "";

        /// <summary>
        /// URLリストに追加するUrlのテキスト
        /// </summary>
        public string InputUrlText
        {
            get => _inputUrlText;
            set => this.RaiseAndSetIfChanged(ref _inputUrlText, value);
        }

        /// <summary>
        /// URL追加ボタンのコマンド
        /// </summary>
        public ICommand AddUrlCommand { get; }

        /// <summary>
        /// テキストボックスからURLを削除するコマンド
        /// </summary>
        public ICommand ClearInputUrlCommand { get; }

        public void AddUrl()
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
            _clearInputUrl();
            _clearErrorMessage();
        }

        private void _clearInputUrl()
        {
            InputUrlText = "";
        }

        private void _clearErrorMessage()
        {
            ErrorMessage = "";
        }

    }
}
