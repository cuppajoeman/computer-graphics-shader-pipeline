// Input:
//   N  3D unit normal vector
// Outputs:
//   T  3D unit tangent vector
//   B  3D unit bitangent vector
void tangent(in vec3 N, out vec3 T, out vec3 B)
{
  // note that these are not actually going to be the tangent vectors, following the hint in the readme
  vec3 tangent_candidate1 = cross(N, vec3(1.0, 0.0, 0.0));
  vec3 tangent_candidate2 = cross(N, vec3(0.0, 1.0, 0.0));
  // avoid degenerate tangents
  if (length(tangent_candidate1) > length(tangent_candidate2)) {
      T = tangent_candidate1;
  } else {
      T = tangent_candidate2;
  }
  T = normalize(T);
  B = normalize(cross(T, N));
}
