using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core;
using MysteriousAlchemy.Core.Timers;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Skies;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI
{
    public class CrossScrollElements : UIElement
    {
        public List<CSEIcon> icons;
        public CSEIcon SelectIcon;
        public float IconPadding = 30;
        int _selectIndex;
        int SelectIndex
        {
            get { return _selectIndex; }
            set { _selectIndex = Math.Clamp(value, 0, icons.Count - 1); }
        }


        public Vector2 ShowElementSize;

        private bool isDragging;
        private Timer AnimationTimer;
        private List<float> StartOffest;
        private List<float> TargetOffest;
        private List<float> SizeAnimation;
        public CrossScrollElements()
        {
            OverflowHidden = true;
            icons = new List<CSEIcon>();

            AnimationTimer = TimerSystem.RegisterTimer<Timer>(1, 15, IsUI_Timer: true);
            AnimationTimer.pause = true;

            StartOffest = new List<float>();
            TargetOffest = new List<float>();
            SizeAnimation = new List<float>();
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            OverflowHidden = true;
            AnimationTimer.pause = !isDragging;

            for (int i = 0; i < icons.Count; i++)
            {
                float size;
                Vector2 ElementCenter = new(Width.GetValue(Parent.Width.Pixels) / 2f, Height.GetValue(Parent.Height.Pixels) / 2f);
                int offest = i - SelectIndex;
                if (ShowElementSize.X == 0 || ShowElementSize.Y == 0)
                {
                    size = Height.GetValue(Parent.Height.Pixels) * (isDragging ? MathHelper.Lerp(SizeAnimation[i], (i == SelectIndex ? 1f : 0.6f), AnimationTimer.GetEaseInter()) : (i == SelectIndex ? 1f : 0.6f));
                    icons[i].Width.Set(size, 0);
                    icons[i].Height.Set(size, 0);

                    TargetOffest[i] = offest * (Height.GetValue(Parent.Height.Pixels) + IconPadding);
                    icons[i].Left.Set((isDragging ? MathHelper.Lerp(StartOffest[i], TargetOffest[i], AnimationTimer.GetEaseInter()) : TargetOffest[i]) - size / 2f + ElementCenter.X, 0);


                    icons[i].Top.Set(-size / 2f + ElementCenter.Y, 0);
                }
                else
                {
                    Vector2 currectSize = ShowElementSize * (isDragging ? MathHelper.Lerp(SizeAnimation[i], (i == SelectIndex ? 1f : 0.6f), AnimationTimer.GetEaseInter()) : (i == SelectIndex ? 1f : 0.6f));
                    currectSize = new Vector2
                        (
                        Math.Clamp(currectSize.X, 0, Width.GetValue(Parent.Width.Pixels)),
                        Math.Clamp(currectSize.Y, 0, Height.GetValue(Parent.Height.Pixels))
                        );


                    icons[i].Width.Set(currectSize.X, 0);
                    icons[i].Height.Set(currectSize.Y, 0);

                    TargetOffest[i] = offest * (currectSize.X + IconPadding);
                    icons[i].Left.Set((isDragging ? MathHelper.Lerp(StartOffest[i], TargetOffest[i], AnimationTimer.GetEaseInter()) : TargetOffest[i]) - currectSize.X / 2f + ElementCenter.X, 0);

                    icons[i].Top.Set(-currectSize.Y / 2f + ElementCenter.Y, 0);

                }
                icons[i].Recalculate();

            }
            AnimationTimer.ConditionTrigger(isDragging, () =>
            {
                isDragging = false;
                AnimationTimer.pause = true;
                AnimationTimer.ResetTimer();
            });
            base.DrawSelf(spriteBatch);
        }
        public override void ScrollWheel(UIScrollWheelEvent evt)
        {

            if (!isDragging)
            {
                for (int i = 0; i < icons.Count; i++)
                {
                    float size;
                    Vector2 ElementCenter = new(Width.GetValue(Parent.Width.Pixels) / 2f, Height.GetValue(Parent.Height.Pixels) / 2f);
                    int offest = i - SelectIndex;
                    if (ShowElementSize.X == 0 || ShowElementSize.Y == 0)
                    {
                        size = Height.GetValue(Parent.Height.Pixels) * (i == SelectIndex ? 1f : 0.6f);
                        StartOffest[i] = offest * (size + IconPadding);
                        SizeAnimation[i] = (i == SelectIndex ? 1f : 0.6f);
                    }
                    else
                    {
                        Vector2 currectSize = ShowElementSize * (i == SelectIndex ? 1f : 0.6f);
                        currectSize = new Vector2
                            (
                            Math.Clamp(currectSize.X, 0, Width.GetValue(Parent.Width.Pixels)),
                            Math.Clamp(currectSize.Y, 0, Height.GetValue(Parent.Height.Pixels))
                            );
                        StartOffest[i] = offest * (currectSize.X + IconPadding);
                        SizeAnimation[i] = (i == SelectIndex ? 1f : 0.6f);
                    }
                    isDragging = true;
                }

            }
            SelectIndex -= Math.Sign(evt.ScrollWheelValue);
            SoundEngine.PlaySound(MASoundID.MenuTick);
            base.ScrollWheel(evt);

        }
        public void AddIcon(CSEIcon icon, Action<object> WhenClick)
        {
            icon.Index = icons.Count;
            icon.OnLeftClick += Icon_OnLeftClick;
            icons.Add(icon);
            icon.WhenClick = WhenClick;
            StartOffest.Add(0);
            TargetOffest.Add(0);
            SizeAnimation.Add(0.75f);
            Append(icon);
        }
        public void Clear()
        {
            icons.ForEach(o =>
            {
                RemoveChild(o);

            });
            icons.Clear();
        }

        private void Icon_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!isDragging)
            {
                for (int i = 0; i < icons.Count; i++)
                {
                    float size;
                    Vector2 ElementCenter = new(Width.GetValue(Parent.Width.Pixels) / 2f, Height.GetValue(Parent.Height.Pixels) / 2f);
                    int offest = i - SelectIndex;
                    if (ShowElementSize.X == 0 || ShowElementSize.Y == 0)
                    {
                        size = Height.GetValue(Parent.Height.Pixels) * (i == SelectIndex ? 1f : 0.6f);
                        StartOffest[i] = offest * (size + IconPadding);
                        SizeAnimation[i] = (i == SelectIndex ? 1f : 0.6f);
                    }
                    else
                    {
                        Vector2 currectSize = ShowElementSize * (i == SelectIndex ? 1f : 0.6f);
                        currectSize = new Vector2
                            (
                            Math.Clamp(currectSize.X, 0, Width.GetValue(Parent.Width.Pixels)),
                            Math.Clamp(currectSize.Y, 0, Height.GetValue(Parent.Height.Pixels))
                            );
                        StartOffest[i] = offest * (currectSize.X + IconPadding);
                        SizeAnimation[i] = (i == SelectIndex ? 1f : 0.6f);
                    }
                    SoundEngine.PlaySound(MASoundID.MenuTick);
                    isDragging = true;
                }
                SelectIndex = ((CSEIcon)listeningElement).Index;
            }
        }

    }

    public class CSEIcon : UIElement
    {
        public string IconPath;
        object MappingData;
        public Action<object> WhenClick;
        public int Index;
        public Vector4 alphaInFourAngle = Vector4.One;
        public float AngleV = MathHelper.PiOver2;
        public CSEIcon(string IconPath, object MappingData)
        {
            this.IconPath = IconPath;
            this.MappingData = MappingData;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (IsMouseHovering)
                Main.LocalPlayer.mouseInterface = true;
            spriteBatch.Draw(AssetUtils.GetTexture2D(IconPath), GetDimensions().ToRectangle(), Color.White);
        }
        public override void LeftClick(UIMouseEvent evt)
        {
            WhenClick?.Invoke(MappingData);
            base.LeftClick(evt);
        }
    }
}

