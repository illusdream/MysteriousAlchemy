using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Core.System;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Core.Perfab.Projectiles
{
    //快速挥舞的镰刀,攻击方式为大风车，如果蓄力达到一定程度，松开右键时会触发连段，额外进行两次攻击：向鼠标方向挥砍一次，再向人物正前方挥砍有一次
    //在镰刃区造成正常伤害，长柄部分只造成25%伤害，以及高额击退
    public abstract class BaseQuickScythe : BaseHeldProj, IDrawPrimitives, IDrawAddtive
    {
        //动量，用于判断是否可以触发连段
        public float Momentum;
        //触发连段所需最低动量
        public float MaxMomentum { get; set; }
        //长柄指向的角度
        public float AttackAngle { get; set; }

        public float SwingAccTime => 300;
        public float MaxSwingSpeed => MathHelper.TwoPi / 30f;

        public virtual float AngleH { get; set; }
        public virtual float AngleV { get; set; }
        public virtual float WeaponStaticSize => 1;

        public Vector2 WeaponCurretSize => WeaponStaticSize * TextureAssets.Projectile[Projectile.type].Size() * WeaponSizeModify;

        public float WeaponCurretLength => WeaponCurretSize.Length();

        public virtual float HeldPosOffest { get; set; }

        public Vector2 HandPos;

        public Vector2 WeaponHeldPostion => DrawUtils.MartixTrans((AttackAngle).ToRotationVector2() * (WeaponCurretSize / 2f * HeldPosOffest), AngleH, AngleV);

        public Vector2 WeaponTopPosition => DrawUtils.MartixTrans((AttackAngle).ToRotationVector2() * (WeaponCurretSize / 2f * (1 + HeldPosOffest)), AngleH, AngleV);

        public int SoundPlayInterval => 60;

        public Vector2[] SlashTop;
        public Vector2[] SlashBottom;
        public int SlashLength => 20;

        public SpriteSortMode Sort => SpriteSortMode.Immediate;

        public int Timer;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
            base.SetStaticDefaults();
        }
        public override void AI()
        {
            Timer++;
            //PlaySound();
            UpdateProjectileAlive();
            UpdatePlayerArm();
            UpdateAngle();
            UpdatePosition();
            UpdateSlashCache();
            base.AI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            WeaponDraw(Main.spriteBatch);
            return false;
        }
        public override void PostDraw(Color lightColor)
        {

            base.PostDraw(lightColor);
        }
        public void WeaponDraw(SpriteBatch spriteBatch)
        {
            //贴图旋转修正
            float TexRotationFix = MathHelper.PiOver4 * OwnerDirection;
            //武器贴图
            Texture2D weapon = TextureAssets.Projectile[Type].Value;

            if (OwnerDirection == 1)
            {
                DrawUtils.DrawEntityInWorld(
                    spriteBatch,
                    weapon,
                    HandPos - Main.screenPosition + WeaponHeldPostion,
                    Color.White,
                    default,
                    AttackAngle + TexRotationFix * OwnerDirection,
                    DrawUtils.GetCurrectScale(weapon, WeaponCurretSize),
                    AngleH,
                    AngleV
                    );
            }
            else
            {
                DrawUtils.DrawEntityInWorld(
                    spriteBatch,
                    weapon,
                    HandPos - Main.screenPosition + WeaponHeldPostion,
                    Color.White,
                    default,
                    AttackAngle + TexRotationFix * OwnerDirection,
                    DrawUtils.GetCurrectScale(weapon, WeaponCurretSize),
                    AngleH,
                    AngleV,
                    Enum.ModifySpriteEffect.FlipDiagonally
                    );
            }
        }

        public void UpdatePlayerArm()
        {
            //让弹幕图层在在玩家手中
            Owner.heldProj = Projectile.whoAmI;
            //保持物品使用
            Owner.itemTime = Owner.itemAnimation = 2;

            float ArmToward = (Main.MouseWorld - Owner.Center).ToRotation();
            //控制手臂旋转
            Owner.itemRotation = ArmToward;
            //记录手臂位置
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, ArmToward - MathHelper.ToRadians(90f)); // set arm position (90 degree offset since arm starts lowered)
            HandPos = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, ArmToward - (float)Math.PI / 2); // get position of hand
            HandPos.Y += Owner.gfxOffY;

            Owner.direction = Math.Sign((Main.MouseWorld - Owner.Center).X);
        }
        public void UpdatePosition()
        {
            Projectile.Center = Owner.Center;
        }
        public void UpdateAngle()
        {
            AttackAngle += MaxSwingSpeed * OwnerDirection * MathF.Sqrt(Math.Clamp(Timer / SwingAccTime, 0, 1));
        }
        public void UpdateProjectileAlive()
        {
            if (Owner.controlUseItem)
                Projectile.timeLeft = 2;
        }

        public void UpdateSlashCache()
        {
            if (SlashTop is null)
            {
                SlashTop = new Vector2[SlashLength];
                for (int i = 0; i < SlashLength; i++)
                {
                    SlashTop[i] = AttackAngle.ToRotationVector2() * WeaponTopPosition.Length() * 0.75f;
                }
            }
            if (SlashBottom is null)
            {
                SlashBottom = new Vector2[SlashLength];
                for (int i = 0; i < SlashLength; i++)
                {
                    SlashBottom[i] = AttackAngle.ToRotationVector2() * WeaponTopPosition.Length() * 0.75f * 0.75f;
                }
            }
            for (int i = SlashLength - 1; i > 0; i--)
            {
                SlashTop[i] = SlashTop[i - 1];
            }
            SlashTop[0] = AttackAngle.ToRotationVector2() * WeaponTopPosition.Length() * 0.75f;
            for (int i = SlashLength - 1; i > 0; i--)
            {
                SlashBottom[i] = SlashBottom[i - 1];
            }
            SlashBottom[0] = AttackAngle.ToRotationVector2() * WeaponTopPosition.Length() * 0.4f * 0.75f;
        }
        public void PlaySound()
        {
            if (AttackAngle % (MathHelper.TwoPi / 3f) < 0.2f)
                SoundEngine.PlaySound(MASoundID.Swing_Item1);
        }

        public void DrawSlash(Color color, Texture2D shape, Texture2D colorBar)
        {
            List<VertexPositionColorTexture> bars = new List<VertexPositionColorTexture>();

            // 把所有的点都生成出来，按照顺序
            for (int i = 0; i < SlashLength; ++i)
            {
                if (SlashTop[i] == Vector2.Zero)
                {
                    break;
                }
                if (SlashBottom[i] == Vector2.Zero) break;
                //Main.spriteBatch.Draw(Main.magicPixel, oldPosi[i] - Main.screenPosition,
                //    new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0f);

                var factor = i / (float)SlashLength;
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                bars.Add(new VertexPositionColorTexture((SlashTop[i] + HandPos).Vec3(), color, new Vector2(factor, 0)));
                bars.Add(new VertexPositionColorTexture((SlashBottom[i] + HandPos).Vec3(), color, new Vector2(factor, 1)));
            }
            List<VertexPositionColorTexture> triangleList = new List<VertexPositionColorTexture>();

            if (bars.Count > 2)
            {

                // 按照顺序连接三角形

                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    triangleList.Add(bars[i]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 1]);

                    triangleList.Add(bars[i + 1]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 3]);
                }

                Effect effect = AssetUtils.GetEffect("DefaultSlash");
                Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
                Matrix view = Main.GameViewMatrix.TransformationMatrix;
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

                effect.Parameters["transformMatrix"].SetValue(world * view * projection);
                effect.Parameters["shapeTexture"].SetValue(shape);
                effect.Parameters["colorTexture"].SetValue(colorBar);
                effect.CurrentTechnique.Passes[0].Apply();
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public void DrawPrimitives(SpriteBatch spriteBatch)
        {
            DrawSlash(Color.White, AssetUtils.GetTexture2D(AssetUtils.Extra + "Slash_1"), AssetUtils.GetColorBar("Frost2"));
        }

        public void DrawAddtive(SpriteBatch spriteBatch)
        {

        }
    }
    public enum QuickScytheSytle
    {
        Swing, Sweep
    }
}
