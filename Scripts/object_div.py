bl_info = {
    "name": "Object Cutter",
    "description": "Cut object into grid",
    "author": "Creestoph & Bober",
    "version": (1),
    "blender": (2, 74, 0),
    "location": "Properties Editor > Object",
    "warning": "",
    "wiki_url": "",
    "tracker_url": "",
    "category": "Object"}

import bpy
import re
import numpy



def round_up(value, step):
    return step * (1 + (int)(value / step))

def cut_new_chunk(source_shape, big_cube):
    new_obj = source_shape.copy()
    new_obj.data = source_shape.data.copy()
    bpy.context.scene.objects.link(new_obj)
    bpy.context.scene.update()

    source_shape.select = True
    cut1 = source_shape.modifiers.new(name='Cut1', type='BOOLEAN')
    cut1.object = big_cube
    cut1.operation = 'DIFFERENCE'
    bpy.context.scene.objects.active = source_shape
    bpy.ops.object.modifier_apply(apply_as='DATA', modifier="Cut1")

    new_obj.select = True
    cut2 = new_obj.modifiers.new(name='Cut2', type='BOOLEAN')
    cut2.object = big_cube
    cut2.operation = 'INTERSECT'
    bpy.context.scene.objects.active = new_obj
    bpy.ops.object.modifier_apply(apply_as='DATA', modifier="Cut2")

    return new_obj


def cut(context):

    step = 0.7
    cube_radius=100

    # get main object
    obj = bpy.context.scene.objects.active
    bpy.ops.object.transform_apply(rotation=True)

    # create mega cube
    center = obj.location
    height = obj.dimensions.z
    width = obj.dimensions.y
    depth = obj.dimensions.x

    bpy.ops.mesh.primitive_cube_add(radius=cube_radius)
    cube = bpy.context.scene.objects.active

    # chunks array
    chunks = []

    # dividing with z-translation
    z_pos = center.z + cube_radius + height / 2
    cube.location = (center.x, center.y, round_up(z_pos,step))

    while cube.location.z - cube_radius > center.z - height/2:
        cube.location.z -= step
        chunks.append(cut_new_chunk(obj, cube))

    chunks.append(obj)

    # dividing with y-translation
    y_pos = center.y + cube_radius + width / 2
    cube.location = (center.x, round_up(y_pos,step), center.z)

    chunks_number = len(chunks)
    while cube.location.y - cube_radius > center.y - width/2:
        cube.location.y -= step
        for k in range(0, chunks_number):
            chunks.append(cut_new_chunk(chunks[k], cube))

    # dividing with x-translation
    x_pos = center.x + cube_radius + depth / 2
    cube.location = (round_up(x_pos,step), center.y, center.z)

    chunks_number = len(chunks)
    while cube.location.x - cube_radius > center.x - depth/2:
        cube.location.x -= step
        for k in range(0, chunks_number):
            chunks.append(cut_new_chunk(chunks[k], cube))

    # delete mega cube
    bpy.ops.object.select_all(action='DESELECT')
    cube.select = True
    bpy.ops.object.delete()

    # output array
    out = numpy.zeros((int(depth/step)+2, int(width/step)+2, int(height/step)+2))
    for chunk in chunks:
       bpy.context.scene.objects.active = chunk
       chunk.select = True
       bpy.ops.object.origin_set(type='ORIGIN_GEOMETRY', center='MEDIAN')
       out[int((chunk.location.x - (center.x - depth/2))/step), int((chunk.location.y - (center.y - width/2))/step), int((chunk.location.z - (center.z - height/2))/step)] = 1
    return ""


class CutObject(bpy.types.Operator):
    bl_idname = 'object_cutter.cut'
    bl_label = 'Cut object'

    def execute(self, context):
        message = cut(context)

        if message:
            self.report(message[0], message[1])

        return {'FINISHED'}


class ObjectCutterPanel(bpy.types.Panel):
    bl_label = "Object Cutter"
    bl_space_type = "PROPERTIES"
    bl_region_type = "WINDOW"
    bl_context = "object"

    def draw_main(self, scene, layout):
        col = layout.column()
        row = col.row(align=True)
        row.operator("object_cutter.cut")

    def draw(self, context):
        scene = context.scene
        layout = self.layout

        maincol = layout.column()

        self.draw_main(scene, maincol.box())


def register():
    bpy.utils.register_module(__name__)


def unregister():
    bpy.utils.unregister_module(__name__)


if __name__ == "__main__":
    register()
