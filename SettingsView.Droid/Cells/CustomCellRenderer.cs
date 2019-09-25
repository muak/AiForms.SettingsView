using System;
using System.Windows.Input;
using Android.Content;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Xamarin.Forms.Internals;
using ARelativeLayout = Android.Widget.RelativeLayout;

[assembly: ExportRenderer(typeof(CustomCell), typeof(CustomCellRenderer))]
namespace AiForms.Renderers.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class CustomCellRenderer : CellBaseRenderer<CustomCellView> { }


    [Android.Runtime.Preserve(AllMembers = true)]
    public class CustomCellView : CellBaseView
    {
        protected Action Execute { get; set; }
        protected CustomCell CustomCell => Cell as CustomCell;
        protected ICommand _command;
        protected ImageView _indicatorView;
        protected LinearLayout _coreView;
        FormsViewContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.CommandCellView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="cell">Cell.</param>
        public CustomCellView(Context context, Cell cell) : base(context, cell)
        {
            if (!CustomCell.ShowArrowIndicator)
            {
                return;
            }
            _indicatorView = new ImageView(context);
            _indicatorView.SetImageResource(Resource.Drawable.ic_navigate_next);

            var param = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent) {
            };

            using (param)
            {
                AccessoryStack.AddView(_indicatorView, param);
            }
        }

        protected override void CreateContentView()
        {
            base.CreateContentView();

            _container = new FormsViewContainer(Context);

            var layout = FindViewById<ARelativeLayout>(Resource.Id.CellLayout);

            layout.SetPadding(0, 0, 0, 0);

            _coreView = FindViewById<LinearLayout>(Resource.Id.CellBody);
            _coreView.SetPadding(0, 0, 0, 0);
            ContentStack.RemoveFromParent();
            DescriptionLabel.RemoveFromParent();
            _coreView.AddView(_container);
        }

        public void UpdateContent()
        {
            _container.CustomCell = CustomCell;
            _container.FormsCell = CustomCell.Content;
        }

        /// <summary>
        /// Cells the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == CommandCell.CommandProperty.PropertyName ||
               e.PropertyName == CommandCell.CommandParameterProperty.PropertyName)
            {
                UpdateCommand();
            }
        }

        public override void RowSelected(SettingsViewRecyclerAdapter adapter, int position)
        {
            if(!CustomCell.IsSelectable)
            {
                return;
            }
            Execute?.Invoke();
            if (CustomCell.KeepSelectedUntilBack)
            {
                adapter.SelectedRow(this, position);
            }
        }

        /// <summary>
        /// Updates the cell.
        /// </summary>
        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateContent();
            UpdateCommand();
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_command != null)
                {
                    _command.CanExecuteChanged -= Command_CanExecuteChanged;
                }
                Execute = null;
                _command = null;
                _indicatorView?.RemoveFromParent();
                _indicatorView?.SetImageDrawable(null);
                _indicatorView?.SetImageBitmap(null);
                _indicatorView?.Dispose();
                _indicatorView = null;

                _coreView?.RemoveFromParent();
                _coreView?.Dispose();
                _coreView = null;

                _container?.RemoveFromParent();
                _container?.Dispose();
                _container = null;
            }
            base.Dispose(disposing);
        }

        void UpdateCommand()
        {
            if (_command != null)
            {
                _command.CanExecuteChanged -= Command_CanExecuteChanged;
            }

            _command = CustomCell.Command;

            if (_command != null)
            {
                _command.CanExecuteChanged += Command_CanExecuteChanged;
                Command_CanExecuteChanged(_command, System.EventArgs.Empty);
            }

            Execute = () => {
                if (_command == null)
                {
                    return;
                }
                if (_command.CanExecute(CustomCell.CommandParameter))
                {
                    _command.Execute(CustomCell.CommandParameter);
                }
            };
        }

        /// <summary>
        /// Updates the is enabled.
        /// </summary>
        protected override void UpdateIsEnabled()
        {
            if (_command != null && !_command.CanExecute(CustomCell.CommandParameter))
            {
                return;
            }
            base.UpdateIsEnabled();
        }

        void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            if (!CellBase.IsEnabled)
            {
                return;
            }

            SetEnabledAppearance(_command.CanExecute(CustomCell.CommandParameter));
        }
    }
}
