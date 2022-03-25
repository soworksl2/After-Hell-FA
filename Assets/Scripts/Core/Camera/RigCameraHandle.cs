using UnityEngine;
using UnityEngine.InputSystem;

using AfterHellFA.InputActions;

namespace AfterHellFA.Camera
{
    public class RigCameraHandle : MonoBehaviour
    {
        #region serialized Fields

        [SerializeField] private UnityEngine.Camera _camera;

        [Space, Header("Movement Configurations")]

        [SerializeField, Min(0f)] private float _movementSpeed;

        [Space][Header("Handle Configurations")]

        [SerializeField] private bool _invertXAxi;
        [SerializeField] private bool _invertYAxi; 

        [Space]

        [SerializeField, Min(0f)] private float _minInclination = 1f;
        [SerializeField, Min(0.1f)] private float _maxInclination = 89f;

        [Space]

        [SerializeField, Min(0f)] private float _minDistance = 1f;
        [SerializeField, Min(0.1f)] private float _maxDistance = 100f;

        [Space]

        [SerializeField, Min(0f)] private float _rotationSpeed;
        [SerializeField, Min(0f)] private float _rotationSensibility;
        [SerializeField] private float _zoomInfluenty;

        [Space][Header("Properties")]

        [SerializeField] private float _distance = 1f;

        #endregion

        private float _sensibilityOrRotationSpeed;

        private bool _isDragCamera;
        private float _controlRotationCamera;
        private float _controlInclinationCamera;
        private Vector2 _controlMovementDirection;

        public UnityEngine.Camera HandledCamera => this._camera;
        public float CameraDistance {
            get => this._distance;
            set{
                this._distance = value;
            }
        }

        private void OnEnable() {

            // attaching HandleCamera Actions
            GameInputActionsManager.CurrentGameInputActions.NormalMode.RotateCamera.performed += this.OnRotateCamera;
            GameInputActionsManager.CurrentGameInputActions.NormalMode.RotateCamera.canceled += this.OnRotateCamera;

            GameInputActionsManager.CurrentGameInputActions.NormalMode.IsDragCamera.performed += this.OnIsDragCamera;
            GameInputActionsManager.CurrentGameInputActions.NormalMode.IsDragCamera.canceled += this.OnIsDragCamera;

            GameInputActionsManager.CurrentGameInputActions.NormalMode.DragCamera.performed += this.OnDragCamera;
            GameInputActionsManager.CurrentGameInputActions.NormalMode.DragCamera.canceled += this.OnDragCamera;

            GameInputActionsManager.CurrentGameInputActions.NormalMode.ZoomCamera.performed += this.OnZoomCamera;

            //Attaching MovementCamera Actions
            GameInputActionsManager.CurrentGameInputActions.NormalMode.MoveCamera.performed += this.OnMoveCamera;
            GameInputActionsManager.CurrentGameInputActions.NormalMode.MoveCamera.canceled += this.OnMoveCamera;
        }

        private void OnDisable() {
            
            // detaching handle camera actions
            GameInputActionsManager.CurrentGameInputActions.NormalMode.RotateCamera.performed -= this.OnRotateCamera;
            GameInputActionsManager.CurrentGameInputActions.NormalMode.RotateCamera.canceled -= this.OnRotateCamera;

            GameInputActionsManager.CurrentGameInputActions.NormalMode.IsDragCamera.performed -= this.OnIsDragCamera;
            GameInputActionsManager.CurrentGameInputActions.NormalMode.IsDragCamera.canceled -= this.OnIsDragCamera;

            GameInputActionsManager.CurrentGameInputActions.NormalMode.DragCamera.performed -= this.OnDragCamera;
            GameInputActionsManager.CurrentGameInputActions.NormalMode.DragCamera.canceled -= this.OnDragCamera;

            GameInputActionsManager.CurrentGameInputActions.NormalMode.ZoomCamera.performed -= this.OnZoomCamera;

            // detaching movement camera actions
            GameInputActionsManager.CurrentGameInputActions.NormalMode.MoveCamera.performed -= this.OnMoveCamera;
            GameInputActionsManager.CurrentGameInputActions.NormalMode.MoveCamera.canceled -= this.OnMoveCamera;
        }

        private void Start(){
            if(this._camera == null)
            {
                Debug.LogError("The rig camera handle hasn't a camera attached", this);
                return;
            }
        }

        private void Update() {
            this.ApplyRotationAndInclinationToCameraHandle();

            this.MoveCameraHandle();
        }

        private void ApplyRotationAndInclinationToCameraHandle(){
            if(this._controlRotationCamera == 0f && this._controlInclinationCamera == 0f)
                return;

            float xDelta = this._controlInclinationCamera * this._sensibilityOrRotationSpeed * Time.deltaTime;
            float yDelta = this._controlRotationCamera * this._sensibilityOrRotationSpeed * Time.deltaTime;

            Vector3 processedRotation = this.transform.eulerAngles;
            processedRotation += new Vector3(xDelta, yDelta);

            processedRotation.x = Mathf.Clamp(processedRotation.x, this._minInclination, this._maxInclination);

            this.transform.eulerAngles = processedRotation;
        }

        private void MoveCameraHandle(){
            this.transform.position += (this.ProcessControlMovementDirection()) * this._movementSpeed * Time.deltaTime;
        }

        private Vector3 ProcessControlMovementDirection(){
            return 
            Quaternion.Euler(0, this._camera.transform.eulerAngles.y, 0) * 
            (new Vector3(this._controlMovementDirection.x, 0, this._controlMovementDirection.y));
        }

        #region ActionInputsMethods
        
        //Handle actions

        private void OnRotateCamera(InputAction.CallbackContext e){
            if(this._isDragCamera)
                return;

            if(e.phase == InputActionPhase.Canceled){
                this._controlRotationCamera = 0f;
                return;
            }

            this._controlRotationCamera = e.ReadValue<float>();
        }

        private void OnIsDragCamera(InputAction.CallbackContext e){
            this._isDragCamera = e.ReadValueAsButton();

            this._controlRotationCamera = 0f;
            this._controlInclinationCamera = 0f;

            if(this._isDragCamera){
                this._sensibilityOrRotationSpeed = this._rotationSensibility;
            }else{
                this._sensibilityOrRotationSpeed = this._rotationSpeed;
            }
        }

        private void OnDragCamera(InputAction.CallbackContext e){
            if(!this._isDragCamera)
                return;

            Vector2 deltaMouse = e.ReadValue<Vector2>();

            this._controlRotationCamera = deltaMouse.x * (this._invertXAxi? -1f : 1f);
            this._controlInclinationCamera = deltaMouse.y * (this._invertYAxi? -1f : 1f);
        }

        private void OnZoomCamera(InputAction.CallbackContext e){
            this._distance = Mathf.Clamp(this._distance + (e.ReadValue<float>() * this._zoomInfluenty), this._minDistance, this._maxDistance);

            Vector3 newCameraPosition = new Vector3(0, 0, this._distance * -1f);

            this._camera.transform.localPosition = newCameraPosition;
        }

        // Movement actions

        private void OnMoveCamera(InputAction.CallbackContext e){
            if(e.phase == InputActionPhase.Canceled){
                this._controlMovementDirection = Vector2.zero;
                return;
            }
            this._controlMovementDirection = e.ReadValue<Vector2>();
        }

        #endregion
    }
}