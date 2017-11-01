using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

// Copy from https://github.com/tomochan154/toy-box/edit/master/NaturalComparer.cs

namespace AiForms.Renderers
{
    /// <summary>ソートの方向。</summary>
    public enum NaturalSortOrder : int
    {
        #region Enum

        /// <summary>なし。</summary>
        None = 0,
        /// <summary>昇順。</summary>
        Ascending = 1,
        /// <summary>降順。</summary>
        Descending = 2,

        #endregion
    }

    /// <summary>自然順の比較オプション。</summary>
    [Flags()]
    public enum NaturalComparerOptions
    {
        #region Enum

        /// <summary>アラビア数字。</summary>
        Number = 0x1,
        /// <summary>ASCIIローマ数字。</summary>
        RomanNumber = 0x2,
        /// <summary>日本語ローマ数字。</summary>
        JpRomanNumber = 0x4,
        /// <summary>日本語丸数字。</summary>
        CircleNumber = 0x8,
        /// <summary>日本語漢数字。</summary>
        KanjiNumber = 0x10,
        /// <summary>すべての数字。</summary>
        NumberAll = Number | RomanNumber | JpRomanNumber | CircleNumber | KanjiNumber,

        /// <summary>空白文字の存在を無視。</summary>
        IgnoreSpace = 0x10000,
        /// <summary>数字表現の違いを無視。</summary>
        IgnoreNumber = 0x20000,
        /// <summary>全角半角の違いを無視。</summary>
        IgnoreWide = 0x40000,
        /// <summary>大文字小文字の違いを無視。</summary>
        IgnoreCase = 0x80000,
        /// <summary>カタカナひらがなの違いを無視。</summary>
        IgnoreKana = 0x100000,
        /// <summary>すべての無視条件。</summary>
        IgnoreAll = IgnoreSpace | IgnoreNumber | IgnoreWide | IgnoreCase | IgnoreKana,

        /// <summary>既定の比較オプション。</summary>
        Default = NumberAll | IgnoreSpace | IgnoreNumber | IgnoreWide | IgnoreCase,

        #endregion
    }

    /// <summary>自然順の比較機能を提供します。</summary>
    public class NaturalComparer : IComparer<string>, IComparer
    {
        #region Enum

        /// <summary>数字表現文字の種類。</summary>
        private enum CharTypes : uint
        {
            /// <summary>なし。</summary>
            None = 0x0,
            /// <summary>アラビア数字。</summary>
            Number = 0x1,
            /// <summary>ASCIIローマ数字。</summary>
            RomanNumber = 0x2,
            /// <summary>日本語ローマ数字。</summary>
            JpRomanNumber = 0x4,
            /// <summary>日本語丸数字。</summary>
            CircleNumber = 0x8,
            /// <summary>日本語漢数字。</summary>
            KanjiNumber = 0x10,
        }

        #endregion

        #region Field

        /// <summary>ソートの方向を表す int。</summary>
        private int _order;
        /// <summary>自然順の比較オプションを表す <see cref="NaturalComparerOptions"/>。</summary>
        private NaturalComparerOptions _options;
        /// <summary>除外文字を表す char[]。</summary>
        private char[] _ignoreCharacter;
        /// <summary>比較オプションを組み合わせた除外文字を表す char[]。</summary>
        private char[] _ignoreTable;

        #endregion

        #region Constructor

        /// <summary>インスタンスを初期化します。</summary>
        public NaturalComparer()
            : this(NaturalSortOrder.Ascending, NaturalComparerOptions.Default, new char[0])
        {
        }

        /// <summary>インスタンスを初期化します。</summary>
        /// <param name="order">ソート方向を表す <see cref="NaturalSortOrder"/>。</param>
        public NaturalComparer(NaturalSortOrder order)
            : this(order, NaturalComparerOptions.Default, new char[0])
        {
        }

        /// <summary>インスタンスを初期化します。</summary>
        /// <param name="order">ソート方向を表す <see cref="NaturalSortOrder"/>。</param>
        /// <param name="options">比較方法を表す <see cref="NaturalComparerOptions"/>。</param>
        public NaturalComparer(NaturalSortOrder order, NaturalComparerOptions options)
            : this(order, NaturalComparerOptions.Default, new char[0])
        {
        }

        /// <summary>インスタンスを初期化します。</summary>
        /// <param name="order">ソート方向を表す <see cref="NaturalSortOrder"/>。</param>
        /// <param name="options">比較方法を表す <see cref="NaturalComparerOptions"/>。</param>
        /// <param name="ignoreCharacter">無視する文字を表す char[]。</param>
        public NaturalComparer(NaturalSortOrder order, NaturalComparerOptions options, char[] ignoreCharacter)
        {
            this.SortOrder = order;
            _ignoreCharacter = ignoreCharacter;
            this.Options = options; // 最後に実行する必要がある
        }

        #endregion

        #region Property

        /// <summary>ソート方向を取得または設定します。</summary>
        public virtual NaturalSortOrder SortOrder
        {
            get
            {
                switch (_order)
                {
                    case 1:
                        return NaturalSortOrder.Ascending;
                    case -1:
                        return NaturalSortOrder.Descending;
                    default:
                        return NaturalSortOrder.Ascending;
                }
            }
            set
            {
                switch (value)
                {
                    case NaturalSortOrder.Ascending:
                        _order = 1;
                        break;
                    case NaturalSortOrder.Descending:
                        _order = -1;
                        break;
                    default:
                        _order = 0;
                        break;
                }
            }
        }

        /// <summary>自然順の比較オプションを取得または設定します。</summary>
        public virtual NaturalComparerOptions Options
        {
            get { return _options; }
            set
            {
                _options = value;
                if (this.IgnoreSpace)
                {
                    _ignoreTable = new char[_ignoreCharacter.Length + 3];
                    _ignoreCharacter.CopyTo(_ignoreTable, 0);
                    _ignoreTable[_ignoreTable.Length - 3] = ' ';
                    _ignoreTable[_ignoreTable.Length - 2] = '　';
                    _ignoreTable[_ignoreTable.Length - 1] = '\t';
                }
                else
                {
                    _ignoreTable = _ignoreCharacter;
                }
            }
        }

        /// <summary>空白文字の存在を無視するかどうかを取得します。</summary>
        protected virtual bool IgnoreSpace
        {
            get { return ((_options & NaturalComparerOptions.IgnoreSpace) == NaturalComparerOptions.IgnoreSpace); }
        }

        /// <summary>数字表現の違いを無視するかどうかを取得します。</summary>
        protected virtual bool IgnoreNumber
        {
            get { return ((_options & NaturalComparerOptions.IgnoreNumber) == NaturalComparerOptions.IgnoreNumber); }
        }

        /// <summary>全角半角の違いを無視するかどうかを取得します。</summary>
        protected virtual bool IgnoreWide
        {
            get { return ((_options & NaturalComparerOptions.IgnoreWide) == NaturalComparerOptions.IgnoreWide); }
        }

        /// <summary>大文字小文字の違いを無視するかどうかを取得します。</summary>
        protected virtual bool IgnoreCase
        {
            get { return ((_options & NaturalComparerOptions.IgnoreCase) == NaturalComparerOptions.IgnoreCase); }
        }

        /// <summary>カタカナひらがなの違いを無視するかどうかを取得します。</summary>
        protected virtual bool IgnoreKana
        {
            get { return ((_options & NaturalComparerOptions.IgnoreKana) == NaturalComparerOptions.IgnoreKana); }
        }

        #endregion

        #region Method

        /// <summary>オブジェクトの大小関係を比較します。</summary>
        /// <param name="s1">比較対象のオブジェクトを表す string。</param>
        /// <param name="s2">比較対象のオブジェクトを表す string。</param>
        /// <returns>
        /// <list type="table">
        ///   <item><term>0 より小さい</term><description><paramref name="s1"/> が <paramref name="s2"/> より小さい。</description></item>
        ///   <item><term>0</term><description><paramref name="s1"/> と <paramref name="s2"/> は等しい。</description></item>
        ///   <item><term>0 より大きい</term><description><paramref name="s1"/> が <paramref name="s2"/> より大きい。</description></item>
        /// </list>
        /// </returns>
        public int Compare(string s1, string s2)
        {
            return LocalCompare(s1, s2) * _order;
        }

        /// <summary>オブジェクトの大小関係を比較します。</summary>
        /// <param name="s1">比較対象のオブジェクトを表す object。</param>
        /// <param name="s2">比較対象のオブジェクトを表す object。</param>
        /// <returns>
        /// <list type="table">
        ///   <item><term>0 より小さい</term><description><paramref name="s1"/> が <paramref name="s2"/> より小さい。</description></item>
        ///   <item><term>0</term><description><paramref name="s1"/> と <paramref name="s2"/> は等しい。</description></item>
        ///   <item><term>0 より大きい</term><description><paramref name="s1"/> が <paramref name="s2"/> より大きい。</description></item>
        /// </list>
        /// </returns>
        int IComparer.Compare(object s1, object s2)
        {
            return LocalCompare(s1 as string, s2 as string) * _order;
        }

        /// <summary>オブジェクトの大小関係を比較します。</summary>
        /// <param name="s1">比較対象のオブジェクトを表す string。</param>
        /// <param name="s2">比較対象のオブジェクトを表す string。</param>
        /// <returns>
        /// <list type="table">
        ///   <item><term>0 より小さい</term><description><paramref name="s1"/> が <paramref name="s2"/> より小さい。</description></item>
        ///   <item><term>0</term><description><paramref name="s1"/> と <paramref name="s2"/> は等しい。</description></item>
        ///   <item><term>0 より大きい</term><description><paramref name="s1"/> が <paramref name="s2"/> より大きい。</description></item>
        /// </list>
        /// </returns>
        protected virtual int LocalCompare(string s1, string s2)
        {
            // いずれかが null もしくは空文字であれば比較終了
            if (string.IsNullOrEmpty(s1))
            {
                return string.IsNullOrEmpty(s2) ? 0 : -1;
            }
            else if (string.IsNullOrEmpty(s2))
            {
                return 1;
            }

            CharTypes filter = (CharTypes)(_options & NaturalComparerOptions.NumberAll);
            CharTypes t1 = CharTypes.None;
            CharTypes t2 = CharTypes.None;
            int p1 = 0;
            int p2 = 0;
            char c1 = char.MinValue;
            char c2 = char.MinValue;

            s1 = ConvertChar(s1);
            s2 = ConvertChar(s2);

            // 除外文字を読み飛ばす
            if (_ignoreTable.Length > 0)
            {
                SkipIgnoreCharacter(s1, ref p1);
                SkipIgnoreCharacter(s1, ref p2);
            }

            while (p1 < s1.Length && p2 < s2.Length)
            {
                t1 = GetCharType(s1[p1], c1, t1) & filter;
                t2 = GetCharType(s2[p2], c2, t2) & filter;
                c1 = s1[p1];
                c2 = s2[p2];

                // 両方とも何らかの数字の場合
                if ((this.IgnoreNumber || (this.IgnoreNumber == false && t1 == t2)) && t1 != CharTypes.None && t2 != CharTypes.None)
                {
                    int i1 = p1;
                    int i2 = p2;
                    long v1 = 0;
                    long v2 = 0;

                    bool success = GetNumber(s1, t1, ref i1, out v1) && GetNumber(s2, t2, ref i2, out v2);
                    if (success)
                    {
                        if (v1 < v2)
                        {
                            return -1;
                        }
                        else if (v1 > v2)
                        {
                            return 1;
                        }
                        p1 = i1;
                        p2 = i2;
                    }
                    else
                    {
                        int diff = CompareChar(s1[p1], s2[p2]);
                        if (diff != 0)
                        {
                            return diff;
                        }
                        p1++;
                        p2++;
                    }
                }
                // いずれかが数字の場合
                else if ((t1 != CharTypes.None || t2 != CharTypes.None) && t1 != CharTypes.RomanNumber && t2 != CharTypes.RomanNumber)
                {
                    return (t1 != CharTypes.None) ? 1 : -1;
                }
                // 数字でない場合は文字コードを比較する
                else
                {
                    int diff = CompareChar(s1[p1], s2[p2]);
                    if (diff != 0)
                    {
                        return diff;
                    }
                    p1++;
                    p2++;
                }

                // 除外文字を読み飛ばす
                if (_ignoreTable.Length > 0)
                {
                    SkipIgnoreCharacter(s1, ref p1);
                    SkipIgnoreCharacter(s2, ref p2);
                }
            }

            // 共通部分が一致している場合は、残りの文字列長で大小関係を決める
            if (p1 >= s1.Length)
            {
                return (p2 >= s2.Length) ? 0 : -1;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>除外文字を読み飛ばします。</summary>
        /// <param name="source">対象の文字列を表す string。</param>
        /// <param name="pos">開始位置を表す int。</param>
        private void SkipIgnoreCharacter(string source, ref int pos)
        {
            for (; pos < source.Length; pos++)
            {
                if (Array.IndexOf<char>(_ignoreTable, source[pos]) == -1)
                {
                    break;
                }
            }
        }

        /// <summary>文字の種類を取得します。</summary>
        /// <param name="c">取得対象の文字を表す char。</param>
        /// <param name="back">直前の文字を表す char。</param>
        /// <param name="state">直前の文字の種類を表す <see cref="CharTypes"/>。</param>
        /// <returns>取得した文字の種類を表す <see cref="CharTypes"/>。</returns>
        private CharTypes GetCharType(char c, char back, CharTypes state)
        {
            // ASCIIアラビア数字 (0～9)
            if (c >= '0' && c <= '9')
            {
                return CharTypes.Number;
            }
            // 日本語アラビア数字 (０～９)
            else if (c >= '０' && c <= '９')
            {
                return CharTypes.Number;
            }
            // 日本語丸数字 (①～⑳)
            else if (c >= '①' && c <= '⑳')
            {
                return CharTypes.CircleNumber;
            }
            // ASCII英大文字 (A～Z)
            else if (c >= 'A' && c <= 'Z')
            {
                // ASCIIローマ数字 (I,V,X,L,C,D,M)
                if (back < 'A' || back > 'Z')
                {
                    switch (c)
                    {
                        case 'I':
                        case 'V':
                        case 'X':
                        case 'L':
                        case 'C':
                        case 'D':
                        case 'M':
                            return CharTypes.RomanNumber;
                    }
                }
            }
            // ASCII英小文字 (a～z)
            else if (c >= 'a' && c <= 'z')
            {
                // ASCIIローマ数字 (i,v,x,l,c,d,m)
                if ((back < 'A' || back > 'Z') && (back < 'a' || back > 'z'))
                {
                    switch (c)
                    {
                        case 'i':
                        case 'v':
                        case 'x':
                        case 'l':
                        case 'c':
                        case 'd':
                        case 'm':
                            return CharTypes.RomanNumber;
                    }
                }
            }
            // 日本語英大文字 (Ａ～Ｚ)
            else if (c >= 'Ａ' && c <= 'Ｚ')
            {
                // 日本語ローマ数字 (Ｉ,Ｖ,Ｘ,Ｌ,Ｃ,Ｄ,Ｍ)
                if (back < 'Ａ' || back > 'Ｚ')
                {
                    switch (c)
                    {
                        case 'Ｉ':
                        case 'Ｖ':
                        case 'Ｘ':
                        case 'Ｌ':
                        case 'Ｃ':
                        case 'Ｄ':
                        case 'Ｍ':
                            return CharTypes.RomanNumber;
                    }
                }
            }
            // 日本語英小文字 (ａ～ｚ)
            else if (c >= 'ａ' && c <= 'ｚ')
            {
                // 日本語ローマ数字 (ⅰ,ⅴ,ⅹ,ｌ,ｃ,ｄ,ｍ)
                if ((back < 'Ａ' || back > 'Ｚ') && (back < 'ａ' || back > 'ｚ'))
                {
                    switch (c)
                    {
                        case 'ⅰ':
                        case 'ⅴ':
                        case 'ⅹ':
                        case 'ｌ':
                        case 'ｃ':
                        case 'ｄ':
                        case 'ｍ':
                            return CharTypes.RomanNumber;
                    }
                }
            }
            // ローマ数字
            else if (c >= 0x2160 && c <= 0x217F)
            {
                return CharTypes.JpRomanNumber;
            }
            else
            {
                // 日本語漢数字
                if (state == CharTypes.KanjiNumber)
                {
                    switch (c)
                    {
                        case '〇':
                        case '一':
                        case '二':
                        case '三':
                        case '四':
                        case '五':
                        case '六':
                        case '七':
                        case '八':
                        case '九':
                        case '十':
                        case '百':
                        case '千':
                        case '万':
                        case '億':
                        case '兆':
                        case '京':
                        case '壱':
                        case '弐':
                        case '参':
                        case '拾':
                            return CharTypes.KanjiNumber;
                    }
                }
                else
                {
                    switch (c)
                    {
                        case '〇':
                        case '一':
                        case '二':
                        case '三':
                        case '四':
                        case '五':
                        case '六':
                        case '七':
                        case '八':
                        case '九':
                        case '十':
                        case '百':
                        case '千':
                        case '壱':
                        case '弐':
                        case '参':
                            return CharTypes.KanjiNumber;
                    }
                }
            }

            return CharTypes.None;
        }

        /// <summary>比較オプションに合わせて文字を変換します。</summary>
        /// <param name="source">変換する文字を表す string。</param>
        /// <returns>変換結果の文字を表す string。</returns>
        private string ConvertChar(string source)
        {
            StringBuilder buffer = new StringBuilder(source);

            // 全角半角の違いを無視する
            if (this.IgnoreWide)
            {
                ConvertHalf(buffer);
            }

            // 大文字小文字の違いを無視する
            if (this.IgnoreCase)
            {
                ConvertUpperCase(buffer);
            }

            // カタカナひらがなの違いを無視する
            if (this.IgnoreKana)
            {
                ConvertKatakana(buffer);
            }

            return buffer.ToString();
        }

        /// <summary>全角を半角へ変換します。</summary>
        /// <param name="source">変換元の文字列を表す <see cref="StringBuilder"/>。</param>
        private void ConvertHalf(StringBuilder source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] >= '！' && source[i] <= '～')
                {
                    source[i] = (char)(source[i] - '！' + '!');
                }
                else
                {
                    switch (source[i])
                    {
                        case '、': source[i] = '､'; break;
                        case '。': source[i] = '｡'; break;
                        case '〈': source[i] = '<'; break;
                        case '〉': source[i] = '>'; break;
                        case '《': source[i] = '<'; break;
                        case '》': source[i] = '>'; break;
                        case '「': source[i] = '｢'; break;
                        case '」': source[i] = '｣'; break;
                        case '『': source[i] = '｢'; break;
                        case '』': source[i] = '｣'; break;
                        case '【': source[i] = '['; break;
                        case '】': source[i] = ']'; break;
                        case '〔': source[i] = '['; break;
                        case '〕': source[i] = ']'; break;
                    }
                }
            }
        }

        /// <summary>小文字を大文字へ変換します。</summary>
        /// <param name="source">変換元の文字列を表す <see cref="StringBuilder"/>。</param>
        private void ConvertUpperCase(StringBuilder source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if ((source[i] >= 'a' && source[i] <= 'z') && (source[i] >= 'ａ' && source[i] <= 'ｚ'))
                {
                    source[i] = char.ToUpperInvariant(source[i]);
                }
            }
        }

        /// <summary>ひらがなをカタカナへ変換します。</summary>
        /// <param name="source">変換元の文字列を表す <see cref="StringBuilder"/>。</param>
        private void ConvertKatakana(StringBuilder source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] >= 'ぁ' && source[i] <= 'ゞ')
                {
                    source[i] = (char)(source[i] + 'ァ' - 'ぁ');
                }
                else if (source[i] >= 'ｦ' && source[i] <= 'ﾟ')
                {
                    bool replaced = false;

                    if (i + 1 < source.Length)
                    {
                        replaced = true;

                        switch (source[i + 1])
                        {
                            case 'ﾞ':
                                switch (source[i])
                                {
                                    case 'ｶ': source[i] = 'ガ'; break;
                                    case 'ｷ': source[i] = 'ギ'; break;
                                    case 'ｸ': source[i] = 'グ'; break;
                                    case 'ｹ': source[i] = 'ゲ'; break;
                                    case 'ｺ': source[i] = 'ゴ'; break;
                                    case 'ｻ': source[i] = 'ザ'; break;
                                    case 'ｼ': source[i] = 'ジ'; break;
                                    case 'ｽ': source[i] = 'ズ'; break;
                                    case 'ｾ': source[i] = 'ゼ'; break;
                                    case 'ｿ': source[i] = 'ゾ'; break;
                                    case 'ﾀ': source[i] = 'ダ'; break;
                                    case 'ﾁ': source[i] = 'ヂ'; break;
                                    case 'ﾂ': source[i] = 'ヅ'; break;
                                    case 'ﾃ': source[i] = 'デ'; break;
                                    case 'ﾄ': source[i] = 'ド'; break;
                                    case 'ﾊ': source[i] = 'バ'; break;
                                    case 'ﾋ': source[i] = 'ビ'; break;
                                    case 'ﾌ': source[i] = 'ブ'; break;
                                    case 'ﾍ': source[i] = 'ベ'; break;
                                    case 'ﾎ': source[i] = 'ボ'; break;
                                    case 'ｳ': source[i] = 'ヴ'; break;
                                    default: replaced = false; break;
                                }
                                break;
                            case 'ﾟ':
                                switch (source[i])
                                {
                                    case 'ﾊ': source[i] = 'パ'; break;
                                    case 'ﾋ': source[i] = 'ピ'; break;
                                    case 'ﾌ': source[i] = 'プ'; break;
                                    case 'ﾍ': source[i] = 'ペ'; break;
                                    case 'ﾎ': source[i] = 'ポ'; break;
                                    default: replaced = false; break;
                                }
                                break;
                            default:
                                replaced = false;
                                break;
                        }

                        if (replaced)
                        {
                            source.Remove(i + 1, 1);
                        }
                    }

                    if (replaced == false)
                    {
                        switch (source[i])
                        {
                            case 'ｦ': source[i] = 'ヲ'; break;
                            case 'ｧ': source[i] = 'ァ'; break;
                            case 'ｨ': source[i] = 'ィ'; break;
                            case 'ｩ': source[i] = 'ゥ'; break;
                            case 'ｪ': source[i] = 'ェ'; break;
                            case 'ｫ': source[i] = 'ォ'; break;
                            case 'ｬ': source[i] = 'ャ'; break;
                            case 'ｭ': source[i] = 'ュ'; break;
                            case 'ｮ': source[i] = 'ョ'; break;
                            case 'ｯ': source[i] = 'ッ'; break;
                            case 'ｰ': source[i] = 'ー'; break;
                            case 'ｱ': source[i] = 'ア'; break;
                            case 'ｲ': source[i] = 'イ'; break;
                            case 'ｳ': source[i] = 'ウ'; break;
                            case 'ｴ': source[i] = 'エ'; break;
                            case 'ｵ': source[i] = 'オ'; break;
                            case 'ｶ': source[i] = 'カ'; break;
                            case 'ｷ': source[i] = 'キ'; break;
                            case 'ｸ': source[i] = 'ク'; break;
                            case 'ｹ': source[i] = 'ケ'; break;
                            case 'ｺ': source[i] = 'コ'; break;
                            case 'ｻ': source[i] = 'サ'; break;
                            case 'ｼ': source[i] = 'シ'; break;
                            case 'ｽ': source[i] = 'ス'; break;
                            case 'ｾ': source[i] = 'セ'; break;
                            case 'ｿ': source[i] = 'ソ'; break;
                            case 'ﾀ': source[i] = 'タ'; break;
                            case 'ﾁ': source[i] = 'チ'; break;
                            case 'ﾂ': source[i] = 'ツ'; break;
                            case 'ﾃ': source[i] = 'テ'; break;
                            case 'ﾄ': source[i] = 'ト'; break;
                            case 'ﾅ': source[i] = 'ナ'; break;
                            case 'ﾆ': source[i] = 'ニ'; break;
                            case 'ﾇ': source[i] = 'ヌ'; break;
                            case 'ﾈ': source[i] = 'ネ'; break;
                            case 'ﾉ': source[i] = 'ノ'; break;
                            case 'ﾊ': source[i] = 'ハ'; break;
                            case 'ﾋ': source[i] = 'ヒ'; break;
                            case 'ﾌ': source[i] = 'フ'; break;
                            case 'ﾍ': source[i] = 'ヘ'; break;
                            case 'ﾎ': source[i] = 'ホ'; break;
                            case 'ﾏ': source[i] = 'マ'; break;
                            case 'ﾐ': source[i] = 'ミ'; break;
                            case 'ﾑ': source[i] = 'ム'; break;
                            case 'ﾒ': source[i] = 'メ'; break;
                            case 'ﾓ': source[i] = 'モ'; break;
                            case 'ﾔ': source[i] = 'ヤ'; break;
                            case 'ﾕ': source[i] = 'ユ'; break;
                            case 'ﾖ': source[i] = 'ヨ'; break;
                            case 'ﾗ': source[i] = 'ラ'; break;
                            case 'ﾘ': source[i] = 'リ'; break;
                            case 'ﾙ': source[i] = 'ル'; break;
                            case 'ﾚ': source[i] = 'レ'; break;
                            case 'ﾛ': source[i] = 'ロ'; break;
                            case 'ﾜ': source[i] = 'ワ'; break;
                            case 'ﾝ': source[i] = 'ン'; break;
                            case 'ﾞ': source[i] = '゛'; break;
                            case 'ﾟ': source[i] = '゜'; break;
                        }
                    }
                }
            }
        }

        /// <summary>文字列内の数値を取得します。</summary>
        /// <param name="source">対象の文字列を表す string。</param>
        /// <param name="type">開始文字の種類を表す <see cref="CharTypes"/>。</param>
        /// <param name="pos">開始位置を表す int。</param>
        /// <param name="value">取得した数値を表す long。</param>
        /// <returns>数値を取得出来た場合は <see langword="true"/>、それ以外の場合は <see langword="false"/>。</returns>
        private bool GetNumber(string source, CharTypes type, ref int pos, out long value)
        {
            INumberComverter number = null;

            switch (type)
            {
                case CharTypes.Number: number = new NumberConverter(source[pos]); break;
                case CharTypes.RomanNumber: number = new RomanNumberConverter(source[pos]); break;
                case CharTypes.JpRomanNumber: number = new JpRomanNumberConverter(source[pos]); break;
                case CharTypes.CircleNumber: number = new CircleNumberConverter(source[pos]); break;
                case CharTypes.KanjiNumber: number = new KanjiNumberConverter(source[pos]); break;
            }

            for (int i = pos + 1; i < source.Length; i++)
            {
                if (number.AddChar(source[i]) == false)
                {
                    break;
                }
            }

            if (number.IsError == false)
            {
                value = number.Value;
                pos += number.Length;
            }
            else
            {
                value = 0;
            }

            return (number.IsError == false);
        }

        /// <summary>2つの文字コードを比較します。</summary>
        /// <param name="c1">比較する文字を表す char。</param>
        /// <param name="c2">比較する文字を表す char。</param>
        /// <returns>2つの文字コードの大小関係を表す int。</returns>
        private int CompareChar(char c1, char c2)
        {
            // 前中後、上中下の整列を考慮する
            string list = "上前中下後";
            int p1 = list.IndexOf(c1);
            int p2 = list.IndexOf(c2);

            if (p1 >= 0 && p2 >= 0)
            {
                return p1 - p2;
            }

            return StringComparer.CurrentCulture.Compare(c1.ToString(), c2.ToString());
        }

        #endregion

        #region INumberComverter

        /// <summary>数字を数値へ変換する機能を提供します。</summary>
        private interface INumberComverter
        {
            #region Property

            /// <summary>エラーが発生したかどうかを取得します。</summary>
            bool IsError { get; }

            /// <summary>数字から変換した数値を取得します。</summary>
            long Value { get; }

            /// <summary>数値全体の文字数を取得します。</summary>
            int Length { get; }

            #endregion

            #region Method

            /// <summary>数字を追加します。</summary>
            /// <param name="number">数字を表す char。</param>
            /// <returns>数字として成立する場合は <see langword="true"/>、それ以外の場合は <see langword="false"/>。</returns>
            bool AddChar(char number);

            #endregion
        }

        #endregion

        #region NumberConverter

        /// <summary>アラビア数字を数値へ変換する機能を提供します。</summary>
        /// <remarks>ASCIIと日本語の混在は許しません。</remarks>
        private class NumberConverter : INumberComverter
        {
            #region Field

            /// <summary>数値全体の長さを表す int。</summary>
            private int _length;
            /// <summary>アラビア数字の 0 を表す char。</summary>
            private char _numberZero;
            /// <summary>アラビア数字の 9 を表す char。</summary>
            private char _numberNine;
            /// <summary>変換結果の数値を表す long。</summary>
            private long _value;
            /// <summary>カンマ区切りの数値かどうかを表す bool。</summary>
            private bool _isComma;
            /// <summary>先頭から連続したアラビア数字の文字数を表す int。</summary>
            private int _numberCount;
            /// <summary>カンマ区切り以降のアラビア数字の文字数を表す int。</summary>
            private int _commaLength;

            #endregion

            #region Constructor

            /// <summary>インスタンスを初期化します。</summary>
            /// <param name="number">１文字目の数字を表す char。</param>
            public NumberConverter(char number)
            {
                if (number >= '0' && number <= '9')
                {
                    _numberZero = '0';
                    _numberNine = '9';
                }
                else
                {
                    _numberZero = '０';
                    _numberNine = '９';
                }

                _length = 1;
                _value = number - _numberZero;
                _isComma = false;
            }

            #endregion

            #region Property

            /// <summary>エラーが発生したかどうかを取得します。</summary>
            public bool IsError
            {
                get { return false; }
            }

            /// <summary>数字から変換した数値を取得します。</summary>
            public long Value
            {
                get { return _value; }
            }

            /// <summary>数値全体の文字数を取得します。</summary>
            public int Length
            {
                get { return _length; }
            }

            #endregion

            #region Method

            /// <summary>数字を追加します。</summary>
            /// <param name="number">数字を表す char。</param>
            /// <returns>数字として成立する場合は <see langword="true"/>、それ以外の場合は <see langword="false"/>。</returns>
            public bool AddChar(char number)
            {
                // 1文字目の数字と同種のアラビア数字かどうか
                if (number >= _numberZero && number <= _numberNine)
                {
                    if (_isComma)
                    {
                        _commaLength++;
                        if (_commaLength > 3)
                        {
                            _length = _numberCount;
                            return false;
                        }
                    }
                    else
                    {
                        _numberCount++;
                    }

                    _value = _value * 10 + (number - _numberZero);
                }
                // 3桁区切りのカンマかどうか
                else if (_numberZero - number == 4)
                {
                    if (_isComma == false && _numberCount > 3)
                    {
                        return false;
                    }
                    _commaLength = 0;
                }
                // アラビア数字以外の文字が見つかったら終了
                else
                {
                    return false;
                }

                _length++;
                return true;
            }

            #endregion
        }

        #endregion

        #region RomanNumberConverter

        /// <summary>英字表現のローマ数字を数値へ変換する機能を提供します。</summary>
        /// <remarks>大文字と小文字、ASCIIと日本語の混在は許しません。</remarks>
        private class RomanNumberConverter : INumberComverter
        {
            #region Field

            /// <summary>数値全体の長さを表す int。</summary>
            private int _length;
            /// <summary>アルファベットの A を表す char。</summary>
            private char _alphaA;
            /// <summary>未確定の数字を表す long。</summary>
            private long _number;
            /// <summary>変換結果の数値を表す long。</summary>
            private long _value;
            /// <summary>現在単位を表す long。</summary>
            private long _max;
            /// <summary>エラーが発生したかどうかを表す bool。</summary>
            private bool _isError;

            #endregion

            #region Constructor

            /// <summary>インスタンスを初期化します。</summary>
            /// <param name="alpha">１文字目の英字を表す char。</param>
            public RomanNumberConverter(char alpha)
            {
                if (alpha >= 'A' && alpha <= 'Z')
                {
                    _alphaA = 'A';
                }
                else if (alpha >= 'a' && alpha <= 'z')
                {
                    _alphaA = 'a';
                }
                else if (alpha >= 'Ａ' && alpha <= 'Ｚ')
                {
                    _alphaA = 'Ａ';
                }
                else if (alpha >= 'ａ' && alpha <= 'ｚ')
                {
                    _alphaA = 'ａ';
                }

                _length = 1;
                _number = Parse(alpha);
                _max = _number;
            }

            #endregion

            #region Property

            /// <summary>エラーが発生したかどうかを取得します。</summary>
            public bool IsError
            {
                get { return _isError; }
            }

            /// <summary>数字から変換した数値を取得します。</summary>
            public long Value
            {
                get { return _value + _number; }
            }

            /// <summary>数値全体の文字数を取得します。</summary>
            public int Length
            {
                get { return _length; }
            }

            #endregion

            #region Method

            /// <summary>ローマ数字を追加します。</summary>
            /// <param name="roman">ローマ数字を表す char。</param>
            /// <returns>数字として成立する場合は <see langword="true"/>、それ以外の場合は <see langword="false"/>。</returns>
            public bool AddChar(char roman)
            {
                long value = Parse(roman);

                // ローマ数字以外の文字が見つかったら終了
                if (value == 0)
                {
                    _isError = IsAlpha(roman);
                    return false;
                }
                // IV IX などの減算則表記
                else if (value > _max)
                {
                    long mag = value / _max;
                    if (mag == 5 || mag == 10)
                    {
                        _value += value - _number;
                        _number = 0;
                        _max = _max / 2;
                    }
                    else
                    {
                        _isError = IsAlpha(roman);
                        return false;
                    }
                }
                // VI XI など加算則表記
                else if (value < _max)
                {
                    _value += _number;
                    _number = value;
                    _max = value;
                }
                // II XX など同じ数字の繰り返し
                else
                {
                    _number += value;
                }

                _length++;
                return true;
            }

            /// <summary>ローマ数字を数値へ変換します。</summary>
            /// <param name="alpha">1 文字のローマ数字を表す char。</param>
            /// <returns>変換後の数値を表す long。</returns>
            protected long Parse(char alpha)
            {
                switch (alpha - _alphaA)
                {
                    case 08: return 1;      // I
                    case 21: return 5;      // V
                    case 23: return 10;     // X
                    case 11: return 50;     // L
                    case 02: return 100;    // C
                    case 03: return 500;    // D
                    case 12: return 1000;   // M
                }

                return 0;
            }

            /// <summary>指定の文字が英字かどうかを判定します。</summary>
            /// <param name="alpha">検査対象の文字を表す char。</param>
            /// <returns>指定の文字が英字だった場合は <see langword="true"/>、それ以外の場合は <see langword="false"/>。</returns>
            protected bool IsAlpha(char alpha)
            {
                return ((alpha >= 'A' && alpha <= 'Z') || (alpha >= 'a' && alpha <= 'z') || (alpha >= 'Ａ' && alpha <= 'Ｚ') || (alpha >= 'ａ' && alpha <= 'ｚ'));
            }

            #endregion
        }

        #endregion

        #region JpRomanNumberConverter

        /// <summary>全角ローマ数字を数値へ変換する機能を提供します。</summary>
        private class JpRomanNumberConverter : INumberComverter
        {
            #region Field

            /// <summary>数値全体の長さを表す int。</summary>
            private int _length;
            /// <summary>2文字以上の組み合わせが可能かどうかを表す bool。</summary>
            private bool _isMultiChar;
            /// <summary>ローマ数字の 1 を表す char。</summary>
            private char _romanOne;
            /// <summary>未確定の数字を表す long。</summary>
            private long _number;
            /// <summary>変換結果の数値を表す long。</summary>
            private long _value;
            /// <summary>現在単位を表す long。</summary>
            private long _max;

            #endregion

            #region Constructor

            /// <summary>インスタンスを初期化します。</summary>
            /// <param name="roman">１文字目のローマ数字を表す char。</param>
            public JpRomanNumberConverter(char roman)
            {
                _length = 1;

                // 全角ローマ数字(Ⅰ～XII,L,C,D,M)
                if (roman >= 0x2160 && roman <= 0x216F)
                {
                    _romanOne = (char)0x2160;
                }
                // 全角ローマ数字(ⅰ～xii,l,c,d,m)
                else if (roman >= 0x2170 && roman <= 0x217F)
                {
                    _romanOne = (char)0x2170;
                }

                long value = Parse(roman);
                if (value == 0)
                {
                    _value = roman - _romanOne + 1;
                    _isMultiChar = false;
                }
                else
                {
                    _number = value;
                    _max = _number;
                    _isMultiChar = true;
                }
            }

            #endregion

            #region Property

            /// <summary>エラーが発生したかどうかを取得します。</summary>
            public bool IsError
            {
                get { return false; }
            }

            /// <summary>数字から変換した数値を取得します。</summary>
            public long Value
            {
                get { return _value + _number; }
            }

            /// <summary>数値全体の文字数を取得します。</summary>
            public int Length
            {
                get { return _length; }
            }

            #endregion

            #region Method

            /// <summary>数字を追加します。</summary>
            /// <param name="roman">数字を表す char。</param>
            /// <returns>数字として成立する場合は <see langword="true"/>、それ以外の場合は <see langword="false"/>。</returns>
            public bool AddChar(char roman)
            {
                if (_isMultiChar == false)
                {
                    return false;
                }

                long value = Parse(roman);

                // ローマ数字以外の文字が見つかったら終了
                if (value == 0)
                {
                    return false;
                }
                // IV IX などの減算則表記
                else if (value > _max)
                {
                    long mag = value / _max;
                    if (mag == 5 || mag == 10)
                    {
                        _value += value - _number;
                        _number = 0;
                        _max = _max / 2;
                    }
                    else
                    {
                        return false;
                    }
                }
                // VI XI など加算則表記
                else if (value < _max)
                {
                    _value += _number;
                    _number = value;
                    _max = value;
                }
                // II XX など同じ数字の繰り返し
                else
                {
                    _number += value;
                }

                _length++;
                return true;
            }

            /// <summary>ローマ数字を数値へ変換します。</summary>
            /// <param name="roman">1 文字のローマ数字を表す char。</param>
            /// <returns>変換後の数値を表す long。</returns>
            protected long Parse(char roman)
            {
                switch (roman - _romanOne)
                {
                    case 0x0: return 1;     // I
                    case 0x4: return 5;     // V
                    case 0x9: return 10;    // X
                    case 0xC: return 50;    // L
                    case 0xD: return 100;   // C
                    case 0xE: return 500;   // D
                    case 0xF: return 1000;  // M
                }

                return 0;
            }

            #endregion
        }

        #endregion

        #region CircleNumberConverter

        /// <summary>丸数字を数値へ変換する機能を提供します。</summary>
        private class CircleNumberConverter : INumberComverter
        {
            #region Field

            /// <summary>現在の数字を表す long。</summary>
            private long _number;

            #endregion

            #region Constructor

            /// <summary>インスタンスを初期化します。</summary>
            /// <param name="number">１文字目の数字を表す char。</param>
            public CircleNumberConverter(char number)
            {
                // ①～⑳
                if (number >= 0x2460 && number <= 0x2473)
                {
                    _number = number - 0x2460 + 1;
                }
                // (1)～(12)
                else if (number >= 0x2474 && number <= 0x2487)
                {
                    _number = number - 0x2474 + 1;
                }
                // 1.～20.
                else if (number >= 0x2488 && number <= 0x249B)
                {
                    _number = number - 0x2488 + 1;
                }
                // 丸付き21～35
                else if (number >= 0x3251 && number <= 0x325F)
                {
                    _number = number - 0x3251 + 21;
                }
                // {一}～{十}
                else if (number >= 0x3220 && number <= 0x3229)
                {
                    _number = number - 0x3220 + 1;
                }
                // 丸付き一～十
                else if (number >= 0x3280 && number <= 0x3289)
                {
                    _number = number - 0x3280 + 1;
                }
            }

            #endregion

            #region Property

            /// <summary>エラーが発生したかどうかを取得します。</summary>
            public bool IsError
            {
                get { return false; }
            }

            /// <summary>数字から変換した数値を取得します。</summary>
            public long Value
            {
                get { return _number; }
            }

            /// <summary>数値全体の文字数を取得します。</summary>
            public int Length
            {
                get { return 1; }
            }

            #endregion

            #region Method

            /// <summary>数字を追加します。</summary>
            /// <param name="number">数字を表す char。</param>
            /// <returns>数字として成立する場合は <see langword="true"/>、それ以外の場合は <see langword="false"/>。</returns>
            public bool AddChar(char number)
            {
                return false;
            }

            #endregion
        }

        #endregion

        #region KanjiNumberConverter

        /// <summary>漢数字を数値へ変換する機能を提供します。</summary>
        private class KanjiNumberConverter : INumberComverter
        {
            #region Field

            /// <summary>数値全体の長さを表す int。</summary>
            private int _length;
            /// <summary>位取り記数法かどうかを表す bool。</summary>
            private bool _isNumeral;
            /// <summary>直前の数字を表す long。</summary>
            private long _number;
            /// <summary>1万未満の数値を表す long。</summary>
            private long _value1;
            /// <summary>変換結果の数値を表す long。</summary>
            private long _value2;
            /// <summary>1万未満の現在単位を表す long。</summary>
            private long _unit1;
            /// <summary>数値全体の現在単位を表す long。</summary>
            private long _unit2;

            #endregion

            #region Constructor

            /// <summary>インスタンスを初期化します。</summary>
            /// <param name="number">１文字目の数字を表す char。</param>
            public KanjiNumberConverter(char number)
            {
                _length = 1;
                long temp = Parse(number);
                if (temp < 10)
                {
                    _number = temp;
                    _unit1 = 9999;
                    _unit2 = 99999999999999999;
                }
                else
                {
                    _value1 = _unit1 = temp;
                }
            }

            #endregion

            #region Property

            /// <summary>エラーが発生したかどうかを取得します。</summary>
            public bool IsError
            {
                get { return false; }
            }

            /// <summary>数字から変換した数値を取得します。</summary>
            public long Value
            {
                get { return _value2 + _value1 + _number; }
            }

            /// <summary>数値全体の文字数を取得します。</summary>
            public int Length
            {
                get { return _length; }
            }

            #endregion

            #region Method

            /// <summary>数字を追加します。</summary>
            /// <param name="kanji">数字を表す char。</param>
            /// <returns>数字として成立する場合は <see langword="true"/>、それ以外の場合は <see langword="false"/>。</returns>
            public bool AddChar(char kanji)
            {
                long value = Parse(kanji);

                // 2文字目の内容で位取り記数法かどうかを決定する
                if (_length == 1)
                {
                    _isNumeral = (_number + _value1 < 10 && value < 10);
                    if (_isNumeral)
                    {
                        _value2 = _number;
                        _number = 0;
                    }
                }

                if (value < 0)
                {
                    return false;
                }

                if (_isNumeral)
                {
                    if (value > 10)
                    {
                        return false;
                    }

                    _value2 = _value2 * 10 + value;
                    _length++;
                    return true;
                }
                else
                {
                    if (value < 10)
                    {
                        // 9以下の漢数字が連続したらエラー
                        if (_number > 0)
                        {
                            return false;
                        }

                        _number = value;
                    }
                    else if (value <= 1000)
                    {
                        // 前方より大きな単位が出現したらエラー
                        if (_unit1 <= value)
                        {
                            return false;
                        }

                        _value1 += _number * value;
                        _number = 0;
                        _unit1 = value;
                    }
                    else
                    {
                        // 前方より大きな単位が出現したらエラー
                        if (_unit2 <= value)
                        {
                            return false;
                        }

                        _value2 += (_value1 + _number) * value;
                        _value1 = _number = 0;
                        _unit1 = 9999;
                        _unit2 = value;
                    }

                    _length++;
                    return true;
                }
            }

            /// <summary>漢数字を数値へ変換します。</summary>
            /// <param name="kanji">漢数字を表す char。</param>
            /// <returns>変換後の数値を表す long。</returns>
            protected long Parse(char kanji)
            {
                switch (kanji)
                {
                    case '〇': return 0;
                    case '一': return 1;
                    case '二': return 2;
                    case '三': return 3;
                    case '四': return 4;
                    case '五': return 5;
                    case '六': return 6;
                    case '七': return 7;
                    case '八': return 8;
                    case '九': return 9;
                    case '十': return 10;
                    case '百': return 100;
                    case '千': return 1000;
                    case '万': return 10000;
                    case '億': return 100000000;
                    case '兆': return 1000000000000;
                    case '京': return 10000000000000000;
                    case '零': return 0;
                    case '壱': return 1;
                    case '弐': return 2;
                    case '参': return 3;
                    case '拾': return 10;
                }

                return -1;
            }

            #endregion
        }

        #endregion
    }
}
