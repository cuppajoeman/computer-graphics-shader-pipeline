// Set the pixel color using Blinn-Phong shading (e.g., with constant blue and
// gray material color) with a bumpy texture.
// 
// Uniforms:
uniform mat4 view;
uniform mat4 proj;
uniform float animation_seconds;
uniform bool is_moon;
// Inputs:
//                     linearly interpolated from tessellation evaluation shader
//                     output
in vec3 sphere_fs_in;
in vec3 normal_fs_in;
in vec4 pos_fs_in; 
in vec4 view_pos_fs_in; 
// Outputs:
//               rgb color of this pixel
out vec3 color;
// expects: model, blinn_phong, bump_height, bump_position,
// improved_perlin_noise, tangent
void main()
{
  float light_rot_angle = (2 * M_PI * animation_seconds) / 8;  

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

	
  // use bump_position and tangent to calculate normal map, see assignment readme for reference
  vec3 tang, bitang;
  float epsilon = 0.00001;
  tangent(sphere_fs_in, tang, bitang);
  vec3 del_p_t = bump_position(is_moon, sphere_fs_in + epsilon * tang) - bump_position(is_moon, sphere_fs_in);
  vec3 del_p_b = bump_position(is_moon, sphere_fs_in + epsilon * bitang) - bump_position(is_moon, sphere_fs_in);

  vec3 normal = normalize(cross(del_p_t/epsilon, del_p_b/epsilon));

  mat4 model = model(is_moon, animation_seconds);
   
  mat4 normal_matrix = transpose(inverse(view * model));
  vec4 transformed_normal = normal_matrix * vec4(normal, 1);
  vec3 n = normalize(transformed_normal.xyz);

  float noise = 1;

  // full spec
  vec3 ks = vec3(1.0, 1.0, 1.0);

  // shininess
  int p = 800;

  // grey moon or blue planet
  vec3 kd, ka;
  if(is_moon){
    kd = vec3(0.5, 0.5, 0.5) * noise;
    ka = vec3(0.10, 0.10, 0.10);
  } else{
    kd = vec3(0.1, 0.2, 0.7) * .25;
    ka = vec3(0.05, 0.2, 0.35);
  }
    color = blinn_phong(ka, kd, ks, p, n, vector_from_surface_to_cam, light_dir); 
}
