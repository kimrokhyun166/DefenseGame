using UnityEngine;

public class UnitController : MonoBehaviour
{
    private Unit selectedUnit;

    void Update()
    {
        HandleSelection();
        HandleMovement();
    }

    private void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0f;

            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                Unit unit = hit.collider.GetComponent<Unit>();
                if (unit != null)
                {
                    if (selectedUnit != null)
                        selectedUnit.Deselect();

                    selectedUnit = unit;
                    selectedUnit.Select();
                }
            }
            else
            {
                if (selectedUnit != null)
                {
                    selectedUnit.Deselect();
                    selectedUnit = null;
                }
            }
        }
    }

    private void HandleMovement()
    {
        if (Input.GetMouseButtonDown(1) && selectedUnit != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
            targetPos.z = 0f;

            selectedUnit.MoveTo(targetPos);
        }
    }
}
