using Godot;

namespace Godot3DFirstPersonShooter.Assets.Scrips;

public partial class Enemy : CharacterBody3D
{
    [ExportCategory("Stats")]
    [Export] private int _health = 5;
    [Export] private float _moveSpeed = 2f;

    [ExportCategory("Attacking")]
    [Export] private int _damage = 1;
    [Export] private float _attackRate = 1f;
    [Export] private float _attackDist = 2f;

    [ExportCategory("Reward")]
    [Export] private int _scoreToGive = 1;

    // Components
    private Player _player;
    private Timer _timer;
    
    // Aux
    private Vector3 _playerDir = new Vector3();
    private Vector3 _vel = new Vector3();

    public override void _Ready()
    {
        _player = GetNode<Player>("/root/Main/Player");
        _timer = GetNode<Timer>("Timer");
        if (_player == null || _timer == null)
        {
            GD.PushError("null");
            GetTree().Quit();
        }
        else
        {
            // Setup the Timer
            _timer.WaitTime = _attackRate;
            _timer.Start();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // Calculate the direction to the Player
        _playerDir = (_player.Position - Position).Normalized();
        _playerDir.Y = 0;
        
        // Move the enemy towards the player
        if (Position.DistanceTo(_player.Position) > _attackDist)
        {
            Velocity = _playerDir * _moveSpeed;
            MoveAndSlide();
        }
    }

    private void _OnTimerTimeout()
    {
        if (Position.DistanceTo(_player.Position) <= _attackDist)
        {
            Attack();
        }
    }

    private void Attack()
    {
        _player.TakeDamage(_damage);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        QueueFree();
    }
}