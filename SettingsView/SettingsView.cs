using System;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AiForms.Renderers
{
    [ContentProperty("Root")]
    public partial class SettingsView : TableView
    {
        internal static Action _clearCache;
        public static void ClearCache(){
            _clearCache?.Invoke();
        }

        public new SettingsModel Model { get; set; }
        public new event EventHandler ModelChanged;

        public SettingsView()
        {
            VerticalOptions = HorizontalOptions = LayoutOptions.FillAndExpand;
            Root = new SettingsRoot();
            Model = new SettingsModel(Root);
        }

        SettingsRoot _root;
        public new SettingsRoot Root {
            get { return _root; }
            set {
                if (_root != null) {
                    _root.PropertyChanged -= RootOnPropertyChanged;
                    _root.CollectionChanged -= OnCollectionChanged;
                    _root.SectionCollectionChanged -= OnSectionCollectionChanged;
                }

                _root = value;

                //子要素にBindingContextを伝える（多分…）
                SetInheritedBindingContext(_root, BindingContext);

                _root.PropertyChanged += RootOnPropertyChanged;
                _root.CollectionChanged += OnCollectionChanged;
                _root.SectionCollectionChanged += OnSectionCollectionChanged;
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (Root != null)
                SetInheritedBindingContext(Root, BindingContext);
        }

        void RootOnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TableSectionBase.TitleProperty.PropertyName ||
                e.PropertyName == Section.FooterTextProperty.PropertyName ||
                e.PropertyName == Section.IsVisibleProperty.PropertyName) {
                OnModelChanged();
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == HasUnevenRowsProperty.PropertyName ||
                propertyName == HeaderHeightProperty.PropertyName ||
                propertyName == HeaderFontSizeProperty.PropertyName ||
                propertyName == HeaderTextColorProperty.PropertyName ||
                propertyName == HeaderBackgroundColorProperty.PropertyName ||
                propertyName == HeaderTextVerticalAlignProperty.PropertyName ||
                propertyName == HeaderPaddingProperty.PropertyName ||
                propertyName == FooterFontSizeProperty.PropertyName ||
                propertyName == FooterTextColorProperty.PropertyName ||
                propertyName == FooterBackgroundColorProperty.PropertyName ||
                propertyName == FooterPaddingProperty.PropertyName
               ) {

                OnModelChanged();
            }
        }

        //Section単位のCollectionChanged
        public void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnModelChanged();
        }

        //Section内の子要素のCollectionChanged
        public void OnSectionCollectionChanged(object sender, EventArgs childCollectionChangedEventArgs)
        {
            OnModelChanged();
        }

        new void OnModelChanged()
        {
            //Parentを設定しないとViewCellでのサイズが決まらない
            foreach (Cell cell in Root.SelectMany(r => r))
                cell.Parent = this;

            //Nativeに変更を通知するためにイベントを発火させる
            if (ModelChanged != null)
                ModelChanged(this, EventArgs.Empty);

        }

        //既存のTableViewの不要なプロパティの隠蔽
        private new int Intent { get; set; }
    }
}
