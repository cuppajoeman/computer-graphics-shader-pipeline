// Given a 3d position as a seed, compute an even smoother procedural noise
// value. "Improving Noise" [Perlin 2002].
//
// Inputs:
//   st  3D seed
// Values between  -½ and ½ ?
//
// expects: random_direction, improved_smooth_step
float improved_perlin_noise( vec3 st) 
{
  vec3 floor_pos = floor(st);  // position rounded down to nearest integer

  // define the corner points of the cube (in 3d) surrounding the input position
  vec3 corner1 = floor_pos;                                     // bottom-front-left corner
  vec3 corner2 = floor_pos + vec3(0.0, 0.0, 1.0);               // bottom-front-right corner
  vec3 corner3 = floor_pos + vec3(0.0, 1.0, 0.0);               // top-front-left corner
  vec3 corner4 = floor_pos + vec3(0.0, 1.0, 1.0);               // top-front-right corner
  vec3 corner5 = floor_pos + vec3(1.0, 0.0, 0.0);               // bottom-back-left corner
  vec3 corner6 = floor_pos + vec3(1.0, 0.0, 1.0);               // bottom-back-right corner
  vec3 corner7 = floor_pos + vec3(1.0, 1.0, 0.0);               // top-back-left corner
  vec3 corner8 = floor_pos + vec3(1.0, 1.0, 1.0);               // top-back-right corner

  // generate random gradient vectors for each corner of the cube
  vec3 grad1 = random_direction(corner1);  
  vec3 grad2 = random_direction(corner2);  
  vec3 grad3 = random_direction(corner3);  
  vec3 grad4 = random_direction(corner4);  
  vec3 grad5 = random_direction(corner5);  
  vec3 grad6 = random_direction(corner6);  
  vec3 grad7 = random_direction(corner7);  
  vec3 grad8 = random_direction(corner8);  

  // fractional component (remainder after the floor operation)
  // note that the components in this vector are in the range [0, 1)
  vec3 frac = fract(st);

  // calculate dot products between gradient vectors and their respective offsets 
  // this corresponds to how much influence each gradient will have on the input point st
  float dot1 = dot(grad1, frac);  // dot product at corner 1
  float dot2 = dot(grad2, frac - vec3(0.0, 0.0, 1.0));  // dot product at corner 2
  float dot3 = dot(grad3, frac - vec3(0.0, 1.0, 0.0));  // dot product at corner 3
  float dot4 = dot(grad4, frac - vec3(0.0, 1.0, 1.0));  // dot product at corner 4
  float dot5 = dot(grad5, frac - vec3(1.0, 0.0, 0.0));  // dot product at corner 5
  float dot6 = dot(grad6, frac - vec3(1.0, 0.0, 1.0));  // dot product at corner 6
  float dot7 = dot(grad7, frac - vec3(1.0, 1.0, 0.0));  // dot product at corner 7
  float dot8 = dot(grad8, frac - vec3(1.0, 1.0, 1.0));  // dot product at corner 8

  vec3 smoothfactor = improved_smooth_step(frac);  

  // note that mix is linear interpolation, here we implement trilinear interpolation
  float mixx1 = mix(dot1, dot5, smoothfactor.x); 
  float mixx2 = mix(dot2, dot6, smoothfactor.x); 
  float mixx3 = mix(dot3, dot7, smoothfactor.x); 
  float mixx4 = mix(dot4, dot8, smoothfactor.x); 

  float mixy1 = mix(mixx1, mixx3, smoothfactor.y);
  float mixy2 = mix(mixx2, mixx4, smoothfactor.y);

  float finalnoise = mix(mixy1, mixy2, smoothfactor.z);

  return finalnoise - 0.5;  // normalize the output to the range [-1/2, 1/2] ok.... idk why though
}

