using UnityEngine;
using UnityEngine.Assertions;

namespace KartGame.KartSystems 
{
    public class KartPlayerAnimator : MonoBehaviour
    {
        public ArcadeKart Kart;

        public string SteeringParam = "Steering";
        public string GroundedParam = "Grounded";

        int m_SteerHash, m_GroundHash;

        float steeringSmoother;

        void Awake()
        {
            Assert.IsNotNull(Kart, "No ArcadeKart found!");
        }

        void Update()
        {
            steeringSmoother = Mathf.Lerp(steeringSmoother, Kart.Input.TurnInput, Time.deltaTime * 5f);
        }
    }
}
