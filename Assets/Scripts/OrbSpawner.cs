using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [SerializeField] Orb orbPrefab;
    [SerializeField] RectTransform area;
    [SerializeField] float interval = 0.8f;
    [Range(0f, 1f)] [SerializeField] float badChance = 0.3f;
    float timer;

    void Update()
    {
        if (!GameController.Instance.Running) return;
        timer -= Time.deltaTime;
        if (timer > 0f) return;
        timer = interval;
        Orb orb = Instantiate(orbPrefab, area);
        Rect r = area.rect;
        ((RectTransform)orb.transform).anchoredPosition = new Vector2(
            Random.Range(r.xMin + 100f, r.xMax - 100f),
            Random.Range(r.yMin + 100f, r.yMax - 100f));
        orb.Setup(Random.value < badChance);
    }
}