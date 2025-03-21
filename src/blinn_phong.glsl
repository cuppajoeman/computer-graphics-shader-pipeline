// Compute Blinn-Phong Shading given a material specification, a point on a
// surface and a light direction. Assume the light is white and has a low
// ambient intensity.
//
// Inputs:
//   ka  rgb ambient color
//   kd  rgb diffuse color
//   ks  rgb specular color
//   p  specular exponent (shininess)
//   n  unit surface normal direction
//   v  unit direction from point on object to eye
//   l  unit light direction
// Returns rgb color
vec3 blinn_phong(
  vec3 ka,
  vec3 kd,
  vec3 ks,
  float p,
  vec3 n,
  vec3 v,
  vec3 l)
{
  vec3 halfway_vec = normalize(v+l); 
  float diffuse_reflect_amount = max(0,dot(n,l));
  vec3 dra_vec = vec3(1, 1, 1) * diffuse_reflect_amount;
  vec3 view_independent_light = ka + kd * dra_vec;
  float spec_reflect_amount = pow(max(0,dot(n,halfway_vec)),p);
  vec3 sra_vec = vec3(1, 1, 1) * spec_reflect_amount;
  vec3 spec_light = ks * sra_vec;
  vec3 light = view_independent_light + spec_light;
  return light;
}


