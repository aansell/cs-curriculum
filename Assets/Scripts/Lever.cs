using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour, IInteractable
{
  [SerializeField] private bool allowUnclicking;

  [SerializeField] private Sprite offSprite;
  [SerializeField] private Sprite onSprite;

  public UnityEvent onLeverOn;
  public UnityEvent onLeverOff;

  private SpriteRenderer spriteRenderer;
  private bool isOn = false;

  private void Start()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    spriteRenderer.sprite = offSprite;
  }

  public void OnInteract(GameObject interactor)
  {
    if (!isOn)
    {
      isOn = true;
      spriteRenderer.sprite = onSprite;
      onLeverOn.Invoke();
    }
    else if (allowUnclicking)
    {
      isOn = false;
      spriteRenderer.sprite = offSprite;
      onLeverOff.Invoke();
    }
  }
}