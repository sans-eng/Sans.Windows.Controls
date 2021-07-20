using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sans.Windows.Controls
{
    /// <summary>
    /// Provides information for the ValidationWarning attached event on validation.
    /// </summary>
    public class InputValidationWarningEventArgs : InputValidationEventArgs
    {
        #region Constructors
        public InputValidationWarningEventArgs(RoutedEvent routedEvent) : base(routedEvent) { }
        public InputValidationWarningEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }
        public InputValidationWarningEventArgs(RoutedEvent routedEvent, InputValidationResult error) : base(routedEvent, error) { }
        public InputValidationWarningEventArgs(RoutedEvent routedEvent, object source, InputValidationResult error) : base(routedEvent, source, error) { }
        public InputValidationWarningEventArgs(RoutedEvent routedEvent, InputValidationResult error, string propertyNameOnError, double? min = null, double? max = null, int? minLength = null, int? maxLength = null) : base(routedEvent, error, min, max,
            minLength, maxLength)
        {
            PropertyNameOnWarning = propertyNameOnError;
        }
        public InputValidationWarningEventArgs(RoutedEvent routedEvent, object source, InputValidationResult error, string propertyNameOnError, double? min = null, double? max = null, int? minLength = null, int? maxLength = null) : base(routedEvent, source, error, min, max,
            minLength, maxLength)
        {
            PropertyNameOnWarning = propertyNameOnError;
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the property that raising the warning of this validation, if the warning is the result of property validation such as max or min validation.
        /// </summary>
        public string PropertyNameOnWarning { get; }
        #endregion
    }
}
