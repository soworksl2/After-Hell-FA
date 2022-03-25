using UnityEngine;
using UnityEngine.InputSystem;

using AfterHellFA.InputActions;

namespace AfterHellFA.Camera{

    [RequireComponent(typeof(RigCameraHandle))]
    public class RigCameraMovement : MonoBehaviour
    {
        #region serialize fields

        [SerializeField, Min(0f)] private float _speed;

        #endregion

        private RigCameraHandle _rigCameraHandle;
        private Vector2 _controlDirection;

        private void OnEnable() {
            GameInputActionsManager.CurrentGameInputActions.NormalMode.MoveCamera.performed += this.onMoveCamera;
            GameInputActionsManager.CurrentGameInputActions.NormalMode.MoveCamera.canceled += this.onMoveCamera;
        }

        private void Start() {
            this._rigCameraHandle = this.GetComponent<RigCameraHandle>();
        }

        private void Update() {
            this.transform.position += (this.ProcessControlDirection()) * this._speed * Time.deltaTime;
        }

        private Vector3 ProcessControlDirection(){
            return 
            Quaternion.Euler(0, this._rigCameraHandle.HandledCamera.transform.eulerAngles.y, 0) * 
            (new Vector3(this._controlDirection.x, 0, this._controlDirection.y));
        }

        #region input actions methods

        private void onMoveCamera(InputAction.CallbackContext e){
            if(e.phase == InputActionPhase.Canceled){
                this._controlDirection = Vector2.zero;
                return;
            }
            this._controlDirection = e.ReadValue<Vector2>();
        }

        #endregion
    }

}