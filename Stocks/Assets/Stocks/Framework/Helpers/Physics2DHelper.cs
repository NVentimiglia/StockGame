using UnityEngine;

namespace Framework
{
    public static class Physics2DHelper
    {
        private static Collider2D[] _collisions = new Collider2D[8];

        public static int CollisionCheck(Vector2 position, LayerMask mask, GameObject ignore = null)
        {
            var hits = Physics2D.OverlapPointNonAlloc(position, _collisions, mask);

            int collisions = 0;

            for (int i = 0; i < hits; i++)
            {
                var col = _collisions[i];
                if (col.gameObject == ignore)
                    continue;
                collisions++;
            }
            return collisions;
        }

        public static int CollisionCheck(Vector2 position, float radius, LayerMask mask, GameObject ignore = null)
        {
            var hits = Physics2D.OverlapCircleNonAlloc(position, radius, _collisions, mask);

            int collisions = 0;

            for (int i = 0; i < hits; i++)
            {
                var col = _collisions[i];
                if (col.gameObject == ignore)
                    continue;
                collisions++;
            }
            return collisions;
        }
    }
}