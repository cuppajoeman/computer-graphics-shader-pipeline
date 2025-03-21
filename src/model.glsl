// Construct the model transformation matrix. The moon should orbit around the
// origin. The other object should stay still.
//
// Inputs:
//   is_moon  whether we're considering the moon
//   time  seconds on animation clock
// Returns affine model transformation as 4x4 matrix
//
// expects: identity, rotate_about_y, translate, PI
mat4 model(bool is_moon, float time)
{
  if (is_moon) {
    float yaw_angle = (2 * M_PI * time) / 8;
    mat4 shift = translate(vec3(2, 0, 0));
    mat4 transform = rotate_about_y(yaw_angle) * shift * uniform_scale(0.2);
    return transform;
  }
  // as we all know the earth is the center of the universe, thus:
  return identity();
}
