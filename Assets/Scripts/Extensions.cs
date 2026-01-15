using UnityEngine;

//This class is used to do raycasts from one object to another, to tell whether they are touching
public static class Extensions
{
    //Only cast on default layer objects, not player and background ones
    private static LayerMask layerMask = LayerMask.GetMask("Default");
    public static bool Circlecast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        //If rigidbody is not being controlled by physics engine - is stationary (sort of)
        if(rigidbody.isKinematic)
        {
            return false;
        }
#warning This is hard coded, would be better off to find some way of getting collider size 
        float radius = 0.25f;
        float distance = 0.375f;

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, layerMask);
        //Check if we really hit anything and it is not the object we cast from
        return hit.collider != null && hit.rigidbody != rigidbody;
    }

    //If the scalar product of two unit length vectors is greater than let's say sqrt(2)/2, that means
    //that the two vector are at a less than 45 degree angle from each other
    //
    //We use this to for example determine if Mario is hitting a block from below with his head
    public static bool IsScalarProductGreaterThanNumber(this Transform transform, Transform other, Vector2 testDirection, float number)
    {
        Vector2 direction = other.position - transform.position;
        return Vector2.Dot(direction.normalized, testDirection) > number;
    }
}
