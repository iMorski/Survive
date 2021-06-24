using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    public static PlayerContainer Instance;
    
    public Joystick Joystick;

    private void Awake()
    {
        Instance = this;
    }
}
