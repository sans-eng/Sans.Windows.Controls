using Sans.Internals.Controls;
using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Sans.Windows.Controls
{
    /// <summary>
    /// Represents the method that will handle input validation error.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    public delegate void InputValidationErrorEventHandler(object sender, InputValidationErrorEventArgs e);
    /// <summary>
    /// Represents the method that will handle input validation warning.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    public delegate void InputValidationWarningEventHandler(object sender, InputValidationWarningEventArgs e);
    public class TextBoxInputValidation : DependencyObject
    {
        #region Private fields
        /// <summary>
        /// The decimal separator which the value is depend on current culture. to validate the input.
        /// </summary>
        private readonly static char m_DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
        #endregion

        #region Dependency properties
        /// <summary>
        /// Identifies the IsEnabled attached property.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(TextBoxInputValidation), new FrameworkPropertyMetadata(OnIsEnabledPropertyChanged));
        /// <summary>
        /// Identifies the Min attached property.
        /// </summary>
        public static readonly DependencyProperty MinProperty = DependencyProperty.RegisterAttached("Min", typeof(double), typeof(TextBoxInputValidation), new FrameworkPropertyMetadata(double.MinValue, OnMinPropertyChanged));
        /// <summary>
        /// Identifies the Max attached property.
        /// </summary>
        public static readonly DependencyProperty MaxProperty = DependencyProperty.RegisterAttached("Max", typeof(double), typeof(TextBoxInputValidation), new FrameworkPropertyMetadata(double.MaxValue, OnMaxPropertyChanged));
        /// <summary>
        /// Identifies the MinLength attached property.
        /// </summary>
        public static readonly DependencyProperty MinLengthProperty = DependencyProperty.RegisterAttached("MinLength", typeof(int), typeof(TextBoxInputValidation), new FrameworkPropertyMetadata(0, OnMinLengthPropertyChanged));
        /// <summary>
        /// Identifies the MaxLength attached property.
        /// </summary>
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.RegisterAttached("MaxLength", typeof(int), typeof(TextBoxInputValidation), new FrameworkPropertyMetadata(255, OnMaxLengthPropertyChanged));
        /// <summary>
        /// Identifies the DecimalPlace attached property.
        /// </summary>
        public static readonly DependencyProperty DecimalPlaceProperty = DependencyProperty.RegisterAttached("DecimalPlace", typeof(int), typeof(TextBoxInputValidation), new FrameworkPropertyMetadata(3, OnDecimalPlacePropertyChanged));
        /// <summary>
        /// Identifies the ValidationType attached property.
        /// </summary>
        public static readonly DependencyProperty ValidationTypeProperty = DependencyProperty.RegisterAttached("ValidationType", typeof(InputValidationType), typeof(TextBoxInputValidation), new FrameworkPropertyMetadata(InputValidationType.FloatingPoint, OnValidationTypePropertyChanged));
        /// <summary>
        /// Identifies the ErrorTemplate attached property.
        /// </summary>
        public static readonly DependencyProperty ErrorTemplateProperty = DependencyProperty.RegisterAttached("ErrorTemplate", typeof(ControlTemplate), typeof(TextBoxInputValidation), new PropertyMetadata(OnErrorTemplatePropertyChanged));
        /// <summary>
        /// Identifies the NotificationTemplate attached property.
        /// </summary>
        public static readonly DependencyProperty WarningTemplateProperty = DependencyProperty.RegisterAttached("WarningTemplate", typeof(ControlTemplate), typeof(TextBoxInputValidation), new PropertyMetadata(OnWarningTemplatePropertyChanged));
        /// <summary>
        /// Identifies the  NotificationInitialShowDelay attached property.
        /// </summary>
        public static readonly DependencyProperty WarningInitialShowDelayProperty = DependencyProperty.RegisterAttached("WarningInitialShowDelay", typeof(int), typeof(TextBoxInputValidation), new PropertyMetadata(300, OnWarningInitialShowDelayChanged));
        /// <summary>
        /// Identifies the NotificationShowDuration attached property.
        /// </summary>
        public static readonly DependencyProperty WarningShowDurationProperty = DependencyProperty.RegisterAttached("WarningShowDuration", typeof(int), typeof(TextBoxInputValidation), new PropertyMetadata(3000, OnWarningShowDurationProperty));
        /// <summary>
        /// Identifies the IsInsideScrollViewer attached property.
        /// </summary>
        public static readonly DependencyProperty IsClippedProperty = DependencyProperty.RegisterAttached("IsClipped", typeof(bool), typeof(TextBoxInputValidation), new PropertyMetadata(OnIsClippedPropertyChanged));
        /// <summary>
        /// Identifies the ScrollViewer attached property.
        /// </summary>
        public static readonly DependencyProperty AdornerLayerLocationProperty = DependencyProperty.RegisterAttached("AdornerLayerLocation", typeof(UIElement), typeof(TextBoxInputValidation), new PropertyMetadata(OnAdornerLayerLocationPropertyChanged));

        /// <summary>
        /// Identifies the HaseError attached property.
        /// </summary>
        public static readonly DependencyProperty HasErrorProperty = DependencyProperty.RegisterAttached("HasError", typeof(bool), typeof(TextBoxInputValidation), new PropertyMetadata(OnHasErrorPropertyChanged));
        /// <summary>
        /// Identifies the  WarningForeground attached property.
        /// </summary>
        public static readonly DependencyProperty DirtyForegroundProperty = DependencyProperty.RegisterAttached("DirtyForeground", typeof(Brush), typeof(TextBoxInputValidation), new PropertyMetadata(Brushes.Orange, OnDirtyForegroundPropertyChanged));
        /// <summary>
        /// Identifies the  WarningFontStyle attached property.
        /// </summary>
        public static readonly DependencyProperty DirtyFontStyleProperty = DependencyProperty.RegisterAttached("DirtyFontStyle", typeof(FontStyle), typeof(TextBoxInputValidation), new PropertyMetadata(FontStyles.Italic, OnDirtyFontStylePropertyChanged));
        /// <summary>
        /// Identifies the ErrorBackground attached property.
        /// </summary>
        public static readonly DependencyProperty ErrorBackgroundProperty = DependencyProperty.RegisterAttached("ErrorBackground", typeof(Brush), typeof(TextBoxInputValidation), new PropertyMetadata(Brushes.Red, OnErrorBackgroundPropertyChanged));
        /// <summary>
        /// Identifies the Error event.
        /// </summary>
        public static readonly RoutedEvent ValidationErrorEvent = EventManager.RegisterRoutedEvent("ValidationError", RoutingStrategy.Bubble, typeof(InputValidationErrorEventHandler), typeof(TextBoxInputValidation));
        /// <summary>
        /// Identifies the Warning event.
        /// </summary>
        public static readonly RoutedEvent ValidationWarningEvent = EventManager.RegisterRoutedEvent("ValidationWarning", RoutingStrategy.Bubble, typeof(InputValidationWarningEventHandler), typeof(TextBoxInputValidation));


        /// <summary>
        /// Identifies the OriginalFontStyle attached property.
        /// </summary>
        private static readonly DependencyProperty OriginalFontStyleProperty = DependencyProperty.RegisterAttached("OriginalFontStyle", typeof(FontStyle), typeof(TextBoxInputValidation), new PropertyMetadata(FontStyles.Normal));
        /// <summary>
        /// Identifies the OriginalForeground attached property.
        /// </summary>
        private static readonly DependencyProperty OriginalForegroundProperty = DependencyProperty.RegisterAttached("OriginalForeground", typeof(Brush), typeof(TextBoxInputValidation), new PropertyMetadata(SystemColors.WindowTextBrush));
        /// <summary>
        /// Identifies the OriginalBackground attached property.
        /// </summary>
        private static readonly DependencyProperty OriginalBackgroundProperty = DependencyProperty.RegisterAttached("OriginalBackground", typeof(Brush), typeof(TextBoxInputValidation), new PropertyMetadata(SystemColors.WindowBrush));
        /// <summary>
        /// Identifies the  ErrorTemplateAdorner attached property.
        /// </summary>
        private static readonly DependencyProperty ErrorAdornerProperty = DependencyProperty.RegisterAttached("ErrorAdorner", typeof(TemplatedAdorner), typeof(TextBoxInputValidation));
        /// <summary>
        /// Identifies the  NotificationTemplateAdorner attached property.
        /// </summary>
        private static readonly DependencyProperty WarningAdornerProperty = DependencyProperty.RegisterAttached("WarningAdorner", typeof(TemplatedAdorner), typeof(TextBoxInputValidation));
        /// <summary>
        /// Identifies the AdornerHost attached property.
        /// </summary>
        private static readonly DependencyProperty AdornerHostProperty = DependencyProperty.RegisterAttached("AdornerHost", typeof(AdornerLayer), typeof(TextBoxInputValidation));
        /// <summary>
        /// Identifies the alidationContext attached property.
        /// </summary>
        internal static readonly DependencyProperty ValidationContextProperty = DependencyProperty.RegisterAttached("ValidationContext", typeof(InputValidationContext), typeof(TextBoxInputValidation));
        #endregion

        #region Dependency acces methods
        /// <summary>
        /// Sets the value of the <see cref="IsEnabledProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetIsEnabled(DependencyObject o, bool value)
        {
            o.SetValue(IsEnabledProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="IsEnabledProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="IsEnabledProperty"/> property value.</returns>
        public static bool GetIsEnabled(DependencyObject o)
        {
            return (bool)o.GetValue(IsEnabledProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="MinProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetMin(DependencyObject o, double value)
        {
            o.SetValue(MinProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="MinProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="MinProperty"/> property value.</returns>
        public static double GetMin(DependencyObject o)
        {
            return (double)o.GetValue(MinProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="MaxProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetMax(DependencyObject o, double value)
        {
            o.SetValue(MaxProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="MaxProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="MaxProperty"/> property value.</returns>
        public static double GetMax(DependencyObject o)
        {
            return (double)o.GetValue(MaxProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="MinLengthProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetMinLength(DependencyObject o, int value)
        {
            o.SetValue(MinLengthProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="MinLengthProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="MinLengthProperty"/> property value.</returns>
        public static int GetMinLength(DependencyObject o)
        {
            return (int)o.GetValue(MinLengthProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="MaxLengthProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetMaxLength(DependencyObject o, int value)
        {
            o.SetValue(MaxLengthProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="MaxLengthProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="MaxLengthProperty"/> property value.</returns>
        public static int GetMaxLength(DependencyObject o)
        {
            return (int)o.GetValue(MaxLengthProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="DecimalPlaceProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetDecimalPlace(DependencyObject o, int value)
        {
            o.SetValue(DecimalPlaceProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="DecimalPlaceProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="DecimalPlaceProperty"/> property value.</returns>
        public static int GetDecimalPlace(DependencyObject o)
        {
            return (int)o.GetValue(DecimalPlaceProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="ValidationTypeProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetValidationType(DependencyObject o, InputValidationType value)
        {
            o.SetValue(ValidationTypeProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="ValidationTypeProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="ValidationTypeProperty"/> property value.</returns>
        public static InputValidationType GetValidationType(DependencyObject o)
        {
            return (InputValidationType)o.GetValue(ValidationTypeProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="ErrorTemplateProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetErrorTemplate(DependencyObject o, ControlTemplate value)
        {
            o.SetValue(ErrorTemplateProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="ErrorTemplateProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="ErrorTemplateProperty"/> property value.</returns>
        public static ControlTemplate GetErrorTemplate(DependencyObject o)
        {
            return (ControlTemplate)o.GetValue(ErrorTemplateProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="WarningTemplateProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetWarningTemplate(DependencyObject o, ControlTemplate value)
        {
            o.SetValue(WarningTemplateProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="WarningTemplateProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="WarningTemplateProperty"/> property value.</returns>
        public static ControlTemplate GetWarningTemplate(DependencyObject o)
        {
            return (ControlTemplate)o.GetValue(WarningTemplateProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="WarningInitialShowDelayProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetWarningInitialShowDelay(DependencyObject o, int value)
        {
            o.SetValue(WarningInitialShowDelayProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="WarningInitialShowDelayProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="WarningInitialShowDelayProperty"/> property value.</returns>
        public static int GetWarningInitialShowDelay(DependencyObject o)
        {
            return (int)o.GetValue(WarningInitialShowDelayProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="WarningShowDurationProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetWarningShowDuration(DependencyObject o, int value)
        {
            o.SetValue(WarningShowDurationProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="WarningShowDurationProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="WarningShowDurationProperty"/> property value.</returns>
        public static int GetWarningShowDuration(DependencyObject o)
        {
            return (int)o.GetValue(WarningShowDurationProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="HasErrorProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        private static void SetHasError(DependencyObject o, bool value)
        {
            o.SetValue(HasErrorProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="HasErrorProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="HasErrorProperty"/> property value.</returns>
        public static bool GetHasError(DependencyObject o)
        {
            return (bool)o.GetValue(HasErrorProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="DirtyForegroundProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetDirtyForeground(DependencyObject o, Brush value)
        {
            o.SetValue(DirtyForegroundProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="DirtyForegroundProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="DirtyForegroundProperty"/> property value.</returns>
        public static Brush GetDirtyForeground(DependencyObject o)
        {
            return (Brush)o.GetValue(DirtyForegroundProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="DirtyFontStyleProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetDirtyFontStyle(DependencyObject o, FontStyle value)
        {
            o.SetValue(DirtyFontStyleProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="DirtyFontStyleProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="DirtyFontStyleProperty"/> property value.</returns>
        public static FontStyle GetDirtyFontStyle(DependencyObject o)
        {
            return (FontStyle)o.GetValue(DirtyFontStyleProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="ErrorBackgroundProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetErrorBackground(DependencyObject o, Brush value)
        {
            o.SetValue(ErrorBackgroundProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="ErrorBackgroundProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="ErrorBackgroundProperty"/> property value.</returns>
        public static Brush GetErrorBackground(DependencyObject o)
        {
            return (Brush)o.GetValue(ErrorBackgroundProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="IsClippedProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetIsClipped(DependencyObject o, bool value)
        {
            o.SetValue(IsClippedProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="IsClippedProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="IsClippedProperty"/> property value.</returns>
        public static bool GetIsClipped(DependencyObject o)
        {
            return (bool)o.GetValue(IsClippedProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="AdornerLayerLocationProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetAdornerLayerLocation(DependencyObject o, UIElement value)
        {
            o.SetValue(AdornerLayerLocationProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="AdornerLayerLocationProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="AdornerLayerLocationProperty"/> property value.</returns>
        public static UIElement GetAdornerLayerLocation(DependencyObject o)
        {
            return (UIElement)o.GetValue(AdornerLayerLocationProperty);
        }
        /// <summary>
        /// Adds a handler for <see cref="ValidationErrorEvent"/> attached event.
        /// </summary>
        /// <param name="o">The <see cref="UIElement"/> or <see cref="ContentElement"/> that listens to this event.</param>
        /// <param name="handler">The event handler to be added.</param>
        public static void AddValidationErrorHandler(DependencyObject o, InputValidationErrorEventHandler handler)
        {
            if (o is UIElement uiElement) uiElement.AddHandler(ValidationErrorEvent, handler);
        }
        /// <summary>
        ///  Removes a handler for <see cref="ValidationErrorEvent"/> attached event.
        /// </summary>
        /// <param name="o">The <see cref="UIElement"/> or <see cref="ContentElement"/> that listens to this event.</param>
        /// <param name="handler">The event handler to be removed.</param>
        public static void RemoveValidationErrorHandler(DependencyObject o, InputValidationErrorEventHandler handler)
        {
            if (o is UIElement uiElement) uiElement.RemoveHandler(ValidationErrorEvent, handler);
        }
        /// <summary>
        /// Adds a handler for <see cref="ValidationWarningEvent"/> attached event.
        /// </summary>
        /// <param name="o">The <see cref="UIElement"/> or <see cref="ContentElement"/> that listens to this event.</param>
        /// <param name="handler">The event handler to be added.</param>
        public static void AddValidationWarningHandler(DependencyObject o, InputValidationWarningEventHandler handler)
        {
            if (o is UIElement uiElement) uiElement.AddHandler(ValidationWarningEvent, handler);
        }
        /// <summary>
        ///  Removes a handler for <see cref="ValidationWarningEvent"/> attached event.
        /// </summary>
        /// <param name="o">The <see cref="UIElement"/> or <see cref="ContentElement"/> that listens to this event.</param>
        /// <param name="handler">The event handler to be removed.</param>
        public static void RemoveValidationWarningHandler(DependencyObject o, InputValidationWarningEventHandler handler)
        {
            if (o is UIElement uiElement) uiElement.RemoveHandler(ValidationWarningEvent, handler);
        }

        /// <summary>
        /// Sets the value of the <see cref="OriginalFontStyleProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        private static void SetOriginalFontStyle(DependencyObject o, FontStyle value)
        {
            o.SetValue(OriginalFontStyleProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="OriginalFontStyleProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="OriginalFontStyleProperty"/> property value.</returns>
        private static FontStyle GetOriginalFontStyle(DependencyObject o)
        {
            return (FontStyle)o.GetValue(OriginalFontStyleProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="OriginalForegroundProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        private static void SetOriginalForeground(DependencyObject o, Brush value)
        {
            o.SetValue(OriginalForegroundProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="OriginalForegroundProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="OriginalForegroundProperty"/> property value.</returns>
        private static Brush GetOriginalForeground(DependencyObject o)
        {
            return (Brush)o.GetValue(OriginalForegroundProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="OriginalBackgroundProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        private static void SetOriginalBackground(DependencyObject o, Brush value)
        {
            o.SetValue(OriginalBackgroundProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="OriginalBackgroundProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="OriginalBackgroundProperty"/> property value.</returns>
        private static Brush GetOriginalBackground(DependencyObject o)
        {
            return (Brush)o.GetValue(OriginalBackgroundProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="WarningAdornerProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        private static void SetWarningAdorner(DependencyObject o, TemplatedAdorner value)
        {
            o.SetValue(WarningAdornerProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="WarningAdornerProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="WarningAdornerProperty"/> property value.</returns>
        private static TemplatedAdorner GetWarningAdorner(DependencyObject o)
        {
            return (TemplatedAdorner)o.GetValue(WarningAdornerProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="AdornerHostProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        private static void SetAdornerHost(DependencyObject o, AdornerLayer value)
        {
            o.SetValue(AdornerHostProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="AdornerHostProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="AdornerHostProperty"/> property value.</returns>
        private static AdornerLayer GetAdornerHost(DependencyObject o)
        {
            return (AdornerLayer)o.GetValue(AdornerHostProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="ValidationContextProperty"/> for an object.
        /// </summary>
        /// <param name="o">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        internal static void SetValidationContext(DependencyObject o, InputValidationContext value)
        {
            o.SetValue(ValidationContextProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="ValidationContextProperty"/> for an object.
        /// </summary>
        /// <param name="o"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="ValidationContextProperty"/> property value.</returns>
        internal static InputValidationContext GetValidationContext(DependencyObject o)
        {
            return (InputValidationContext)o.GetValue(ValidationContextProperty);
        }
        #endregion

        #region Dependency property callbacks
        private static void OnIsEnabledPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox textBox)) throw new InvalidOperationException("The control is not TextBox.");

            if ((bool)e.NewValue)
            {
                //get original properties
                SetOriginalBackground(textBox, textBox.Background);
                SetOriginalFontStyle(textBox, textBox.FontStyle);
                SetOriginalForeground(textBox, textBox.Foreground);

                Attach(textBox);
                //initial validation
                if (textBox.IsVisible) OnError(textBox, ValidateText(textBox));

            }
            else
            {
                Detach(textBox);
                OnError(textBox, InputValidationResult.Success);
                textBox.Foreground = GetOriginalForeground(textBox);
                textBox.FontStyle = GetOriginalFontStyle(textBox);
            }
        }
        private static void OnMinPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox textBox)) throw new InvalidOperationException("The control is not TextBox.");

            //initial validation
            if (GetIsEnabled(textBox) && textBox.IsVisible) OnError(textBox, ValidateText(textBox));
        }
        private static void OnMaxPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox textBox)) throw new InvalidOperationException("The control is not TextBox.");

            //initial validation
            if (GetIsEnabled(textBox) && textBox.IsVisible) OnError(textBox, ValidateText(textBox));
        }
        private static void OnMinLengthPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox textBox)) throw new InvalidOperationException("The control is not TextBox.");

            //initial validation
            if (GetIsEnabled(textBox) && textBox.IsVisible) OnError(textBox, ValidateText(textBox));
        }
        private static void OnMaxLengthPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox textBox)) throw new InvalidOperationException("The control is not TextBox.");

            //initial validation
            if (GetIsEnabled(textBox) && textBox.IsVisible) OnError(textBox, ValidateText(textBox));
        }
        private static void OnDecimalPlacePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox textBox)) throw new InvalidOperationException("The control is not TextBox.");

            //initial validation
            if (GetIsEnabled(textBox) && textBox.IsVisible) OnError(textBox, ValidateText(textBox));
        }
        private static void OnValidationTypePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox textBox)) throw new InvalidOperationException("The control is not TextBox.");

            //initial validation
            if (GetIsEnabled(textBox) && textBox.IsVisible) OnError(textBox, ValidateText(textBox));
        }
        private static void OnErrorTemplatePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox textBox)) throw new InvalidOperationException("The control is not TextBox.");

            //initial validation
            if (GetIsEnabled(textBox) && textBox.IsVisible) OnError(textBox, ValidateText(textBox));
        }
        private static void OnWarningTemplatePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox textBox)) throw new InvalidOperationException("The control is not TextBox.");

            //attach notification adorner
            AttachWarningAdorner(textBox);
        }
        private static void OnWarningInitialShowDelayChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox)) throw new InvalidOperationException("The control is not TextBox.");
        }
        private static void OnWarningShowDurationProperty(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox)) throw new InvalidOperationException("The control is not TextBox.");
        }
        private static void OnHasErrorPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox)) throw new InvalidOperationException("The control is not TextBox.");
        }
        private static void OnDirtyForegroundPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox)) throw new InvalidOperationException("The control is not TextBox.");
        }
        private static void OnDirtyFontStylePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox)) throw new InvalidOperationException("The control is not TextBox.");
        }
        private static void OnErrorBackgroundPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox)) throw new InvalidOperationException("The control is not TextBox.");
        }
        private static void OnIsClippedPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox textBox)) throw new InvalidOperationException("The control is not TextBox.");

            //Remove all adorners on layer before reinitialize host
            RemoveErrorAdorner(textBox);
            RemoveWarningAdorner(textBox);

            //Get adornerHost and attach it.
            AttachAdornerHost(textBox);
            //attach notification adorner
            AttachWarningAdorner(textBox);

            //initial validation
            if (GetIsEnabled(textBox) && textBox.IsVisible) OnError(textBox, ValidateText(textBox));
        }
        private static void OnAdornerLayerLocationPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is TextBox textBox)) throw new InvalidOperationException("The control is not TextBox.");

            //Remove all adorners on layer before reinitialize host
            RemoveErrorAdorner(textBox);
            RemoveWarningAdorner(textBox);

            //Get adornerHost and attach it.
            AttachAdornerHost(textBox);
            //attach notification adorner
            AttachWarningAdorner(textBox);

            //initial validation
            if (GetIsEnabled(textBox) && textBox.IsVisible) OnError(textBox, ValidateText(textBox));
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Attach the events of the textbox.
        /// </summary>
        private static void Attach(TextBox textBox)
        {
            //for catch enter key and white spaces key.
            textBox.PreviewKeyDown += OnPreviewKeyDown;
            //for validate before lost focus
            textBox.PreviewLostKeyboardFocus += OnPreviewLostKeyboardFocus;
            //for validate on text changed
            textBox.TextChanged += OnTextChanged;
            //for validate on text input
            textBox.PreviewTextInput += OnPreviewTextInput;
            //for validate on pasting
            DataObject.AddPastingHandler(textBox, OnPasting);
            //for catch adorner layer host
            textBox.IsVisibleChanged += OnIsVisibleChanged;

            //for catch adorner layer host
            textBox.Loaded += OnLoaded;
        }

        /// <summary>
        /// Detach the events of the textbox.
        /// </summary>
        private static void Detach(TextBox textBox)
        {
            textBox.PreviewKeyDown -= OnPreviewKeyDown;
            textBox.PreviewLostKeyboardFocus -= OnPreviewLostKeyboardFocus;
            textBox.TextChanged -= OnTextChanged;
            textBox.PreviewTextInput -= OnPreviewTextInput;
            DataObject.RemovePastingHandler(textBox, OnPasting);
            textBox.IsVisibleChanged -= OnIsVisibleChanged;

            //for catch adorner layer host
            textBox.Loaded -= OnLoaded;
        }
        /// <summary>
        /// Give feedback to the UI, indicates by the status of the error, true: error exist, false: reset previous state.
        /// </summary>
        /// <param name="textBox">The textbox to set the feedback.</param>
        /// <param name="status">The status of the error, true: error exist, false: reset previous state.</param>
        private static void OnError(TextBox textBox, InputValidationResult validationResult)
        {
            RaiseErrorEvent(textBox, validationResult);
            SetHasError(textBox, validationResult != InputValidationResult.Success);

            if (validationResult != InputValidationResult.Success)
            {
                textBox.Background = GetErrorBackground(textBox);

                //   System.Media.SystemSounds.Exclamation.Play();

                AttachErrorAdorner(textBox);
            }
            else
            {
                textBox.Background = GetOriginalBackground(textBox);
                RemoveErrorAdorner(textBox);
            }
        }
        /// <summary>
        /// Give notify the UI, indicates by the status of the warning, true: warn the UI, false: reset previous state.
        /// </summary>
        /// <param name="textBox">The textbox to set the warning.</param>
        /// <param name="status">The status of the warning, true: warn the UI, false: reset previous state.</param>
        private static void OnWarning(TextBox textBox, bool status)
        {
            if (status)
            {
                ShowWarningAdorner(textBox);
                System.Media.SystemSounds.Beep.Play();
            }
            else
            {
                ResetWarningAdorner(textBox);
            }
        }
        /// <summary>
        /// Notify the UI that the source is dirty if any binding exist, otherwise will ignore.
        /// </summary>
        /// <param name="textBox">The textbox to set the notification.</param>
        /// <param name="status">The status of the notification, true: notify the UI, false: reset previous state.</param>
        private static void OnSourceIsDirty(TextBox textBox, bool status)
        {
            if (status)
            {
                textBox.Foreground = GetDirtyForeground(textBox);
                textBox.FontStyle = GetDirtyFontStyle(textBox);
            }
            else
            {
                textBox.Foreground = GetOriginalForeground(textBox);
                textBox.FontStyle = GetOriginalFontStyle(textBox);
            }
        }
        /// <summary>
        /// Raise <see cref="ValidationErrorEvent"/> if <paramref name="validationResult"/> is not <see cref="InputValidationResult.Success"/>.
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="validationResult"></param>
        private static void RaiseErrorEvent(TextBox textBox, InputValidationResult validationResult)
        {
            InputValidationErrorEventArgs eventArgs;
            InputValidationContext context;

            var max = GetMax(textBox);
            var min = GetMin(textBox);
            var maxLength = GetMaxLength(textBox);
            var minLength = GetMinLength(textBox);
            var validationType = GetValidationType(textBox);

            switch (validationResult)
            {
                case InputValidationResult.ExceptionOccured:
                case InputValidationResult.InvalidFormat:
                case InputValidationResult.ParsingFailed:
                    eventArgs = new InputValidationErrorEventArgs(ValidationErrorEvent, textBox, validationResult);
                    break;

                case InputValidationResult.BelowMinLimit:
                    eventArgs = new InputValidationErrorEventArgs(ValidationErrorEvent, textBox, validationResult, "Min", min: min, max: max);
                    break;

                case InputValidationResult.OverMaxLimit:
                    eventArgs = new InputValidationErrorEventArgs(ValidationErrorEvent, textBox, validationResult, "Max", min: min, max: max);
                    break;

                case InputValidationResult.BelowMinLength:
                    eventArgs = new InputValidationErrorEventArgs(ValidationErrorEvent, textBox, validationResult, "MinLength", minLength: minLength, maxLength: maxLength);
                    break;

                case InputValidationResult.OverMaxLength:
                    eventArgs = new InputValidationErrorEventArgs(ValidationErrorEvent, textBox, validationResult, "MaxLength", minLength: minLength, maxLength: maxLength);
                    break;

                default: return;
            }

            if (validationType == InputValidationType.String) context = new InputValidationContext(validationType, validationResult, minLength: minLength, maxLength: maxLength);
            else context = new InputValidationContext(validationType, validationResult, min: min, max: max);

            SetValidationContext(textBox, context);
            textBox.RaiseEvent(eventArgs);
        }
        /// <summary>
        /// Raise <see cref="ValidationWarningEvent"/> if <paramref name="validationResult"/> is not <see cref="InputValidationResult.Success"/>.
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="validationResult"></param>
        private static void RaiseWarningEvent(TextBox textBox, InputValidationResult validationResult)
        {
            InputValidationWarningEventArgs eventArgs;
            InputValidationContext context;

            var max = GetMax(textBox);
            var min = GetMin(textBox);
            var maxLength = GetMaxLength(textBox);
            var minLength = GetMinLength(textBox);
            var validationType = GetValidationType(textBox);

            switch (validationResult)
            {
                case InputValidationResult.ExceptionOccured:
                case InputValidationResult.InvalidFormat:
                case InputValidationResult.ParsingFailed:
                case InputValidationResult.Canceled:
                    eventArgs = new InputValidationWarningEventArgs(ValidationWarningEvent, textBox, validationResult);
                    break;

                case InputValidationResult.MinLimitReached:
                    eventArgs = new InputValidationWarningEventArgs(ValidationWarningEvent, textBox, validationResult, "Min", min: min, max: max);
                    break;

                case InputValidationResult.MaxLimitReached:
                    eventArgs = new InputValidationWarningEventArgs(ValidationWarningEvent, textBox, validationResult, "Max", min: min, max: max);
                    break;

                case InputValidationResult.MinLengthReached:
                    eventArgs = new InputValidationWarningEventArgs(ValidationWarningEvent, textBox, validationResult, "MinLength", minLength: minLength, maxLength: maxLength);
                    break;

                case InputValidationResult.MaxLengthReached:
                    eventArgs = new InputValidationWarningEventArgs(ValidationWarningEvent, textBox, validationResult, "MaxLength", minLength: minLength, maxLength: maxLength);
                    break;

                default: return;
            }

            if (validationType == InputValidationType.String) context = new InputValidationContext(validationType, validationResult, minLength: minLength, maxLength: maxLength);
            else context = new InputValidationContext(validationType, validationResult, min: min, max: max);

            GetWarningAdorner(textBox)?.SetVisualChildDataContext(context);
            textBox.RaiseEvent(eventArgs);
        }
        /// <summary>
        /// Attach the error adorner using <see cref="ErrorTemplateProperty"/> and add it to the <see cref="AdornerHostProperty"/>.
        /// </summary>
        /// <param name="textBox">The text box to adorn.</param>
        private static void AttachErrorAdorner(TextBox textBox)
        {
            //Remove existing adorner if exist, to avoid multi adorner attached
            RemoveErrorAdorner(textBox);

            ControlTemplate template = new ControlTemplate();

            if (GetErrorTemplate(textBox) == null)
            {
                FrameworkElementFactory elemementFactory = new FrameworkElementFactory(typeof(Border));
                elemementFactory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                elemementFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Stretch);
                elemementFactory.SetValue(FrameworkElement.MarginProperty, new Thickness(-1));
                elemementFactory.SetValue(Border.BorderBrushProperty, Brushes.Red);
                elemementFactory.SetValue(Border.BorderThicknessProperty, new Thickness(1));

                template.VisualTree = elemementFactory;
            }
            else
            {
                template = GetErrorTemplate(textBox);
            }

            TemplatedAdorner templatedAdorner = new TemplatedAdorner(textBox, template);
            templatedAdorner.PlacementMode = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            if (GetIsClipped(textBox) && GetAdornerLayerLocation(textBox) is UIElement uIElement) templatedAdorner.Container = uIElement;

            GetAdornerHost(textBox)?.Add(templatedAdorner);
        }
        /// <summary>
        /// Remove the error adorner from the <see cref="AdornerHostProperty"/>.
        /// </summary>
        /// <param name="textBox">The text box which adorn.</param>
        private static void RemoveErrorAdorner(TextBox textBox)
        {
            var adornerLayer = GetAdornerHost(textBox);

            Adorner[] adorners = adornerLayer?.GetAdorners(textBox);
            System.Collections.Generic.IEnumerable<Adorner> errorAdorners = adorners?.Where(p => p is TemplatedAdorner templatedAdorner && templatedAdorner.PlacementMode == System.Windows.Controls.Primitives.PlacementMode.Bottom);

            if (errorAdorners?.Count() > 0)
            {
                foreach (Adorner item in errorAdorners)
                {
                    adornerLayer.Remove(item);
                }
            }
        }
        /// <summary>
        /// Attach the warning adorner using <see cref="WarningTemplateProperty"/> then set to <see cref="WarningAdornerProperty"/> and add it to the <see cref="AdornerHostProperty"/>.
        /// </summary>
        /// <param name="textBox">The text box to adorn.</param>
        private static void AttachWarningAdorner(TextBox textBox)
        {
            //Remove existing adorner if exist, to avoid multi adorner attached
            RemoveWarningAdorner(textBox);

            ControlTemplate template = new ControlTemplate();

            if (GetWarningTemplate(textBox) == null)
            {
                FrameworkElementFactory elemementFactory = new FrameworkElementFactory(typeof(Border));
                elemementFactory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                elemementFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Stretch);
                elemementFactory.SetValue(FrameworkElement.MarginProperty, new Thickness(-1));
                elemementFactory.SetValue(Border.BorderBrushProperty, Brushes.Orange);
                elemementFactory.SetValue(Border.BorderThicknessProperty, new Thickness(1));

                template.VisualTree = elemementFactory;
            }
            else
            {
                template = GetWarningTemplate(textBox);
            }

            TemplatedAdorner templatedAdorner = new TemplatedAdorner(textBox, template);
            if (GetIsClipped(textBox) && GetAdornerLayerLocation(textBox) is UIElement uIElement) templatedAdorner.Container = uIElement;
            templatedAdorner.PlacementMode = System.Windows.Controls.Primitives.PlacementMode.Top;
            templatedAdorner.Visibility = Visibility.Collapsed;
            GetAdornerHost(textBox)?.Add(templatedAdorner);

            //set adorner created on backing field to call later.
            SetWarningAdorner(textBox, templatedAdorner);
        }
        /// <summary>
        /// Remove the warning adorner from the <see cref="AdornerHostProperty"/>.
        /// </summary>
        /// <param name="textBox">The text box which adorn.</param>
        private static void RemoveWarningAdorner(TextBox textBox)
        {
            var adornerLayer = GetAdornerHost(textBox);

            var adorners = adornerLayer?.GetAdorners(textBox);

            if (adorners?.Length > 0 && Array.Find(adorners, p => p is TemplatedAdorner templatedAdorner && templatedAdorner.PlacementMode == System.Windows.Controls.Primitives.PlacementMode.Top) is TemplatedAdorner adorner)
                adornerLayer.Remove(adorner);

            //remove adorner on backing field
            SetWarningAdorner(textBox, null);
        }
        /// <summary>
        /// Show the warning adorner on the <see cref="AdornerHostProperty"/> which is referenced to <see cref="WarningAdornerProperty"/>.
        /// </summary>
        /// <param name="textBox">The text box which adorn.</param>
        private static void ShowWarningAdorner(TextBox textBox)
        {
            //get adorner and show it using animation.
            var notificationAdorner = GetWarningAdorner(textBox);

            if (notificationAdorner != null)
            {
                ObjectAnimationUsingKeyFrames objectAnimation = new ObjectAnimationUsingKeyFrames()
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(GetWarningInitialShowDelay(textBox) +
                    GetWarningShowDuration(textBox)))
                };
                objectAnimation.KeyFrames.Add(
                    new DiscreteObjectKeyFrame(Visibility.Visible, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(GetWarningInitialShowDelay(textBox))))
                    );
                objectAnimation.KeyFrames.Add(
                   new DiscreteObjectKeyFrame(Visibility.Collapsed, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(GetWarningInitialShowDelay(textBox) + GetWarningShowDuration(textBox))))
                   );

                notificationAdorner?.BeginAnimation(UIElement.VisibilityProperty, objectAnimation);
            }
        }
        /// <summary>
        /// Hide the warning adorner on the <see cref="AdornerHostProperty"/> which is referenced to <see cref="WarningAdornerProperty"/>.
        /// </summary>
        /// <param name="textBox">The text box which adorn.</param>
        private static void ResetWarningAdorner(TextBox textBox)
        {
            var notificationAdorner = GetWarningAdorner(textBox);
            notificationAdorner?.BeginAnimation(UIElement.VisibilityProperty, null);
        }
        /// <summary>
        /// attach the adorner layer of <see cref="AdornerLayerLocationProperty"/> to the <see cref="AdornerHostProperty"/>.
        /// </summary>
        /// <param name="textBox">The text box which the attached property attached.</param>
        private static void AttachAdornerHost(TextBox textBox)
        {
            if (GetIsClipped(textBox) && GetAdornerLayerLocation(textBox) is UIElement uIElement)
            {
                var layoutAdorner = AdornerLayer.GetAdornerLayer(uIElement);
                if (layoutAdorner == null) throw new ArgumentException("cannot found adorner layer on specified UIElement.");
                else SetAdornerHost(textBox, layoutAdorner);
            }
            else
            {
                SetAdornerHost(textBox, AdornerLayer.GetAdornerLayer(textBox));
            }
        }

        #region Validation functions
        /// <summary>
        /// Check for validity preview text input.
        /// </summary>
        private static InputValidationResult ValidatePreviewText(TextBox textBox, string previewText)
        {
            switch (GetValidationType(textBox))
            {
                case InputValidationType.Integer:
                    return ValidateIntegerTypePreviewText(textBox, previewText);

                case InputValidationType.FloatingPoint:

                    //Count decimal place, if more than decimal place defined, return false
                    if (previewText.Contains(m_DecimalSeparator) && previewText.Length - previewText.IndexOf(m_DecimalSeparator) - 1 > GetDecimalPlace(textBox))
                        return InputValidationResult.InvalidFormat;

                    //cancel if the last is decimal separator
                    if (previewText.Length > 0 && previewText[previewText.Length - 1] == m_DecimalSeparator && previewText.Count(X => X == m_DecimalSeparator) <= 1)
                        return InputValidationResult.Canceled;

                    if (previewText.Contains("-"))
                    {
                        //return true if leading sign is on the first index while input length == 1
                        if (GetMin(textBox) < 0 && previewText.Length == 1) return InputValidationResult.Canceled;
                    }

                    return ValidateDoubleTypePreviewText(textBox, previewText);

                case InputValidationType.String:
                    return ValidateStringTypePreviewText(textBox, previewText);

                default: return InputValidationResult.ExceptionOccured;

            }
        }
        /// <summary>
        /// Check for validity text.
        /// </summary>
        private static InputValidationResult ValidateText(TextBox textBox)
        {
            if (textBox.Text.Length == 0) return InputValidationResult.InvalidFormat;

            switch (GetValidationType(textBox))
            {
                case InputValidationType.FloatingPoint:

                    const NumberStyles validNumberStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign;

                    if (double.TryParse(textBox.Text, validNumberStyles, CultureInfo.CurrentCulture, out double floatingPointResult))
                    {
                        if (floatingPointResult < GetMin(textBox)) return InputValidationResult.BelowMinLimit;
                        else if (floatingPointResult > GetMax(textBox)) return InputValidationResult.OverMaxLimit;
                        else return InputValidationResult.Success;
                    }
                    else
                    {
                        return InputValidationResult.ParsingFailed;
                    }

                case InputValidationType.Integer:

                    if (BigInteger.TryParse(textBox.Text, out BigInteger intResult))
                    {
                        if (intResult < new BigInteger(GetMin(textBox))) return InputValidationResult.BelowMinLimit;
                        else if (intResult > new BigInteger(GetMax(textBox))) return InputValidationResult.OverMaxLimit;
                        else return InputValidationResult.Success;
                    }
                    else
                    {
                        return InputValidationResult.ParsingFailed;
                    }

                case InputValidationType.String:
                    if (textBox.Text.Length < GetMinLength(textBox)) return InputValidationResult.BelowMinLength;
                    else if (textBox.Text.Length > GetMaxLength(textBox)) return InputValidationResult.OverMaxLength;
                    else return InputValidationResult.Success;

                default: return InputValidationResult.ExceptionOccured;
            }
        }
        /// <summary>
        /// Get element current text and join with input text.
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="previewTextInput"></param>
        /// <returns></returns>
        private static string GetPreviewText(TextBox textBox, string previewTextInput)
        {
            int selectionStart = textBox.SelectionStart;
            if (textBox.Text.Length < selectionStart)
                selectionStart = textBox.Text.Length;

            int selectionLength = textBox.SelectionLength;
            if (textBox.Text.Length < selectionStart + selectionLength)
                selectionLength = textBox.Text.Length - selectionStart;

            var realtext = textBox.Text.Remove(selectionStart, selectionLength);

            int caretIndex = textBox.CaretIndex;
            if (realtext.Length < caretIndex)
                caretIndex = realtext.Length;

            return realtext.Insert(caretIndex, previewTextInput);
        }
        /// <summary>
        /// Validate integer preview value text.
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="previewText"></param>
        /// <returns></returns>
        private static InputValidationResult ValidateIntegerTypePreviewText(TextBox textBox, string previewText)
        {
            if (BigInteger.TryParse(previewText, out BigInteger previewParsed))
            {
                //cancel validation if text is in replace mode
                if (textBox.SelectionLength == textBox.Text.Length && textBox.Text.Length > 0) return InputValidationResult.Canceled;

                //return false if preview parsed is 0 and string length is more than max or min.
                //   if (previewParsed == 0 && ((previewValue.Length > Max.ToString().Length) || (previewValue.Length > Min.ToString().Length))) return false;

                //cancel range validation if current text meet certain specification on statement
                if (!BigInteger.TryParse(textBox.Text, out BigInteger currentParsed) || (currentParsed < new BigInteger(GetMin(textBox)) && previewParsed >= currentParsed && previewParsed < new BigInteger(GetMax(textBox)))
                    || (currentParsed > new BigInteger(GetMax(textBox)) && previewParsed <= currentParsed && previewParsed > new BigInteger(GetMin(textBox)))) return InputValidationResult.Canceled;

                //return validation range
                if (previewParsed < new BigInteger(GetMin(textBox))) return InputValidationResult.MinLimitReached;
                else if (previewParsed > new BigInteger(GetMax(textBox))) return InputValidationResult.MaxLimitReached;
                else return InputValidationResult.Success;
            }
            else
            {
                return InputValidationResult.ParsingFailed;
            }
        }
        /// <summary>
        /// Validate double preview value text.
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="previewText"></param>
        /// <returns></returns>
        private static InputValidationResult ValidateDoubleTypePreviewText(TextBox textBox, string previewText)
        {
            const NumberStyles validNumberStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign;

            if (double.TryParse(previewText, validNumberStyles, CultureInfo.CurrentCulture, out double previewParsed))
            {
                //cancel validation if text is in replace mode
                if (textBox.SelectionLength == textBox.Text.Length) return InputValidationResult.Canceled;

                //return canceled if preview parsed is 0 and string length is more than max or min.
                // if (previewParsed == 0 && ((previewValue.Length > Max.ToString().Length) || (previewValue.Length > Min.ToString().Length))) return false;

                //cancel range validation if current text meet certain specification on statement
                if (!double.TryParse(textBox.Text, out double currentParsed) || (currentParsed < GetMin(textBox) && previewParsed >= currentParsed && previewParsed < GetMax(textBox)) || (currentParsed > GetMax(textBox) && previewParsed <= currentParsed && previewParsed > GetMin(textBox))) return InputValidationResult.Canceled;

                //return validation range
                if (previewParsed < GetMin(textBox)) return InputValidationResult.MinLimitReached;
                else if (previewParsed > GetMax(textBox)) return InputValidationResult.MaxLimitReached;
                else return InputValidationResult.Success;
            }
            else
            {
                return InputValidationResult.ParsingFailed;
            }
        }
        /// <summary>
        /// Validate string preview text.
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="previewText"></param>
        /// <returns></returns>
        private static InputValidationResult ValidateStringTypePreviewText(TextBox textBox, string previewText)
        {
            //cancel validation if text is in replace mode
            if (textBox.SelectionLength == textBox.Text.Length) return InputValidationResult.Canceled;

            //return canceled if current text is not valid, to avoid locking.
            if ((textBox.Text.Length < GetMinLength(textBox) && previewText.Length <= GetMaxLength(textBox)) || (textBox.Text.Length > GetMaxLength(textBox) && previewText.Length >= GetMinLength(textBox))) return InputValidationResult.Canceled;

            //Length validation
            if (previewText.Length < GetMinLength(textBox)) return InputValidationResult.MinLengthReached;
            else if (previewText.Length > GetMaxLength(textBox)) return InputValidationResult.MaxLengthReached;
            else return InputValidationResult.Success;
        }
        #endregion Validation functions

        #endregion Private methods

        #region Event handlers
        /// <summary>
        /// Capturing enter key down for update source if no error exist
        /// and disable white spaces key.
        /// </summary>
        private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;

            var test = AdornerLayer.GetAdornerLayer(textBox);

            if (e.Key == Key.Enter && !GetHasError(textBox))
            {
                BindingExpression be = textBox.GetBindingExpression(TextBox.TextProperty);

                be?.UpdateSource();

                //Set back appearence
                OnSourceIsDirty(textBox, be?.IsDirty != false);
                e.Handled = true;
            }

            //disable white space if not string type
            if (GetValidationType(textBox) != InputValidationType.String && (e.Key == Key.Tab || e.Key == Key.Space))
            {
                e.Handled = true;
                RaiseWarningEvent(textBox, InputValidationResult.InvalidFormat);
                OnWarning(textBox, true);
            }

            //disable white space if string type and current text has reach max limit
            if (GetValidationType(textBox) == InputValidationType.String && (e.Key == Key.Tab || e.Key == Key.Space) && textBox.Text.Length >= GetMaxLength(textBox))
            {
                e.Handled = true;
                RaiseWarningEvent(textBox, InputValidationResult.MaxLengthReached);
                OnWarning(textBox, true);
            }
        }
        /// <summary>
        /// compare source before lost keyboard focus.
        /// </summary>
        private static void OnPreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;

            //update target if binding is dirty
            BindingExpression be = textBox.GetBindingExpression(TextBox.TextProperty);
            be?.UpdateTarget();
            OnSourceIsDirty(textBox, be?.IsDirty != false);

            ResetWarningAdorner(textBox);
        }
        /// <summary>
        /// Validate on text changed and send error if failed.
        /// </summary>
        private static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;

            //Validate text on changed.
            OnError(textBox, ValidateText(textBox));

            BindingExpression be = textBox.GetBindingExpression(TextBox.TextProperty);
            OnSourceIsDirty(textBox, be?.IsDirty != false);
        }
        /// <summary>
        /// Validate on preview text input and send notification if failed.
        /// </summary>
        private static void OnPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;

            //Validate on preview text
            var validationResult = ValidatePreviewText(textBox, GetPreviewText(textBox, e.Text));

            RaiseWarningEvent(textBox, validationResult);
            OnWarning(textBox, validationResult != InputValidationResult.Success && validationResult != InputValidationResult.Canceled);
            e.Handled = validationResult != InputValidationResult.Success && validationResult != InputValidationResult.Canceled;
        }
        /// <summary>
        /// Validate on pasting and send notification if failed.
        /// </summary>
        private static void OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;

            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pastedText = (string)e.DataObject.GetData(typeof(string));

                //Validate on preview text
                var validationResult = ValidatePreviewText(textBox, GetPreviewText(textBox, pastedText));

                RaiseWarningEvent(textBox, validationResult);
                OnWarning(textBox, validationResult != InputValidationResult.Success && validationResult != InputValidationResult.Canceled);
                if (validationResult != InputValidationResult.Success && validationResult != InputValidationResult.Canceled) e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
                RaiseWarningEvent(textBox, InputValidationResult.InvalidFormat);
                OnWarning(textBox, true);
            }
        }
        /// <summary>
        /// for catch adorner layer on loaded, because will not rendered if not loaded.
        /// </summary>
        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;

            //Get adornerHost and attach it.
            AttachAdornerHost(textBox);

            //attach notification adorner.
            AttachWarningAdorner(textBox);

            //initial validation., if enabled.
            if (GetIsEnabled(textBox) && textBox.IsVisible) OnError(textBox, ValidateText(textBox));

            //remove subcription after catch layout adorner
            textBox.Loaded -= OnLoaded;
        }
        /// <summary>
        /// for catch adorner layer on IsVisibled changed, because if not visible adorner layer not rendered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is TextBox textBox) || !textBox.IsVisible) return;

            //Remove all adorners on layer before reinitialize host
            RemoveErrorAdorner(textBox);
            RemoveWarningAdorner(textBox);

            //Get adornerHost and attach it.
            AttachAdornerHost(textBox);
            //attach notification adorner
            AttachWarningAdorner(textBox);

            //initial validation
            if (GetIsEnabled(textBox) && textBox.IsVisible) OnError(textBox, ValidateText(textBox));
        }
        #endregion Event handlers
    }

    /// <summary>
    /// Determine the input validation mode of input validation.
    /// </summary>
    public enum InputValidationType
    {
        /// <summary>
        /// Input validation is String mode.
        /// </summary>
        String,
        /// <summary>
        /// Input validation is floating point mode.
        /// </summary>
        FloatingPoint,
        /// <summary>
        /// Input validation is Integer mode.
        /// </summary>
        Integer
    }
}
