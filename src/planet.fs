// Generate a procedural planet and orbiting moon. Use layers of (improved)
// Perlin noise to generate planetary features such as vegetation, gaseous
// clouds, mountains, valleys, ice caps, rivers, oceans. Don't forget about the
// moon. Use `animation_seconds` in your noise input to create (periodic)
// temporal effects.
//
// Uniforms:
uniform mat4 view;
uniform mat4 proj;
uniform float animation_seconds;
uniform bool is_moon;
// Inputs:
in vec3 sphere_fs_in;
in vec3 normal_fs_in;
in vec4 pos_fs_in; 
in vec4 view_pos_fs_in; 
// Outputs:
out vec3 color;
// expects: model, blinn_phong, bump_height, bump_position,
// improved_perlin_noise, tangent
void main()
{
  float light_rot_angle = (2 * M_PI * animation_seconds) / 40;  

  mat4 Rmat = mat4(
    cos(light_rot_angle), 0, -sin(light_rot_angle), 0,
    0, 1, 0, 0,
    sin(light_rot_angle), 0, cos(light_rot_angle), 0,
    0, 0, 0, 1);
	
  vec3 vector_from_surface_to_cam = -((view * view_pos_fs_in).xyz);
  vec3 light_dir = (view * Rmat * vec4(1, 0.5, 0, 0)).xyz;
  vec3 surface_normal = normalize(normal_fs_in);

  vector_from_surface_to_cam = normalize(vector_from_surface_to_cam);
  light_dir = normalize(light_dir);

  float noise = (improved_perlin_noise(4 * sphere_fs_in) + 1) * 4;
  noise = abs(noise);  
	
  // use bump_position and tangent to calculate normal map, see assignment readme for reference
  vec3 tang, bitang;
  float epsilon = 0.00001;
  tangent(sphere_fs_in * animation_seconds, tang, bitang);
  vec3 del_p_t = bump_position(is_moon, sphere_fs_in + epsilon * tang) - bump_position(is_moon, sphere_fs_in);
  vec3 del_p_b = bump_position(is_moon, sphere_fs_in + epsilon * bitang) - bump_position(is_moon, sphere_fs_in);

  vec3 normal = normalize(cross(del_p_t/epsilon, del_p_b/epsilon));

  mat4 model = model(is_moon, animation_seconds);
   
  mat4 normal_matrix = transpose(inverse(view*model));
  vec4 transformed_normal = normal_matrix * vec4(normal, 1);
  vec3 n = normalize(transformed_normal.xyz) * noise * 1.5; // gives us "clouds"

  vec3 ka, ks, kd;
  int p;

  if (is_moon) {
    ka = vec3(0.05, 0.05, 0.05);
    kd = vec3(0.35, 0.35, 0.35);
    ks = vec3(0.5, 0.5, 0.5);
    p = 50;
  } else { // doing earth
    float bump_h = bump_height(is_moon, sphere_fs_in);
    float mountain_thresh = -0.25;
    float grass_thresh = -0.45;
    if (bump_h >= mountain_thresh){
      ka = vec3(0.23, 0.20, 0.20);
      kd = vec3(0.25, 0.25, 0.20);
      ks = vec3(0.80, 0.80, 0.80);
      p = 750;
    } else if (bump_h >= grass_thresh){
      ka = vec3(0.30, 0.70, 0.30);
      kd = vec3(0.30, 0.70, 0.30);
      ks = vec3(0.2, 0.2, 0.2);
      p = 200;
    } else { // water
      ka = vec3(0.01, 0.01, 0.12);
      kd = vec3(0.2, 0.4, 0.8);
      ks = vec3(1.0, 1.0, 1.0);
      p = 750;
    }
  }
  
  color = blinn_phong(ka, kd, ks, p, n, vector_from_surface_to_cam, light_dir); 
}
