using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;

namespace ThreeStateImageButtonSample
{
    /// <summary>
    /// 三態按鈕，支援 Normal/Press/Hover/Select/Disable
    /// Todo: 在 WP 上使用 hover 的效果不好，只能 for 藍芽滑鼠 + WP 使用
    /// </summary>
    [TemplatePartAttribute(Name = "ThreeStateImage", Type = typeof(Image))]
    public class ThreeStateImageButton : Control
    {
        public static readonly DependencyProperty NormalImageUriProperty =
            DependencyProperty.Register("NormalImageUri", typeof(Uri), typeof(ThreeStateImageButton), new PropertyMetadata(null, OnNormalImageUriPropertyChanged));

        public Uri NormalImageUri
        {
            get { return (Uri)GetValue(NormalImageUriProperty); }
            set { SetValue(NormalImageUriProperty, value); }
        }

        private static void OnNormalImageUriPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ThreeStateImageButton sourceButton = d as ThreeStateImageButton;
            if (sourceButton != null)
            {
                sourceButton.normalBitmapImage = new BitmapImage() { UriSource = sourceButton.NormalImageUri };
                sourceButton.isUriChanged = true;
                sourceButton.refreshCurrentStateImage();
            }
        }

        public static readonly DependencyProperty PointerOverImageUriProperty =
            DependencyProperty.Register("PointerOverImageUri", typeof(Uri), typeof(ThreeStateImageButton), new PropertyMetadata(null, OnPointerOverImageUriPropertyChanged));

        public Uri PointerOverImageUri
        {
            get { return (Uri)GetValue(PointerOverImageUriProperty); }
            set { SetValue(PointerOverImageUriProperty, value); }
        }
        private static void OnPointerOverImageUriPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ThreeStateImageButton sourceButton = d as ThreeStateImageButton;
            if (sourceButton != null)
            {
                sourceButton.pointerOverBitmapImage = new BitmapImage() { UriSource = sourceButton.PointerOverImageUri };
                sourceButton.isUriChanged = true;
                sourceButton.refreshCurrentStateImage();
            }
        }

        public static readonly DependencyProperty PressedImageUriProperty =
            DependencyProperty.Register("PressedImageUri", typeof(Uri), typeof(ThreeStateImageButton), new PropertyMetadata(null, OnPressedImageUriPropertyChanged));

        public Uri PressedImageUri
        {
            get { return (Uri)GetValue(PressedImageUriProperty); }
            set { SetValue(PressedImageUriProperty, value); }
        }

        private static void OnPressedImageUriPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ThreeStateImageButton sourceButton = d as ThreeStateImageButton;
            if (sourceButton != null)
            {
                sourceButton.pressedBitmapImage = new BitmapImage() { UriSource = sourceButton.PressedImageUri };
                sourceButton.isUriChanged = true;
                sourceButton.refreshCurrentStateImage();
            }
        }

        public static readonly DependencyProperty DisabledImageUriProperty =
            DependencyProperty.Register("DisabledImageUri", typeof(Uri), typeof(ThreeStateImageButton), new PropertyMetadata(null, OnDisabledImageUriPropertyChanged));

        public Uri DisabledImageUri
        {
            get { return (Uri)GetValue(DisabledImageUriProperty); }
            set { SetValue(DisabledImageUriProperty, value); }
        }

        private static void OnDisabledImageUriPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ThreeStateImageButton sourceButton = d as ThreeStateImageButton;
            if (sourceButton != null)
            {
                sourceButton.disabledBitmapImage = new BitmapImage() { UriSource = sourceButton.DisabledImageUri };
                sourceButton.refreshCurrentStateImage();
            }
        }


        public static readonly DependencyProperty SelectedImageUriProperty =
            DependencyProperty.Register("SelectedImageUri", typeof(Uri), typeof(ThreeStateImageButton), new PropertyMetadata(null, OnSelectedImageUriPropertyChanged));

        public Uri SelectedImageUri
        {
            get { return (Uri)GetValue(SelectedImageUriProperty); }
            set { SetValue(SelectedImageUriProperty, value); }
        }

        private static void OnSelectedImageUriPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ThreeStateImageButton sourceButton = d as ThreeStateImageButton;
            if (sourceButton != null)
            {
                sourceButton.selectedBitmapImage = new BitmapImage() { UriSource = sourceButton.SelectedImageUri };
                sourceButton.refreshCurrentStateImage();
            }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(Boolean), typeof(ThreeStateImageButton), new PropertyMetadata(false, OnIsSelectedPropertyChanged));

        public Boolean IsSelected
        {
            get { return (Boolean)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        private static void OnIsSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ThreeStateImageButton sourceButton = d as ThreeStateImageButton;
            if (sourceButton != null && sourceButton.SelectedImageUri != null)
            {
                if (sourceButton.IsEnabled == true)
                {
                    if ((Boolean)e.NewValue == true)
                    {
                        sourceButton.CurrentState = ThreeStateImageButtonState.Selected;
                        sourceButton.IsHitTestVisible = false;
                    }
                    else
                    {
                        sourceButton.CurrentState = ThreeStateImageButtonState.Normal;
                        sourceButton.IsHitTestVisible = true;
                    }
                }
                else
                {
                    if ((Boolean)e.NewValue == true)
                    {
                        sourceButton.tempEnableChangedState = ThreeStateImageButtonState.Selected;
                    }
                    else
                    {
                        sourceButton.tempEnableChangedState = ThreeStateImageButtonState.Normal;
                    }
                }
                sourceButton.refreshCurrentStateImage();
            }
        }

        public static readonly DependencyProperty ImageLoadingModeProperty =
            DependencyProperty.Register("ImageLoadingMode", typeof(ImageLoadingMode), typeof(ThreeStateImageButton), new PropertyMetadata(ImageLoadingMode.ThreeImageUri));

        public ImageLoadingMode ImageLoadingMode
        {
            get { return (ImageLoadingMode)GetValue(ImageLoadingModeProperty); }
            set { SetValue(ImageLoadingModeProperty, value); }
        }


        public delegate void ThreeStateImageButtonStateChanged(ThreeStateImageButtonState currentState);
        public event ThreeStateImageButtonStateChanged OnThreeStateImageButtonStateChanged;

        private Image ThreeStateImage;
        private BitmapImage normalBitmapImage;
        private BitmapImage pointerOverBitmapImage;
        private BitmapImage pressedBitmapImage;
        private BitmapImage disabledBitmapImage;
        private BitmapImage selectedBitmapImage;
        private ThreeStateImageButtonState _currentState;
        private ThreeStateImageButtonState tempEnableChangedState;
        public event RoutedEventHandler Click;
        private Boolean isPointerOver = false;
        private Boolean isPressing = false;
        private Boolean isUriChanged = false;
        private ImageLoadingMode imageLoadingMode;
        /// <summary>
        /// ThreeStateImageButton 的目前狀態
        /// </summary>
        public ThreeStateImageButtonState CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                _currentState = value;
                refreshCurrentStateImage();
                if (OnThreeStateImageButtonStateChanged != null)
                {
                    OnThreeStateImageButtonStateChanged(_currentState);
                }
            }
        }

        private void refreshCurrentStateImage()
        {
            try
            {
                if (ThreeStateImage != null)
                {
                    switch (_currentState)
                    {
                        case ThreeStateImageButtonState.Normal:
                            ThreeStateImage.Source = normalBitmapImage;
                            break;
                        case ThreeStateImageButtonState.PointerOver:
                            ThreeStateImage.Source = pointerOverBitmapImage;
                            break;
                        case ThreeStateImageButtonState.Pressed:
                            ThreeStateImage.Source = pressedBitmapImage;
                            break;
                        case ThreeStateImageButtonState.Disabled:
                            // 並非所有按鈕，都有給 Disabled 圖，所以要先確認是否有這個狀態
                            if (disabledBitmapImage != null)
                            {
                                ThreeStateImage.Source = disabledBitmapImage;
                            }
                            break;
                        case ThreeStateImageButtonState.Selected:
                            // 並非所有按鈕，都有給 Selected 圖，所以要先確認是否有這個狀態
                            if (selectedBitmapImage != null)
                            {
                                ThreeStateImage.Source = selectedBitmapImage;
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public ThreeStateImageButton()
        {
            CurrentState = ThreeStateImageButtonState.Normal;

            this.DefaultStyleKey = typeof(ThreeStateImageButton);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ThreeStateImage = base.GetTemplateChild("ThreeStateImage") as Image;
            if (ThreeStateImage == null || NormalImageUri == null || PointerOverImageUri == null || PressedImageUri == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                normalBitmapImage = new BitmapImage() { UriSource = NormalImageUri };
                pointerOverBitmapImage = new BitmapImage() { UriSource = PointerOverImageUri };
                pressedBitmapImage = new BitmapImage() { UriSource = PressedImageUri };

                ThreeStateImage.Source = normalBitmapImage;
                this.PointerPressed += ThreeStateImageButton_PointerPressed;
                this.PointerReleased += ThreeStateImageButton_PointerReleased;
                this.PointerEntered += ThreeStateImageButton_PointerEntered;
                this.PointerExited += ThreeStateImageButton_PointerExited;
                this.LostFocus += ThreeStateImageButton_LostFocus;
                this.IsEnabledChanged += ThreeStateImageButton_IsEnabledChanged;

                // 這邊需要直接讀取 IsEnabled，才能正確初始化
                if (this.IsEnabled == false)
                {
                    CurrentState = ThreeStateImageButtonState.Disabled;
                }

                imageLoadingMode = ImageLoadingMode;
            }
        }

        private void ThreeStateImageButton_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            isPressing = true;
            CurrentState = ThreeStateImageButtonState.Pressed;
            VisualStateManager.GoToState(this, "Pressed", true);
        }

        private void ThreeStateImageButton_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (isPressing == true)
            {
                if (isPointerOver || isUriChanged == true)
                {
                    CurrentState = ThreeStateImageButtonState.PointerOver;
                    VisualStateManager.GoToState(this, "PointerOver", true);

                    FlyoutBase myFlyout = Flyout.GetAttachedFlyout(this);

                    if (myFlyout != null)
                    {
                        Flyout.ShowAttachedFlyout(this);
                    }
                    else if (Click != null)
                    {
                        Click(this, new RoutedEventArgs());
                    }

                    isUriChanged = false;
                }
                else
                {
                    CurrentState = ThreeStateImageButtonState.Normal;
                    VisualStateManager.GoToState(this, "Normal", true);
                }
            }

            isPressing = false;
        }

        private void ThreeStateImageButton_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            isPointerOver = true;

            if (isPressing)
            {
                CurrentState = ThreeStateImageButtonState.Pressed;
                VisualStateManager.GoToState(this, "Pressed", true);
            }
            else
            {
                CurrentState = ThreeStateImageButtonState.PointerOver;
                VisualStateManager.GoToState(this, "PointerOver", true);
            }
        }

        private void ThreeStateImageButton_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            isPointerOver = false;
            CurrentState = ThreeStateImageButtonState.Normal;
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void ThreeStateImageButton_LostFocus(object sender, RoutedEventArgs e)
        {
            isPressing = false;
        }

        private void ThreeStateImageButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((Boolean)e.NewValue == false)
            {
                tempEnableChangedState = CurrentState;
                CurrentState = ThreeStateImageButtonState.Disabled;
            }
            else
            {
                CurrentState = tempEnableChangedState;
                if (CurrentState == ThreeStateImageButtonState.Selected)
                {
                    this.IsHitTestVisible = false;
                }
                else
                {
                    this.IsHitTestVisible = true;
                }
            }
        }
    }
}
