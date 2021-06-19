using Microsoft.Expression.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sans.Windows.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Sans.Windows.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Sans.Windows.Controls;assembly=Sans.Windows.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    /// 

    [TemplatePart(Name = Level.ElementIndicator, Type = typeof(Arc))]
    [TemplatePart(Name = Level.ElementBackIndicator, Type = typeof(Arc))]
    [TemplatePart(Name = Level.ElementValue, Type = typeof(TextBlock))]
    public class Level : Control
    {
        #region Constants
        private const string ElementIndicator = "PART_Indicator";
        private const string ElementBackIndicator = "PART_BackIndicator";
        private const string ElementValue = "PART_Value";
        #endregion
        #region Constructors
        static Level()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Level), new FrameworkPropertyMetadata(typeof(Level)));
        }
        #endregion

        #region private Fields
        private Microsoft.Expression.Shapes.Arc indicator;
        private Microsoft.Expression.Shapes.Arc backIndicator;
        private TextBlock valueText;

        bool isMouseDown;
        Point previousePointerPosition;
        #endregion

        #region private Properties
        private Arc Indicator
        {
            get
            {
                return indicator;
            }
            set
            {
                
                indicator = value;
                if (indicator != null)
                {
                    indicator.StrokeThickness = Thickness;
                    OnValueChanged();

                }                   
            }
        }
        private Arc BackIndicator
        {
            get
            {
                return backIndicator;
            }
            set
            {

                backIndicator = value;
                if (backIndicator != null)
                {
                    backIndicator.Stroke = BackgroundIndicator;
                    backIndicator.StrokeThickness = Thickness;
                }
            }
        }
        private TextBlock ValueText
        {
            get
            {
                return valueText;
            }
            set
            {
                if(valueText != null)
                {
                    valueText.MouseWheel -= Control_MouseWheel;
                    valueText.MouseDown -= Control_MouseDown;
                    valueText.MouseMove -= Control_MouseMove;
                    valueText.MouseUp -= Control_MouseUp;
                    valueText.MouseLeftButtonDown -= Control_MouseLeftButtonDown;
                }
                    
                valueText = value;

                if (valueText != null)
                {
                    valueText.MouseWheel += Control_MouseWheel;
                    valueText.MouseDown += Control_MouseDown;
                    valueText.MouseMove += Control_MouseMove;
                    valueText.MouseUp += Control_MouseUp;
                    valueText.MouseLeftButtonDown += Control_MouseLeftButtonDown;

                    OnValueChanged();

                }
            }
        }

        #endregion

        #region Public methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Indicator = GetTemplateChild(ElementIndicator) as Arc;
            BackIndicator = GetTemplateChild(ElementBackIndicator) as Arc;
            ValueText = GetTemplateChild(ElementValue) as TextBlock;

        }

        #endregion

        #region public properties
        /// <summary>
        /// Gets or sets value.
        /// </summary>
        public double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
            }

            set
            {
                SetValue(ValueProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets default value.
        /// </summary>
        public double DefaultValue
        {
            get
            {
                return (double)GetValue(DefaultValueProperty);
            }

            set
            {
                SetValue(DefaultValueProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets unit of value.
        /// </summary>
        public string Unit
        {
            get
            {
                return (string)GetValue(UnitProperty);
            }

            set
            {
                SetValue(UnitProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets limit minimum value.
        /// </summary>
        public double Min
        {
            get
            {
                return (double)GetValue(MinProperty);
            }

            set
            {
                SetValue(MinProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets limit maximum value.
        /// </summary>
        public double Max
        {
            get
            {
                return (double)GetValue(MaxProperty);
            }

            set
            {
                SetValue(MaxProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets interval of set value when mouse wheel, mouse drag or touch drag through the control.
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
        /// Gets or sets treshold of the level.
        /// </summary>
        public double DragThresHold
        {
            get
            {
                return (double)GetValue(ThresholdProperty);
            }

            set
            {
                SetValue(ThresholdProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets background Indicator color of the level.
        /// </summary>
        public Brush BackgroundIndicator
        {
            get
            {
                return (Brush)GetValue(BackgroundIndicatorProperty);
            }

            set
            {
                SetValue(BackgroundIndicatorProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets low accent color of the level.
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
        /// Gets or sets mid accent color of the level.
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
        /// Gets or sets high accent color of the level.
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
        /// Gets or sets full accent color of the level.
        /// </summary>
        public Brush AccentFull
        {
            get
            {
                return (Brush)GetValue(AccentFullProperty);
            }

            set
            {
                SetValue(AccentFullProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets thickness of the level.
        /// </summary>
        public double Thickness
        {
            get
            {
                return (double)GetValue(ThicknessProperty);
            }

            set
            {
                SetValue(ThicknessProperty, value);
            }
        }
        #endregion
        #region Dependency properties
        [Description("Gets or sets a value."), Category("Level")]
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(Level), new PropertyMetadata(60.000,new PropertyChangedCallback(ValueChangedCallback)));

        [Description("Gets or sets a default value."), Category("Level")]
        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register(nameof(DefaultValue), typeof(double), typeof(Level), new PropertyMetadata(50.000));

        [Description("Gets or sets a value unit."), Category("Level")]
        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register(nameof(Unit), typeof(string), typeof(Level), new PropertyMetadata("%"));

        [Description("Gets or sets a min value."), Category("Level")]
        public static readonly DependencyProperty MinProperty = DependencyProperty.Register(nameof(Min), typeof(double), typeof(Level), new PropertyMetadata(0.000));

        [Description("Gets or sets a max value."), Category("Level")]
        public static readonly DependencyProperty MaxProperty = DependencyProperty.Register(nameof(Max), typeof(double), typeof(Level), new PropertyMetadata(100.000));

        [Description("Gets or sets an interval of set value when mouse wheel, mouse drag or touch drag through the control."), Category("Level")]
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof(Interval), typeof(double), typeof(Level), new PropertyMetadata(1.0));

        [Description("Gets or sets a Threshold of mouse drag or touch drag."), Category("Level")]
        public static readonly DependencyProperty ThresholdProperty = DependencyProperty.Register(nameof(DragThresHold), typeof(double), typeof(Level), new PropertyMetadata(0.0));

        [Description("Gets or sets a background of the level."), Category("Level")]
        public static readonly DependencyProperty BackgroundIndicatorProperty = DependencyProperty.Register(nameof(BackgroundIndicator), typeof(Brush), typeof(Level), new PropertyMetadata(Brushes.Gray));

        [Description("Gets or sets a low accent of the level."), Category("Level")]
        public static readonly DependencyProperty AccentLowProperty = DependencyProperty.Register(nameof(AccentLow), typeof(Brush), typeof(Level), new PropertyMetadata(Brushes.Orange));

        [Description("Gets or sets a mid accent of the level."), Category("Level")]
        public static readonly DependencyProperty AccentMidProperty = DependencyProperty.Register(nameof(AccentMid), typeof(Brush), typeof(Level), new PropertyMetadata(Brushes.GreenYellow));

        [Description("Gets or sets a High accent of the level."), Category("Level")]
        public static readonly DependencyProperty AccentHighProperty = DependencyProperty.Register(nameof(AccentHigh), typeof(Brush), typeof(Level), new PropertyMetadata(Brushes.Green));

        [Description("Gets or sets a full accent of the level."), Category("Level")]
        public static readonly DependencyProperty AccentFullProperty = DependencyProperty.Register(nameof(AccentFull), typeof(Brush), typeof(Level), new PropertyMetadata(Brushes.Blue));

        [Description("Gets or sets a level thickness."), Category("Level")]
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(nameof(Thickness), typeof(double), typeof(Level), new PropertyMetadata(10.0));      

        #endregion

        #region Dependency property changed event handlers
        public static void ValueChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if(obj is Level level)
            {
                level.OnValueChanged();
            }
        }
        #endregion

        #region Event handlers
        private void Control_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double d = e.Delta / 120; // Mouse wheel 1 click (120 delta) = 1 step
            double temp = (d * Interval) + Value;
            if (temp >= Min && temp <= Max) Value = temp;
        }
        private void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            (sender as TextBlock).CaptureMouse();
            previousePointerPosition = e.GetPosition((TextBlock)sender);
        }
        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point newPointerPosition = e.GetPosition((TextBlock)sender);
                double dY = previousePointerPosition.Y - newPointerPosition.Y;
                if (Math.Abs(dY) > DragThresHold)
                {
                    double temp = Value + (Math.Sign(dY) * Interval);
                    if (temp >= Min && temp <= Max) Value = temp;
                    previousePointerPosition = newPointerPosition;
                }
            }
        }
        private void Control_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
            (sender as TextBlock).ReleaseMouseCapture();
        }
        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) Value = DefaultValue;
        }

        #endregion
        #region Private methods
        private void OnValueChanged()
        {
            //update value
            if (valueText != null) valueText.Text = Value.ToString() + Unit;
            if (indicator != null)
            {
                indicator.EndAngle = 360 / (Max - Min) * (Value - Min) - 180;

                // update indicator accent
                if (indicator.EndAngle >= -180 && indicator.EndAngle < -90) indicator.Stroke = AccentLow;
                else if (indicator.EndAngle >= -90 && indicator.EndAngle < 90) indicator.Stroke = AccentMid;
                else if (indicator.EndAngle >= 90 && indicator.EndAngle < 180) indicator.Stroke = AccentHigh;
                else if (indicator.EndAngle >= 180) indicator.Stroke = AccentFull;
            }

            // update back indicator accent       
            if (backIndicator != null)
            {
                if (indicator.EndAngle <= -180) backIndicator.Stroke = Brushes.Red;
                else backIndicator.Stroke = BackgroundIndicator;
            }
           
        }
        #endregion




    }
}
