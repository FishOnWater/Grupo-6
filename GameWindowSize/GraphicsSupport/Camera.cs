using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    static public class Camera
    {
        static private Vector2 sOrigin = Vector2.Zero; // Origin of the world
        static private float sWidth = 100f; // Width of the world
        static private float sRatio = -1f; // Ratio between camera window and pixel
        private static float sHeight;


        //internal static void SetCameraWindow(Vector2 vector2, float v)
        //{
        //    throw new NotImplementedException();
        //}

        //internal static void ComputePixelPosition(Vector2 pos, out int pixelX, out int pixelY)
        //{
        //    throw new NotImplementedException();
        //}

        internal static Rectangle ComputePixelRectangle(object position, object size)
        {
            throw new NotImplementedException();
        }

        static private float cameraWindowToPixelRatio()
        {
            if (sRatio < 0f)
                sRatio = (float)Game1.sGraphics.PreferredBackBufferWidth / sWidth;
            return sRatio;
        }

        static public void SetCameraWindow(Vector2 origin, float width)
        {
            sOrigin = origin;
            sWidth = width;
        }


        static public void ComputePixelPosition(Vector2 cameraPosition, out int x, out int y)
        {
            float ratio = cameraWindowToPixelRatio();

            // Convert the position to pixel space
            x = (int)(((cameraPosition.X - sOrigin.X) * ratio) + 0.5f);
            y = (int)(((cameraPosition.Y - sOrigin.Y) * ratio) + 0.5f);

            y = Game1.sGraphics.PreferredBackBufferHeight - y;
        }


        static public Rectangle ComputePixelRectangle(Vector2 position, Vector2 size)
        {
            float ratio = cameraWindowToPixelRatio();

            // Convert size from camera window space to pixel space.
            int width = (int)((size.X * ratio) + 0.5f);
            int height = (int)((size.Y * ratio) + 0.5f);

            // Convert the position to pixel space
            int x, y;
            ComputePixelPosition(position, out x, out y);

            // Reference position is the center
            y -= height / 2;
            x -= width / 2;

            return new Rectangle(x, y, width, height);
        }

        internal static Rectangle ComputePixelRectangle(string position, object size)
        {
            throw new NotImplementedException();
        }

        /// Accessors to the camera window bounds
        static public Vector2 CameraWindowLowerLeftPosition
        { get { return sOrigin; } }
        static public Vector2 CameraWindowUpperRightPosition
        { get { return sOrigin + new Vector2(sWidth, sHeight); } }

        // Support collision with the camera bounds
        public enum CameraWindowCollisionStatus
        {
            CollideTop = 0,
            CollideBottom = 1,
            CollideLeft = 2,
            CollideRight = 3,
            InsideWindow = 4
        };

        static public CameraWindowCollisionStatus CollidedWithCameraWindow(TexturedPrimitive.TexturedPrimitive prim)
        {
            Vector2 min = CameraWindowLowerLeftPosition;
            Vector2 max = CameraWindowUpperRightPosition;
            if (prim.MaxBound.Y > max.Y)
                return CameraWindowCollisionStatus.CollideTop;
            if (prim.MinBound.X < min.X)
                return CameraWindowCollisionStatus.CollideLeft;
            if (prim.MaxBound.X > max.X)
                return CameraWindowCollisionStatus.CollideRight;
            if (prim.MinBound.Y < min.Y)
                return CameraWindowCollisionStatus.CollideBottom;
            return CameraWindowCollisionStatus.InsideWindow;
        }


    }
}
