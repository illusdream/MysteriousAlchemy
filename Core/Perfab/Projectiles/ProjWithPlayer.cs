using Microsoft.Xna.Framework;
using MysteriousAlchemy.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Core.Perfab.Projectiles
{
    public class ProjWithPlayer : ModProjectile
    {
        public override string Texture => AssetUtils.Projectiles + Name;
        private Player _player;
        public Player player { get { return _player; } }
        public override void OnSpawn(IEntitySource source)
        {
            _player = Main.player[Projectile.owner];
            base.OnSpawn(source);
        }
    }
}