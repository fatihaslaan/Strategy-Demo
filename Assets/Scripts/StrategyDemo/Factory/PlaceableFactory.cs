using UnityEngine;
using StrategyDemo.Entity_NS;
using Base.Pooling_NS;
using StrategyDemo.Pooling_NS;

public class PlaceableFactory : MonoBehaviour
{
    public BasePlaceableEntityController GetPlaceableEntity(SO_BasePlaceableEntityData data)
    {
        if (data.EntityPrefab is BaseUnitEntityController && data is SO_BaseUnitEntityData)
        {
            BaseUnitEntityController unit = EntityObjectPooler.Instance.GetUnit((BaseUnitEntityController)data.EntityPrefab);
            unit.SetEntity(data as SO_BaseUnitEntityData);
            return unit;
        }

        BasePlaceableEntityController placeableEntity = EntityObjectPooler.Instance.GetBuilding(data.EntityPrefab);
        placeableEntity.SetEntity(data);
        return placeableEntity;
    }
}
