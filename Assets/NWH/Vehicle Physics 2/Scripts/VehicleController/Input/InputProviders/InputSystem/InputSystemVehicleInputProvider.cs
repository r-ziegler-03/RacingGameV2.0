using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
using NWH.NUI;
#endif

namespace NWH.VehiclePhysics2.Input
{
    /// <summary>
    /// Handles input through Unity's new Input System.
    /// This version supports multiplayer separation by allowing initialization from an external source.
    /// </summary>
    public partial class InputSystemVehicleInputProvider : VehicleInputProviderBase
    {
        private const int H_SHIFTER_GEAR_COUNT = 10;

        public VehicleInputActions vehicleInputActions;
        [Tooltip("Should mouse be used for input?")]
        public bool mouseInput;

        private readonly bool[] _shiftIntoHeld = new bool[H_SHIFTER_GEAR_COUNT];
        private float _throttle;
        private float _brakes;
        private float _steering;
        private float _clutch;
        private float _handbrake;
        private bool _horn;
        private bool _boost;

        public new void Awake()
        {
            //base.Awake();
            // Do not set up bindings here — this is handled in Initialize()
        }

        public void Initialize(VehicleInputActions actions)
        {
            vehicleInputActions = actions;

            // Hook up gear shift events
            SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftIntoR1, 0);
            SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto0, 1);
            SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto1, 2);
            SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto2, 3);
            SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto3, 4);
            SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto4, 5);
            SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto5, 6);
            SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto6, 7);
            SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto7, 8);
            SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto8, 9);

            // Other events
            vehicleInputActions.VehicleControls.Horn.started += ctx => _horn = true;
            vehicleInputActions.VehicleControls.Horn.canceled += ctx => _horn = false;
            vehicleInputActions.VehicleControls.Boost.started += ctx => _boost = true;
            vehicleInputActions.VehicleControls.Boost.canceled += ctx => _boost = false;
        }

        private void SetupGearShiftInput(InputAction gearShiftAction, int index)
        {
            gearShiftAction.started += ctx => _shiftIntoHeld[index] = true;
            gearShiftAction.canceled += ctx => _shiftIntoHeld[index] = false;
        }

        public void Update()
        {
            if (vehicleInputActions == null)
                return;

            // Prevent reading input if this instance isn’t assigned a unique, enabled map
            if (!vehicleInputActions.asset.enabled)
                return;

            _throttle = mouseInput
                ? Mathf.Clamp(GetMouseVertical(), 0f, 1f)
                : vehicleInputActions.VehicleControls.Throttle.ReadValue<float>();

            _brakes = mouseInput
                ? -Mathf.Clamp(GetMouseVertical(), -1f, 0f)
                : vehicleInputActions.VehicleControls.Brakes.ReadValue<float>();

            _steering = mouseInput
                ? Mathf.Clamp(GetMouseHorizontal(), -1f, 1f)
                : vehicleInputActions.VehicleControls.Steering.ReadValue<float>();

            _clutch = vehicleInputActions.VehicleControls.Clutch.ReadValue<float>();
            _handbrake = vehicleInputActions.VehicleControls.Handbrake.ReadValue<float>();
        }

        public override float Throttle() => _throttle;
        public override float Brakes() => _brakes;
        public override float Steering() => _steering;
        public override float Clutch() => _clutch;
        public override float Handbrake() => _handbrake;

        public override bool EngineStartStop() => vehicleInputActions.VehicleControls.EngineStartStop.triggered;
        public override bool ExtraLights() => vehicleInputActions.VehicleControls.ExtraLights.triggered;
        public override bool HighBeamLights() => vehicleInputActions.VehicleControls.HighBeamLights.triggered;
        public override bool HazardLights() => vehicleInputActions.VehicleControls.HazardLights.triggered;
        public override bool Horn() => _horn;
        public override bool LeftBlinker() => vehicleInputActions.VehicleControls.LeftBlinker.triggered;
        public override bool LowBeamLights() => vehicleInputActions.VehicleControls.LowBeamLights.triggered;
        public override bool RightBlinker() => vehicleInputActions.VehicleControls.RightBlinker.triggered;
        public override bool ShiftDown() => vehicleInputActions.VehicleControls.ShiftDown.triggered;
        public override bool ShiftUp() => vehicleInputActions.VehicleControls.ShiftUp.triggered;
        public override bool TrailerAttachDetach() => vehicleInputActions.VehicleControls.TrailerAttachDetach.triggered;
        public override bool FlipOver() => vehicleInputActions.VehicleControls.FlipOver.triggered;
        public override bool Boost() => _boost;
        public override bool CruiseControl() => vehicleInputActions.VehicleControls.CruiseControl.triggered;

        public override int ShiftInto()
        {
            for (int i = 0; i < H_SHIFTER_GEAR_COUNT; i++)
                if (_shiftIntoHeld[i])
                    return i - 1;

            return -999;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            vehicleInputActions?.Disable();
            _throttle = _brakes = _steering = _clutch = _handbrake = 0;
        }

        private float GetMouseHorizontal()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            float percent = Mathf.Clamp(mousePos.x / Screen.width, -1f, 1f);
            return percent < 0.5f ? -(0.5f - percent) * 2f : (percent - 0.5f) * 2f;
        }

        private float GetMouseVertical()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            float percent = Mathf.Clamp(mousePos.y / Screen.height, -1f, 1f);
            return percent < 0.5f ? -(0.5f - percent) * 2f : (percent - 0.5f) * 2f;
        }
    }
}

#if UNITY_EDITOR
namespace NWH.VehiclePhysics2.Input
{
    [CustomEditor(typeof(InputSystemVehicleInputProvider))]
    public partial class InputSystemVehicleInputProviderEditor : NVP_NUIEditor
    {
        public override bool OnInspectorNUI()
        {
            if (!base.OnInspectorNUI()) return false;

            drawer.Info("Modify 'VehicleInputActions' asset to change bindings.");
            drawer.Field("mouseInput");
            drawer.EndEditor(this);
            return true;
        }

        public override bool UseDefaultMargins() => false;
    }
}
#endif
