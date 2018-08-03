using System;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sample.ViewModels
{
    public class FixPropViewModel
    {
        public ReactiveCollection<Product> SaleProducts { get; set; } = new ReactiveCollection<Product>();
        public ReactiveCollection<Product> PurchasedProducts { get; set; } = new ReactiveCollection<Product>();
        public AsyncReactiveCommand<string> PurchaseCommand { get; set; } = new AsyncReactiveCommand<string>();
        public ReactivePropertySlim<bool> IsShowPurchased { get; } = new ReactivePropertySlim<bool>();
        public ReactivePropertySlim<bool> IsAvailable { get; } = new ReactivePropertySlim<bool>(true);

        public FixPropViewModel()
        {
            SaleProducts.AddRangeOnScheduler(new List<Product> {
                new Product{Name = "月額プラン", Price = "¥200" ,Id = "premium1"},
                new Product{Name = "年額プラン", Price = "¥2,000" , Id = "premium12"},
            });

            PurchasedProducts.CollectionChangedAsObservable().Subscribe(_ => {
                IsShowPurchased.Value = PurchasedProducts.Count > 0;
            });

            PurchaseCommand.Subscribe(async id => {
                await Task.Delay(50);
                IsAvailable.Value = false;
                PurchasedProducts.Clear();
                PurchasedProducts.AddRangeOnScheduler(new List<Product> {
                    new Product{Name = "月額プランLabel", Price = "¥200" ,Id = "premium1"},
                    new Product{Name = "年額プランLabel", Price = "¥2,000" , Id = "premium12"},
                });
            });
                           
        }

        public class Product
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Price { get; set; }
            public bool IsValid { get; set; }
            public string ExpiredDate { get; set; }
            public bool IsPurchased { get; set; }
        }
    }
}
