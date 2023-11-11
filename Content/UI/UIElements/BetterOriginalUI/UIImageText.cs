using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI
{
    public class UIImageText : UIElement
    {
        private string LocalizationText;
        public float scale = 1;
        //计算下个UIElement的横坐标
        private float TotalLeft = 0;
        public UIImageText(string LocalizationText, float height)
        {
            Height.Set(height, 0);
            this.LocalizationText = LocalizationText;
            AddElements();
        }

        private UIText AddUIText(string text, float scale = 1)
        {
            UIText instance = new UIText(text, scale);
            instance.TextOriginX = 0;
            instance.TextOriginY = 0;
            instance.VAlign = 0.5f;
            Append(instance);
            return instance;
        }
        private UISampleIcon AddIcon(string path)
        {
            UISampleIcon instance = new UISampleIcon(path, new Tuple<float, float>(Height.Pixels, 0));
            Append(instance);
            return instance;
        }
        private void AddElements()
        {
            TotalLeft = 0;
            //解析语句
            //@"/@Tex:(.*?)@/" -> "/@Tex:地址@/"
            Regex TextureRegex = new Regex(@"/@Tex:(.*?)@/");
            //FIFO，从前到后处理文本，以获得正确的字符串顺序
            Queue<Match> matches = new Queue<Match>();

            var results = TextureRegex.Matches(LocalizationText);
            foreach (var result in results)
            {
                matches.Enqueue((Match)result);
            }

            RecursionMatches(ref matches, LocalizationText);
            //设置本体宽度，防止出现一些奇奇怪怪的问题
            Width.Set(TotalLeft, 0);
        }
        //递归处理文本
        private void RecursionMatches(ref Queue<Match> matches, string localizationText)
        {
            //退出递归条件
            if (localizationText == "")
                return;
            if (matches.Count == 0)
            {
                var uitextEnd = AddUIText(localizationText, scale);
                uitextEnd.Left.Set(TotalLeft, 0);
                TotalLeft += uitextEnd.MinWidth.Pixels;
                return;
            }


            Match fristMatch = matches.Dequeue();
            //将文字从图片部分切成两段
            string[] strings = localizationText.Split(fristMatch.ToString(), 2, StringSplitOptions.None);
            //把第一段文字转换成UIText
            if (strings[0] != "")
            {
                var uitext = AddUIText(strings[0], scale);
                uitext.Left.Set(TotalLeft, 0);
                TotalLeft += uitext.MinWidth.Pixels;
            }


            //转换位于中间的图片
            string texturePath = fristMatch.Groups[1].ToString();
            var icon = AddIcon(texturePath);
            icon.Left.Set(TotalLeft, 0);
            icon.CalculateCurrectSize();
            TotalLeft += icon.Width.Pixels;
            //递归处理剩下部分
            RecursionMatches(ref matches, strings[1]);
        }


        public void SetLocalization(string LocalizationText)
        {
            this.LocalizationText = LocalizationText;
            AddElements();
        }
    }
}
