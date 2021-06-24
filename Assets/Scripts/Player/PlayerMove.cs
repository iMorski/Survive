public class PlayerMove : CharacterMove
{
    private Joystick Joystick;

    private void Start()
    {
        Joystick = PlayerContainer.Instance.Joystick;
    }
    
    private void FixedUpdate() { ChangePosition(Joystick.Direction); }
}
