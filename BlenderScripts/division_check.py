import bpy
import json
import os.path

main_directory = "D:/Blenderowe/Glass Ruin/"
hero_name = 'TheMan.000'

original_fbx = bpy.ops.import_scene.fbx( filepath = main_directory + "out/" + hero_name + "/fbx/" + hero_name + ".fbx" )

current_level = [hero_name]
current_level_nr = 1
ok = True
while ok:
    children = []
    for parent in current_level:
        current_path = main_directory + "out/" + hero_name + "/json/" + parent + "_description.json"
        if not os.path.isfile(current_path):
            ok = False
        else:
            json_file = open( current_path )
            current_json = json.load(json_file)
            json_file.close()
            
            current_fbx = bpy.ops.import_scene.fbx( filepath = main_directory + "out/" + hero_name + "/fbx/" + parent + "_chunks.fbx" )

            for j in current_json["chunks"]:
                children.append(j["name"])
                o = bpy.context.scene.objects[j["name"]]
                p = bpy.context.scene.objects[parent]
                o.location.x = p.location.x - j["relative_position"]["x"]
                o.location.y = p.location.y - j["relative_position"]["z"]
                o.location.z = p.location.z + j["relative_position"]["y"] + 50
    
    current_level = children
    current_level_nr += 1