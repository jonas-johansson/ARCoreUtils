# ARCoreUtils

Unity plugin that makes ARCore trackable planes receive shadows and provide collision.

## Getting Started

* Drag the ARSurfaceManager prefab into the scene.
* Add a white directional light in the scene, pointing downward.
* Deploy to device.

## Features

### Collision

It's helpful to have objects in the virtual scene collide with the ground of the real scene.

The ARCore SDK's trackable planes are essentially identified flat surfaces such as the ground or a tabletop. 

ARCoreUtils asks the SDK for a list of points in each trackable plane's boundary polygon (retrieved in clockwise order). A mesh is created for these points via triangulation. With the mesh ready, we create a GameObject and add a MeshCollider component that references it.

### Shadows

Casting shadows from objects in the virtual scene helps glue them to the real world.

The only problem with that is that there is no floor in the virtual scene that can receive shadows from the drone. Basically, we need to render an invisible floor that can receive shadow.

From what I know, there's no suitable shader in Unity out-of-the-box, so I decided to create a new one. I'll get back to that shader in a bit, but first: we need a light source that casts shadows. Make sure you have a white directional light in the virtual scene, pointing downward. It's not a perfect representation of the real scene light, of course, but most light come from above, so it'll do for now.

A MeshRenderer and a MeshFilter is created for all trackable planes, using the same mesh we created for the collision previously. The MeshRenderer uses a material with the new shader.

#### The ARSurface shader
##### Desired result
* Final pixel color = real world pixel color (from camera) * virtual light at this pixel (lit/shadowed).

##### Vertex shader
* Just pass through data.

##### Fragment shader
* Calculate the amount of virtual light hitting this fragment.
* White when not in shadow.
* Black when in shadow.

##### Blending
Syntax in Unity is “Blend SrcFactor DstFactor”.
* The generated color is multiplied by SrcFactor.
* The color already on screen is multiplied by DstFactor.
* The two are added together.

To accomplish our goal we can:
* Multiply source (virtual floor surface color) with Zero.
* Multiply destination (real world color) with SrcColor (the color of our virtual plane - white/black depending on lit/shadowed).
* The two are added together. Since the first one is Zero, only the second part contributes.

The blending is therefore set up as “Blend Zero SrcColor”.

The best way to wrap your head around this is to turn blending off and create a plane in the scene. Inspect it, and imagine that the color of the plane will be multiplied with the background.

Note that the shadow strength can be lowered to make it a bit softer, the math still holds up.



Happy hacking!