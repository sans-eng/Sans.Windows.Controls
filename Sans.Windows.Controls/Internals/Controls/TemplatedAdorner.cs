using Sans.Windows.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace Sans.Internals.Controls
{
    internal sealed class TemplatedAdorner : Adorner
    {
        private FrameworkElement _child;

        private FrameworkElement _referenceElement;
        internal PlacementMode PlacementMode { get; set; }
        internal UIElement Container { get; set; }
        public TemplatedAdorner(FrameworkElement adornedElement, ControlTemplate adornerTemplate)
            : base(adornedElement)
        {
            Debug.Assert(adornedElement != null, "adornedElement should not be null");
            Debug.Assert(adornerTemplate != null, "adornerTemplate should not be null");

            Control control = new Control();

            control.DataContext = TextBoxInputValidation.GetValidationContext(adornedElement);
            control.IsHitTestVisible = false;
            control.IsTabStop = false;   
            control.Template = adornerTemplate;
            _child = control;
            AddVisualChild(_child);


        }

        /// <summary>
        /// Adorners don't always want to be transformed in the same way as the elements they
        /// adorn.  Adorners which adorn points, such as resize handles, want to be translated
        /// and rotated but not scaled.  Adorners adorning an object, like a marquee, may want
        /// all transforms.  This method is called by AdornerLayer to allow the adorner to
        /// filter out the transforms it doesn't want and return a new transform with just the
        /// transforms it wants applied.  An adorner can also add an additional translation
        /// transform at this time, allowing it to be positioned somewhere other than the upper
        /// left corner of its adorned element.
        /// </summary>
        /// <param name="transform">The transform applied to the object the adorner adorns</param>
        /// <returns>Transform to apply to the adorner</returns>
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            if (ReferenceElement == null)
                return transform;

            GeneralTransformGroup group = new GeneralTransformGroup();
            group.Children.Add(transform);

            GeneralTransform t = this.TransformToDescendant(ReferenceElement);
            if (t != null)
            {
                group.Children.Add(t);
            }
            return group;
        }

        public FrameworkElement ReferenceElement
        {
            get => _referenceElement;
            set => _referenceElement = value;
        }

        /// <summary>
        ///  Derived classes override this property to enable the Visual code to enumerate
        ///  the Visual children. Derived classes need to return the number of children
        ///  from this method.
        ///
        ///    By default a Visual does not have any children.
        ///
        ///  Remark: During this virtual method the Visual tree must not be modified.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return _child != null ? 1 : 0; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index != 0) throw new ArgumentOutOfRangeException();
            return _child;
        }

        public FrameworkElement Child
        {
            get { return _child; }
            set
            {
                if (_child != null)
                {
                    RemoveVisualChild(_child);
                }
                _child = value;
                if (_child != null)
                {
                    AddVisualChild(_child);
                }
            }
        }
        public void SetVisualChildDataContext(object dataContext)
        {
            if (_child != null) _child.DataContext = dataContext;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _child.Measure(constraint);

            if (!IsUserVisible(AdornedElement)) _child.Visibility = Visibility.Collapsed;
            else _child.Visibility = Visibility.Visible;
            return _child.DesiredSize;
        }
        /// <summary>
        ///     Default control arrangement is to only arrange
        ///     the first visual child. No transforms will be applied.
        /// </summary>
        protected override Size ArrangeOverride(Size size)
        {
            Size finalSize = base.ArrangeOverride(size);

            Point placement;

            if (PlacementMode == PlacementMode.Bottom) placement = new Point(0, AdornedElement.RenderSize.Height);
            else placement = new Point(0, -finalSize.Height);

            _child?.Arrange(new Rect(placement, new Size(AdornedElement.RenderSize.Width, finalSize.Height)));

            if (!IsUserVisible(AdornedElement)) _child.Visibility = Visibility.Collapsed;
            else _child.Visibility = Visibility.Visible;

            return finalSize;
        }
        private bool IsUserVisible(UIElement element)
        {
            if (!element.IsVisible) return false;

            if (Container == null) return true;

            GeneralTransform childTransform = element.TransformToAncestor(Container);
            Rect rectangle = childTransform.TransformBounds(new Rect(new Point(0, 0), element.RenderSize));
            Rect result = Rect.Intersect(new Rect(new Point(0, element.RenderSize.Height), new Size(Container.RenderSize.Width, Container.RenderSize.Height - (element.RenderSize.Height *2))), rectangle);

            return result != Rect.Empty;
        }
    }
}
