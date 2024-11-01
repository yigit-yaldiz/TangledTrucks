using BaseAssets;
using UnityEngine;

public enum GroupColor
{
    Red,
    Blue,
    Green,
    Purple
}

public enum MaterialType
{
    FirstVehicle,
    SecondVehicle,
    Rope
}

[Tooltip("This class must be assigned to each VehicleGroup")]
public class GroupCategorizer : MonoBehaviour
{
    [SerializeField] Materials _materials;

    [OnValueChanged("ChangeGroupColor")]
    [SerializeField] GroupColor _color;
    
    FirstVehicleMaterialHandler[] _firstVehicleMaterials;
    SecondVehicleMaterialHandler[] _secondVehicleMaterials;
    RopeMaterialHandler[] _ropeMaterials;

    Material SelectTheMaterial(MaterialType type)
    {
        Material conclusion = null;

        switch (_color)
        {
            case GroupColor.Red:
                if (type == MaterialType.FirstVehicle)
                {
                    conclusion = _materials.RedFirstVehicle;
                }
                else if (type == MaterialType.SecondVehicle)
                {
                    conclusion = _materials.RedSecondVehicle;
                }
                else if (type == MaterialType.Rope)
                {
                    conclusion = _materials.RedRope;
                }

                ChangeGroupName(GroupColor.Red);
                break;

            case GroupColor.Blue:
                if (type == MaterialType.FirstVehicle)
                {
                    conclusion = _materials.BlueFirstVehicle;
                }
                else if (type == MaterialType.SecondVehicle)
                {
                    conclusion = _materials.BlueSecondVehicle;
                }
                else if (type == MaterialType.Rope)
                {
                    conclusion = _materials.BlueRope;
                }

                ChangeGroupName(GroupColor.Blue);
                break;

            case GroupColor.Green:
                if (type == MaterialType.FirstVehicle)
                {
                    conclusion = _materials.GreenFirstVehicle;
                }
                else if (type == MaterialType.SecondVehicle)
                {
                    conclusion = _materials.GreenSecondVehicle;
                }
                else if (type == MaterialType.Rope)
                {
                    conclusion = _materials.GreenRope;
                }

                ChangeGroupName(GroupColor.Green);
                break;

            case GroupColor.Purple:
                if (type == MaterialType.FirstVehicle)
                {
                    conclusion = _materials.PurpleFirstVehicle;
                }
                else if (type == MaterialType.SecondVehicle)
                {
                    conclusion = _materials.PurpleSecondVehicle;
                }
                else if (type == MaterialType.Rope)
                {
                    conclusion = _materials.PurpleRope;
                }

                ChangeGroupName(GroupColor.Purple);
                break;

            default:
                Debug.LogWarning("Check color you selected");
                break;
        }

        return conclusion;
    }

    void ChangeGroupColor()
    {
        _firstVehicleMaterials = GetComponentsInChildren<FirstVehicleMaterialHandler>();
        _secondVehicleMaterials = GetComponentsInChildren<SecondVehicleMaterialHandler>();
        _ropeMaterials = GetComponentsInChildren<RopeMaterialHandler>();

        foreach (FirstVehicleMaterialHandler vehicle in _firstVehicleMaterials)
        {
            vehicle.ChangeMaterial(SelectTheMaterial(MaterialType.FirstVehicle));
        }

        foreach (SecondVehicleMaterialHandler vehicle in _secondVehicleMaterials)
        {
            vehicle.ChangeMaterial(SelectTheMaterial(MaterialType.SecondVehicle));
        }

        foreach (RopeMaterialHandler rope in _ropeMaterials)
        {
            rope.ChangeMaterial(SelectTheMaterial(MaterialType.Rope));
        }
    }

    void ChangeGroupName(GroupColor color)
    {
        string newGroupName = $"{color} Group";

        int sameCount = 0;

        foreach (Transform child in transform.parent)
        {
            if (child != transform && newGroupName == child.name)
            {
                sameCount++;
            }
        }

        if (sameCount > 0)
        {
            newGroupName = $"{color} Group ({sameCount})";
        }

        transform.name = newGroupName;
    }
}


