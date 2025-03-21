// Add (hard code) an orbiting (point or directional) light to the scene. Light
// the scene using the Blinn-Phong Lighting Model.
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
// expects: PI, blinn_phong
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
  
  // full spec
  vec3 ks = vec3(1.0, 1.0, 1.0);

  // shininess
  int p = 800;

  // grey moon or blue planet
  vec3 kd, ka;
  if(is_moon){
    ka = vec3(0.10, 0.10, 0.10);
    kd = vec3(0.5, 0.5, 0.5);
  } else{
    ka = vec3(0.05, 0.1, 0.35);
    kd = vec3(0.1, 0.2, 0.7);
  }

  color = blinn_phong(ka, kd, ks, p, surface_normal, vector_from_surface_to_cam, light_dir); 
}
