using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DotNetToolBox.WPF.Controls
{
    /// <summary>
    /// This custom popup is used by the validation error template.
    /// It provides some additional nice features:
    ///     - repositioning if host-window size or location changed
    ///     - repositioning if host-window gets maximized and vice versa
    ///     - it's only topmost if the host-window is activated
    /// </summary>
    public class CustomValidationPopup : Popup
    {
        public static readonly DependencyProperty CloseOnMouseLeftButtonDownProperty = DependencyProperty.Register("CloseOnMouseLeftButtonDown", typeof(bool), typeof(CustomValidationPopup), new PropertyMetadata(true));

        private Window hostWindow;

        public CustomValidationPopup()
        {
            Loaded += CustomValidationPopup_Loaded;
            Opened += CustomValidationPopup_Opened;
        }

        /// <summary>
        /// Gets/sets if the popup can be closed by left mouse button down.
        /// </summary>
        public bool CloseOnMouseLeftButtonDown
        {
            get => (bool) GetValue(CloseOnMouseLeftButtonDownProperty);
            set => SetValue(CloseOnMouseLeftButtonDownProperty, value);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (CloseOnMouseLeftButtonDown)
            {
                this.SetCurrentValue(Popup.IsOpenProperty, false);
            }
        }

        private void CustomValidationPopup_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(PlacementTarget is FrameworkElement target))
            {
                return;
            }

            hostWindow = Window.GetWindow(target);
            if (hostWindow == null)
            {
                return;
            }

            hostWindow.LocationChanged -= hostWindow_SizeOrLocationChanged;
            hostWindow.LocationChanged += hostWindow_SizeOrLocationChanged;
            hostWindow.SizeChanged -= hostWindow_SizeOrLocationChanged;
            hostWindow.SizeChanged += hostWindow_SizeOrLocationChanged;
            target.SizeChanged -= hostWindow_SizeOrLocationChanged;
            target.SizeChanged += hostWindow_SizeOrLocationChanged;
            hostWindow.StateChanged -= hostWindow_StateChanged;
            hostWindow.StateChanged += hostWindow_StateChanged;
            hostWindow.Activated -= hostWindow_Activated;
            hostWindow.Activated += hostWindow_Activated;
            hostWindow.Deactivated -= hostWindow_Deactivated;
            hostWindow.Deactivated += hostWindow_Deactivated;

            Unloaded -= CustomValidationPopup_Unloaded;
            Unloaded += CustomValidationPopup_Unloaded;
        }

        private void CustomValidationPopup_Opened(object sender, EventArgs e)
        {
            SetTopmostState(true);
        }

        private void hostWindow_Activated(object sender, EventArgs e)
        {
            SetTopmostState(true);
        }

        private void hostWindow_Deactivated(object sender, EventArgs e)
        {
            SetTopmostState(false);
        }

        private void CustomValidationPopup_Unloaded(object sender, RoutedEventArgs e)
        {
            if (PlacementTarget is FrameworkElement target)
            {
                target.SizeChanged -= hostWindow_SizeOrLocationChanged;
            }

            if (hostWindow != null)
            {
                hostWindow.LocationChanged -= hostWindow_SizeOrLocationChanged;
                hostWindow.SizeChanged -= hostWindow_SizeOrLocationChanged;
                hostWindow.StateChanged -= hostWindow_StateChanged;
                hostWindow.Activated -= hostWindow_Activated;
                hostWindow.Deactivated -= hostWindow_Deactivated;
            }

            Unloaded -= CustomValidationPopup_Unloaded;
            Opened -= CustomValidationPopup_Opened;
            hostWindow = null;
        }

        private void hostWindow_StateChanged(object sender, EventArgs e)
        {
            if (hostWindow == null || hostWindow.WindowState == WindowState.Minimized) return;

            var target = PlacementTarget as FrameworkElement;

            if (!(target?.DataContext is AdornedElementPlaceholder holder)) return;
            if (holder.AdornedElement == null) return;

            PopupAnimation = PopupAnimation.None;
            IsOpen = false;
            var errorTemplate = holder.AdornedElement.GetValue(Validation.ErrorTemplateProperty);
            holder.AdornedElement.SetValue(Validation.ErrorTemplateProperty, null);
            holder.AdornedElement.SetValue(Validation.ErrorTemplateProperty, errorTemplate);
        }

        private void hostWindow_SizeOrLocationChanged(object sender, EventArgs e)
        {
            var offset = HorizontalOffset;
            // "bump" the offset to cause the popup to reposition itself on its own
            HorizontalOffset = offset + 1;
            HorizontalOffset = offset;
        }

        private bool? appliedTopMost;

        private void SetTopmostState(bool isTop)
        {
            // Don’t apply state if it’s the same as incoming state
            if (appliedTopMost == isTop) return;
            if (hostWindow == null) return;
            
            hostWindow.Topmost = true;

            appliedTopMost = isTop;
        }
    }
}
