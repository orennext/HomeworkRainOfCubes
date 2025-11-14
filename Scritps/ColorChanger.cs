using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public void SelectRandomColor(Cube cubeObject)
    {
        Renderer renderer;

        if (cubeObject.gameObject.TryGetComponent<Renderer>(out renderer))
            renderer.material.color = Random.ColorHSV();
    }
}
