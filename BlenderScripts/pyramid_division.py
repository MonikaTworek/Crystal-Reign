#open script in Blender Text Editor and play 'Run Script'

import bpy
import json
import random
import copy
import os

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

def divide_to_size(obj, pyramid, max_dimension, min_dimension):
    chunks = []
    to_divide = []

    if min(obj.dimensions) < min_dimension:
        obj.select = True
        bpy.ops.object.delete()
    elif max(obj.dimensions) < max_dimension:
        chunks.append(obj)
    else:
        to_divide.append(obj)

    while len(to_divide) > 0:   
        cut_ok = False
        while not cut_ok:
            pyramid[0].rotation_euler = (random.uniform(0, 3.14), random.uniform(0, 3.14), random.uniform(0, 3.14))
            pyramid[0].location = to_divide[0].location
            obs = cut_to_chunks(to_divide[0], pyramid)
            cut_ok = True
            for i in range(4):
                if max(obs[i].dimensions) > max(to_divide[0].dimensions) or max(obs[i].dimensions) == 0:
                    cut_ok = False
            if not cut_ok:
                for i in range(4):
                    obs[i].select = True
                bpy.ops.object.delete()
        
        to_divide[0].select = True
        bpy.ops.object.delete()
        del to_divide[0]
        
        for i in range(4):
            obs[i].select = True
            bpy.ops.object.origin_set(type='ORIGIN_GEOMETRY', center='MEDIAN')
            bpy.ops.object.shade_flat()
            if min(obs[i].dimensions) < min_dimension:
                bpy.ops.object.delete()
            elif max(obs[i].dimensions) < max_dimension:
                chunks.append(obs[i])
                obs[i].select = False
            else:
                to_divide.append(obs[i])
                obs[i].select = False
    
    return chunks

#get main objects
main_directory = "D:/Blenderowe/Glass Ruin/"
if not os.path.exists(main_directory + "out"):
    os.makedirs(main_directory + "out")

main_objs = copy.copy(bpy.context.selected_objects)
bpy.ops.object.select_all(action='DESELECT')
for m in main_objs:
    if not os.path.exists(main_directory + "out/" + m.name):
        os.makedirs(main_directory + "out/" + m.name)
        os.makedirs(main_directory + "out/" + m.name + "/fbx")
        os.makedirs(main_directory + "out/" + m.name + "/json")
    
    if os.path.exists(main_directory + "out/" + m.name + "/" + m.name + "_done.txt"):
        m.select = True

main_objs = [x for x in main_objs if x not in bpy.context.selected_objects]
bpy.ops.object.delete()

#export originals
bpy.ops.object.select_all(action='DESELECT')
for m in main_objs:
    m.select = True
    bpy.ops.export_scene.fbx(filepath=main_directory + "out/" + m.name + "/fbx/" + m.name + ".fbx", use_selection = True)
    m.select = False

#create cutting shape
bpy.ops.import_scene.fbx( filepath = main_directory + "pyramid.fbx" )
pyramid = [bpy.context.scene.objects['pyramid_1'], bpy.context.scene.objects['pyramid_2'], bpy.context.scene.objects['pyramid_3'],
bpy.context.scene.objects['pyramid_4']]
bpy.ops.object.select_all(action='DESELECT')

#dividing----------------------------------------------------------------------------------------
for obj in main_objs:
    objects_array = [obj]
    current_obj_name = obj.name
    print("dividing ", current_obj_name, "-----------------------------------------------------------------------------------------------------------")
    
    #initial max_dimension
    max_dimension = 5;
    min_dimension = 0.01

    while max_dimension*3 < max(obj.dimensions):
        max_dimension *= 3
        min_dimension *= 3
        
    while max_dimension >= 5:
        new_objects_array = []
        for o in objects_array:
            basic_name = o.name
            center = copy.copy(o.location)
    		
            #crushing
            chunks = divide_to_size(o, pyramid, max_dimension, min_dimension)

            #renaming
            for i, chunk in enumerate(chunks):
                chunk.name = basic_name + "_" + str(i)

            #json export
            out = []
            for chunk in chunks:
                rel_pos = chunk.location - center
                out.append({"name": chunk.name, 
                            "relative_position": {'x': -rel_pos.x, 'y': rel_pos.z, 'z': -rel_pos.y}})
            data = {"chunks": out}
            file = open(main_directory + "out/" + current_obj_name + "/json/" + basic_name + "_description.json", "w+")
            file.write(json.dumps(data))
            file.close()
            
            #fbx export
            for chunk in chunks:
                chunk.select = True
            bpy.ops.export_scene.fbx(filepath=main_directory + "out/" + current_obj_name + "/fbx/" + basic_name + "_chunks.fbx", use_selection = True)
            
            #reorganization
            if max_dimension == 5:
                bpy.ops.object.delete()
            else:
                for chunk in chunks:
                    chunk.select = False
                new_objects_array.extend(chunks)
        
        objects_array = new_objects_array
        max_dimension /= 3
        min_dimension /= 3
    file = open(main_directory + "out/" + current_obj_name + "/" + current_obj_name + "_done.txt", "w+")
    file.close()

#delete mega shape
bpy.ops.object.select_all(action='DESELECT')
pyramid[0].select = True
pyramid[1].select = True
pyramid[2].select = True
pyramid[3].select = True
bpy.ops.object.delete() 