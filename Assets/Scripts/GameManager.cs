using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject puzzle;
    [SerializeField] private GameObject joystick;
    private PlayerInput input;
    
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    public void UIOnOff()
    {
        puzzle.SetActive(!puzzle.activeSelf);
        joystick.SetActive(!puzzle.activeSelf);
    }
}
