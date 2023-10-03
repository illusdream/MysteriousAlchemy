using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Core.Perfab;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.System
{
    public class NarrationSystem : Hook, IUpdate
    {
        List<Narration> Narrations;
        public int LoaderIndex => 1;

        public int UpdateIndex => 1;

        public static NarrationSystem instance;
        public NarrationSystem()
        {
            instance = this;
        }
        public void Load()
        {
            Narrations = new List<Narration>();
        }

        public void Unload()
        {
            if (Narrations != null)
                Narrations = null;
        }
        public void AddNarration(Vector2 pos, string text, float alpha, TextSpreadMode textSpreadMode, int EntryTime, int ShowTime, int HideTime)
        {
            var instance = new Narration(pos, Vector2.Zero, text, alpha, textSpreadMode, EntryTime, ShowTime, HideTime);
            Narrations.Add(instance);
        }
        public void DrawAll(SpriteBatch spriteBatch)
        {
            foreach (var unit in Narrations)
            {
                unit.Draw(spriteBatch);
            }
        }

        public void Update()
        {
            foreach (var text in Narrations)
            {
                text.Update();
            }
            Narrations.RemoveAll((n) => !n.active);
        }
    }
}
