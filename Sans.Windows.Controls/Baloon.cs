using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sans.Windows.Controls
{
    /// <summary>
    /// Represents a baloon control.
    /// </summary>
    public class Baloon : ToolTip
    {

        #region Constructors
        static Baloon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Baloon), new FrameworkPropertyMetadata(typeof(Baloon)));
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets a <see cref="HeaderText"/> of current <see cref="Baloon"/>.
        /// </summary>
        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }
        #endregion

        #region Dependency properties
        /// <summary>
        /// Gets or sets a <see cref="HeaderText"/> of current <see cref="Baloon"/>.
        /// </summary>
        public static DependencyProperty HeaderTextProperty = DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(Baloon));
        #endregion


        #region override methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }
        #endregion
    }
}
