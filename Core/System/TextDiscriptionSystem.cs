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
    public class TextDiscriptionSystem : Hook, IUpdate
    {
        List<TextDescription> textDescriptions;
        public int LoaderIndex => 1;

        public int UpdateIndex => 1;

        public static TextDiscriptionSystem instance;
        public TextDiscriptionSystem()
        {
            instance = this;
        }
        public void Load()
        {
            textDescriptions = new List<TextDescription>();
        }

        public void Unload()
        {
            if (textDescriptions != null)
                textDescriptions = null;
        }
        public void AddTextDiscription(Vector2 pos, string text, float alpha, TextSpreadMode textSpreadMode, int EntryTime, int ShowTime, int HideTime)
        {
            var instance = new TextDescription(pos, Vector2.Zero, text, alpha, textSpreadMode, EntryTime, ShowTime, HideTime);
            textDescriptions.Add(instance);
        }
        public void DrawAll(SpriteBatch spriteBatch)
        {
            foreach (var unit in textDescriptions)
            {
                unit.Draw(spriteBatch);
            }
        }

        public void Update()
        {
            foreach (var text in textDescriptions)
            {
                text.Update();
            }
            textDescriptions.RemoveAll((n) => !n.active);
        }
    }
}
