class_name player
extends CharacterBody3D

# Stats
@export var _cur_hp: int = 10
@export var _max_hp: int = 10;
@export var _ammo: int = 15;
var _score: int = 0;
	
# Physics
@export var _move_speed: float = 5
@export var _jump_force: float = 5
@export var _gravity: float = 9.8
	
# Camera look
@export var _min_look_angle: float = -90
@export var _max_look_angle: float = 90
@export var _sensitivity: float = 10

# Aux
var _vel: Vector3 = Vector3.ZERO
var _mouse_delta: Vector2 = Vector2.ZERO
var _input_var: Vector2 = Vector2.ZERO
var _forward: Vector3 = Vector3.ZERO
var _right: Vector3 = Vector3.ZERO
var _relative_dir: Vector3 = Vector3.ZERO
var _cam_rot_deg: Vector3 = Vector3.ZERO
var _player_rot_deg: Vector3 = Vector3.ZERO

# Components
var _camera_3D: Camera3D
var _muzzle: Node3D
var _bullet_scene: PackedScene
var _ui: ui

func _ready():
	_camera_3D = get_node("Camera3D")
	_muzzle = get_node("Camera3D/Gun/Muzzle")
	# Hide and lock Mouse cursor
	Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
	_bullet_scene = ResourceLoader.load("res://Assets/Prefabs/bullet.tscn")
	_ui = get_node("/root/Main/CanvasLayer/UI")

	if ((_camera_3D == null) || (_muzzle == null) || (_bullet_scene == null) || (_ui == null)):
		push_error("null")
		get_tree().quit(1)
	else:
		# Error:
		# void Godot3DFirstPersonShooter.Assets.Scrips.Ui.BarHealthUpdate(int, int):
		# System.NullReferenceException: Object reference not set to an instance of an object.
		# i got a promise of instance? idk but I guess there is a better way to solve this than
		# changing the order of the objects in the main scene...
		#/
		# Set the ui
		_ui.bar_health_update(_cur_hp, _max_hp)
		_ui.txt_ammo_update(_ammo)
		_ui.txt_score_update(_score)

func _physics_process(delta):
	_vel.x = 0
	_vel.z = 0
	_input_var = Vector2.ZERO

	if (Input.is_action_pressed("ui_up")):
		_input_var.y -= 1
	if (Input.is_action_pressed("ui_down")):
		_input_var.y += 1
	if (Input.is_action_pressed("ui_left")):
		_input_var.x -= 1
	if (Input.is_action_pressed("ui_right")):
		_input_var.x += 1

	_input_var = _input_var.normalized()
		
	# Get the forward and right directions
	_forward = global_transform.basis.z
	_right = global_transform.basis.x
	_relative_dir = (_forward * _input_var.y + _right * _input_var.x)

	# Set the velocity
	_vel.x = _relative_dir.x * _move_speed
	_vel.z = _relative_dir.z * _move_speed
		
	# Apply gravity
	_vel.y -= _gravity * delta
		
	# Move the player
	velocity = _vel
	move_and_slide()
		
	# Jumping
	if (Input.is_action_pressed("ui_accept") && is_on_floor()):
		_vel.y = _jump_force

func _input(event):
	if event is InputEventMouseMotion:
		_mouse_delta = event.relative

func _process(delta):

# Rotate Camera along the X axis
	_cam_rot_deg = _camera_3D.rotation_degrees
	_cam_rot_deg.x -= _mouse_delta.y * _sensitivity * delta
		
	# Clamp Camera X rotation axis
	_cam_rot_deg.x = clamp(_cam_rot_deg.x, _min_look_angle, _max_look_angle)
	_camera_3D.rotation_degrees = _cam_rot_deg
		
	# Rotate the Player along the Y axis
	_player_rot_deg = rotation_degrees
	_player_rot_deg.y -= _mouse_delta.x * _sensitivity * delta
	rotation_degrees = _player_rot_deg
		
	# Reset Mouse delta vector
	_mouse_delta = Vector2.ZERO
		
	# Check if we have shoot
	if(Input.is_action_just_pressed("shoot") && _ammo > 0):
		shoot()

func shoot() -> void:
	var _bullet: Node3D = _bullet_scene.instantiate()
	get_node("/root/Main").add_child(_bullet)
	_bullet.global_transform = _muzzle.global_transform
	_ammo -= 1;
	_ui.txt_ammo_update(_ammo)

func take_damage(damage: int) -> void:
	_cur_hp -= damage;
	_ui.bar_health_update(_cur_hp, _max_hp)
	if (_cur_hp <= 0):
		die()

func die() -> void:
	get_tree().reload_current_scene()

func score_add(amount: int) -> void:
	_score += amount
	_ui.txt_score_update(_score)

func health_add(amount: int) -> void:
	_cur_hp += amount
	if (_cur_hp > _max_hp):
		_cur_hp = _max_hp
	_ui.bar_health_update(_cur_hp, _max_hp);
	
func ammo_add(amount: int) -> void:
	_ammo += amount
	_ui.txt_ammo_update(_ammo)
