using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableTurret : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject turretPrefab;
    private GameObject draggingTurret;
    private Vector3 startPosition;
    private Collider2D draggingTurretCollider;
    private UIShop uiShop;  // UIShop referans�n� ekleyin

    private Color validColor = Color.green;
    private Color invalidColor = Color.red;
    private SpriteRenderer draggingTurretSpriteRenderer;

    private void Start()
    {
        uiShop = FindObjectOfType<UIShop>();  // UIShop referans�n� bulun
        if (uiShop == null)
        {
            Debug.LogError("UIShop component not found in the scene.");
        }
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameController.coin < 100)
        {
            Debug.Log("Yeterli paran�z yok.");
            return;
        }

        if (draggingTurret == null)
        {
            // S�r�klemeye ba�larken turret prefab'�n� olu�tur
            draggingTurret = Instantiate(turretPrefab, transform.position, Quaternion.identity);
            draggingTurretCollider = draggingTurret.GetComponent<Collider2D>();
            draggingTurretSpriteRenderer = draggingTurret.GetComponent<SpriteRenderer>();

            if (draggingTurretCollider != null)
            {
                draggingTurretCollider.enabled = false; // S�r�kleme s�ras�nda collider'� devre d��� b�rak
            }

            // Ate� etmeyi durdur
            Turret turretScript = draggingTurret.GetComponent<Turret>();
            Turret2 turret2Script = draggingTurret.GetComponent<Turret2>();
            Turret3 turret3Script = draggingTurret.GetComponent<Turret3>();

            if (turretScript != null)
            {
                turretScript.SetPlaced(false);
            }

            if (turret2Script != null)
            {
                turret2Script.SetPlaced(false);
            }

            if (turret3Script != null)
            {
                turret3Script.SetPlaced(false);
            }



        }
        startPosition = transform.position;
        uiShop.CloseMenu();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingTurret == null)
            return;

        // S�r�klenirken turret'i mouse konumuna ta��
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
        mousePosition.z = 0;
        draggingTurret.transform.position = mousePosition;

        // Yerle�tirmenin ge�erli olup olmad���n� kontrol et ve g�rsel geri bildirim ver
        if (IsValidPlacement(mousePosition))
        {
            draggingTurretSpriteRenderer.color = validColor;
        }
        else
        {
            draggingTurretSpriteRenderer.color = invalidColor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingTurret == null)
            return;

        // S�r�kleme bitti�inde, uygun bir yere b�rak�lmad�ysa turret'i yok et
        if (!IsValidPlacement(draggingTurret.transform.position))
        {
            Destroy(draggingTurret);
        }
        else
        {
            // Yerle�tirme ge�erliyse, collider'� tekrar etkinle�tir
            if (draggingTurretCollider != null)
            {
                draggingTurretCollider.enabled = true;
            }
            draggingTurretSpriteRenderer.color = Color.white; // Rengi eski haline getir

            // Ate� etmeye ba�la

            Turret turretScript = draggingTurret.GetComponent<Turret>();
            Turret2 turret2Script = draggingTurret.GetComponent<Turret2>();
            Turret3 turret3Script = draggingTurret.GetComponent<Turret3>();

            if (turretScript != null)
            {
                turretScript.SetPlaced(true);            
            }

            if (turret2Script != null)
            {
                turret2Script.SetPlaced(true);
            }

            if (turret3Script != null)
            {
                turret3Script.SetPlaced(true);
            }



            // 50 coin azalt
            GameController.coin -= 100;
        }
        draggingTurret = null;
        uiShop.OpenMenu();
    }

    private bool IsValidPlacement(Vector3 position)
    {
        // Yerle�tirme alan� kontrol�
        Collider2D placementCollider = Physics2D.OverlapPoint(position, LayerMask.GetMask("PlacementArea"));
        if (placementCollider == null)
        {
            Debug.Log("Yerle�tirme alan�nda de�il.");
            return false; // Yerle�tirme alan�nda de�il
        }

        // Mevcut kulelerle �ak��ma kontrol�
        float checkRadius = 0.5f; // Kontrol yar��ap�n� ayarlay�n
        Collider2D[] overlappingColliders = Physics2D.OverlapCircleAll(position, checkRadius, LayerMask.GetMask("Turret"));

        foreach (Collider2D collider in overlappingColliders)
        {
            if (collider != draggingTurretCollider)
            {
                Debug.Log($"�ak��an Kule: {collider.gameObject.name}");
                return false; // Ba�ka bir kuleyle �ak��ma var
            }
        }

        Debug.Log("Ge�erli yerle�tirme.");
        return true; // Ge�erli yerle�tirme
    }



    public void OnButtonClicked()
    {
        StartDragging();

        // Men� a��k durumdaysa kapat
        if (uiShop != null && uiShop.MenuOpen)
        {
            Debug.Log("Toggling menu off.");
            uiShop.ToggleMenu();
        }
        else
        {
            Debug.LogError("UIShop component or MenuOpen property is not accessible.");
        }
        // CloseMenu fonksiyonunu �a��rarak men�y� kapat
        if (uiShop != null)
        {
            uiShop.CloseMenu();
        }
        else
        {
            Debug.LogError("UIShop component is not accessible.");
        }
    }

    private void StartDragging()
    {
        if (GameController.coin < 100)
        {
            Debug.Log("Yeterli paran�z yok.");
            return;
        }

        // S�r�kleme i�lemini ba�lat
        if (draggingTurret == null)
        {
            draggingTurret = Instantiate(turretPrefab, transform.position, Quaternion.identity);
            draggingTurretCollider = draggingTurret.GetComponent<Collider2D>();
            draggingTurretSpriteRenderer = draggingTurret.GetComponent<SpriteRenderer>();

            if (draggingTurretCollider != null)
            {
                draggingTurretCollider.enabled = false; // S�r�kleme s�ras�nda collider'� devre d��� b�rak
            }
            startPosition = transform.position;

            // Ate� etmeyi durdur
            Turret turretScript = draggingTurret.GetComponent<Turret>();
           
          
                turretScript.SetPlaced(false);
            
           
        }
    }
}
