// Create a bumpy surface by using procedural noise to generate a height (
// displacement in normal direction).
//
// Inputs:
//   is_moon  whether we're looking at the moon or centre planet
//   s  3D position of seed for noise generation
// Returns elevation adjust along normal (values between -0.1 and 0.1 are
//   reasonable.
float bump_height( bool is_moon, vec3 s)
{
  if (is_moon){
    float moon_bump_rate = 1;
    float moon_bump = improved_perlin_noise(s * moon_bump_rate);
    // normalize to (-1, 1)
    return smooth_heaviside(moon_bump, 0.5);
  } else {
    float earth_bump_rate = 5;
    float earth_bump = improved_perlin_noise(s * earth_bump_rate);
    // normalize to (-1, 1)
    return smooth_heaviside(earth_bump, 0.5);
  }
}
