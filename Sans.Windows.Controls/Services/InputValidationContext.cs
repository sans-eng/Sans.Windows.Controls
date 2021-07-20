using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Sans.Windows.Controls.Primitives.Bases;

namespace Sans.Windows.Controls
{
    /// <summary>
    /// Represents an input validation context that created after any input validation triggered.
    /// </summary>
    public class InputValidationContext : BaseViewModel
    {
        #region Constructors
        internal InputValidationContext()
        {

        }
        /// <summary>
        /// Instantiate new instance of <see cref="InputValidationContext"/>
        /// </summary>
        /// <param name="validationResult">Validation result.</param>
        /// <param name="propertyNameOnError">Property name that raising the error of this validation, if the error is the result of property validation such as max or min validation. </param>
        /// <param name="max">Max limit of associated input.</param>
        /// <param name="min">Min limit of associated input.</param>
        /// <param name="maxLength">Max text length of associated input.</param>
        /// <param name="minLength">Min text length of associated input.</param>
        public InputValidationContext(InputValidationType validationType, InputValidationResult validationResult, double? max = null, double? min = null, int? maxLength = null, int? minLength = null)
        {
            ValidationType = validationType;
            Max = max;
            Min = min;
            MaxLength = maxLength;
            MinLength = minLength;
            ValidationResult = validationResult;
        }
        #endregion

        #region Private fields
        private InputValidationType m_ValidationType;
        private double? m_Max;
        private double? m_Min;
        private int? m_MaxLength;
        private int? m_MinLength;
        private InputValidationResult m_ValidationResult;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets <see cref="Controls.ValidationType"/> of Current <see cref="InputValidationContext"/>.
        /// </summary>
        public InputValidationType ValidationType
        {
            get { return m_ValidationType; }
            private set { SetProperty(ref m_ValidationType, value); }
        }
        /// <summary>
        /// Gets max limit defined of Current <see cref="InputValidationContext"/>.
        /// </summary>
        public double? Max
        {
            get { return m_Max; }
            private set { SetProperty(ref m_Max, value); }
        }
        /// <summary>
        /// Gets min limit defined of Current <see cref="InputValidationContext"/>.
        /// </summary>
        public double? Min
        {
            get { return m_Min; }
            private set { SetProperty(ref m_Min, value); }
        }
        /// <summary>
        /// Gets max length defined of Current <see cref="InputValidationContext"/>.
        /// </summary>
        public int? MaxLength
        {
            get { return m_MaxLength; }
            private set { SetProperty(ref m_MaxLength, value); }
        }
        /// <summary>
        /// Gets min length defined of Current <see cref="InputValidationContext"/>.
        /// </summary>
        public int? MinLength
        {
            get { return m_MinLength; }
            private set { SetProperty(ref m_MinLength, value); }
        }
        /// <summary>
        /// Gets the <see cref="InputValidationResult"/> of current <see cref="InputValidationContext"/>.
        /// </summary>
        public InputValidationResult ValidationResult
        {
            get { return m_ValidationResult; }
            private set { SetProperty(ref m_ValidationResult, value); }
        }
        #endregion
    }
}
