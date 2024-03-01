using Godot;

namespace Godot3DFirstPersonShooter.Assets.Scrips;

public partial class Bullet : Area3D
{
    [Export] private float _speed = 30f;
    [Export] private float _damage = 1f;

    public override void _Process(double delta)
    {
        /* Deprecated in Godot 4
        translation += global_transform.basis.z * _speed * delta;
        */
        Position += GlobalTransform.Basis.Z * _speed *(float)delta;
    }

    private void _Destroy()
    {
        QueueFree();
    }

    private void _OnBodyEntered(Node3D body)
    {
        if (body.HasMethod("TakeDamage"))
        {
            body.Call("TakeDamage", _damage);
            _Destroy();
        }
    }
}