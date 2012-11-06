
using Microsoft.Xna.Framework;
namespace StormGame
{
    class KillCounter : GameplayUIElement
    {
        private int kills;
        private int maxKills;

        public KillCounter(int maxKills)
        {
            kills = 0;
            this.maxKills = maxKills;
        }

        public override void Draw()
        {
            Globals.SpriteBatch.DrawString(Globals.Font1, kills.ToString(), Position, Color.White);
            Globals.SpriteBatch.DrawString(Globals.Font1, maxKills.ToString(), Position + new Vector2(0,15), Color.White); 
        }

        public void AddKill()
        {
            kills++;
        }
    }
}
