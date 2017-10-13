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



def cut_new_chunk(source_shape, big_cube):
    new_obj = source_shape.copy()
    new_obj.data = source_shape.data.copy()
    bpy.context.scene.objects.link(new_obj)

    source_shape.select = True
    cut1 = source_shape.modifiers.new(name='Cut1', type='BOOLEAN')
    cut1.object = big_cube
    cut1.operation = 'DIFFERENCE'
    bpy.context.scene.objects.active = source_shape
    bpy.ops.object.convert(target="MESH")

    new_obj.select = True
    cut2 = new_obj.modifiers.new(name='Cut2', type='BOOLEAN')
    cut2.object = big_cube
    cut2.operation = 'INTERSECT'
    bpy.context.scene.objects.active = new_obj
    bpy.ops.object.convert(target="MESH")

    return new_obj


def cut(context):

    step = 0.7

    # get main object
    obj = bpy.context.scene.objects.active
    bpy.ops.object.transform_apply(rotation=True)

    # create mega cube
    center = obj.location
    height = obj.dimensions.z
    width = obj.dimensions.y
    depth = obj.dimensions.x

    bpy.ops.mesh.primitive_cube_add()
    cube = bpy.context.scene.objects.active
    cube.scale = (100, 100, 100)

    # chunks array
    chunks = []

    # dividing with z-translation
    cube.location = (center.x, center.y, center.z + 100 + height / 2)

    i = step
    while i < height:
        cube.location.z -= step
        chunks.append(cut_new_chunk(obj, cube))
        i += step

    chunks.append(obj)

    # dividing with y-translation
    cube.location = (center.x, center.y + 100 + width / 2, center.z)

    i = step
    chunks_number = len(chunks)
    while i < width:
        cube.location.y -= step
        for k in range(0, chunks_number):
            chunks.append(cut_new_chunk(chunks[k], cube))
        i += step

    # dividing with x-translation
    cube.location = (center.x + 100 + depth / 2, center.y, center.z)

    i = step
    chunks_number = len(chunks)
    while i < depth:
        cube.location.x -= step
        for k in range(0, chunks_number):
            chunks.append(cut_new_chunk(chunks[k], cube))
        i += step

    # delete mega cube
    bpy.ops.object.select_all(action='DESELECT')
    cube.select = True
    bpy.ops.object.delete()

    # output array
    # out = numpy.zeros((int(depth/step)+2, int(width/step)+2, int(height/step)+2))
    # for chunk in chunks:
    #    bpy.context.scene.objects.active = chunk
    #    chunk.select = True
    #    bpy.ops.object.origin_set(type='ORIGIN_GEOMETRY', center='MEDIAN')
    #    out[int((chunk.location.x - (center.x - depth/2))/step), int((chunk.location.y - (center.y - width/2))/step), int((chunk.location.z - (center.z - height/2))/step)] = 1
    return ""


class CutObject(bpy.types.Operator):
    bl_idname = 'object_cutter.cut'
    bl_label = 'Cut object'

    def execute(self, context):
        message = cut(context)

        if message:
            self.report(message[0], message[1])

        return {'FINISHED'}


class RNObjectCutterPanel(bpy.types.Panel):
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
