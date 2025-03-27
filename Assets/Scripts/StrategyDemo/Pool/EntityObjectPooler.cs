using Base.Core;
using StrategyDemo.Entity_NS;
using System.Collections.Generic;

namespace StrategyDemo.Pooling_NS
{
    public class EntityObjectPooler : Singleton<EntityObjectPooler>
    {
        private Queue<BasePlaceableEntityController> _buildingEntites = new ();
        private Queue<BaseUnitEntityController> _unitEntites = new ();

        public BasePlaceableEntityController GetBuilding(BasePlaceableEntityController prefab)
        {
            if (_buildingEntites.Count > 0)
            {
                BasePlaceableEntityController placeable = _buildingEntites.Dequeue();
                placeable.gameObject.SetActive(true);
                return placeable;
            }
            return Instantiate(prefab);
        }

        public void ReturnBuilding(BasePlaceableEntityController controller)
        {
            controller.gameObject.SetActive(false);
            _buildingEntites.Enqueue(controller);
        }

        public BaseUnitEntityController GetUnit(BaseUnitEntityController prefab)
        {
            if (_unitEntites.Count > 0)
            {
                BaseUnitEntityController placeable = _unitEntites.Dequeue();
                placeable.gameObject.SetActive(true);
                return placeable;
            }
            return Instantiate(prefab);
        }

        public void ReturnUnit(BaseUnitEntityController controller)
        {
            controller.gameObject.SetActive(false);
            _unitEntites.Enqueue(controller);
        }
    }
}