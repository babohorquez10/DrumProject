using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Velocidad : MonoBehaviour
{
    private bool movingDown;
    private Hand scrActual;
    private Vector3 velocidadBaqueta = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        movingDown = false;      
    }

    void Update()
    {
        scrActual = GetComponent<Interactable>().attachedToHand;

        if (scrActual != null)
        {
            velocidadBaqueta = scrActual.GetTrackedObjectVelocity();
            movingDown = velocidadBaqueta.y < 0;
        }

    }

    public bool isMovingDown()
    {
        return movingDown;
    }

    
}
