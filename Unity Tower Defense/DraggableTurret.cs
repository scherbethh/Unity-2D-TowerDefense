using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableTurret : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject turretPrefab;
    private GameObject draggingTurret;
    private Vector3 startPosition;
    private Collider2D draggingTurretCollider;
    private UIShop uiShop;  // UIShop referansýný ekleyin

    private Color validColor = Color.green;
    private Color invalidColor = Color.red;
    private SpriteRenderer draggingTurretSpriteRenderer;

    private void Start()
    {
        uiShop = FindObjectOfType<UIShop>();  // UIShop referansýný bulun
        if (uiShop == null)
        {
            Debug.LogError("UIShop component not found in the scene.");
        }
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameController.coin < 100)
        {
            Debug.Log("Yeterli paranýz yok.");
            return;
        }

        if (draggingTurret == null)
        {
            // Sürüklemeye baþlarken turret prefab'ýný oluþtur
            draggingTurret = Instantiate(turretPrefab, transform.position, Quaternion.identity);
            draggingTurretCollider = draggingTurret.GetComponent<Collider2D>();
            draggingTurretSpriteRenderer = draggingTurret.GetComponent<SpriteRenderer>();

            if (draggingTurretCollider != null)
            {
                draggingTurretCollider.enabled = false; // Sürükleme sýrasýnda collider'ý devre dýþý býrak
            }

            // Ateþ etmeyi durdur
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

        // Sürüklenirken turret'i mouse konumuna taþý
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
        mousePosition.z = 0;
        draggingTurret.transform.position = mousePosition;

        // Yerleþtirmenin geçerli olup olmadýðýný kontrol et ve görsel geri bildirim ver
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

        // Sürükleme bittiðinde, uygun bir yere býrakýlmadýysa turret'i yok et
        if (!IsValidPlacement(draggingTurret.transform.position))
        {
            Destroy(draggingTurret);
        }
        else
        {
            // Yerleþtirme geçerliyse, collider'ý tekrar etkinleþtir
            if (draggingTurretCollider != null)
            {
                draggingTurretCollider.enabled = true;
            }
            draggingTurretSpriteRenderer.color = Color.white; // Rengi eski haline getir

            // Ateþ etmeye baþla

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
        // Yerleþtirme alaný kontrolü
        Collider2D placementCollider = Physics2D.OverlapPoint(position, LayerMask.GetMask("PlacementArea"));
        if (placementCollider == null)
        {
            Debug.Log("Yerleþtirme alanýnda deðil.");
            return false; // Yerleþtirme alanýnda deðil
        }

        // Mevcut kulelerle çakýþma kontrolü
        float checkRadius = 0.5f; // Kontrol yarýçapýný ayarlayýn
        Collider2D[] overlappingColliders = Physics2D.OverlapCircleAll(position, checkRadius, LayerMask.GetMask("Turret"));

        foreach (Collider2D collider in overlappingColliders)
        {
            if (collider != draggingTurretCollider)
            {
                Debug.Log($"Çakýþan Kule: {collider.gameObject.name}");
                return false; // Baþka bir kuleyle çakýþma var
            }
        }

        Debug.Log("Geçerli yerleþtirme.");
        return true; // Geçerli yerleþtirme
    }



    public void OnButtonClicked()
    {
        StartDragging();

        // Menü açýk durumdaysa kapat
        if (uiShop != null && uiShop.MenuOpen)
        {
            Debug.Log("Toggling menu off.");
            uiShop.ToggleMenu();
        }
        else
        {
            Debug.LogError("UIShop component or MenuOpen property is not accessible.");
        }
        // CloseMenu fonksiyonunu çaðýrarak menüyü kapat
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
            Debug.Log("Yeterli paranýz yok.");
            return;
        }

        // Sürükleme iþlemini baþlat
        if (draggingTurret == null)
        {
            draggingTurret = Instantiate(turretPrefab, transform.position, Quaternion.identity);
            draggingTurretCollider = draggingTurret.GetComponent<Collider2D>();
            draggingTurretSpriteRenderer = draggingTurret.GetComponent<SpriteRenderer>();

            if (draggingTurretCollider != null)
            {
                draggingTurretCollider.enabled = false; // Sürükleme sýrasýnda collider'ý devre dýþý býrak
            }
            startPosition = transform.position;

            // Ateþ etmeyi durdur
            Turret turretScript = draggingTurret.GetComponent<Turret>();
           
          
                turretScript.SetPlaced(false);
            
           
        }
    }
}
