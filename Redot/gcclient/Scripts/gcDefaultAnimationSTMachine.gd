extends AnimationTree

@onready var playback = get("parameters/playback")
var _Name = ''

func _process(_delta: float) -> void:
	if get_parent().get_parent().get_parent().get_parent().get_node('.').name!=Global.Name:
		var anim = "GcDefault_idl"
		if Input.is_action_pressed("w"):
			anim = "GcDefault_walk_forward"
		elif Input.is_action_pressed("s"):
			anim = "GcDefault_walk_backward"
		else:
			anim = "GcDefault_idl"
		update_animation(anim)

@rpc("any_peer")
func update_animation(anim: String):
	if _Name != Global.Name:
		playback.travel(anim)
