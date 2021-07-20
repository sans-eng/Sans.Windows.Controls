using Sans.Windows.Controls.Extensions;
using Sans.Windows.Controls.Utils;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Sans.Windows.Controls
{
    /// <summary>
    ///     Represents a loading indicator.
    /// </summary>
    [TemplatePart(Name = TemplateBorderName, Type = typeof(Border))]
    public class LoadingIndicator : Control
    {
        #region Constructors
        static LoadingIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LoadingIndicator),
                new FrameworkPropertyMetadata(typeof(LoadingIndicator)));
        }
        #endregion

        #region Const
        internal const string TemplateBorderName = "PART_Border";
        #endregion

        #region Private fields
        private Border PART_Border;
        #endregion

        #region Dependency properties
        /// <summary>
        /// Identifies the <see cref="SpeedRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SpeedRatioProperty =
            DependencyProperty.Register(nameof(SpeedRatio), typeof(double), typeof(LoadingIndicator), new PropertyMetadata(1d,
                OnSpeedRatioChanged));

        /// <summary>
        /// Identifies the <see cref="IsActive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(LoadingIndicator), new PropertyMetadata(true,
                OnIsActiveChanged));

        /// <summary>
        /// Identifies the <see cref="Mode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
            nameof(Mode), typeof(LoadingIndicatorMode), typeof(LoadingIndicator),
            new PropertyMetadata(default(LoadingIndicatorMode)));
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the mode of current <see cref="LoadingIndicator"/>.
        /// </summary>
        public LoadingIndicatorMode Mode
        {
            get { return (LoadingIndicatorMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the speed ratio of the animation.
        /// </summary>
        public double SpeedRatio
        {
            get { return (double)GetValue(SpeedRatioProperty); }
            set { SetValue(SpeedRatioProperty, value); }
        }

        /// <summary>
        ///  Gets or sets whether the loading indicator is active.
        /// </summary>
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        #endregion

        #region Dependency property changed handler
        private static void OnSpeedRatioChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var li = (LoadingIndicator)o;

            if (li.PART_Border == null || !li.IsActive)
            {
                return;
            }

            SetStoryBoardSpeedRatio(li.PART_Border, (double)e.NewValue);
        }

        private static void OnIsActiveChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is LoadingIndicator loadingIndicator)) return;

            if (loadingIndicator.PART_Border == null)
            {
                return;
            }

            if (!(bool)e.NewValue)
            {
                VisualStateManager.GoToElementState(loadingIndicator.PART_Border, IndicatorVisualStateNames.InactiveState.Name,
                    false);
                loadingIndicator.PART_Border.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            }
            else
            {
                VisualStateManager.GoToElementState(loadingIndicator.PART_Border, IndicatorVisualStateNames.ActiveState.Name, false);

                loadingIndicator.PART_Border.SetCurrentValue(VisibilityProperty, Visibility.Visible);

                SetStoryBoardSpeedRatio(loadingIndicator.PART_Border, loadingIndicator.SpeedRatio);
            }
        }
        #endregion
        #region Private Methods
        private static void SetStoryBoardSpeedRatio(FrameworkElement element, double speedRatio)
        {
            foreach (var activeState in element.GetActiveVisualStates()) activeState.Storyboard.SetSpeedRatio(element, speedRatio);
        }
        #endregion

        #region Public methods

        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code
        ///     or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Border = (Border)GetTemplateChild(TemplateBorderName);

            if (PART_Border == null)
            {
                return;
            }

            VisualStateManager.GoToElementState(PART_Border,
                IsActive
                    ? IndicatorVisualStateNames.ActiveState.Name
                    : IndicatorVisualStateNames.InactiveState.Name, false);

            SetStoryBoardSpeedRatio(PART_Border, SpeedRatio);

            PART_Border.SetCurrentValue(VisibilityProperty, IsActive ? Visibility.Visible : Visibility.Collapsed);

            SizeChanged += LoadingIndicator_SizeChanged;
        }

        private void LoadingIndicator_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
        #endregion
    }
}