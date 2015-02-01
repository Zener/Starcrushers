///////////////////////////////////////////////////////////////////////////////////////////////
// Starcrushers
// Programmed by Carlos Peris
// Barcelona, March-June 2007
///////////////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace ShipsWar
{
    class Entity
    {
        public float yaw;
        protected Vector3 position;
        public int side;
        public Universe universe;
        public int State;

        public Entity()
        {
        }


        public Entity(Universe _universe)
        {
            universe = _universe;
        }

        
        public void setPosition(Vector3 _pos)
        {
            position.X = _pos.X;
            position.Y = _pos.Y;
            position.Z = _pos.Z;
        }


        public Vector3 getPosition()
        {
            return position;
        }


        public float distance(Entity other)
        {
            return Vector3.Distance(position, other.getPosition());
        }

        

        public void CalculateYaw(Vector3 ori, Vector3 dest)
        {
            float x1 = ori.X;
            float x2 = dest.X;
            float y1 = ori.Y;
            float y2 = dest.Y;


            float dx = x2 - x1;
            float dy = y2 - y1;
            double angle = 0.0d;

            // Calculate angle
            if (dx == 0.0)
            {
                if (dy == 0.0)
                    angle = 0.0;
                else if (dy > 0.0)
                    angle = Math.PI / 2.0;
                else
                    angle = Math.PI * 3.0 / 2.0;
            }
            else if (dy == 0.0)
            {
                if (dx > 0.0)
                    angle = 0.0f;
                else
                    angle = (float)Math.PI;
            }
            else
            {
                if (dx < 0.0)
                    angle = (float)Math.Atan(dy / dx) + Math.PI;
                else if (dy < 0.0)
                    angle = (float)Math.Atan(dy / dx) + (2 * Math.PI);
                else
                    angle = (float)Math.Atan(dy / dx);
            }

            // Convert to degrees
            //angle = angle * 180 / Math.PI;
            /*float dot = Vector3.Dot(new Vector3(), d);
            float deno = (ori.Length() * dest.Length());
            float angle = (float)Math.Acos(dot / deno);*/
            yaw = (float)angle;
        }
    }
}
