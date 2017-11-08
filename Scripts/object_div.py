
import bpy
import re
import json

def round_down(value, step):
    return value - value%step

def cut_new_chunk(source_shape, big_cube):
    new_obj = source_shape.copy()
    new_obj.data = source_shape.data.copy()
    new_obj.animation_data_clear()
    bpy.context.scene.objects.link(new_obj)

    cut1 = source_shape.modifiers.new(name='Cut1', type='BOOLEAN')
    cut1.object = big_cube
    cut1.operation= 'DIFFERENCE'
    cut1.solver = 'CARVE'
    bpy.context.scene.objects.active = source_shape
    bpy.ops.object.modifier_apply(apply_as='DATA', modifier = 'Cut1')

    cut2 = new_obj.modifiers.new(name='Cut2', type='BOOLEAN')
    cut2.object = big_cube
    cut2.operation= 'INTERSECT'
    cut2.solver = 'CARVE'
    bpy.context.scene.objects.active = new_obj
    bpy.ops.object.modifier_apply(apply_as='DATA', modifier = 'Cut2')
    
    return new_obj


step = 0.7
cube_radius = 100

# get main object
obj = bpy.context.scene.objects.active
bpy.ops.object.transform_apply(rotation=True)
parent_obj_name = obj.data.name

# create mega cube
center = obj.location
size_z = obj.dimensions.z
size_y = obj.dimensions.y
size_x = obj.dimensions.x

bpy.ops.mesh.primitive_cube_add(radius=cube_radius)
cube = bpy.context.scene.objects.active

# chunks array
chunks = []

# dividing with z-translation
z_pos = round_down(center.z + size_z / 2, step) + cube_radius
cube.location = (center.x, center.y, z_pos)

while cube.location.z - cube_radius > center.z - size_z / 2:
    chunks.append(cut_new_chunk(obj, cube))
    cube.location.z -= step

chunks.append(obj)

# dividing with y-translation
y_pos = round_down(center.y + size_y / 2,step) + cube_radius
cube.location = (center.x, y_pos, center.z)

chunks_number = len(chunks)
while cube.location.y - cube_radius > center.y - size_y / 2:
    for k in range(0, chunks_number):
        chunks.append(cut_new_chunk(chunks[k], cube))
    cube.location.y -= step

# dividing with x-translation
x_pos = round_down(center.x + size_x / 2,step) + cube_radius
cube.location = (x_pos, center.y, center.z)

chunks_number = len(chunks)
while cube.location.x - cube_radius > center.x - size_x / 2:
    for k in range(0, chunks_number):
        chunks.append(cut_new_chunk(chunks[k], cube))
    cube.location.x -= step

new_chunks = []
bpy.ops.object.select_all(action='DESELECT')
for chunk in chunks:
    if len(chunk.data.vertices) <= 3:
        chunk.select = True
    else: 
        new_chunks.append(chunk)
bpy.ops.object.delete()

chunks = new_chunks   

# delete mega cube
bpy.ops.object.select_all(action='DESELECT')
cube.select = True
bpy.ops.object.delete()

# output array
out = [[[
    "none" for ___ in range(int(size_y / step) + 2)]
    for __ in range(int(size_z / step) + 2)]
    for _ in range(int(size_x / step) + 2)]

eps = 0.0001
origin = {'x': round_down(center.x - size_x / 2,step),
          'y': round_down(center.z - size_z / 2,step),
          'z': round_down(center.y - size_y / 2,step)}

for i, chunk in enumerate(chunks):
    chunk.data.name = chunk.name = parent_obj_name + "_c" + str(i)
    bpy.ops.object.select_all(action='DESELECT')
    bpy.context.scene.objects.active = chunk
    chunk.select = True
    bpy.ops.object.origin_set(type='ORIGIN_GEOMETRY', center='MEDIAN')
    bpy.context.scene.cursor_location = (
        round_down(chunk.location.x,step) + step / 2, round_down(chunk.location.y,step) + step / 2, round_down(chunk.location.z,step) + step / 2)
    bpy.ops.object.origin_set(type='ORIGIN_CURSOR')
    
    
    iscube = True
    for v in chunk.data.vertices:
        if abs(v.co.x) < step / 2 - eps and \
                        abs(v.co.y) < step / 2 - eps and \
                        abs(v.co.z) < step / 2 - eps:
            iscube = False
            break
    
    for i in [-1, 1]:
        for j in [-1, 1]:
            for k in [-1, 1]:
                exists = False
                for v in chunk.data.vertices:
                    if i*(v.co[0]) >= step/2 - eps and \
                    j*(v.co[1]) >= step/2 - eps and \
                    k*(v.co[2]) >= step/2 - eps:
                        exists = True
                        break
                if not exists:
                    iscube = False;
        
    name = "cube"
    if not iscube:
        name = chunk.data.name
    out[int((chunk.location.x - origin['x']) / step)] \
        [int((chunk.location.z - origin['y']) / step)] \
        [int((chunk.location.y - origin['z']) / step)] = name
origin['x'] += step / 2
origin['y'] += step / 2
origin['z'] += step / 2

data = {"origin": origin,
        "map": out,
        "unit_size": {"x": step, "y": step, "z": step}}
file = open("D:\\University\\CrystalReign\\test.json", "w+")
file.write(json.dumps(data))
file.close()