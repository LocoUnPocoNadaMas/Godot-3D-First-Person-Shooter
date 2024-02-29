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
    
    // camera look
    private float _minLookAngle = -90f;
    private float _maxLookAngle = 90f;
    private float _sensitivity = 10f;

    private Vector3 _vel = new Vector3();
    private Vector2 _mouseDelta = new Vector2();

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
}