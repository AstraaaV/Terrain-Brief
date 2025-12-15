extends MeshInstance3D

@export var size := 50
@export var height_scale: float = 5.0
@export var noise_scale: float = 20.0
@export var octaves: int = 4
@export var lacunarity: float = 2.0
@export var gain: float = 0.5

var noise := FastNoiseLite.new()

# Called when the node enters the scene tree for the first time.
func _ready():
	noise.noise_type = FastNoiseLite.TYPE_PERLIN
	noise.fractal_octaves = octaves
	noise.fractal_lacunarity = lacunarity
	noise.fractal_gain = gain
	noise.seed = randi()
	
	generate_terrain()

func generate_terrain():
	var st = SurfaceTool.new()
	st.begin(Mesh.PRIMITIVE_TRIANGLES)
	
	for x in range(size - 1):
		for z in range(size - 1):
			var h1 = noise.get_noise_2d(x * noise_scale, z * noise_scale) * height_scale
			var h2 = noise.get_noise_2d((x + 1) * noise_scale, z * noise_scale) * height_scale
			var h3 = noise.get_noise_2d(x * noise_scale, (z + 1) * noise_scale) * height_scale
			var h4 = noise.get_noise_2d((x + 1) * noise_scale, (z + 1) * noise_scale) * height_scale
			
			var v1 = Vector3(x, h1, z)
			var v2 = Vector3(x + 1, h2, z)
			var v3 = Vector3(x, h3, z + 1)
			var v4 = Vector3(x + 1, h4, z + 1)
			
			st.add_vertex(v1)
			st.add_vertex(v2)
			st.add_vertex(v3)
			
			st.add_vertex(v2)
			st.add_vertex(v4)
			st.add_vertex(v3)
	st.generate_normals()
	
	mesh = st.commit()

func _on_button_pressed():
	noise.seed = randi()
	generate_terrain()
