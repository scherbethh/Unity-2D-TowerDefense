using UnityEngine;

public class Turret3 : MonoBehaviour
{
    [Header("References")]
    

    [Header("Attribute")]
    [SerializeField] private float rotationSpeed = 180f;

    private Color clickedColor = Color.blue; // Turret'e t�klan�ld���nda de�i�ecek renk
    private Color originalColor = Color.white; // Orijinal renk

    private bool isPlaced = false; // Turret'in yerle�tirilip yerle�tirilmedi�ini kontrol eder
    private bool isClicked = false; // Turrete t�kland���n� kontrol eder

    private Quaternion targetRotation;

    // D��man layer'�n� tan�mlay�n
   

    

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (isPlaced && Input.GetMouseButtonDown(0))
        {
            // Fare pozisyonunda bir raycast at

            // E�er raycast turrete �arpt�ysa ve t�klama tu�una bas�ld�ysa
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Turret'e t�klan�ld���nda
                if (!isClicked)
                {
                    // Turret belirgin hale gelsin
                    GetComponent<Renderer>().material.color = clickedColor; // Turret'in rengini de�i�tir
                    isClicked = true;
                }
                else
                {
                    // Turret belirginli�i iptal edilsin
                    GetComponent<Renderer>().material.color = originalColor; // Turret'in rengini eski haline getir
                    isClicked = false;
                }
            }
        }

        // E�er turret belirgin hale getirildiyse ve fare pozisyonunda bir t�klama yap�ld�ysa
        if (isClicked && isPlaced && Input.GetMouseButtonDown(0)
            && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()
            && hit.collider.gameObject.layer != LayerMask.NameToLayer("Turret"))
        {
            // Fare pozisyonunu turret'in konumuna d�n��t�r
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = transform.position.z; // Turret'in z pozisyonunu koru (2D oyun i�in)

            // Turret'in hedefine do�ru d�nme i�lemi
            Vector2 direction = targetPosition - transform.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            targetRotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);
        }

        // Turret d�nme i�lemi
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

       
    }

    

    public void SetPlaced(bool placed)
    {
        isPlaced = placed;
    }
}
