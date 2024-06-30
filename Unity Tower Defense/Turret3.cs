using UnityEngine;

public class Turret3 : MonoBehaviour
{
    [Header("References")]
    

    [Header("Attribute")]
    [SerializeField] private float rotationSpeed = 180f;

    private Color clickedColor = Color.blue; // Turret'e týklanýldýðýnda deðiþecek renk
    private Color originalColor = Color.white; // Orijinal renk

    private bool isPlaced = false; // Turret'in yerleþtirilip yerleþtirilmediðini kontrol eder
    private bool isClicked = false; // Turrete týklandýðýný kontrol eder

    private Quaternion targetRotation;

    // Düþman layer'ýný tanýmlayýn
   

    

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (isPlaced && Input.GetMouseButtonDown(0))
        {
            // Fare pozisyonunda bir raycast at

            // Eðer raycast turrete çarptýysa ve týklama tuþuna basýldýysa
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Turret'e týklanýldýðýnda
                if (!isClicked)
                {
                    // Turret belirgin hale gelsin
                    GetComponent<Renderer>().material.color = clickedColor; // Turret'in rengini deðiþtir
                    isClicked = true;
                }
                else
                {
                    // Turret belirginliði iptal edilsin
                    GetComponent<Renderer>().material.color = originalColor; // Turret'in rengini eski haline getir
                    isClicked = false;
                }
            }
        }

        // Eðer turret belirgin hale getirildiyse ve fare pozisyonunda bir týklama yapýldýysa
        if (isClicked && isPlaced && Input.GetMouseButtonDown(0)
            && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()
            && hit.collider.gameObject.layer != LayerMask.NameToLayer("Turret"))
        {
            // Fare pozisyonunu turret'in konumuna dönüþtür
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = transform.position.z; // Turret'in z pozisyonunu koru (2D oyun için)

            // Turret'in hedefine doðru dönme iþlemi
            Vector2 direction = targetPosition - transform.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            targetRotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);
        }

        // Turret dönme iþlemi
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

       
    }

    

    public void SetPlaced(bool placed)
    {
        isPlaced = placed;
    }
}
