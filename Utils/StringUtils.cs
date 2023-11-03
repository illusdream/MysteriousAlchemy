using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MysteriousAlchemy.Utils
{
    /// <summary>
    /// 用来对String做额外处理
    /// </summary>
    public class StringUtils
    {
        int OldIndex_LetterByLetter = -1;
        public void AppendLetterByLetter(ref string value, string appendString, float interget)
        {
            float safeInter = Math.Clamp(interget, 0f, 1f);
            int letterIndex = Math.Clamp((int)(safeInter * (appendString.Length - 1)), 0, appendString.Length - 1);
            if (letterIndex == OldIndex_LetterByLetter)
            {
                return;
            }

            value += appendString[letterIndex];
            OldIndex_LetterByLetter = letterIndex;
        }
    }
}
