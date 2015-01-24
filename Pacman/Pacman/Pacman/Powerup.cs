using _3dgl;
using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Powerup : GameObject
    {
        public Powerup()
        {

        }

        //If PowerUp is in collision with Pacman we need to remove it from the map
        public override void Collision_GameObject(GameObject gameObject)
        {
            base.Collision_GameObject(gameObject);

            if (gameObject is Pacman)
            {
                GameObjectList gameObjectList = (GameObjectList)this.Parent;
                gameObjectList.Remove(this);
            }

        }

        // Load the 3D Model of the powerup
        public static void Load(ModelLibrary modelLibrary)
        {
            ModelBuilder modelBuilder = modelLibrary.BeginModel();

            modelBuilder.PrimitiveBatch.Translate(Vector3.One * 0.5f);
            modelBuilder.PrimitiveBatch.Scale(Vector3.One * 0.5f);
            modelBuilder.PrimitiveBatch.Translate(Vector3.One * -0.5f);

            modelBuilder.BuildFromTexture("voxels/powerup", 5);
            modelLibrary.EndModel("powerup");

        }

        // Draw the 3D Model of the Powerup
        public override void Draw(DrawManager drawManager)
        {
            drawManager.Translate(this.Position);
            drawManager.DrawModel("powerup");
            drawManager.Translate(-this.Position);
        }
    }
}
