using UnityEngine;

public class SlimeEffectController : MonoBehaviour
{
    [SerializeField] public GameObject LandEffectPrefab;
    [SerializeField] public GameObject currentEffect;
    [SerializeField] public Vector2 offset = new Vector2(-1f, -3f);

    public void LandEffect()
    {
        Vector2 pos = transform.position;
        pos += offset;
        Instantiate(LandEffectPrefab, pos, Quaternion.identity);
    }
}
