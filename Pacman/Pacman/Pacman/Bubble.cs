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

            if (gameObject is Pacman)
            {
                GameObjectList gameObjectList = (GameObjectList)this.Parent;
                gameObjectList.Remove(this); 
            }
            
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
            drawHelper.Translate(this.Position);
            drawHelper.Translate(1 / 3f, 1 / 3f);
            drawHelper.Scale(1 / 3f, 1 / 3f);
            drawHelper.DrawBox(Color.White);
            drawHelper.Scale(3, 3);
            drawHelper.Translate(-1 / 3f, -1 / 3f);
            drawHelper.Translate(-this.Position);

            Game.DrawManager.Translate(this.Position);
            Game.DrawManager.DrawModel("bubble");
            Game.DrawManager.Translate(-this.Position);
        }
    }
}
