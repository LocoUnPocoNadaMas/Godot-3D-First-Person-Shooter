using Godot;

namespace Godot3DFirstPersonShooter.Assets.Scrips;

public partial class Player : CharacterBody3D
{
    // Stats
    [Export] private int _curHp = 10;
    [Export] private int _maxHp = 10;
    [Export] private int _ammo = 15;
    private int _score = 0;
    
    // Physics
    private float _moveSpeed = 5f;
    private float _jumpForce = 5f;
    private float _gravity = 9.8f;
    
    // Camera look
    private float _minLookAngle = -90f;
    private float _maxLookAngle = 90f;
    private float _sensitivity = 10f;

    // Aux
    private Vector3 _vel = new Vector3();
    private Vector2 _mouseDelta = new Vector2();
    private Vector2 _input = new Vector2();
    private Vector3 _forward = new Vector3();
    private Vector3 _right = new Vector3();
    private Vector3 _relativeDir = new Vector3();

    // Components
    private Camera3D _camera3D;
    private Node3D _muzzle;

    public override void _Ready()
    {
        _camera3D = GetNode<Camera3D>("Camera3D");
        _muzzle = GetNode<Node3D>("Camera3D/Gun/Muzzle");
        if (_camera3D.Equals(null) || _muzzle.Equals(null))
        {
            GD.PushError("null");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        _vel.X = 0;
        _vel.Z = 0;
        _input = new Vector2();

        if (Input.IsActionPressed("ui_up"))
            _input.Y -= 1;
        if (Input.IsActionPressed("ui_down"))
            _input.Y += 1;
        if (Input.IsActionPressed("ui_left"))
            _input.X -= 1;
        if (Input.IsActionPressed("ui_right"))
            _input.X += 1;

        _input = _input.Normalized();
        
        // Get the forward and right directions
        _forward = GlobalTransform.Basis.Z;
        _right = GlobalTransform.Basis.X;
        _relativeDir = (_forward * _input.Y + _right * _input.X);

        // Set the velocity
        _vel.X = _relativeDir.X * _moveSpeed;
        _vel.Z = _relativeDir.Z * _moveSpeed;
        
        // Apply gravity
        _vel.Y -= _gravity * (float)delta;
        
        // Move the player
        Velocity = _vel;
        MoveAndSlide();
        
        // Jumping
        if (Input.IsActionPressed("ui_accept") && IsOnFloor())
            _vel.Y = _jumpForce;
        
    }
}