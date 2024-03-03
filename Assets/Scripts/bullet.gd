class_name bullet
extends Area3D

@export var _speed: float = 30;
@export var _damage: float = 1;

func _process(delta):
	# Deprecated in Godot 4
	# translation += global_transform.basis.z * _speed * delta
	position += global_transform.basis.z * _speed * delta

func _destroy() -> void:
	queue_free()

func _on_body_entered(body):
	if(body.has_method("take_damage")):
		body.call("take_damage", _damage)
		_destroy()
