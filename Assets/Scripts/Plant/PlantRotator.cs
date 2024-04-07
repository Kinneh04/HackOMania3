using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlantRotator : MonoBehaviour
{
    public const float MaxTapTime = 0.3f;
    public const float MaxTapDistTravelled = 20f;
    public const float RotateSpeed = 500f;
    const float zoomSpeed = 4.0f, minScale = 2.0f, maxScale = 5.0f, minPinchSpeed = 5.0f, varianceInDistance = 5.0f;

    //[SerializeField] Shader GlobalShader;

    private float _screenWidth;
    private float _screenHeight;

    private Vector3 _startPos;
    private Vector3 _endPos;
    private Vector3 _inputVector;

    private Touch touch;
    private bool hasTouch = false;

    Vector3 _plantPos;

    private bool autoRotate = true;
    private IEnumerator rotatePlantCoroutine;

    void Start()
    {
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;

        _startPos = Vector3.zero;
        _endPos = Vector3.zero;
        _inputVector = Vector3.zero;

        _plantPos = Vector3.zero;
        var origRot = Camera.main.transform.rotation;
        Camera.main.transform.LookAt(_plantPos);
        Camera.main.transform.rotation = origRot;

        rotatePlantCoroutine = RotatePlant();
        StartCoroutine(rotatePlantCoroutine);
        //Camera.main.SetReplacementShader(GlobalShader, "");
    }

    /// <summary>
    /// Update rotation of cube depending on received input vector
    /// 受信した入力ベクトルに応じて、立方体の回転を更新する。
    /// </summary>
    private void UpdateRotation()
    {
        float rotationVertical = -180f * _inputVector.x;
        //float rotationHorizontal = 30f * _inputVector.y;
        Camera.main.transform.RotateAround(_plantPos, new Vector3(0f, rotationVertical, 0f), Time.deltaTime * RotateSpeed);
        /*var newRot = Camera.main.transform.rotation.eulerAngles 
            + new Vector3(rotationHorizontal, 0f, 0f);
        float newRotX = newRot.x < 350f && newRot.x > 300f ? 350f : newRot.x > 10f && newRot.x < 20f ? 10f : newRot.x;
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(newRotX, newRot.y, newRot.z));*/
        //Camera.main.transform.Rotate(new Vector3(rotationHorizontal, 0f, 0f), Space.World);
        //transform.Rotate(new Vector3(rotationHorizontal, rotationVertical, 0f), Space.World);
    }


    #region Input
    // <summary>
    /// Check for different input types and call the corresponding actions in list
    /// 異なる入力タイプをチェックし、リストで対応するアクションを呼び出す
    /// </summary>
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            hasTouch = true;
        }
        else
        {
            hasTouch = false;
        }

        if (GetIsBeginTap())
        {
            OnBeginTap();
        }
        else if (GetIsHoldTap())
        {
            OnHoldTap();
        }
        else if (GetIsEndTap())
        {
            OnEndTap();
        }

        UpdateRotation();
        PinchInput();
    }

    /// <summary>
    /// Checks for when a tap first began
    /// 初めてタップが始まった時のチェック項目
    /// </summary>
    /// <returns>whether a tap is first received タップが初めて受信されたかどうか</returns>
    private bool GetIsBeginTap()
    {
        return (Input.GetMouseButtonDown(0)
            || (hasTouch && touch.phase == TouchPhase.Began))
            && GetIsTapOnPlant();
    }

    /// <summary>
    /// Checks for when a tap is being held 
    /// タップが保持されているときのチェック
    /// </summary>
    /// <returns>whether a tap is held タップが行われているかどうか</returns>
    private bool GetIsHoldTap()
    {
        return Input.GetMouseButton(0)
            || (hasTouch && (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary));
    }

    /// <summary>
    /// Checks for when a tap has been released 
    /// タップが解除されたときのチェック
    /// </summary>
    /// <returns>whether a tap is released タップの切れ具合</returns>
    private bool GetIsEndTap()
    {
        return Input.GetMouseButtonUp(0)
            || (hasTouch && touch.phase == TouchPhase.Ended);
    }

    private bool GetIsTapOnPlant()
    {
        var selectedObj = EventSystem.current.currentSelectedGameObject;
        if (selectedObj == null)
            return false;

        return selectedObj.CompareTag("PlantPanel");
    }

    private IEnumerator RotatePlant()
    {
        yield return new WaitForSeconds(0.25f);
        while (autoRotate)
        {
            Camera.main.transform.RotateAround(_plantPos, new Vector3(0f, 1.0f, 0f), Time.deltaTime * 10f);
            yield return null;
        }
    }

    private void PinchInput()
    {
        if (!GetIsTapOnPlant())
            return;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0)
        {

            if (Input.mouseScrollDelta.y > 0)
            {

                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView + (1 * zoomSpeed), 60, 125);
            }

            else if (Input.mouseScrollDelta.y < 0)
            {

                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - (1 * zoomSpeed), 60, 125);
            }
        }
#elif UNITY_ANDROID
        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
        {

            var curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
            var prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
            var touchDelta = curDist.magnitude - prevDist.magnitude;
            var speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
            var speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;

            if ((touchDelta + varianceInDistance <= 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
            {

                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView + (1 * zoomSpeed), 60, 125);
            }

            if ((touchDelta + varianceInDistance > 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
            {

                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - (1 * zoomSpeed), 60, 125);
            }

        }
#endif
    }
    #endregion

    /// <summary>
    /// Set the start position of initial tap
    /// イニシャルタップの開始位置を設定する
    /// </summary>
    public void OnBeginTap()
    {
        if (GetIsTapOnPlant())
            autoRotate = false;

        _startPos = Input.mousePosition;
    }

    /// <summary>
    /// Calculate current input vector based on distance travelled from initial tap
    /// 初期タップからの移動距離から現在の入力ベクトルを計算する
    /// </summary>
    public void OnHoldTap()
    {
        /*if (!GameManager.Instance.GetIsPlayingOrProcessing())
            return;*/

        if (!GetIsTapOnPlant())
        {
            OnEndTap();
            return;
        }

        _endPos = Input.mousePosition;
        _inputVector.x = (_endPos.x - _startPos.x) / _screenWidth;
        _inputVector.y = (_endPos.y - _startPos.y) / _screenHeight;
        _startPos = Input.mousePosition;
    }

    /// <summary>
    /// Reset input vector on tap end
    /// タップエンドの入力ベクトルをリセットする
    /// </summary>
    public void OnEndTap()
    {
        autoRotate = true;
        StopCoroutine(rotatePlantCoroutine);
        rotatePlantCoroutine = RotatePlant();
        StartCoroutine(rotatePlantCoroutine);
        _inputVector = Vector3.zero;
    }
}
