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


        public static void Load(ModelLibrary modelLibrary)
        {
            ModelBuilder mb = modelLibrary.BeginModel();

            mb.PrimitiveBatch.Translate(Vector3.One * 0.5f);
            mb.PrimitiveBatch.Scale(Vector3.One * 0.15f);
            mb.PrimitiveBatch.Translate(Vector3.One * -0.5f);
            mb.PrimitiveBatch.DrawCube();

            modelLibrary.EndModel("bubble");
        }
        
        public override void Draw(DrawManager drawManager)
        {
            drawManager.Translate(this.Position);
            drawManager.DrawModel("bubble");
            drawManager.Translate(-this.Position);
        }
    }
}
