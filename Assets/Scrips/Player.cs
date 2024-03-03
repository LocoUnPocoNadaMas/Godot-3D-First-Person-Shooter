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
    private Vector3 _camRotDeg = new Vector3();
    private Vector3 _playerRotDeg = new Vector3();

    // Components
    private Camera3D _camera3D;
    private Node3D _muzzle;
    private PackedScene _bulletScene;
    private Ui _ui;

    public override void _Ready()
    {
        _camera3D = GetNode<Camera3D>("Camera3D");
        _muzzle = GetNode<Node3D>("Camera3D/Gun/Muzzle");
        // Hide and lock Mouse cursor
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _bulletScene = ResourceLoader.Load<PackedScene>("res://Assets/Prefabs/bullet.tscn");
        _ui = GetNode<Ui>("/root/Main/CanvasLayer/UI");
        
        if ((_camera3D == null) || (_muzzle == null) || (_bulletScene == null) || (_ui == null))
        {
            GD.PushError("null");
            GetTree().Quit();
        }
        else
        {
            /* Error:
             * void Godot3DFirstPersonShooter.Assets.Scrips.Ui.BarHealthUpdate(int, int):
             * System.NullReferenceException: Object reference not set to an instance of an object.
             * i got a promise of instance? idk but I guess there is a better way to solve this than
             * changing the order of the objects in the main scene...
             */
            // Set the ui
            _ui.BarHealthUpdate(_curHp, _maxHp);
            _ui.TxtAmmoUpdate(_ammo);
            _ui.TxtScoreUpdate(_score);
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

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            _mouseDelta = mouseMotion.Relative;
        }
    }

    public override void _Process(double delta)
    {
        // Rotate Camera along the X axis
        _camRotDeg = _camera3D.RotationDegrees;
        _camRotDeg.X -= _mouseDelta.Y * _sensitivity * (float)delta;
        
        // Clamp Camera X rotation axis
        _camRotDeg.X = Mathf.Clamp(_camRotDeg.X, _minLookAngle, _maxLookAngle);
        _camera3D.RotationDegrees = _camRotDeg;
        
        // Rotate the Player along the Y axis
        _playerRotDeg = RotationDegrees;
        _playerRotDeg.Y -= _mouseDelta.X * _sensitivity * (float)delta;
        RotationDegrees = _playerRotDeg;
        
        // Reset Mouse delta vector
        _mouseDelta = new Vector2();
        
        // Check if we have shoot
        if(Input.IsActionJustPressed("shoot") && _ammo > 0)
            Shoot();
    }

    private void Shoot()
    {
        var bullet = _bulletScene.Instantiate<Node3D>();
        GetNode("/root/Main").AddChild(bullet);
        bullet.GlobalTransform = _muzzle.GlobalTransform;
        _ammo -= 1;
        _ui.TxtAmmoUpdate(_ammo);
    }

    public void TakeDamage(int damage)
    {
        _curHp -= damage;
        _ui.BarHealthUpdate(_curHp, _maxHp);
        if (_curHp <= 0)
            Die();
    }

    private void Die()
    {
        GetTree().ReloadCurrentScene();
    }

    public void ScoreAdd(int amount)
    {
        _score += amount;
        _ui.TxtScoreUpdate(_score);
    }

    public void HealthAdd(int amount)
    {
        _curHp += amount;
        if (_curHp > _maxHp)
            _curHp = _maxHp;
        _ui.BarHealthUpdate(_curHp, _maxHp);
    }
    
    public void AmmoAdd(int amount)
    {
        _ammo += amount;
        _ui.TxtAmmoUpdate(_ammo);
    }
}