extends Node2D

var Enet = ENetMultiplayerPeer.new()
var Datas = {}
var connected_peers = [] # Player list

@rpc("any_peer")
func Login(PlayerName, _ID):
	var peer_id = multiplayer.get_remote_sender_id()
	if PlayerName in Datas:
		BackMessage.rpc(PlayerName, _ID, 1)
	else:
		Datas[PlayerName] = {
			'PlayerName': PlayerName,
			'peer_id': peer_id
		}
		BackMessage.rpc(PlayerName, _ID, 0)

@rpc("any_peer")
func BackMessage(PlayerName,_ID,No):
	pass

func _ready() -> void:
	Enet.create_server(2025, 5)
	multiplayer.multiplayer_peer = Enet
	multiplayer.peer_connected.connect(_PC)
	multiplayer.peer_disconnected.connect(_PD)

var timer = 10
func print_children_names(parent_node: Node, delta: float) -> void:
	if timer > 0:
		timer -= delta
	else:
		timer = 10
		for child in parent_node.get_children():
			print(child.name)

func _process(delta: float) -> void:
	#print_children_names($Players, delta)
	pass

func _PC(id):
	print('Oyuncu Girdi : '+str(id))
	rpc_id(id, "ReceivePlayerList", Datas.keys()) # this is for send old user data for new user!!
	connected_peers.append(id)

func _PD(id):
	print('Oyuncu Çıktı : ' + str(id))
	connected_peers.erase(id)

	var player_name_to_remove = null
	for name in Datas.keys():
		if Datas[name].peer_id == id:
			player_name_to_remove = name
			break

	if player_name_to_remove:
		Datas.erase(player_name_to_remove)
		if $Players.has_node(player_name_to_remove):
			$Players.get_node(player_name_to_remove).queue_free()

	if connected_peers.size() == 0:
		print("Son oyuncu çıktı, sunucu kapanıyor...")
		#get_tree().quit()

@rpc("any_peer")
func PlayerCreate(_Name):
	var Player = preload('res://player.tscn').instantiate()
	Player._Name = _Name
	Player.name = _Name
	$Players.add_child(Player)

@rpc("authority")
func ReceivePlayerList(player_names):
	for name in player_names:
		if not $Players.has_node(name):
			var player = preload("res://player.tscn").instantiate()
			player.name = name
			$Players.add_child(player)
