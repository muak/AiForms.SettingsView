using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Renderers.Droid
{
    //TODO: ボツ予定（使ってない）
    public static class CellRegistrar
    {
        public static Dictionary<Type, int> Registered {
            get
            {
                if (_registered == null)
                {
                    CreateRegistered();
                }
                return _registered;
            }
        }

        static Dictionary<Type, int> _registered;

        static void CreateRegistered()
        {
            var cellTypes = typeof(Cell).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(Cell))).ToList();
            cellTypes.AddRange(typeof(CellBase).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(CellBase))));

            //index begin from 2. 0 and 1 are the cell of Header/Footer.
            _registered = cellTypes.Select((x, idx) => new { x, index = idx + 2 })
                                   .ToDictionary(key => key.x, val => val.index);
        }
    }

}
