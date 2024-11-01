using UnityEngine;

[Tooltip("This class must be assigned to the vehicle towing the trailer")]
public class VehicleDistanceController : MonoBehaviour
{
    [Tooltip("Max Distance between vehicle & trailer")]
    [SerializeField] float _maxDistance;

    [SerializeField] SecondVehicle _trailer;

    private void Update()
    {
        ChangeTrailerPosition();
    }

    private void ChangeTrailerPosition()
    {
        float distance = Vector3.Distance(transform.position, _trailer.transform.position);

        if (distance > _maxDistance)
        {
            Vector3 direction = (_trailer.transform.position - transform.position).normalized;
            _trailer.transform.position = transform.position + direction * _maxDistance;

            #region Rotation Setting
            //if (direction != Vector3.zero)
            //{
            //    _trailer.transform.forward = direction;
            //    _trailer.transform.eulerAngles += new Vector3(0, 180, 0);
            //}
            #endregion
        }
    }
}
