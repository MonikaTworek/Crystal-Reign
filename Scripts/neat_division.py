import bpy

#open script in Blender Text Editor and play 'Run Script'

import bpy
import numpy
import random

def cut_to_chunks(source_shape, cutting_shapes):
    new_obs = []
    for i in range(4):
        new_obj = source_shape.copy()
        new_obj.data = source_shape.data.copy()
        new_obj.animation_data_clear()
        bpy.context.scene.objects.link(new_obj)

        cut = new_obj.modifiers.new(name='Cut1', type='BOOLEAN')
        cut.object = cutting_shapes[i]
        cut.operation= 'INTERSECT'
        bpy.context.scene.objects.active = new_obj
        bpy.ops.object.modifier_apply(apply_as='DATA', modifier = 'Cut1')
        
        new_obs.append(new_obj)
    
    return new_obs

max_dimension = 1;
min_dimension = 0.05

#get main object
obj = bpy.context.selected_objects[0]
bpy.context.scene.objects.active = obj
bpy.ops.object.transform_apply( rotation = True )
bpy.ops.object.origin_set(type='ORIGIN_GEOMETRY', center='MEDIAN')

#create cutting shape
bpy.ops.import_scene.fbx( filepath = "pyramid.fbx" )
pyramid = [bpy.context.scene.objects['pyramid_1'], bpy.context.scene.objects['pyramid_2'], bpy.context.scene.objects['pyramid_3'],
bpy.context.scene.objects['pyramid_4']]

#chunks array
chunks = []
to_divide = [obj]

#cutting
while len(to_divide) > 0:
    future_append = []
    future_remove = []
    for ob in to_divide:        
        pyramid[0].rotation_euler = (random.uniform(0, 3.14), random.uniform(0, 3.14), random.uniform(0, 3.14))
        pyramid[0].location = ob.location
        
        obs = cut_to_chunks(ob, pyramid)
        
        future_remove.append(ob)
        bpy.ops.object.select_all(action='DESELECT')
        ob.select = True
        bpy.context.scene.objects.active = ob
        bpy.ops.object.delete() 
        
        for i in range(4):
            bpy.ops.object.select_all(action='DESELECT')
            obs[i].select = True
            bpy.context.scene.objects.active = obs[i]
            bpy.ops.object.origin_set(type='ORIGIN_GEOMETRY', center='MEDIAN')
            if min(obs[i].dimensions) < min_dimension:
                bpy.ops.object.delete()
            elif max(obs[i].dimensions) < max_dimension:
                chunks.append(obs[i])
            else:
                future_append.append(obs[i])
    to_divide = [x for x in to_divide if x not in future_remove]
    to_divide.extend(future_append)
    
#delete mega shape
bpy.ops.object.select_all(action='DESELECT')
pyramid[0].select = True
pyramid[1].select = True
pyramid[2].select = True
pyramid[3].select = True
bpy.ops.object.delete() 