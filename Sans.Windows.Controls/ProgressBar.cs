using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sans.Windows.Controls
{
    public class ProgressBar : System.Windows.Controls.Control
    {
        #region Constructors
        static ProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(typeof(ProgressBar)));
        }
        #endregion

        #region Constanst
        private string IndicatorName = "PART_Indicator";
        private string TrackName = "PART_Track";
        private string GlowRectName = "PART_GlowRect";
        #endregion
    }
}
