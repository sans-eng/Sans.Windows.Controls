namespace Sans.Windows.Controls
{
    /// <summary>
    /// Determine the input validation error that indicates the error on input validation error raised.
    /// </summary>
    public enum InputValidationResult
    {
        /// <summary>
        /// Validation return succes.
        /// </summary>
        Success,
        /// <summary>
        /// The input is over max limit. 
        /// </summary>
        OverMaxLimit,
        /// <summary>
        /// The input is below min limit. 
        /// </summary>
        BelowMinLimit,
        /// <summary>
        /// The input is over max length. 
        /// </summary>
        OverMaxLength,
        /// <summary>
        /// The input is below min length. 
        /// </summary>
        BelowMinLength,
        /// <summary>
        /// The input is reach max limit. 
        /// </summary>
        MaxLimitReached,
        /// <summary>
        /// The input is reach min limit. 
        /// </summary>
        MinLimitReached,
        /// <summary>
        /// The input is reach max length. 
        /// </summary>
        MaxLengthReached,
        /// <summary>
        /// The input is reach min length. 
        /// </summary>
        MinLengthReached,
        /// <summary>
        /// The input is failed during parsing. 
        /// </summary>
        ParsingFailed,
        /// <summary>
        /// The input is invalid format. 
        /// </summary>
        InvalidFormat,
        /// <summary>
        /// Exception occured during validation. 
        /// </summary>
        ExceptionOccured,
        /// <summary>
        /// Validation is Canceled.
        /// </summary>
        Canceled
    }
}
