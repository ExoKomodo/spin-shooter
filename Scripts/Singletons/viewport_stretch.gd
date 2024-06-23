extends Node

@onready var viewport = get_viewport()

var minimum_size = Vector2(360, 640)

func _ready():
	viewport.connect('size_changed', Callable(self, 'window_resize'))
	window_resize()

func window_resize():
	var current_size = get_window().get_size()

	var scale_factor = minimum_size.y / current_size.y
	var new_size = Vector2(current_size.x * scale_factor, minimum_size.y)

	if new_size.x < minimum_size.x:
		scale_factor = minimum_size.x / new_size.x
		new_size = Vector2(minimum_size.x, new_size.y * scale_factor)
	if new_size.y < minimum_size.y:
		scale_factor = minimum_size.y / new_size.y
		new_size = Vector2(new_size.x * scale_factor, minimum_size.y)

	viewport.set_size(new_size)
