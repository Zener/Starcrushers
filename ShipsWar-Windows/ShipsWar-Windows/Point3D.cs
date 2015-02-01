///////////////////////////////////////////////////////////////////////////////////////////////
// Starcrushers
// Programmed by Carlos Peris
// Barcelona, March-June 2007
///////////////////////////////////////////////////////////////////////////////////////////////


using System;

namespace ZFramework
{
	/// <summary>
	/// Descripción breve de Point3D.
	/// </summary>
	public class Point3D
	{
		public float x, y, z;
			

		public Point3D()
		{
			x = 0;
			y = 0;
			z = 0;			
		}

		public Point3D(float _x, float _y, float _z)
		{
			x = _x;
			y = _y;
			z = _z;
		}


		public float dist(Point3D p1, Point3D p2)
		{
			float dx = p1.x - p2.x, dy = p1.y - p2.y, dz = p1.z - p2.z;
			return (float)Math.Sqrt((dx*dx) + (dy*dy) + (dz*dz));
	    }	
	}
}
