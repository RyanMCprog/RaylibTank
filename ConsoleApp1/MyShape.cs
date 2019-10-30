using System;
using System.Collections.Generic;
using System.Text;
using Raylib;
namespace ConsoleApp1
{
    class MyShape
    {
        
        public Vector3 position = new Vector3();
        public List<Vector3> MyPoints = new List<Vector3>();
        public AABB myABb = new AABB();

        public void Draw(Color Ball)
        {
            Vector3 Last = new Vector3(); 
            for(int idx = 0; idx < MyPoints.Count; idx++)
            {
                if(idx>0)
                Raylib.Raylib.DrawLineEx(new Raylib.Vector2(
                                                              ( position + Last).x,
                                                              ( position + Last).y
                                                           ) ,
                                        new Raylib.Vector2(
                                                          (position + MyPoints[idx]).x,
                                                          (position + MyPoints[idx]).y
                                                          ),
                    2, Ball);


                Last = MyPoints[idx];
            }
        }
    }

    
}
