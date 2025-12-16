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

# Called when the node enters the scene tree for the first time.
func _ready():
	# Configure noise generator
	noise.noise_type = FastNoiseLite.TYPE_PERLIN
	noise.fractal_octaves = octaves
	noise.fractal_lacunarity = lacunarity
	noise.fractal_gain = gain
	
	# Uses random seed
	noise.seed = randi()
	
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
			st.add_vertex(v1)
			st.add_vertex(v2)
			st.add_vertex(v3)
			
			# Second triangle
			st.add_vertex(v2)
			st.add_vertex(v4)
			st.add_vertex(v3)
	# Auto gens normals for lighting
	st.generate_normals()
	
	# Clears old mesh and assigns new one
	mesh = null
	mesh = st.commit()

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
