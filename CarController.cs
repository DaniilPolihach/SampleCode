using System;
using DavidJalbert.TinyCarControllerAdvance;
using UnityEngine;

namespace Game
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private TCCAPlayer _vehicleController;
        [SerializeField] private CarBodyController _carBodyController;
        [SerializeField] private CarWheelController[] _carWheelsControllers;
        [Space]
        [SerializeField] private Projector _shadowProjector;
        [SerializeField] private Vector3 _shadowProjectorOffset;
        [Space]
        [SerializeField] private float _pitchPower = 15.0f;
        [Space]
        [SerializeField] private GameObject _rocketsRoot;
        
        public Vector3 Position => _vehicleController.getCarBody().transform.position;   
        public Vector3 Velocity => _vehicleController.getRigidbody().velocity;

        public Vector3 Forward => transform.forward;

        public float Speed => _vehicleController.getForwardVelocity();
        public float MaxSpeed => _vehicleController.getWheelsMaxSpeed();
        public bool Accelerate { get; set; }
        public bool Brake { get; set; }

        public float MotorSpeed { get; private set; }

        public bool IsBodyContactsTrack => _carBodyController.IsContactsTrack;
        public bool IsWheelsContactsTrack => _vehicleController.isGrounded();

        public bool IsInAir => !IsBodyContactsTrack && !IsWheelsContactsTrack;
        public bool IsFlying { get; set; }
        private float FlyStartTick { get; set; }

        public bool IsRocketsActive
        {
            get => _rocketsRoot.activeSelf;
            set
            {
                if (_rocketsRoot.activeSelf != value)
                {
                    _rocketsRoot.SetActive(value);
                }
            }
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetSpeed(float speed)
        {
            MotorSpeed = speed;
        }

        public void Tick(float tick)
        {
            CheckInAir(tick);
            MoveCar();
            FollowShadow();
        }

        private void FixedUpdate()
        {
            PitchAlign();
        }

        private void FollowShadow()
        {
            var pos = _vehicleController.getCarBody().transform.position;

            if (_shadowProjector != null)
            {
                _shadowProjector.transform.position = pos + _shadowProjectorOffset;
            }
        }

        private void MoveCar()
        {
            if (Accelerate)
            {
                _vehicleController.setMotor(1);
            }
            else if (Brake)
            {
                _vehicleController.setMotor(-1);
            }
            else
            {
                _vehicleController.setMotor(0);
            }
        }

        private void PitchAlign()
        {
            if (!IsFlying)
                return;

            if (Accelerate)
            {
                _vehicleController.getRigidbody().AddTorque(Vector3.left * _pitchPower, ForceMode.Acceleration);
            }
            else if (Brake)
            {
                _vehicleController.getRigidbody().AddTorque(Vector3.right * _pitchPower, ForceMode.Acceleration);
            }
        }

        private void CheckInAir(float tick)
        {
            if (IsInAir && FlyStartTick < 0)
            {
                FlyStartTick = tick;
            }
            else if (!IsInAir)
            {   
                FlyStartTick = -1;
            }

            IsFlying = FlyStartTick > 0 && tick > FlyStartTick;
        }

        public void Stop()
        {
            CleanUp();
        }

        public void CleanUp()
        {
            _vehicleController.getRigidbody().constraints = 
                RigidbodyConstraints.FreezePositionX | 
                RigidbodyConstraints.FreezeRotationY |
                RigidbodyConstraints.FreezeRotationZ;
            
            _vehicleController.setMotor(0);
        }

        public void Destroy()
        {
            var force = 50.0f;
            var radius = 25.0f;
            var position = transform.position;
            
            _carBodyController.Explode(position, force * 2, radius);
            
            foreach(var wheelController in _carWheelsControllers)
            {
                wheelController.Explode(position, force, radius);
            }
        }

        public void Push(Vector3 force)
        {
            _vehicleController.getCarBody().getRigidbody().AddForce(force, ForceMode.Force);
        }

        private void OnValidate()
        {    
            if (_vehicleController == null)
            {
                _vehicleController = GetComponent<TCCAPlayer>();
            }

            if (_carBodyController == null)
            {
                _carBodyController = GetComponentInChildren<CarBodyController>();
            }

            if (_carWheelsControllers == null || _carWheelsControllers.Length == 0)
            {
                _carWheelsControllers = GetComponentsInChildren<CarWheelController>();
            }
            
            if (_shadowProjector == null)
            {
                _shadowProjector = GetComponentInChildren<Projector>();

                if (_shadowProjector != null)
                {
                    _shadowProjectorOffset = _shadowProjector.transform.position - _carBodyController.transform.position;
                }
            }
        }
    }
}