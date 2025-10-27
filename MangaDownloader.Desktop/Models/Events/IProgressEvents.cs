
namespace MangaDownloader.Desktop.Models.Events
{
    public interface IProgressEvents
    {
        /// <summary>
        /// progressを初期化する
        /// </summary>
        /// <param name="maxValue">progressの最大値</param>
        void OnInitializeProgress(int maxValue);

        /// <summary>
        /// progressを進める
        /// </summary>
        /// <param name="addingValue">追加する値(default:1)</param>
        void OnUpdateProgress(int addingValue = 1);

        void OnResetProgress();
    }
}
