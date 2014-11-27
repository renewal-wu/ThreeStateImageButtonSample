
namespace ThreeStateImageButtonSample
{
    /// <summary>
    /// 讀取 ThreeStateImageButton 圖片網址的模式，此屬性不支援動態修改，請在初始化時即指定模式
    /// </summary>
    public enum ImageLoadingMode
    {
        /// <summary>
        /// 分開指定三張圖
        /// </summary>
        ThreeImageUri,
        /// <summary>
        /// <para>
        /// 指定資料夾，由 ThreeStateImageButton 自動依照固定格式抓取三張圖
        /// </para>
        /// <para>
        /// 資料夾格式範例: /Image/Playlist/PauseButton/
        /// </para>
        /// <para>
        /// 檔案格式範例: Normal.png  PointerOver.png  Pressed.png
        /// </para>
        /// </summary>
        ImagesFolder
    }
}
