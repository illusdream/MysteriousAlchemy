﻿using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour;
using MysteriousAlchemy.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Hook = MysteriousAlchemy.Core.Interface.Hook;

namespace MysteriousAlchemy.Core.System
{
    //自定义的绘制系统
    //拖尾之类的暂时不做迁移——还没想好
    //目前功能 ：对话相关的绘制，部分场景，第一次拿起武器的文字描述
    //           自定义粒子系统的绘制
    //           彩色字体可能单独拿出来绘制——一份特效，一份UI 如果需要做到部分字体被拖尾遮住可能需要，但是加个条件判断不就好了吗
    //           Default和Immediate分开绘制，防止多次end，begin——immediate会立刻绘制
    public class CustomDrawSystem : Hook
    {

        public int LoaderIndex => 1;

        public void Load()
        {

            var drawMethod = typeof(Main).GetMethod("DrawDust", BindingFlags.Instance | BindingFlags.NonPublic);
            MonoModHooks.Add(drawMethod, CustomDraw);
        }

        public void Unload()
        {

        }
        /// <summary>
        /// 在Dust的绘制后插入自己的绘制
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        public void CustomDraw(Terraria.On_Main.orig_DrawDust orig, Main self)
        {
            orig(self);

            if (Main.gameMenu)
                return;

            SpriteBatch spriteBatch = Main.spriteBatch;


            #region 绘制Addtive
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            //绘制Projectile
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.ModProjectile is IDrawAddtive)
                {
                    (projectile.ModProjectile as IDrawAddtive).DrawAddtive(spriteBatch);
                }
            }
            //绘制NPC
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc.ModNPC is IDrawAddtive)
                {
                    (npc.ModNPC as IDrawAddtive).DrawAddtive(spriteBatch);
                }
            }
            spriteBatch.End();
            #endregion

            #region 绘制NonPremultiplied
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            //绘制Projectile
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.ModProjectile is IDrawNonPremultiplied)
                {

                    (projectile.ModProjectile as IDrawNonPremultiplied).DrawNonPremultiplied(spriteBatch);
                }
            }
            //绘制NPC
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc.ModNPC is IDrawNonPremultiplied)
                {
                    (npc.ModNPC as IDrawNonPremultiplied).DrawNonPremultiplied(spriteBatch);
                }
            }
            spriteBatch.End();
            #endregion
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            NarrationSystem.instance.DrawAll(spriteBatch);
            spriteBatch.End();
        }
    }
}
