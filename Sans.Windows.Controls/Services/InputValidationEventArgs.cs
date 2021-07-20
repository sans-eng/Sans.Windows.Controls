using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sans.Windows.Controls
{
    /// <summary>
    /// Provides information for the Validation attached event on validation.
    /// </summary>
    public class InputValidationEventArgs : RoutedEventArgs
    {
        #region Constructors
        public InputValidationEventArgs(RoutedEvent routedEvent) : base(routedEvent) { }
        public InputValidationEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }
        public InputValidationEventArgs(RoutedEvent routedEvent, InputValidationResult error) : base(routedEvent)
        {
            ValidationResult = error;
        }
        public InputValidationEventArgs(RoutedEvent routedEvent, object source, InputValidationResult error) : base(routedEvent, source)
        {
            ValidationResult = error;
        }
        public InputValidationEventArgs(RoutedEvent routedEvent, InputValidationResult validationResult, double? min = null, double? max = null, int? minLength = null, int? maxLength = null) : base(routedEvent)
        {
            ValidationResult = validationResult;
            Max = max;
            Min = min;
            MaxLength = maxLength;
            MinLength = minLength;
        }
        public InputValidationEventArgs(RoutedEvent routedEvent, object source, InputValidationResult validationResult, double? min = null, double? max = null, int? minLength = null, int? maxLength = null) : base(routedEvent, source)
        {
            ValidationResult = validationResult;
            Max = max;
            Min = min;
            MaxLength = maxLength;
            MinLength = minLength;
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets max limit defined of input that error occured.
        /// </summary>
        public double? Max { get; }
        /// <summary>
        /// Gets min limit defined of input that error occured.
        /// </summary>
        public double? Min { get; }
        /// <summary>
        /// Gets max length defined of input that error occured.
        /// </summary>
        public int? MaxLength { get; }
        /// <summary>
        /// Gets min length defined of input that error occured.
        /// </summary>
        public int? MinLength { get; }
        /// <summary>
        /// Gets or sets the error that occured on this <see cref="TextBoxInputValidation"/>.
        /// </summary>
        public InputValidationResult ValidationResult { get; }
        #endregion
    }
}
