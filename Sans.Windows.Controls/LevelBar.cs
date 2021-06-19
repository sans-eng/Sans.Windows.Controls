using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;

namespace Sans.Windows.Controls
{
    [StyleTypedProperty(Property = nameof(LabelStyle), StyleTargetType = typeof(Label))]
    public class LevelBar : System.Windows.Controls.ProgressBar
    {
        #region Constructors
        static LevelBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LevelBar), new FrameworkPropertyMetadata(typeof(LevelBar)));
        }
        #endregion

        #region Const
        private const string IndicatorName = "PART_Indicator";
        private const string TickMarkContainerName = "PART_TickMarkContainer";
        private const string LabelContainerName = "PART_LabelContainer";
        private const string LimitMarkContainerName = "PART_LimitMarkContainer";
        private const string TrackName = "PART_Track";
        #endregion

        #region Private fields
        private Border m_Indicator;
        private Border m_Track;
        private Canvas m_TickMarkContainer;
        private Canvas m_LimitMarkContainer;
        private Canvas m_LabelContainer;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets tick marks color of the level bar.
        /// </summary>
        public Brush TickMarkColor
        {
            get { return (Brush)GetValue(TickMarkColorProperty); }
            set { SetValue(TickMarkColorProperty, value); }
        }
        /// <summary>
        /// Gets or sets level marks color of the level bar.
        /// </summary>
        public Brush LimitMarkColor
        {
            get { return (Brush)GetValue(LimitMarkColorProperty); }
            set { SetValue(LimitMarkColorProperty, value); }
        }
        /// <summary>
        /// Gets or sets low accent color of the level bar.
        /// </summary>
        public Brush AccentLow
        {
            get
            {
                return (Brush)GetValue(AccentLowProperty);
            }

            set
            {
                SetValue(AccentLowProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets mid accent color of the level bar.
        /// </summary>
        public Brush AccentMid
        {
            get
            {
                return (Brush)GetValue(AccentMidProperty);
            }

            set
            {
                SetValue(AccentMidProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets high accent color of the level bar.
        /// </summary>
        public Brush AccentHigh
        {
            get
            {
                return (Brush)GetValue(AccentHighProperty);
            }

            set
            {
                SetValue(AccentHighProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets full accent color of the level bar.
        /// </summary>
        public Brush AccentOverValue
        {
            get
            {
                return (Brush)GetValue(AccentOverValueProperty);
            }

            set
            {
                SetValue(AccentOverValueProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets upper limit of the level bar.
        /// </summary>
        public double UpperLimit
        {
            get
            {
                return (double)GetValue(UpperLimitProperty);
            }

            set
            {
                SetValue(UpperLimitProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets lower limit of the level bar.
        /// </summary>
        public double LowerLimit
        {
            get
            {
                return (double)GetValue(LowerLimitProperty);
            }

            set
            {
                SetValue(LowerLimitProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets label interval of the level bar.
        /// </summary>
        public double Interval
        {
            get
            {
                return (double)GetValue(IntervalProperty);
            }

            set
            {
                SetValue(IntervalProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets minor tick mark of the level bar.
        /// </summary>
        public int MinorTickMarkCount
        {
            get
            {
                return (int)GetValue(MinorTickMarkCountProperty);
            }

            set
            {
                SetValue(MinorTickMarkCountProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets corner radius of the level bar.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }

            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets style of the label.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Label", Justification = "Matches System.Windows.Controls.Label")]
        public Style LabelStyle
        {
            get
            {
                return (Style)GetValue(LabelStyleProperty);
            }

            set
            {
                SetValue(LabelStyleProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets corner label orientation of the level bar.
        /// </summary>
        public Orientation LabelOrientation
        {
            get
            {
                return (Orientation)GetValue(LabelOrientationProperty);
            }

            set
            {
                SetValue(LabelOrientationProperty, value);
            }
        }

        #endregion

        #region private properties
        private Border Indicator
        {
            get { return m_Indicator; }
            set
            {
                m_Indicator = value;
            }
        }
        private Border Track
        {
            get { return m_Track; }
            set
            {
                if (m_Track != null) m_Track.SizeChanged -= Track_SizeChanged;
                m_Track = value;
                if (m_Track != null) m_Track.SizeChanged += Track_SizeChanged;
            }
        }
        private Canvas TickMarkContainer
        {
            get { return m_TickMarkContainer; }
            set
            {
                m_TickMarkContainer = value;
            }
        }
        private Canvas LabelContainer
        {
            get { return m_LabelContainer; }
            set
            {
                m_LabelContainer = value;
            }
        }
        private Canvas LimitMarkContainer
        {
            get { return m_LimitMarkContainer; }
            set
            {
                m_LimitMarkContainer = value;
            }
        }
        #endregion

        #region Dependency properties
        [Description("Gets or sets a tick mark color."), Category(nameof(LevelBar))]
        public static readonly DependencyProperty TickMarkColorProperty = DependencyProperty.Register(nameof(TickMarkColor), typeof(Brush), typeof(LevelBar), new PropertyMetadata(Brushes.Black));

        [Description("Gets or sets a level mark color."), Category(nameof(LevelBar))]
        public static readonly DependencyProperty LimitMarkColorProperty = DependencyProperty.Register(nameof(LimitMarkColor), typeof(Brush), typeof(LevelBar), new PropertyMetadata(Brushes.Blue));

        [Description("Gets or sets a tick mark interval."), Category(nameof(LevelBar))]
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof(Interval), typeof(double), typeof(LevelBar), new PropertyMetadata(10.0, OnIntervalPropertyChanged));

        [Description("Gets or sets a minor tick mark count."), Category(nameof(LevelBar))]
        public static readonly DependencyProperty MinorTickMarkCountProperty = DependencyProperty.Register(nameof(MinorTickMarkCount), typeof(int), typeof(LevelBar), new PropertyMetadata(5, OnMinorTickMarkCountPropertyChanged));

        [Description("Gets or sets a low accent of the level bar."), Category(nameof(LevelBar))]
        public static readonly DependencyProperty AccentLowProperty = DependencyProperty.Register(nameof(AccentLow), typeof(Brush), typeof(LevelBar), new PropertyMetadata(Brushes.Green));

        [Description("Gets or sets a mid accent of the level bar."), Category(nameof(LevelBar))]
        public static readonly DependencyProperty AccentMidProperty = DependencyProperty.Register(nameof(AccentMid), typeof(Brush), typeof(LevelBar), new PropertyMetadata(Brushes.GreenYellow));

        [Description("Gets or sets a High accent of the level bar."), Category(nameof(LevelBar))]
        public static readonly DependencyProperty AccentHighProperty = DependencyProperty.Register(nameof(AccentHigh), typeof(Brush), typeof(LevelBar), new PropertyMetadata(Brushes.Orange));

        [Description("Gets or sets a over value accent of the level bar."), Category(nameof(LevelBar))]
        public static readonly DependencyProperty AccentOverValueProperty = DependencyProperty.Register(nameof(AccentOverValue), typeof(Brush), typeof(LevelBar), new PropertyMetadata(Brushes.Red));

        [Description("Gets or sets an upper limit of the level bar."), Category(nameof(LevelBar))]
        public static readonly DependencyProperty UpperLimitProperty = DependencyProperty.Register(nameof(UpperLimit), typeof(double), typeof(LevelBar), new PropertyMetadata(100.0, OnUpperLimitPropertyChanged));

        [Description("Gets or sets a lower limit of the level bar."), Category(nameof(LevelBar))]
        public static readonly DependencyProperty LowerLimitProperty = DependencyProperty.Register(nameof(LowerLimit), typeof(double), typeof(LevelBar), new PropertyMetadata(0.0, OnLowerLimitPropertyChanged));

        [Description("Gets or sets a corner radius of the level bar."), Category(nameof(LevelBar))]
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(LevelBar), null);

        [Description("Gets or sets a style of the label."), Category(nameof(LevelBar))]
        public static readonly DependencyProperty LabelStyleProperty = DependencyProperty.Register(nameof(LabelStyle), typeof(Style), typeof(LevelBar), null);

        [Description("Gets or sets a orientation of the label."), Category(nameof(LevelBar))]
        public static readonly DependencyProperty LabelOrientationProperty = DependencyProperty.Register(nameof(LabelOrientation), typeof(Orientation), typeof(LevelBar), new PropertyMetadata(OnLabelOrientationPropertyChanged));

        #endregion

        #region Dependency event handlers
        private static void OnUpperLimitPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (!(dependencyObject is LevelBar sender)) return;

            sender.OnUpperLimitChanged((double)args.OldValue, (double)args.NewValue);
        }
        private static void OnLowerLimitPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (!(dependencyObject is LevelBar sender)) return;

            sender.OnLowerLimitChanged((double)args.OldValue, (double)args.NewValue);
        }
        private static void OnIntervalPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (!(dependencyObject is LevelBar sender)) return;

            sender.OnIntervalChanged((double)args.OldValue, (double)args.NewValue);
        }
        private static void OnMinorTickMarkCountPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (!(dependencyObject is LevelBar sender)) return;

            sender.OnMinorTickMarkCountChanged((int)args.OldValue, (int)args.NewValue);
        }
        private static void OnCornerRadiusPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (!(dependencyObject is LevelBar sender)) return;

            sender.OnCornerRadiusChanged((double)args.OldValue, (double)args.NewValue);
        }
        private static void OnLabelOrientationPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (!(dependencyObject is LevelBar sender)) return;

            sender.OnLabelOrientationChanged((Orientation)args.OldValue, (Orientation)args.NewValue);
        }
        #endregion

        #region Override methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Indicator = GetTemplateChild(IndicatorName) as Border;
            Track = GetTemplateChild(TrackName) as Border;
            TickMarkContainer = GetTemplateChild(TickMarkContainerName) as Canvas;
            LimitMarkContainer = GetTemplateChild(LimitMarkContainerName) as Canvas;
            LabelContainer = GetTemplateChild(LabelContainerName) as Canvas;

            Initialize();
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            UpdateProgress();
            UpdateCornerRadius();
        }
        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);
            UpdateTickMark();
            UpdateLabel();
            UpdateLimitMark();
        }
        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);
            UpdateTickMark();
            UpdateLabel();
            UpdateLimitMark();
        }
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateLabel();
            UpdateTickMark();
            UpdateLimitMark();
        }
        #endregion

        #region Protected methods
        protected virtual void OnUpperLimitChanged(double oldValue, double newValue)
        {
            UpdateProgress();
            UpdateLimitMark();
        }
        protected virtual void OnLowerLimitChanged(double oldValue, double newValue)
        {
            UpdateProgress();
            UpdateLimitMark();
        }
        protected virtual void OnIntervalChanged(double oldValue, double newValue)
        {
            UpdateTickMark();
            UpdateLabel();
        }
        protected virtual void OnMinorTickMarkCountChanged(int oldValue, int newValue)
        {
            UpdateTickMark();
        }
        protected virtual void OnCornerRadiusChanged(double oldValue, double newValue)
        {
            UpdateCornerRadius();
        }
        protected virtual void OnLabelOrientationChanged(Orientation oldValue, Orientation newValue)
        {
            UpdateLabel();
        }
        #endregion

        #region Private methods
        private void UpdateProgress()
        {
            if (Indicator == null) return;
            if (UpperLimit > LowerLimit)
            {
                if (Value >= LowerLimit && Value < LowerLimit + (UpperLimit - LowerLimit) / 3)
                {
                    Indicator.Background = AccentLow;
                }
                else if (Value >= LowerLimit + ((UpperLimit - LowerLimit) / 3) && Value <= LowerLimit + ((UpperLimit - LowerLimit) / 3 * 2))
                {
                    Indicator.Background = AccentMid;
                }
                else if (Value > LowerLimit + ((UpperLimit - LowerLimit) / 3 * 2) && Value <= UpperLimit)
                {
                    Indicator.Background = AccentHigh;
                }
                else if (Value < LowerLimit || Value > UpperLimit)
                {
                    Indicator.Background = AccentOverValue;
                }
            }
        }
        private void UpdateTickMark()
        {
            if (Maximum > Minimum && TickMarkContainer != null && Track?.ActualWidth > 0 && Track?.ActualHeight > 0)
            {
                TickMarkContainer.Children.Clear();

                double value = Minimum;

                //Major tick mark
                double step = 0;
                while (step <= Maximum)
                {
                    var majorTickMarkXPos = (step * Track.ActualWidth) / (Maximum - Minimum);

                    Line majorTickMark = new Line { X1 = majorTickMarkXPos, X2 = majorTickMarkXPos, Y1 = 0, Y2 = Track.ActualHeight / 4, Stroke = TickMarkColor, StrokeThickness = 1 };

                    //if (majorTickMarkXPos == 0) Canvas.SetLeft(majorTickMark, 1);
                    //else if (majorTickMarkXPos == Maximum) Canvas.SetLeft(majorTickMark, -1);
                    //else Canvas.SetLeft(majorTickMark, 0);

                    //Canvas.SetLeft(line, 0.55);

                    if (step > 0 && step < Maximum)
                        TickMarkContainer.Children.Add(majorTickMark);

                    double minorStep = step;
                    while (minorStep < step + Interval && step < Maximum)
                    {
                        minorStep += Interval / MinorTickMarkCount;

                        var minorTickMarkXPos = (minorStep * Track.ActualWidth) / (Maximum - Minimum);

                        Line minorTickMark = new Line { X1 = minorTickMarkXPos, X2 = minorTickMarkXPos, Y1 = 0, Y2 = Track.ActualHeight / 6, Stroke = TickMarkColor, StrokeThickness = 1 };
                        if (minorStep < step + Interval && minorStep < Maximum)
                            TickMarkContainer.Children.Add(minorTickMark);
                    }


                    step += Interval;
                    value += Interval;
                }
            }
        }
        private void UpdateLabel()
        {
            if (Maximum > Minimum && LabelContainer != null && Track?.ActualWidth > 0 && Track?.ActualHeight > 0)
            {
                LabelContainer.Children.Clear();
                double labelValue = Minimum;

                double step = 0;
                while (labelValue <= Maximum)
                {
                    var xPos = (step * Track.ActualWidth) / (Maximum - Minimum);

                    //Calculate label height
                    FormattedText formattedText = new FormattedText(labelValue.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Foreground, 96);

                    Label label = new Label
                    {
                        Content = labelValue.ToString(),
                        Margin = new Thickness(0),
                        Padding = new Thickness(0),
                    };


                    if (LabelStyle != null) label.Style = LabelStyle;
                       
                    if(LabelOrientation == Orientation.Vertical)
                    {
                        RotateTransform rotateTransform = new RotateTransform(90);
                        label.LayoutTransform = rotateTransform;

                        Canvas.SetLeft(label, xPos - (formattedText.Height / 2) + 10);
                        Canvas.SetTop(label, 0);
                        LabelContainer.Height = formattedText.Width + (formattedText.Width * 0.1);
                    }
                    else
                    {
                        Canvas.SetLeft(label, xPos - (formattedText.Width / 2) + 10);
                        Canvas.SetTop(label, 0);
                        LabelContainer.Height = formattedText.Height + (formattedText.Height * 0.1);
                    }

          

                    LabelContainer.Children.Add(label);
                    

                    step += Interval;
                    labelValue += Interval;
                }
            }
        }
        private void UpdateLimitMark()
        {
            if (LimitMarkContainer?.ActualHeight > 0 && Track?.ActualHeight > 0 && Track?.ActualWidth > 0)
            {
                LimitMarkContainer.UpdateLayout();
                LimitMarkContainer.Children.Clear();
                // add limit mark
                if (UpperLimit < Maximum)
                {

                    var upperLimitXPos = (UpperLimit * Track.ActualWidth) / (Maximum - Minimum);
                    Line upperLimitline = new Line { X1 = upperLimitXPos, X2 = upperLimitXPos, Y1 = 0, Y2 = LimitMarkContainer.ActualHeight, Stroke = LimitMarkColor, StrokeThickness = 2, MinHeight = 1, MinWidth = 1 };
                    LimitMarkContainer.Children.Add(upperLimitline);
                }

                if (LowerLimit > Minimum)
                {
                    var lowerLimitXPos = (LowerLimit * Track.ActualWidth) / (Maximum - Minimum);
                    Line lowerLimitline = new Line { X1 = lowerLimitXPos, X2 = lowerLimitXPos, Y1 = 0, Y2 = LimitMarkContainer.ActualHeight, Stroke = LimitMarkColor, StrokeThickness = 2, MinHeight = 1, MinWidth = 1 };
                    LimitMarkContainer.Children.Add(lowerLimitline);
                }

                LimitMarkContainer.UpdateLayout();
            }
        }
        private void UpdateCornerRadius()
        {
            if (Indicator != null && Track != null)
            {
                if (Value >= Maximum) Indicator.CornerRadius = CornerRadius;
                else Indicator.CornerRadius = new CornerRadius(CornerRadius.TopLeft, 0, 0, CornerRadius.BottomLeft);


                Track.CornerRadius = CornerRadius;
            }
        }
        private void Initialize()
        {
            UpdateCornerRadius();
            UpdateProgress();
            UpdateLabel();
            UpdateTickMark();
            UpdateLimitMark();

        }

        #endregion
        #region Event handlers
        private void Track_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateTickMark();
        }
        #endregion
    }
}