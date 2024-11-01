using UnityEngine;

[CreateAssetMenu(fileName = "Materials", menuName = "VehicleMaterials")]
public class Materials : ScriptableObject
{
    [Header("Red")]
    public Material RedFirstVehicle;
    public Material RedSecondVehicle;
    public Material RedRope;

    [Header("Blue")]
    public Material BlueFirstVehicle;
    public Material BlueSecondVehicle;
    public Material BlueRope;

    [Header("Green")]
    public Material GreenFirstVehicle;
    public Material GreenSecondVehicle;
    public Material GreenRope;

    [Header("Purple")]
    public Material PurpleFirstVehicle;
    public Material PurpleSecondVehicle;
    public Material PurpleRope;
}
