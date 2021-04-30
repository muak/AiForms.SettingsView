using System;
using System.ComponentModel;

namespace AiForms.Renderers
{
    public class CellPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        public Section Section { get; }

        public CellPropertyChangedEventArgs(string propertyName, Section section) : base(propertyName)
        {
            Section = section;
        }
    }
}
