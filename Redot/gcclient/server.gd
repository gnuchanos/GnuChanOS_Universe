extends Node2D


var Enet = ENetMultiplayerPeer.new()
var Name_ = ''
var ID_ = 0

@rpc("any_peer")
func Login(PlayerName, _ID):
	var sender_id = multiplayer.get_remote_sender_id()
	# Aynı isimde oyuncu varsa cevap dön
	for p in $Players.get_children():
		if p.name == PlayerName:
			rpc_id(sender_id, "BackMessage", PlayerName, _ID, 1)  # Zaten var
			return

@rpc("any_peer")
func BackMessage(PlayerName,_ID,No):
	if PlayerName == Name_ and _ID == ID_:
		if No == 1:
			$CanvasLayer/ColorRect/debug.text = 'Oyuncu Zaten Kayıtlı Amına Koduğum!'
		elif No == 0:
			var Player = preload('res://player.tscn').instantiate()
			Player._Name = PlayerName
			Global.Name = PlayerName
			Player.name = PlayerName
			$Players.add_child(Player)
			$CanvasLayer/ColorRect.visible = false
			rpc("PlayerCreate", PlayerName)  # Burada rpc çağrısı

func _ready() -> void:
	Enet.create_client("172.27.131.20", 2025)
	multiplayer.multiplayer_peer = Enet

	multiplayer.peer_disconnected.connect(_on_server_disconnected)
	multiplayer.connection_failed.connect(_on_connection_failed)

func _on_server_disconnected(id):
	print("Sunucu bağlantısı kesildi, istemci kapanıyor...")
	#get_tree().quit()

func _on_connection_failed():
	print("Sunucuya bağlanılamadı, istemci kapanıyor...")
	#get_tree().quit()

@rpc("any_peer")
func PlayerCreate(_Name):
	var Player = preload('res://player.tscn').instantiate()
	Player._Name = _Name
	Player.name = _Name
	$Players.add_child(Player)

func _on_button_pressed() -> void:
	if multiplayer.multiplayer_peer == null:
		print("Multiplayer peer hazır değil, bağlantı kurulmadı.")
		$CanvasLayer/ColorRect/debug.text = "Multiplayer peer hazır değil, bağlantı kurulmadı."
		return
	
	if multiplayer.multiplayer_peer.get_connection_status() != MultiplayerPeer.CONNECTION_CONNECTED:
		print("Henüz sunucuya bağlanmadınız!")
		$CanvasLayer/ColorRect/debug.text = "Henüz sunucuya bağlanmadınız!"
		return

	randomize()
	ID_ = randi_range(1, 10000)
	Name_ = $CanvasLayer/ColorRect/name.text
	rpc("Login", Name_, ID_)

@rpc("authority")
func ReceivePlayerList(player_names):
	for name in player_names:
		if not $Players.has_node(name):
			var player = preload("res://player.tscn").instantiate()
			player.name = name
			$Players.add_child(player)

# Game System
