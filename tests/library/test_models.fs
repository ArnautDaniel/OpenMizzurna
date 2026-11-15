\ the models on the game's data (render/scene3d.c): where their parts stand, as the original's hierarchy puts them
\ (src: model_link_hierarchy, func_8001B33C, model_pose_update)
USING: tester engine ;

: rest-place ( model part -- x y z )   \ a part's place at rest (world units from the model's origin, y down)
    0 0 rot part-at 0= abort" no such part" ;

testing a part hanging from one listed after it still hangs from it (091_2: part 0 from part 3)
s" models/091_2.gltf" model constant hung
T{ hung 3 rest-place -> 0 -220 209 }T                 \ part 3: on the root, (0 220 209) up and forward
T{ hung 0 rest-place -> 0 -129 241 }T                 \ part 0: from part 3, (0 -91 32) - not from the origin

testing a root part stands at the model's origin, whatever its own place (src: a shell's parts all at the area's)
T{ s" models/011_4.3.0.gltf" model 1 rest-place -> 0 0 0 }T   \ (its place: -2994 0 -100)

testing a part's place: its offset halved in a figure's model (TMD scale -1), doubled in an area's objects (+1) -
\ in model units, half the world's (src: func_8001CCEC; func_8001E0F8's 4 x, then model_pose_update's / 2)
T{ hung 2 rest-place -> 0 -136 -201 }T               \ (091_2's part 2: 0 136 -201 on the root)
T{ s" models/002_4.3.1.gltf" model 13 rest-place -> -5600 0 8384 }T   \ (area 2's object 13: -1400 0 2096)

test-summary
