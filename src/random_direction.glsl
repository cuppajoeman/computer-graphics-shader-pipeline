// Generate a pseudorandom unit 3D vector
// 
// Inputs:
//   seed  3D seed
// Returns psuedorandom, unit 3D vector drawn from uniform distribution over
// the unit sphere (assuming random2 is uniform over [0,1]²).
//
// expects: random2.glsl, PI.glsl
vec3 random_direction( vec3 seed)
{
  vec2 rand_vec = random2(seed);
  float yaw = 2 * M_PI * rand_vec.x;
  float pitch = M_PI * rand_vec.y;
  // get a unit vector pointing to a place on the unit sphere
  float x = -cos(yaw) * sin(pitch);
  float y = -sin(yaw) * sin(pitch);
  float z = -cos(pitch);
  // double normalizing to be sure, redundant in theory
  return normalize(vec3(x, y, z));
}
