using UnityEngine;

namespace _Project.Scripts.Models.EnemyModel.Features
{
    public class EnemyRotation
    {
        public void FacePlayer(Transform enemyTransform, Transform playerTransform)
        {
            float directionX = playerTransform.position.x - enemyTransform.position.x;
            
            if (Mathf.Approximately(directionX, 0f))
                return;

            if (directionX < 0f)
            {
                enemyTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                enemyTransform.localRotation = Quaternion.Euler(0f, -180f, 0f);
            }
        }
    }
}