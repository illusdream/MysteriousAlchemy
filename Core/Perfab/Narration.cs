using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Utils;
using rail;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;

namespace MysteriousAlchemy.Core.Perfab
{
    public class Narration : StateMachine
    {
        //世界坐标
        public Vector2 Position;
        public Vector2 Velocity;

        public string Text;
        public string FianlText;
        public TextSpreadMode SpreadMode;
        public float Alpha;
        public int EntryTime;
        public int ShowTime;
        public int HideTime;
        public int Timer;
        public bool active;

        public Narration(Vector2 position, Vector2 velocity, string text, float alpha, TextSpreadMode spreadMode, int entryTime, int showTime, int hideTime) : base()
        {
            Position = position;
            Velocity = velocity;
            Text = text;
            Alpha = alpha;
            SpreadMode = spreadMode;
            EntryTime = entryTime;
            ShowTime = showTime;
            HideTime = hideTime;
            active = true;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (FianlText != null)
            {
                DynamicSpriteFont font = (ReLogic.Graphics.DynamicSpriteFont)FontAssets.MouseText.Value;
                spriteBatch.DrawString(font, FianlText, DrawUtil.ToScreenPosition(Position), Color.White * Alpha, 0, font.MeasureString(FianlText) / 2f, 1, SpriteEffects.None, 0);
            }
        }
        public void Update()
        {
            Timer++;
            AI();
        }
        public override void Initialize()
        {
            RegisterState(new EntryState());
            RegisterState(new ShowState());
            RegisterState(new HideState());
            SetState<EntryState>();
            base.Initialize();
        }

        public class EntryState : IState
        {
            public void ExitState(IStateMachine stateMachine)
            {


            }

            public void OnState(IStateMachine stateMachine)
            {
                Narration text = SMUtils.GetStateMachineCurrect<Narration>(stateMachine);


                if ((stateMachine as Narration).Timer > (stateMachine as Narration).EntryTime)
                {
                    (stateMachine as Narration).Timer = 0;
                    stateMachine.SetState<ShowState>();
                }

            }

            void IState.EntryState(IStateMachine stateMachine)
            {

            }
        }
        public class ShowState : IState
        {
            public void EntryState(IStateMachine stateMachine)
            {

            }

            public void ExitState(IStateMachine stateMachine)
            {

            }
            StringUtils stringUtils = new StringUtils();
            public void OnState(IStateMachine stateMachine)
            {
                Narration TD = SMUtils.GetStateMachineCurrect<Narration>(stateMachine);
                float inter = TD.Timer / (float)TD.ShowTime;

                switch (TD.SpreadMode)
                {
                    case TextSpreadMode.letter:
                        stringUtils.AppendLetterByLetter(ref TD.FianlText, TD.Text, inter);
                        break;
                    case TextSpreadMode.word:
                        TD.FianlText = TD.Text;
                        break;
                    case TextSpreadMode.immediate:
                        TD.FianlText = TD.Text;
                        break;
                    default:
                        break;
                }
                if ((stateMachine as Narration).Timer > (stateMachine as Narration).ShowTime)
                {
                    (stateMachine as Narration).Timer = 0;
                    stateMachine.SetState<HideState>();
                }
            }
        }
        public class HideState : IState
        {
            public void EntryState(IStateMachine stateMachine)
            {

            }

            public void ExitState(IStateMachine stateMachine)
            {

            }

            public void OnState(IStateMachine stateMachine)
            {
                Narration TD = SMUtils.GetStateMachineCurrect<Narration>(stateMachine);
                float inter = TD.Timer / (float)TD.HideTime;
                TD.Alpha = (1 - inter);
                if ((stateMachine as Narration).Timer > (stateMachine as Narration).HideTime)
                {
                    (stateMachine as Narration).active = false;
                }
            }
        }
    }
    public enum TextSpreadMode
    {
        /// <summary>
        /// 一个一个字母显现
        /// </summary>
        letter,
        /// <summary>
        /// 以单词为单位显现
        /// </summary>
        word,

        /// <summary>
        /// 立刻显示所有文字
        /// </summary>
        immediate
    }
}
