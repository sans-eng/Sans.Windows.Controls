using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sans.Windows.Controls
{
    /// <summary>
    /// Provides information for the ValidationError attached event on validation.
    /// </summary>
    public class InputValidationErrorEventArgs : InputValidationEventArgs
    {
        #region Constructors
        public InputValidationErrorEventArgs(RoutedEvent routedEvent) : base(routedEvent) { }
        public InputValidationErrorEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }
        public InputValidationErrorEventArgs(RoutedEvent routedEvent, InputValidationResult error) : base(routedEvent, error) { }
        public InputValidationErrorEventArgs(RoutedEvent routedEvent, object source, InputValidationResult error) : base(routedEvent, source, error) { }
        public InputValidationErrorEventArgs(RoutedEvent routedEvent, InputValidationResult validationResult, string propertyNameOnError, double? min = null, double? max = null, int? minLength = null, int? maxLength = null) : base(routedEvent, validationResult, min, max,
            minLength, maxLength)
        {
            PropertyNameOnError = propertyNameOnError;
        }
        public InputValidationErrorEventArgs(RoutedEvent routedEvent, object source, InputValidationResult validationResult, string propertyNameOnError, double? min = null, double? max = null, int? minLength = null, int? maxLength = null) : base(routedEvent, source, validationResult, min, max,
            minLength, maxLength)
        {
            PropertyNameOnError = propertyNameOnError;
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the property that raising the error of this validation, if the error is the result of property validation such as max or min validation.
        /// </summary>
        public string PropertyNameOnError { get;}
        #endregion
    }
}
