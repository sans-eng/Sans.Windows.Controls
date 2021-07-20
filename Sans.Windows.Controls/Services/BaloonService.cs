using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace Sans.Windows.Controls
{
    public class BaloonService : DependencyObject
    {
        #region Public properties

        #endregion

        #region Dependency properties
        [Description("Identifies the Baloon attached property.")]
        public static readonly DependencyProperty BaloonProperty = DependencyProperty.RegisterAttached("Baloon", typeof(Baloon), typeof(BaloonService));
        [Description("Identifies the IsEnabled attached property.")]
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(BaloonService), new PropertyMetadata(OnIsEnabledPropertyChanged));
        [Description("Identifies the ShowOnEventName attached property.")]
        public static readonly DependencyProperty ShowOnEventNameProperty = DependencyProperty.RegisterAttached("ShowOnEventName", typeof(string), typeof(BaloonService), new PropertyMetadata("MouseEnter", OnShowOnEventNamePropertyChanged));
        [Description("Identifies the CloseOnEventName attached property.")]
        public static readonly DependencyProperty CloseOnEventNameProperty = DependencyProperty.RegisterAttached("CloseOnEventName", typeof(string), typeof(BaloonService), new PropertyMetadata("MouseLeave", OnCloseOnEventNamePropertyChanged));
        [Description("Identifies the InitialShowDelay attached property.")]
        public static readonly DependencyProperty InitialShowDelayProperty = DependencyProperty.RegisterAttached("InitialShowDelay", typeof(int), typeof(BaloonService), new PropertyMetadata(200));
        [Description("Identifies the ShowDuration attached property.")]
        public static readonly DependencyProperty ShowDurationProperty = DependencyProperty.RegisterAttached("ShowDuration", typeof(int), typeof(BaloonService), new PropertyMetadata(3000));
        [Description("Identifies the IsOpen attached property.")]
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.RegisterAttached("IsOpen", typeof(bool), typeof(BaloonService));
        [Description("Identifies the Placement attached property.")]
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.RegisterAttached("Placement", typeof(PlacementMode), typeof(BaloonService), new PropertyMetadata(PlacementMode.Top, OnPlacementPropertyChanged));
        [Description("Identifies the PlacementRectangle attached property.")]
        public static readonly DependencyProperty PlacementRectangleProperty = DependencyProperty.RegisterAttached("PlacementRectangle", typeof(Rect), typeof(BaloonService), new PropertyMetadata(default(Rect), OnPlacementRectanglePropertyChanged));
        [Description("Identifies the PlacementTarget attached property.")]
        public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.RegisterAttached("PlacementTarget", typeof(UIElement), typeof(BaloonService), new PropertyMetadata(OnPlacementTargetPropertyChanged));
        [Description("Identifies the HasDropShadow attached property.")]
        public static readonly DependencyProperty HasDropShadowProperty = DependencyProperty.RegisterAttached("HasDropShadow", typeof(bool), typeof(BaloonService), new PropertyMetadata(default(bool), OnHasDropShadowPropertyChanged));
        [Description("Identifies the VerticalOffset attached property.")]
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(BaloonService), new PropertyMetadata(-5d, VerticalOffsetPropertyChanged));
        [Description("Identifies the HorizontalOffset attached property.")]
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(BaloonService), new PropertyMetadata(default(double), HorizontalOffsetPropertyChanged));
        #endregion

        #region Routed events
        [Description("Identifies the BaloonOpening event.")]
        public static readonly RoutedEvent BaloonOpeningEvent = EventManager.RegisterRoutedEvent("BaloonOpening", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(BaloonService));
        [Description("Identifies the BaloonClosing event.")]
        public static readonly RoutedEvent BaloonClosingEvent = EventManager.RegisterRoutedEvent("BaloonClosing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(BaloonService));
        #endregion

        #region Public methods
        /// <summary>
        /// Gets the value of the <see cref="BaloonProperty"/> for an object.
        /// </summary>
        /// <param name="element"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="BaloonProperty"/> property value.</returns>
        public static Baloon GetBaloon(DependencyObject element)
        {
            return (Baloon)element.GetValue(BaloonProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="BaloonProperty"/> for an object.
        /// </summary>
        /// <param name="element">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetBaloon(DependencyObject element, Baloon value)
        {
            element.SetValue(BaloonProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="ShowOnEventNameProperty"/> for an object.
        /// </summary>
        /// <param name="element"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="ShowOnEventNameProperty"/> property value.</returns>
        public static string GetShowOnEventName(DependencyObject element)
        {
            return (string)element.GetValue(ShowOnEventNameProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="ShowOnEventNameProperty"/> for an object.
        /// </summary>
        /// <param name="element">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetShowOnEventName(DependencyObject element, string value)
        {
            element.SetValue(ShowOnEventNameProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="CloseOnEventNameProperty"/> for an object.
        /// </summary>
        /// <param name="element"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="CloseOnEventNameProperty"/> property value.</returns>
        public static string GetCloseOnEventName(DependencyObject element)
        {
            return (string)element.GetValue(CloseOnEventNameProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="CloseOnEventNameProperty"/> for an object.
        /// </summary>
        /// <param name="element">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetCloseOnEventName(DependencyObject element, string value)
        {
            element.SetValue(CloseOnEventNameProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="IsEnabledProperty"/> for an object.
        /// </summary>
        /// <param name="element"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="IsEnabledProperty"/> property value.</returns>
        public static bool GetIsEnabled(DependencyObject element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="IsEnabledProperty"/> for an object.
        /// </summary>
        /// <param name="element">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetIsEnabled(DependencyObject element, bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="InitialShowDelayProperty"/> for an object.
        /// </summary>
        /// <param name="element"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="InitialShowDelayProperty"/> property value.</returns>
        public static int GetInitialShowDelay(DependencyObject element)
        {
            return (int)element.GetValue(InitialShowDelayProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="InitialShowDelayProperty"/> for an object.
        /// </summary>
        /// <param name="element">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetInitialShowDelay(DependencyObject element, int value)
        {
            element.SetValue(InitialShowDelayProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="ShowDurationProperty"/> for an object.
        /// </summary>
        /// <param name="element"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="ShowDurationProperty"/> property value.</returns>
        public static int GetShowDuration(DependencyObject element)
        {
            return (int)element.GetValue(ShowDurationProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="ShowDurationProperty"/> for an object.
        /// </summary>
        /// <param name="element">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetShowDuration(DependencyObject element, int value)
        {
            element.SetValue(ShowDurationProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="IsOpenProperty"/> for an object.
        /// </summary>
        /// <param name="element"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="IsOpenProperty"/> property value.</returns>
        public static bool GetIsOpen(DependencyObject element)
        {
            return (bool)element.GetValue(IsOpenProperty);
        }
        /// <summary>
        /// Gets the value of the <see cref="PlacementProperty"/> for an object.
        /// </summary>
        /// <param name="element"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="PlacementProperty"/> property value.</returns>
        public static PlacementMode GetPlacement(DependencyObject element)
        {
            return (PlacementMode)element.GetValue(PlacementProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="PlacementProperty"/> for an object.
        /// </summary>
        /// <param name="element">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetPlacement(DependencyObject element, PlacementMode value)
        {
            element.SetValue(PlacementProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="PlacementRectangleProperty"/> for an object.
        /// </summary>
        /// <param name="element"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="PlacementRectangleProperty"/> property value.</returns>
        public static Rect GetPlacementRectangle(DependencyObject element)
        {
            return (Rect)element.GetValue(PlacementRectangleProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="PlacementRectangleProperty"/> for an object.
        /// </summary>
        /// <param name="element">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetPlacementRectangle(DependencyObject element, Rect value)
        {
            element.SetValue(PlacementRectangleProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="PlacementTargetProperty"/> for an object.
        /// </summary>
        /// <param name="element"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="PlacementTargetProperty"/> property value.</returns>
        public static UIElement GetPlacementTarget(DependencyObject element)
        {
            return (UIElement)element.GetValue(PlacementTargetProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="PlacementTargetProperty"/> for an object.
        /// </summary>
        /// <param name="element">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        private static void SetPlacementTarget(DependencyObject element, UIElement value)
        {
            element.SetValue(PlacementTargetProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="HasDropShadowProperty"/> for an object.
        /// </summary>
        /// <param name="element"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="HasDropShadowProperty"/> property value.</returns>
        public static bool GetHasDropShadow(DependencyObject element)
        {
            return (bool)element.GetValue(HasDropShadowProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="HasDropShadowProperty"/> for an object.
        /// </summary>
        /// <param name="element">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetHasDropShadow(DependencyObject element, bool value)
        {
            element.SetValue(HasDropShadowProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="VerticalOffsetProperty"/> for an object.
        /// </summary>
        /// <param name="element"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="VerticalOffsetProperty"/> property value.</returns>
        public static double GetVerticalOffset(DependencyObject element)
        {
            return (double)element.GetValue(VerticalOffsetProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="VerticalOffsetProperty"/> for an object.
        /// </summary>
        /// <param name="element">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetVerticalOffset(DependencyObject element, double value)
        {
            element.SetValue(VerticalOffsetProperty, value);
        }
        /// <summary>
        /// Gets the value of the <see cref="HorizontalOffsetProperty"/> for an object.
        /// </summary>
        /// <param name="element"> The object from which the property value is read.</param>
        /// <returns> The object's <see cref="HorizontalOffsetProperty"/> property value.</returns>
        public static double GetHorizontalOffset(DependencyObject element)
        {
            return (double)element.GetValue(HorizontalOffsetProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="HorizontalOffsetProperty"/> for an object.
        /// </summary>
        /// <param name="element">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetHorizontalOffset(DependencyObject element, double value)
        {
            element.SetValue(HorizontalOffsetProperty, value);
        }
        /// <summary>
        /// Adds a handler for <see cref="BaloonClosingEvent"/> attached event.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> or <see cref="ContentElement"/> that listens to this event.</param>
        /// <param name="handler">The event handler to be added.</param>
        public static void AddBaloonClosingHandler(DependencyObject element, RoutedEventHandler handler)
        {
            if (element is UIElement uiElement) uiElement.AddHandler(BaloonClosingEvent, handler);
        }
        /// <summary>
        ///  Removes a handler for <see cref="BaloonClosingEvent"/> attached event.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> or <see cref="ContentElement"/> that listens to this event.</param>
        /// <param name="handler">The event handler to be removed.</param>
        public static void RemoveBaloonClosingHandler(DependencyObject element, RoutedEventHandler handler)
        {
            if (element is UIElement uiElement) uiElement.RemoveHandler(BaloonClosingEvent, handler);
        }
        /// <summary>
        /// Adds a handler for <see cref="BaloonOpeningEvent"/> attached event.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> or <see cref="ContentElement"/> that listens to this event.</param>
        /// <param name="handler">The event handler to be added.</param>
        public static void AddBaloonOpeningHandler(DependencyObject element, RoutedEventHandler handler)
        {
            if (element is UIElement uiElement) uiElement.AddHandler(BaloonOpeningEvent, handler);
        }
        /// <summary>
        ///  Removes a handler for <see cref="BaloonOpeningEvent"/> attached event.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> or <see cref="ContentElement"/> that listens to this event.</param>
        /// <param name="handler">The event handler to be removed.</param>
        public static void RemoveBaloonOpeningHandler(DependencyObject element, RoutedEventHandler handler)
        {
            if (element is UIElement uiElement) uiElement.RemoveHandler(BaloonOpeningEvent, handler);
        }
        #endregion

        #region Dependency property callbacks
        private static void OnIsEnabledPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is FrameworkElement frameworkElement)) return;

            if ((bool)e.NewValue)
            {
                //Disable default ToolTipService
                ToolTipService.SetIsEnabled(o, false);
                AttachShowEvent(frameworkElement, GetShowOnEventName(frameworkElement));
                AttachCloseEvent(frameworkElement, GetCloseOnEventName(frameworkElement));
            }
            else
            {
                DetachShowEvent(frameworkElement, GetShowOnEventName(frameworkElement));
                DetachCloseEvent(frameworkElement, GetCloseOnEventName(frameworkElement));
            }
        }
        private static void OnShowOnEventNamePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is FrameworkElement frameworkElement)) return;

            if (e.OldValue is string oldValue && !string.IsNullOrWhiteSpace(oldValue)) DetachShowEvent(frameworkElement, oldValue);

            if (e.NewValue is string newValue && !string.IsNullOrWhiteSpace(newValue) && GetIsEnabled(frameworkElement)) AttachShowEvent(frameworkElement, newValue);
        }
        private static void OnCloseOnEventNamePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is FrameworkElement frameworkElement)) return;

            if (e.OldValue is string oldValue && !string.IsNullOrWhiteSpace(oldValue)) DetachCloseEvent(frameworkElement, oldValue);

            if (e.NewValue is string newValue && !string.IsNullOrWhiteSpace(newValue) && GetIsEnabled(frameworkElement)) AttachCloseEvent(frameworkElement, newValue);
        }
        private static void OnPlacementPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var baloon = (Baloon)GetBaloon(o);

            if (baloon != null) baloon.Placement = (PlacementMode)e.NewValue;
        }
        private static void OnPlacementRectanglePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var baloon = (Baloon)GetBaloon(o);

            if (baloon != null) baloon.PlacementRectangle = (Rect)e.NewValue;
        }
        private static void OnPlacementTargetPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var baloon = (Baloon)GetBaloon(o);

            if (baloon != null) baloon.PlacementTarget = (UIElement)e.NewValue;
        }
        private static void OnHasDropShadowPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var baloon = (Baloon)GetBaloon(o);

            if (baloon != null) baloon.HasDropShadow = (bool)e.NewValue;
        }
        private static void VerticalOffsetPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var baloon = (Baloon)GetBaloon(o);

            if (baloon != null) baloon.VerticalOffset = (double)e.NewValue;
        }
        private static void HorizontalOffsetPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var baloon = (Baloon)GetBaloon(o);

            if (baloon != null) baloon.HorizontalOffset = (double)e.NewValue;
        }
        #endregion

        #region Private methods
        private static void AttachShowEvent(FrameworkElement frameworkElement, string eventName)
        {
            //Get Event name
            if (!string.IsNullOrWhiteSpace(eventName) && GetIsEnabled(frameworkElement))
            {
                var targetEvent = frameworkElement.GetType().GetEvent(eventName);
                if (targetEvent == null) throw new ArgumentException("Cannot find the specified event on current element. Event name :" + eventName);
                if (targetEvent.EventHandlerType.IsSubclassOf(typeof(RoutedEvent))) throw new ArgumentException("Event name is not RoutedEvent.");

                var methodInfo = typeof(BaloonService).GetMethod(nameof(OnShowEventRaised), BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);

                Delegate handler = Delegate.CreateDelegate(targetEvent.EventHandlerType, methodInfo);

                targetEvent.AddEventHandler(frameworkElement, handler);
            }
        }
        private static void DetachShowEvent(FrameworkElement frameworkElement, string eventName)
        {
            //Get Event name
            if (!string.IsNullOrWhiteSpace(eventName))
            {
                var targetEvent = frameworkElement.GetType().GetEvent(eventName);
                if (targetEvent == null) throw new ArgumentException("Cannot find the specified event on current element. Event name :" + eventName);
                if (targetEvent.EventHandlerType.IsSubclassOf(typeof(RoutedEvent))) throw new ArgumentException("Event name is not RoutedEvent.");

                var methodInfo = typeof(BaloonService).GetMethod(nameof(OnShowEventRaised), BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);

                Delegate handler = Delegate.CreateDelegate(targetEvent.EventHandlerType, methodInfo);

                targetEvent.RemoveEventHandler(frameworkElement, handler);
            }
        }
        private static void AttachCloseEvent(FrameworkElement frameworkElement, string eventName)
        {
            //Get Event name
            if (!string.IsNullOrWhiteSpace(eventName) && GetIsEnabled(frameworkElement))
            {
                var targetEvent = frameworkElement.GetType().GetEvent(eventName);
                if (targetEvent == null) throw new ArgumentException("Cannot find the specified event on current element. Event name :" + eventName);
                if (targetEvent.EventHandlerType.IsSubclassOf(typeof(RoutedEvent))) throw new ArgumentException("Event name is not RoutedEvent.");

                var methodInfo = typeof(BaloonService).GetMethod(nameof(OnCloseEventRaised), BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);

                Delegate handler = Delegate.CreateDelegate(targetEvent.EventHandlerType, methodInfo);

                targetEvent.AddEventHandler(frameworkElement, handler);
            }
        }
        private static void DetachCloseEvent(FrameworkElement frameworkElement, string eventName)
        {
            //Get Event name
            if (!string.IsNullOrWhiteSpace(eventName))
            {
                var targetEvent = frameworkElement.GetType().GetEvent(eventName);
                if (targetEvent == null) throw new ArgumentException("Cannot find the specified event on current element. Event name :" + eventName);
                if (targetEvent.EventHandlerType.IsSubclassOf(typeof(RoutedEvent))) throw new ArgumentException("Event name is not RoutedEvent.");

                var methodInfo = typeof(BaloonService).GetMethod(nameof(OnCloseEventRaised), BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);

                Delegate handler = Delegate.CreateDelegate(targetEvent.EventHandlerType, methodInfo);

                targetEvent.RemoveEventHandler(frameworkElement, handler);
            }
        }
        private static void SetBaloon(FrameworkElement frameworkElement)
        {
            //Remove old baloon animation
            var currentBaloon = GetBaloon(frameworkElement);
            currentBaloon?.BeginAnimation(Baloon.IsOpenProperty, null);

            //Set baloon
            Baloon baloon = (frameworkElement.GetValue(FrameworkElement.ToolTipProperty) is Baloon tooltip ? tooltip : new Baloon()) ?? (global::Sans.Windows.Controls.Baloon)new Baloon();

            baloon.Placement = GetPlacement(frameworkElement);
            baloon.HorizontalOffset = GetHorizontalOffset(frameworkElement);
            baloon.VerticalOffset = GetVerticalOffset(frameworkElement);
            baloon.PlacementTarget = frameworkElement;

            SetPlacementTarget(frameworkElement, frameworkElement);

            SetBaloon(frameworkElement, baloon);
        }
        #endregion

        #region Event handlers
        private static void OnShowEventRaised(object sender, RoutedEventArgs e)
        {
            if (!(sender is FrameworkElement frameworkElement) || (GetBaloon(frameworkElement) is Baloon currentBaloon && currentBaloon.IsOpen)) return;

            //Set baloon
            SetBaloon(frameworkElement);

            var baloon = GetBaloon(frameworkElement);

            if (baloon != null)
            {
                BooleanAnimationUsingKeyFrames booleanAnimation = new BooleanAnimationUsingKeyFrames() { Duration = new Duration(TimeSpan.FromMilliseconds(GetShowDuration(frameworkElement) + GetInitialShowDelay(frameworkElement))) };
                booleanAnimation.KeyFrames.Add(
                    new DiscreteBooleanKeyFrame(true, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(GetInitialShowDelay(frameworkElement))))
                    );
                booleanAnimation.KeyFrames.Add(
                   new DiscreteBooleanKeyFrame(false, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(GetShowDuration(frameworkElement) + GetInitialShowDelay(frameworkElement))))
                   );

                baloon.BeginAnimation(ToolTip.IsOpenProperty, booleanAnimation);
            }
        }
        private static void OnCloseEventRaised(object sender, RoutedEventArgs e)
        {
            if (!(sender is FrameworkElement frameworkElement)) return;

            var baloon = GetBaloon(frameworkElement);

            if (baloon != null)
            {
                baloon.BeginAnimation(ToolTip.IsOpenProperty, null);
                if (baloon.IsOpen) baloon.IsOpen = false;
            }
        }
        #endregion
    }
}
