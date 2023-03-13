using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorMathUtils
{
    /// <summary>
    /// calculate two vector's intersection
    /// </summary>
    /// <param name="lineAstart"></param>
    /// <param name="lineAdir"></param>
    /// <param name="lineBstart"></param>
    /// <param name="lineBdir"></param>
    /// <returns></returns>
    public static Vector3 GetIntersection(Vector3 lineAstart, Vector3 lineAdir, Vector3 lineBstart, Vector3 lineBdir)
    {
        //check if at the same plane
        Vector3 N = Vector3.Cross((lineAstart - lineBstart), (lineBstart + lineBdir));
        if(Mathf.Abs(Vector3.Dot(N,(lineAstart + lineAdir))) > 1e-6)
        {
            Debug.Log("not at the same plane");
            return Vector3.zero;
        }
        float Ax = lineAstart.x, Ay = lineAstart.y, Az = lineAstart.z;
        float Adx = lineAdir.x, Ady = lineAdir.y, Adz = lineAdir.z;
        float Bx = lineBstart.x, By = lineBstart.y, Bz = lineBstart.z;
        float Bdx = lineBdir.x, Bdy = lineBdir.y, Bdz = lineBdir.z;
       
        float Aratio = Ady / Adx;


        float t = (Aratio * Ax - (Ay + Aratio * Bx) + By) / (Aratio * Bdx + Bdy);
        return lineBstart + t * lineBdir;
    }
}
