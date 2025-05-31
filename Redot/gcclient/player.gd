extends CharacterBody3D

#Client Tarafı
const SPEED = 5.0
const JUMP_VELOCITY = 4.5
var mouse_sense = 0.1
@onready var head = $head
@onready var cam = $head/Camera3D

var _Name = ''

var IDD = 0
var Ilk = true



func _ready() -> void:
	if _Name==Global.Name:
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
		if Ilk==true:
			$Label.text=Global.Name
			name=Global.Name
			Ilk = false

func _input(event):
	if name==Global.Name:
		#get mouse input for camera rotation
		if event is InputEventMouseMotion:
			rotate_y(deg_to_rad(-event.relative.x * mouse_sense))
			head.rotate_x(deg_to_rad(-event.relative.y * mouse_sense))
			head.rotation.x = clamp(head.rotation.x, deg_to_rad(-89), deg_to_rad(89))

func _physics_process(delta: float) -> void:
	if name==Global.Name:
		$head/Camera3D.current=true
		# Add the gravity.
		if not is_on_floor():
			velocity += get_gravity() * delta

		# Handle jump.
		if Input.is_action_just_pressed("ui_accept") and is_on_floor():
			velocity.y = JUMP_VELOCITY

		# Get the input direction and handle the movement/deceleration.
		# As good practice, you should replace UI actions with custom gameplay actions.
		var input_dir := Input.get_vector("a", "d", "w", "s")
		var direction := (transform.basis * Vector3(input_dir.x, 0, input_dir.y)).normalized()
		if direction:
			velocity.x = direction.x * SPEED
			velocity.z = direction.z * SPEED
		else:
			velocity.x = move_toward(velocity.x, 0, SPEED)
			velocity.z = move_toward(velocity.z, 0, SPEED)
		move_and_slide()
		GlobalPosition.rpc({
			'Name': name,
			'Pos': global_position,
			'Rot': global_rotation,          # karakter gövdesi rotasyonu
			'HeadRot': head.rotation         # kafa rotasyonu
		})
	elif name!=Global.Name:
		$head/Camera3D.current=false

@rpc("any_peer")
func GlobalPosition(_Data):
	if _Data['Name'] == name:
		global_position = _Data['Pos']
		if _Data.has("Rot"):
			global_rotation = _Data['Rot']
		if _Data.has("HeadRot"):
			head.rotation = _Data['HeadRot']
