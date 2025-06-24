using UnityEngine;

namespace GJK
{
    public interface IVertexShape
    {
        Vector2 Support(Vector2 direction);
    }
}