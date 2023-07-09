using GameManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class GameManager
    {
        private readonly Map1 _map;
        private readonly Hero _hero;

        public GameManager()
        {
            _map = new Map1();
            _hero = new Hero(Glob.Content.Load<Texture2D>("hero"), 
                new Vector2(Glob.WindowSize.X / 2, 200));
        }

        public void Update()
        {
            _hero.Update();
        }

        public void Draw()
        {
            Glob.SpriteBatch.Begin();
            _map.Draw();
            _hero.Draw();
            Glob.SpriteBatch.End();
        }
    }
}
