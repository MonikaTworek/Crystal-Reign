#open script in Blender Text Editor and play 'Run Script'

import bpy
import numpy
import random

def cut_new_chunk(source_shape, cutting_shape):
    new_obj = source_shape.copy()
    new_obj.data = source_shape.data.copy()
    new_obj.animation_data_clear()
    bpy.context.scene.objects.link(new_obj)

    cut1 = source_shape.modifiers.new(name='Cut1', type='BOOLEAN')
    cut1.object = cutting_shape
    cut1.operation= 'DIFFERENCE'
    bpy.context.scene.objects.active = source_shape
    bpy.ops.object.modifier_apply(apply_as='DATA', modifier = 'Cut1')

    cut2 = new_obj.modifiers.new(name='Cut2', type='BOOLEAN')
    cut2.object = cutting_shape
    cut2.operation= 'INTERSECT'
    bpy.context.scene.objects.active = new_obj
    bpy.ops.object.modifier_apply(apply_as='DATA', modifier = 'Cut2')
    
    return new_obj

max_dimension = 1.5;
min_dimension = 0.05

#get main object
obj = bpy.context.selected_objects[0]
bpy.context.scene.objects.active = obj
bpy.ops.object.transform_apply( rotation = True )
bpy.ops.object.origin_set(type='ORIGIN_GEOMETRY', center='MEDIAN')

#create cutting cube
bpy.ops.mesh.primitive_cube_add()
cube = bpy.context.scene.objects.active 
cube.scale = (100, 100, 100)
bpy.context.scene.cursor_location = (cube.location.x, cube.location.y, cube.location.z - 100)
bpy.ops.object.origin_set(type='ORIGIN_CURSOR')

#chunks array
chunks = []
to_divide = [obj]

#cutting
while len(to_divide) > 0:
    future_append = []
    future_remove = []
    for ob in to_divide:        
        cube.rotation_euler = (random.uniform(0, 3.14), random.uniform(0, 3.14), random.uniform(0, 3.14))
        cube.location = ob.location
        
        ob2 = cut_new_chunk(ob, cube)
        
        bpy.ops.object.select_all(action='DESELECT')
        ob.select = True
        bpy.context.scene.objects.active = ob
        bpy.ops.object.origin_set(type='ORIGIN_GEOMETRY', center='MEDIAN')
        if min(ob.dimensions) < min_dimension:
            future_remove.append(ob)
            bpy.ops.object.delete() 
        elif max(ob.dimensions) < max_dimension:
            future_remove.append(ob)
            chunks.append(ob)
        
        bpy.ops.object.select_all(action='DESELECT')
        ob2.select = True
        bpy.context.scene.objects.active = ob2
        bpy.ops.object.origin_set(type='ORIGIN_GEOMETRY', center='MEDIAN')
        if min(ob2.dimensions) < min_dimension:
            bpy.ops.object.delete() 
        elif max(ob.dimensions) < max_dimension:
            chunks.append(ob2)
        else:
            future_append.append(ob2)
    to_divide = [x for x in to_divide if x not in future_remove]
    to_divide.extend(future_append)
    
#delete mega cube
bpy.ops.object.select_all(action='DESELECT')
cube.select = True
bpy.ops.object.delete() 