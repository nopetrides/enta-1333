using UnityEngine;

namespace Lecture_6
{
    public class DamageSystem : MonoBehaviour
    {
        // Particle System
        // UI Manager
        // Combat log

        /// <summary>
        ///     Effects within a cell, dealing damage to units within the cell
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="hpChange"></param>
        public void ChangeHp(GridCell source, CellUnit target, int hpChange)
        {
            
            // update the units health
            target.ChangeHealth(hpChange);
            // todo update the hp bar
            // todo update the unit animation state to "flinch" if not already flinched
            
            // todo Log the damage in the combat log
        }

        /// <summary>
        ///     Unit dealing damage to a structure
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="hpChange"></param>
        public void ChangeHp(CellUnit source, PlacedBuildingBase target, int hpChange)
        {
            
        }

        /// <summary>
        ///     Unit dealing damage to another unit directly
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="hpChange"></param>
        public void ChangeHp(CellUnit source, CellUnit target, int hpChange)
        {
            
        }
        
    }
}
