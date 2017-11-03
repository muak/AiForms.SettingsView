using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    /// <summary>
    /// Section.
    /// </summary>
    public class Section : TableSectionBase<Cell>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Section"/> class.
        /// </summary>
        public Section()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Section"/> class.
        /// </summary>
        /// <param name="title">Title.</param>
        public Section(string title) : base(title)
        {

        }

        /// <summary>
        /// The is visible property.
        /// </summary>
        public static BindableProperty IsVisibleProperty =
            BindableProperty.Create(
                nameof(IsVisible),
                typeof(bool),
                typeof(Section),
                true,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.Section"/> is visible.
        /// </summary>
        /// <value><c>true</c> if is visible; otherwise, <c>false</c>.</value>
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// The footer text property.
        /// </summary>
        public static BindableProperty FooterTextProperty =
            BindableProperty.Create(
                nameof(FooterText),
                typeof(string),
                typeof(Section),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the footer text.
        /// </summary>
        /// <value>The footer text.</value>
        public string FooterText
        {
            get { return (string)GetValue(FooterTextProperty); }
            set { SetValue(FooterTextProperty, value); }
        }
    }
}
