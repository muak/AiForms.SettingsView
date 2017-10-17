using System;
using Xamarin.Forms;
using AiForms.Renderers;
using Reactive.Bindings;
using static Sample.ViewModels.ViewModelBase;
using Xamarin.Forms.Internals;
using System.Collections.Generic;
using System.Linq;

namespace Sample.ViewModels
{
    public class ParentPropTestViewModel:ViewModelBase
    {
        public ParentPropTestViewModel()
        {
            
        }

        protected override void ParentChanged(object obj)
        {
            base.ParentChanged(obj);
        } 

    }
}
