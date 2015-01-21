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

        public override void Collision_GameObject(GameObject gameObject)
        {
            base.Collision_GameObject(gameObject);

            if (gameObject is Pacman)
            {
                GameObjectList gameObjectList = (GameObjectList)this.Parent;
                gameObjectList.Remove(this);

                Level level = (Level)this.Parent;
                level.GhostHouse.Blinky.Frighten();
                level.GhostHouse.Pinky.Frighten();
                level.GhostHouse.Clyde.Frighten();
                level.GhostHouse.Inky.Frighten(); 

            }

        }

        public static void Load(ModelLibrary modelLibrary)
        {
            ModelBuilder modelBuilder = modelLibrary.BeginModel();

            modelBuilder.PrimitiveBatch.Translate(Vector3.One * 0.5f);
            modelBuilder.PrimitiveBatch.Scale(Vector3.One * 0.5f);
            modelBuilder.PrimitiveBatch.Translate(Vector3.One * -0.5f);

            modelBuilder.BuildFromTexture("voxels/powerup", 5);
            modelLibrary.EndModel("powerup");

        }

        public override void Draw(DrawManager drawManager)
        {
            /*drawHelper.Translate(this.Position);
            drawHelper.Translate(1 / 4f, 1 / 4f);
            drawHelper.Scale(1 / 2f, 1 / 2f);
            drawHelper.DrawBox(Color.Green);
            drawHelper.Scale(2, 2);
            drawHelper.Translate(-1 / 4f, -1 / 4f);
            drawHelper.Translate(-this.Position);*/

            drawManager.Translate(this.Position);
            drawManager.DrawModel("powerup");
            drawManager.Translate(-this.Position);
        }
    }
}
