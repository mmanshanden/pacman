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

        public override void Load()
        {
            ModelBuilder modelBuilder = Game.DrawManager.ModelLibrary.BeginModel();

            modelBuilder.PrimitiveBatch.Scale(Vector3.One * 0.4f);

            modelBuilder.BuildFromTexture("voxels/powerup", 5);
            Game.DrawManager.ModelLibrary.EndModel("powerup");

        }

        public override void Draw(DrawHelper drawHelper)
        {
            /*drawHelper.Translate(this.Position);
            drawHelper.Translate(1 / 4f, 1 / 4f);
            drawHelper.Scale(1 / 2f, 1 / 2f);
            drawHelper.DrawBox(Color.Green);
            drawHelper.Scale(2, 2);
            drawHelper.Translate(-1 / 4f, -1 / 4f);
            drawHelper.Translate(-this.Position);*/

            Game.DrawManager.Translate(this.Position);
            Game.DrawManager.DrawModel("powerup");
            Game.DrawManager.Translate(-this.Position);
        }
    }
}
