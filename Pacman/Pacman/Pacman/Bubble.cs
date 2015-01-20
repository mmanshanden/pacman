using _3dgl;
using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Bubble : GameObject
    {
        public Bubble()
        {

        }

        public override void Collision_GameObject(GameObject gameObject)
        {
            base.Collision_GameObject(gameObject);

            Pacman pacman = gameObject as Pacman;
            if (pacman == null)
                return; 

            GameObjectList gameObjectList = (GameObjectList)this.Parent;
            gameObjectList.Remove(this);
        }


        public override void Load()
        {
            ModelBuilder mb = Game.DrawManager.ModelLibrary.BeginModel();

            mb.PrimitiveBatch.Translate(Vector3.One * 0.5f);
            mb.PrimitiveBatch.Scale(Vector3.One * 0.15f);
            mb.PrimitiveBatch.Translate(Vector3.One * -0.5f);
            mb.PrimitiveBatch.DrawCube();

            Game.DrawManager.ModelLibrary.EndModel("bubble");
        }
        
        public override void Draw(DrawHelper drawHelper)
        {
            Game.DrawManager.Translate(this.Position);
            Game.DrawManager.DrawModel("bubble");
            Game.DrawManager.Translate(-this.Position);
        }
    }
}
