
# Particle Systems
> [🏠 README.md](../../README.md) | [⬅️ Evaluation Report](../evaluation/evaluation-report.md) | [➡️ Water Shader](../water-shader/water-shader.md)

A physics based particle system used to generate particles in-game.

## Table of Contents
- [Particle Systems](#particle-systems)
  - [Table of Contents](#table-of-contents)
  - [Asset Files](#asset-files)
  - [Attributes](#attributes)
  - [Breakdown](#breakdown)
    - [Calculating directional square](#calculating-directional-square)
    - [Sampling points on the direction square](#sampling-points-on-the-direction-square)
    - [Instantiating particles](#instantiating-particles)
    - [Controlling particle life span](#controlling-particle-life-span)

## Asset Files
* `./Assets/Scripts/ParticleManager.cs` Code used to generate particles. Used as a Singleton by statically assigning to a single game object.

## Attributes

    public  void  generateParticles (Vector3  initialPos, Vector3  direction, float  radius, float  force, int  numParticles)

- Initial position. A singular 3D point of where the particles will be coming from
- Direction. Direction in which particles will travel 
- Radius. A circle denoting the spread of the particles.
- Force. Amount of physical force applied to each particle.
- Number of particles. Number of particles spawned.
### Additional attributes
- Particle timeout. Lifespan of each particle
- Particle material. Material of each particle

## Breakdown
The particle system generates particles using Unity's Physics Engine. Particles travel in random directions, where each direction must be no more than X units away from the user-given "general" direction, where X is the radius specified by the user, and the direction could be any vector within the 3D world space. Hence, the larger the radius, the more sporadic the particles will be.

Users also have the option to adjust the force applied particles, where the stronger the force, the further particles travel.

In order to optimize memory usage, each particle can only exist for a specified period of time before they are destroyed.

### Calculating directional square

In order to facilitate variations and randomness in each particle's direction, the "direction plane", or the plane that is orthogonal to the direction vector, must be calculated. Furthermore, a square on the direction plane, whose center will be the direction vector's intersection with the plane, must also be defined. 

When particles spawn at the user-specified origin, it will travel towards a random point within the directional square.

For this particle system, the center of the square will be distanced 1 unit away from the user-specified origin. In other words, the direction vector's intercept with the direction plane will be 1 unit away from the origin. This distance can be adjusted, although it makes no functional difference in terms of particle's directions or the distance each particle will travel. Placing the direction plane further away from the origin will simply reduce the range of directions each particle can travel, which can be compensated by having a larger directional square.
```c#
// Calculate square on plane given by the direction vector

Vector3 N = direction.normalized;
Vector3 v1 = new Vector3(1, 1, 0);
v1.z = (-N.y - N.x) / N.z;
v1 = v1.normalized;
Vector3 v2 = Vector3.Cross(N, v1);
v2 = v2.normalized;
```
Here, v1 and v2 are direction vectors representing the X and Y axis of the direction square, with respect to the direction plane. Note that the X and Y axis are of "plane space", rather than "world space".

### Sampling points on the direction square

Once the direction square has been calculated, points that exist within the square must be sampled. Each point is used to calculate a single particle's direction of travel, where the direction is simply from the user-specified origin, to the sampled point. This ensures that all particles will travel within the direction square, yet maintains a certain level of variation in terms of each particle's direction.

With the direction square defined by its X and Y axis (two direction vectors), then all points on the square can be iterated by travelling across both axis at small steps to sample points. Of course, there is an infinite number of points within the square, hence the axis is traversed in small "steps". The size of each steps controls the granularity of the points being sampled. Smaller steps leads to more points, and vice versa. For this particle systems, the step size is adjusted dynamically according to the number of particles being generated, with the following formula

- `length = 2 * R / Sqrt(2 * nums)`, where R is the radius of the square and nums is the number of particles.

In practice, this means that more particles will lead to more points being sampled.

```c#
// Start position of particles
Vector3 center = initialPos + direction;
Vector3 v1Start = center - v1 * radius;
Vector3 v2Start = center - v2 * radius;

// Step size of each sample point
float sideLength = Mathf.Sqrt(2 * numParticles);

// Calculating the direction of each particle
for(int i = 0; i < sideLength; i++)
{
	Vector3 currV1 = v1Start + v1 * radius * 2 / sideLength * i;
	for(int j = 0; j < sideLength; j++)
	{
		Vector3 currV2 = v2Start + v2 * radius * 2 / sideLength * j;
		Vector3 currPos = currV2 + (currV1 - center);
		Vector3 dir = currPos - initialPos;
		dir = dir.normalized;
		dirs.Add(dir);
	}
}

// List of randomly selected sample points to generate particles
IEnumerable<Vector3> sampleDirs = dirs.OrderBy(x => rnd.Next()).Take(numParticles);
```
After the points are sampled, they are randomly shuffled and a certain number of points is taken out to be used to calculate particle directions. This ensures that the points being used is randomly sampled.

### Instantiating particles

Particles is created and force is applied using the direction calculated for each individual particle. Unity's Physics Engine controls the overall path each particle takes.

```c#
foreach(Vector3 dir in sampleDirs)
{
	GameObject particle = GameObject.CreatePrimitive(PrimitiveType.Cube);
	particle.transform.position = initialPos;
	particle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
	particle.GetComponent<Renderer>().material = particleMaterial;
	particle.AddComponent<Rigidbody>().mass = 0.5f;
	particle.GetComponent<Collider>().enabled = false;
	particle.GetComponent<Rigidbody>().AddForceAtPosition(dir * force, transform.position);
	particles.Add(new ParticleLifetime() { particle = particle, createdTime = Time.realtimeSinceStartupAsDouble });
}
```

### Controlling particle life span

In order to prevent memory leak, and to optimize game performance, each generated particle is only allowed to exist for a certain number of seconds, which is specified by the user before the particle system is created. After the time has passed, the particles will be destroyed.

```c#
// Go through every particle and check if they should be removed

struct ParticleLifetime
{
	public GameObject particle;
	public double createdTime;
}

for (int i = 0; i < particles.Count; i++)
{
	ParticleLifetime particleInfo = particles[i];
	if (Time.realtimeSinceStartupAsDouble - particleInfo.createdTime > this.particleTimeout)
	{
		Destroy(particleInfo.particle);
		particles.RemoveAt(i);
	}
	else
	{
		i++;
	}
}
```

> Written and Implemented by Bowen Feng