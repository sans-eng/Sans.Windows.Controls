using System;
using System.Windows.Markup;

namespace Sans.Windows.Controls.Extension
{
    internal sealed class IndicatorVisualStateGroupNames : MarkupExtension
    {
        #region Private fields
        private static IndicatorVisualStateGroupNames _internalActiveStates;
        private static IndicatorVisualStateGroupNames _sizeStates;
        #endregion

        #region Public properties
        public string Name { get; }
        public static IndicatorVisualStateGroupNames ActiveStates
        {
            get { return _internalActiveStates ?? (_internalActiveStates = new IndicatorVisualStateGroupNames("ActiveStates")); }
        }
        public static IndicatorVisualStateGroupNames SizeStates
        {
            get { return _sizeStates ?? (_sizeStates = new IndicatorVisualStateGroupNames("SizeStates"));}
        }
        #endregion

        #region Private fields
        private IndicatorVisualStateGroupNames(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name) + "is null, empty or only contain white space.");
            }

            Name = name;
        }
        #endregion

        #region Public methods
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Name;
        }
        #endregion
    }
}