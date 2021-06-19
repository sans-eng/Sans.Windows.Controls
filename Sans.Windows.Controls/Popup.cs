using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sans.Windows.Controls
{
    [TemplatePart(Name = Popup.ElementHeaderName, Type = typeof(Grid))]
    [TemplatePart(Name = Popup.ElementContentName, Type = typeof(TextBlock))]
    [TemplatePart(Name = Popup.ElementBorderName, Type = typeof(Border))]

    [Obsolete("This class still under development")]
    public class Popup : System.Windows.Controls.Primitives.Popup
    {
        #region Constants
        private const string ElementHeaderName = "PART_Header";
        private const string ElementContentName = "PART_Content";
        private const string ElementBorderName = "PART_Border";
        #endregion

        #region Constructors
        static Popup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Popup), new FrameworkPropertyMetadata(typeof(Popup)));
        }
        #endregion

        #region Private fields
        private Grid m_Header;
        private TextBlock m_Content;
        private Border m_Border;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets a <see cref="ClosedTime"/> of current <see cref="Popup"/>.
        /// </summary>
        public TimeSpan? ClosedTime
        {
            get { return (TimeSpan)GetValue(ClosedTimeProperty); }
            set { SetValue(ClosedTimeProperty, value); }
        }
        /// <summary>
        /// Gets or sets an <see cref="Icon"/> of current <see cref="Popup"/>.
        /// </summary>
        public PopupIcon Icon
        {
            get { return (PopupIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        /// <summary>
        /// Gets or sets a <see cref="HeaderText"/> of current <see cref="Popup"/>.
        /// </summary>
        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }
        /// <summary>
        /// Gets or sets a <see cref="Content"/> of current <see cref="Popup"/>.
        /// </summary>
        public string Content
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }
        /// <summary>
        /// Gets or sets a control template.
        /// </summary>
        public ControlTemplate Template
        {
            get { return (ControlTemplate)GetValue(TemplateProperty); }
            set { SetValue(TemplateProperty, value); }
        }
        #endregion

        #region Dependency properties
        /// <summary>
        /// Gets or sets a <see cref="ClosedTime"/> of current <see cref="Popup"/>.
        /// </summary>
        public static DependencyProperty ClosedTimeProperty = DependencyProperty.Register(nameof(ClosedTime), typeof(TimeSpan), typeof(Popup));
        /// <summary>
        /// Gets or sets an <see cref="Icon"/> of current <see cref="Popup"/>.
        /// </summary>
        public static DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(PopupIcon), typeof(Popup));
        /// <summary>
        /// Gets or sets a <see cref="HeaderText"/> of current <see cref="Popup"/>.
        /// </summary>
        public static DependencyProperty HeaderTextProperty = DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(Popup));
        /// <summary>
        /// Gets or sets a <see cref="Content"/> of current <see cref="Popup"/>.
        /// </summary>
        public static DependencyProperty ContentProperty = DependencyProperty.Register(nameof(Content), typeof(string), typeof(Popup));
        /// <summary>
        /// Gets or sets a control template.
        /// </summary>
        public static DependencyProperty TemplateProperty = DependencyProperty.Register(nameof(Template), typeof(ControlTemplate), typeof(Popup));
        #endregion


        #region override methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_Border = GetTemplateChild(ElementBorderName) as Border;
            m_Header = GetTemplateChild(ElementHeaderName) as Grid;
            m_Content = GetTemplateChild(ElementContentName) as TextBlock;

        }
        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
        }
        #endregion

        #region Private methods
        private void Initialize()
        {
            //set icon
            Rectangle icon = new Rectangle
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            VisualBrush visual = new VisualBrush();
            Path path = new Path();

            switch (Icon)
            {
                case PopupIcon.Error:
                    path.Data = Geometry.Parse("m 153.3124,40.0626 c 1.956,0 3.7276,0.7932 5.0101,2.0755 1.2833,1.2822 2.0765,3.0542 2.0765,5.0111 0,1.9561 -0.7932,3.7276 -2.0754,5.0101 -1.2836,1.2833 -3.0552,2.0765 -5.0112,2.0765 -1.9569,0 -3.729,-0.7932 -5.0112,-2.0754 -1.2822,-1.2836 -2.0754,-3.0551 -2.0754,-5.0112 0,-1.9569 0.7932,-3.7289 2.0754,-5.0111 1.2822,-1.2823 3.0543,-2.0755 5.0112,-2.0755 z m -0.9052,7.0357 -2.0171,2.9426 c -0.1394,0.2018 -0.2435,0.3882 -0.3119,0.558 -0.063,0.1563 -0.0947,0.2957 -0.0947,0.4165 0,0.1004 0.0134,0.1872 0.0397,0.2581 0.0232,0.063 0.0572,0.1156 0.1002,0.1553 0.0504,0.0465 0.1054,0.081 0.1651,0.1032 0.0649,0.0233 0.1393,0.0353 0.2218,0.0353 0.061,0 0.1144,-0.0043 0.1596,-0.0123 0.0427,-0.0076 0.0846,-0.0205 0.1254,-0.0369 0.0372,-0.0159 0.0734,-0.0356 0.107,-0.0578 0.0302,-0.0195 0.0603,-0.0446 0.0899,-0.0742 l 0.0941,-0.1019 0.0753,-0.0994 c 0.727,-1.099 1.4446,-2.2057 2.1686,-3.3073 l 1.7844,2.8035 0.4104,0.6169 c 0.0279,0.0392 0.0597,0.0753 0.0939,0.1073 0.0345,0.0326 0.075,0.0633 0.1191,0.0901 0.0394,0.023 0.0841,0.0411 0.132,0.0529 0.0517,0.0126 0.1122,0.0191 0.1812,0.0191 0.0638,0 0.1213,-0.0065 0.1723,-0.0189 0.0501,-0.012 0.0994,-0.0314 0.1462,-0.0566 0.0427,-0.0233 0.0816,-0.0518 0.1153,-0.0841 0.0328,-0.0312 0.0613,-0.0673 0.0851,-0.1079 0.023,-0.0388 0.0408,-0.0816 0.0529,-0.1273 0.0117,-0.0443 0.018,-0.0925 0.018,-0.1446 0,-0.058 -0.008,-0.1171 -0.0216,-0.1757 -0.0159,-0.0636 -0.0411,-0.1328 -0.0742,-0.2057 -0.0342,-0.0764 -0.0822,-0.167 -0.1418,-0.2694 -0.0603,-0.1038 -0.1312,-0.2171 -0.212,-0.3381 -0.6856,-0.9794 -1.3463,-2.0684 -2.0023,-3.0781 l 1.866,-2.7896 c 0.1495,-0.2226 0.2606,-0.4205 0.3335,-0.5928 0.066,-0.1558 0.0991,-0.2877 0.0991,-0.3942 0,-0.0611 -0.0112,-0.1164 -0.0331,-0.1654 -0.0225,-0.0504 -0.0592,-0.1002 -0.1076,-0.1473 l -0.003,-0.0028 c -0.0487,-0.0443 -0.1043,-0.0785 -0.1656,-0.101 -0.0668,-0.0241 -0.1435,-0.0367 -0.2297,-0.0367 -0.0501,0 -0.0956,0.0038 -0.1347,0.0112 -0.037,0.0069 -0.071,0.0176 -0.1003,0.031 l -0.005,0.0019 c -0.0306,0.0134 -0.0613,0.0315 -0.0901,0.0528 -0.0312,0.0233 -0.0616,0.051 -0.0909,0.083 -0.0306,0.0329 -0.0646,0.0756 -0.1013,0.1254 -0.7056,1.0347 -1.3852,2.1039 -2.0743,3.1499 l -1.5522,-2.5125 c -0.121,-0.1963 -0.2303,-0.3636 -0.3264,-0.5005 -0.0909,-0.129 -0.1681,-0.2272 -0.2303,-0.2921 -0.0468,-0.0493 -0.1018,-0.0863 -0.1651,-0.1101 -0.0701,-0.0263 -0.1547,-0.04 -0.2532,-0.04 -0.083,0 -0.158,0.0123 -0.2243,0.0367 -0.0641,0.023 -0.1235,0.0594 -0.1777,0.1076 -0.0547,0.0487 -0.0942,0.0975 -0.118,0.1457 -0.0211,0.0435 -0.0318,0.0895 -0.0318,0.138 0,0.0687 0.008,0.138 0.0233,0.2059 0.0164,0.0725 0.0419,0.1473 0.0756,0.2248 0.0353,0.0813 0.0824,0.1755 0.1402,0.2806 0.0561,0.1038 0.1237,0.218 0.2018,0.342 l 1.7835,2.7542 z");
                    icon.Fill = Brushes.Red;
                    break;
                case PopupIcon.Information:
                    path.Data = Geometry.Parse("m 77.2441,99.9213 c -3.9141,0 -7.0866,3.1728 -7.0866,7.0866 0,3.9138 3.1725,7.0866 7.0866,7.0866 3.9141,0 7.0866,-3.1728 7.0866,-7.0866 0,-3.9138 -3.1725,-7.0866 -7.0866,-7.0866 z m 0.7693,5.9374 v 4.4578 c 0,0.3084 -0.0731,0.5417 -0.2197,0.6998 -0.1465,0.1582 -0.3316,0.2373 -0.5573,0.2373 -0.2256,0 -0.4087,-0.0811 -0.5496,-0.2429 -0.1406,-0.1622 -0.212,-0.3935 -0.212,-0.6942 v -4.4116 c 0,-0.3067 0.0714,-0.536 0.212,-0.6902 0.1409,-0.1542 0.324,-0.2313 0.5496,-0.2313 0.2257,0 0.4108,0.0771 0.5573,0.2313 0.1466,0.1542 0.2197,0.3702 0.2197,0.644 z m -0.7617,-1.5928 c -0.214,0 -0.3971,-0.0655 -0.5493,-0.1967 -0.1525,-0.131 -0.2276,-0.3181 -0.2276,-0.559 0,-0.216 0.0771,-0.3951 0.2333,-0.536 0.1562,-0.1409 0.3373,-0.2121 0.5436,-0.2121 0.2007,0 0.3762,0.0635 0.5304,0.1908 0.1542,0.1293 0.2313,0.3144 0.2313,0.5573 0,0.2373 -0.0751,0.4224 -0.2256,0.5553 -0.1503,0.133 -0.3277,0.2004 -0.5361,0.2004 z");
                    icon.Fill = Brushes.Blue;
                    break;
                case PopupIcon.Question:
                    path.Data = Geometry.Parse("m 39.685,99.9213 c -3.914,0 -7.0866,3.1728 -7.0866,7.0866 0,3.9138 3.1726,7.0866 7.0866,7.0866 3.9141,0 7.0867,-3.1728 7.0867,-7.0866 0,-3.9138 -3.1726,-7.0866 -7.0867,-7.0866 z m -2.808,5.182 c 0,-0.3546 0.1151,-0.7152 0.3433,-1.0817 0.2282,-0.3643 0.5621,-0.6673 0.9992,-0.9071 0.4394,-0.2398 0.9494,-0.3586 1.5344,-0.3586 0.5429,0 1.0222,0.0998 1.4383,0.301 0.4144,0.1996 0.7348,0.4717 0.961,0.817 0.2262,0.3433 0.3396,0.7194 0.3396,1.1219 0,0.3184 -0.0652,0.5984 -0.1939,0.8382 -0.1284,0.2399 -0.2818,0.4468 -0.4604,0.6194 -0.1763,0.1746 -0.4946,0.468 -0.955,0.8805 -0.1284,0.115 -0.2301,0.2165 -0.307,0.305 -0.0765,0.0881 -0.134,0.1686 -0.1706,0.2415 -0.0383,0.0728 -0.0669,0.1457 -0.0882,0.2185 -0.0192,0.0732 -0.0518,0.2016 -0.0921,0.3855 -0.0709,0.3895 -0.2934,0.5831 -0.6693,0.5831 -0.1936,0 -0.3585,-0.0632 -0.4909,-0.1899 -0.1344,-0.1284 -0.1996,-0.3163 -0.1996,-0.5675 0,-0.3146 0.0479,-0.587 0.146,-0.8172 0.0978,-0.2299 0.2262,-0.4315 0.3872,-0.6061 0.1613,-0.1743 0.3799,-0.3815 0.6523,-0.6213 0.2395,-0.2089 0.4141,-0.3682 0.5196,-0.4737 0.1074,-0.1074 0.1975,-0.2262 0.2704,-0.3566 0.0728,-0.1304 0.1094,-0.2744 0.1094,-0.4277 0,-0.2994 -0.1114,-0.5525 -0.3336,-0.7577 -0.2245,-0.2053 -0.5103,-0.3087 -0.8632,-0.3087 -0.4124,0 -0.7154,0.1034 -0.911,0.3126 -0.1936,0.2072 -0.3586,0.514 -0.493,0.9187 -0.1284,0.4218 -0.3699,0.633 -0.7248,0.633 -0.2109,0 -0.3875,-0.0731 -0.5312,-0.2225 -0.144,-0.1477 -0.2169,-0.307 -0.2169,-0.4796 z m 2.7409,6.1565 c -0.2285,0 -0.4278,-0.0748 -0.5984,-0.2225 -0.171,-0.1477 -0.2552,-0.3549 -0.2552,-0.6213 0,-0.2362 0.0825,-0.4334 0.2475,-0.5947 0.1647,-0.161 0.3662,-0.2416 0.6061,-0.2416 0.2358,0 0.4351,0.0806 0.5964,0.2416 0.161,0.1613 0.2415,0.3585 0.2415,0.5947 0,0.2625 -0.0842,0.4697 -0.2532,0.6193 -0.1686,0.1497 -0.3625,0.2245 -0.5847,0.2245 z");
                    icon.Fill = Brushes.Blue;
                    break;
                case PopupIcon.Stop:
                    path.Data = Geometry.Parse("m 12.9195,2.9605 c -3.9141,0 -7.0866,3.1728 -7.0866,7.0866 0,3.9138 3.1725,7.0866 7.0866,7.0866 3.914,0 7.0866,-3.1728 7.0866,-7.0866 0,-3.9138 -3.1726,-7.0866 -7.0866,-7.0866 z m -3.8741,7.1612 c 0.403,0.072 0.7803,0.6519 1.4037,1.1727 V 10.1945 9.6503 6.6546 c 0,-0.2543 0.2633,-0.4606 0.5879,-0.4606 h 0.0646 c 0.3249,0 0.5882,0.2063 0.5882,0.4606 v 2.9957 h 0.2517 V 5.6891 c 0,-0.2548 0.2636,-0.4609 0.5879,-0.4609 h 0.0644 c 0.3251,0 0.5882,0.2061 0.5882,0.4609 v 3.9612 h 0.1964 V 6.2867 c 0,-0.254 0.2636,-0.4604 0.5876,-0.4604 h 0.0646 c 0.3249,0 0.5885,0.2064 0.5885,0.4604 v 3.3636 h 0.1605 V 7.1889 c 0,-0.2542 0.2633,-0.4606 0.5881,-0.4606 h 0.0644 c 0.3248,0 0.5882,0.2064 0.5882,0.4606 v 2.4614 h 0.0023 v 3.2675 c 0,0 0,1.9482 -2.7871,1.9482 -1.7376,0 -2.4743,-0.6364 -2.7025,-1.0706 -0.002,-0.0029 -1.7277,-2.3653 -2.0889,-2.9092 -0.3608,-0.5448 0.08,-0.8589 0.6013,-0.7645 z");
                    icon.Fill = Brushes.Red;
                    break;
                case PopupIcon.Warning:
                    path.Data = Geometry.Parse("m 59.5329,111.2598 c -0.2279,0 -0.4271,-0.0748 -0.5961,-0.2225 -0.1703,-0.1477 -0.2551,-0.3552 -0.2551,-0.6219 0,-0.2313 0.0805,-0.4294 0.2438,-0.5927 0.1627,-0.1624 0.36,-0.2438 0.5964,-0.2438 0.2355,0 0.4354,0.0814 0.6021,0.2438 0.1669,0.1633 0.2494,0.3614 0.2494,0.5927 0,0.2633 -0.0845,0.47 -0.2514,0.6194 -0.1687,0.1502 -0.3646,0.225 -0.5891,0.225 z m -0.6213,-3.6978 -0.1803,-2.6897 c -0.0326,-0.5225 -0.0496,-0.9009 -0.0496,-1.1288 0,-0.3112 0.0805,-0.553 0.2438,-0.7271 0.1627,-0.1749 0.3759,-0.2608 0.64,-0.2608 0.3221,0 0.5375,0.1111 0.6444,0.3337 0.1091,0.2222 0.1635,0.5442 0.1635,0.9626 0,0.2475 -0.0133,0.4983 -0.0385,0.7532 l -0.2421,2.7689 c -0.0269,0.3291 -0.0822,0.5822 -0.1681,0.7585 -0.0842,0.1743 -0.2262,0.2631 -0.4263,0.2631 -0.201,0 -0.341,-0.0842 -0.4196,-0.2552 -0.0768,-0.1709 -0.1346,-0.4291 -0.1672,-0.7784 z m 0.616,-7.6407 c -3.9141,0 -7.0867,3.1728 -7.0867,7.0866 0,3.9138 3.1726,7.0866 7.0867,7.0866 3.914,0 7.0866,-3.1728 7.0866,-7.0866 0,-3.9138 -3.1726,-7.0866 -7.0866,-7.0866 z");
                    icon.Fill = Brushes.Orange;
                    break;
            }

            visual.Visual = path;
            icon.OpacityMask = visual;

            //set header text
            TextBlock headerText = new TextBlock();
            if (string.IsNullOrWhiteSpace(HeaderText))
            {
                headerText.Text = HeaderText;
            }

            //set content
            if(!string.IsNullOrWhiteSpace(m_Content?.Text) && !string.IsNullOrWhiteSpace(Content))
            {
                m_Content.Text = Content;
            }

            
        }
        #endregion

    }
}
