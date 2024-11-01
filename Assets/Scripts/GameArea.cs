using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GameArea : MonoBehaviour
{
    public static GameArea Instance { get; private set; }
    public BoxCollider GameAreaBox => _gameAreaBox;

    BoxCollider _gameAreaBox;

    private void Awake()
    {
        Instance = this;
        _gameAreaBox = GetComponent<BoxCollider>();
    }
}
