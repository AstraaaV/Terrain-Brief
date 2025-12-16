extends MeshInstance3D
# This script procedurally generates terrain

# Exposed parameters
# The below variabes are exposed to the Inspector
# so a designer can tweak terrain without modifying code.
@export var size := 50 # width / depth
@export var height_scale: float = 5.0 # max height
@export var noise_scale: float = 20.0 # controls noise frequency
@export var octaves: int = 4 # number of noise layers
@export var lacunarity: float = 2.0 # frequency multiplier between octaves
@export var gain: float = 0.5 # amplitude reduction between octaves

# Used to generate Perlin Noise
var noise := FastNoiseLite.new()
var terrain_material: StandardMaterial3D

# Called when the node enters the scene tree for the first time.
func _ready():
	# Configure noise generator
	noise.noise_type = FastNoiseLite.TYPE_PERLIN
	noise.fractal_octaves = octaves
	noise.fractal_lacunarity = lacunarity
	noise.fractal_gain = gain
	
	# Uses random seed
	noise.seed = randi()
	
	terrain_material = StandardMaterial3D.new()
	terrain_material.vertex_color_use_as_albedo = true
	
	# Generate terrain mesh
	generate_terrain()

# Terrain generation - builds terrain mesh using triangle geometry
# and height values sampled from Perlin Noise.
func generate_terrain():
	var st = SurfaceTool.new()
	st.begin(Mesh.PRIMITIVE_TRIANGLES)
	
	# Loops over 2D grid to gen vertices
	for x in range(size - 1):
		for z in range(size - 1):
			# Sample noise vals for each corner of quad
			var h1 = noise.get_noise_2d(x * noise_scale, z * noise_scale) * height_scale
			var h2 = noise.get_noise_2d((x + 1) * noise_scale, z * noise_scale) * height_scale
			var h3 = noise.get_noise_2d(x * noise_scale, (z + 1) * noise_scale) * height_scale
			var h4 = noise.get_noise_2d((x + 1) * noise_scale, (z + 1) * noise_scale) * height_scale
			
			# Converts height into positions
			var v1 = Vector3(x, h1, z)
			var v2 = Vector3(x + 1, h2, z)
			var v3 = Vector3(x, h3, z + 1)
			var v4 = Vector3(x + 1, h4, z + 1)
			
			# First triamgle
			# Assign colours based on height
			st.set_color(height_to_color(h1))
			st.add_vertex(v1)
			
			st.set_color(height_to_color(h2))
			st.add_vertex(v2)
			
			st.set_color(height_to_color(h3))
			st.add_vertex(v3)
			
			# Second triangle
			st.set_color(height_to_color(h2))
			st.add_vertex(v2)
			
			st.set_color(height_to_color(h4))
			st.add_vertex(v4)
			
			st.set_color(height_to_color(h3))
			st.add_vertex(v3)
	# Auto gens normals for lighting
	st.generate_normals()
	
	# Clears old mesh and assigns new one
	mesh = null
	mesh = st.commit()
	
	# Applies material so vertex colours are visible
	mesh.surface_set_material(0, terrain_material)

# Height -> colour mapping

func height_to_color(height: float) -> Color:
	var h = height / height_scale
	
	# Water
	if h < -0.2:
		return Color(0.1, 0.3, 0.8) # Blue
		
	# Sand
	elif h < 0.0:
		return Color(0.8, 0.7, 0.4) # Sandy
			
	# Grass
	elif h < 0.4:
		return Color(0.2, 0.6, 0.2)	# Green
	
	# Rock
	elif h < 0.7:
		return Color(0.5, 0.5, 0.5) # Greyish
		
	# Snow
	else:
		return Color(1.0, 1.0, 1.0) # White

# UI interaction - called when regenerate button is pressed
func _on_button_pressed():
	print("BUTTON PRESSED")
	
	# Changes noise seed again
	noise.seed = randi()
	
	# Reapply noise settings
	noise.fractal_octaves = octaves
	noise.fractal_lacunarity = lacunarity
	noise.fractal_gain = gain
	
	# Regenerate terrain mesh
	generate_terrain()
