using System;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;

namespace AiForms.Renderers.iOS
{
    internal class IndentLabel : UILabel
    {
        VerticalAlign vAlign;

        float Height;
        float FontHeight;
        float Padding = 8f;
        float PaddingLeft = 14f;

        public IndentLabel(float header, float text, LayoutAlignment formsAlign) : base() {
            Height = header;
            FontHeight = text;
            if (formsAlign == LayoutAlignment.Fill) {
                vAlign = VerticalAlign.End;
            }
            else {
                vAlign = (VerticalAlign)formsAlign;
            }
        }

        public override void DrawText(CGRect rect) {

            rect.X = PaddingLeft;
            switch (vAlign) {
                case VerticalAlign.Start :
                    rect.Y = Padding;
                    break;
                case VerticalAlign.Center:
                    rect.Y = (Height / 2f) - (FontHeight / 2f);
                    break;
                case VerticalAlign.End:                 
                    rect.Y = rect.Height - FontHeight - Padding;
                    break;
            }
            rect.Height = FontHeight;
            base.DrawText(rect);

        }

        enum VerticalAlign
        {
            Start=0,
            Center=1,
            End=2
        }
    }
}
