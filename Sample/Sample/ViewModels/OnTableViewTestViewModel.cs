using System;
namespace Sample.ViewModels
{
    //試してみたが表示はされるけどほぼアクション系がAdaptorの方で処理しているので動かない。
    //なのでTableViewやListViewでは基本的には使えないものとする。
    //一応このクラスは置いておく
    public class OnTableViewTestViewModel:PickerCellTestViewModel
    {
        public OnTableViewTestViewModel()
        {
        }

        protected override void CellChanged(object obj)
        {
            base.CellChanged(obj);
        }
    }
}
