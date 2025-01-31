using TMPro;
using UnityEngine;

public class DistanceText : MonoBehaviour
{
    [SerializeField] ARRuler ruler;

    private TMP_Text txtDistance;

    private void Awake()
    {
        txtDistance = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (ruler.Distance == 0f)
        {
            txtDistance.color = Color.red;
            txtDistance.text = "CAN NOT MEASURE";
        }
        else
        {
            txtDistance.color = Color.green;
            txtDistance.text = $"{ruler.Distance:N2}mm";
        }
    }
}
