# Water Shader
> Written and Implemented by Chuah Xin Yu

The other HLSL/CG shader that we would like to be marked on is the water shader. This shader is used as part of the plane prefab for one of the levels in the game.

## Asset Files
> List of exact paths to respective shader asset files
* `./Assets/Shaders/Water.shader` The shader code, written in HLSL
* `./Assets/Shaders`

## Attributes

## Breakdown
* This shader implements botj the `vert()` and `frag()` functions
  * The Vertex Shader determines the porition of the vertices and is used to create the wave movement
  * the pixel/fragment shader determines the colour and transparency of each pixel

### Tags and SubShader Properties
```c
...
SubShader
{
    Tags { 
        "Queue" = "Transparent" //change queue to transparent
        "RenderType" = "Transparent"
    }
    // display destination color without modify the source color
    Blend SrcAlpha OneMinusSrcAlpha
    // ZWrite Off is for drawing semitransparent effect
    ZWrite Off
    // Disable Culling, to show all faces of the object
    Cull Off
```
* `Blend SrcAlpha OneMinusSrcAlpha` is particularly important for this shader is we wanted the water to have be transparent. Without this, the destination colour usually starts off as (0f, 0f, 0f, 0f), which is black, which is not what we want. We want this shader to add to the current source colour, which is what this line does.
* TODO Image: Shader without `Blend SrcAlpha OneMinusSrcAlpha`

### Vertex Shader
* TODO


### Fragment Shader
* TODO