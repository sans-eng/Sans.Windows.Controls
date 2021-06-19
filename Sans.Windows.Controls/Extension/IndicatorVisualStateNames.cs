using System;
using System.Windows.Markup;

namespace Sans.Windows.Controls.Extension
{
    internal sealed class IndicatorVisualStateNames : MarkupExtension
    {
        #region Private fields
        private static IndicatorVisualStateNames _activeState;
        private static IndicatorVisualStateNames _inactiveState;
        #endregion

        #region Public properties
        public string Name { get; }
        public static IndicatorVisualStateNames ActiveState
        {
            get { return _activeState ?? (_activeState = new IndicatorVisualStateNames("Active")); }
        }           
        public static IndicatorVisualStateNames InactiveState
        {
            get { return _inactiveState ?? (_inactiveState = new IndicatorVisualStateNames("Inactive")); }
        }
        #endregion

        #region Private methods
        private IndicatorVisualStateNames(string name)
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