using System.Collections.Generic;
using UnityEngine;


namespace CastleInvasion
{
    public class BatteringRam : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private List<BatteringRamRow> rows;
        public List<BatteringRamRow> Rows => rows;
    }
}
