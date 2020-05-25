# nerf_Unity

### :gem: [**Project page**](https://kwea123.github.io/nerf_pl/) (live demo!)

Unity projects for my implementation of [nerf_pl (Neural Radiance Fields)](https://github.com/kwea123/nerf_pl)

### Tutorial and demo videos
<a href="https://www.youtube.com/playlist?list=PLDV2CyUo4q-K02pNEyDr7DYpTQuka3mbV">
<img src="https://user-images.githubusercontent.com/11364490/80913471-d5781080-8d7f-11ea-9f72-9d68402b8271.png">
</a>

# Installation and usage

This project is build on Unity 2019.3.9f1. It contains 3 scenes (under `Scenes/` folder):
*  [MeshRender](#meshrender)
*  [MixedReality](#mixedreality)
*  [VolumeRender](#volumerender)

Due to large size of the files, I put the assets in [release](https://github.com/kwea123/nerf_Unity/releases), you need to download from there and import to Unity. **Make sure you download them and put under `Assets/` before opening Unity.** Follow the instructions below for each scene:

## MeshRender

Render reconstructed meshes.
![image](https://user-images.githubusercontent.com/11364490/82660030-91807900-9c64-11ea-8f4f-7ac3c57f7d9e.png)

### Data preparation

1.  Download the mesh files (`*.ply`) from [here](https://github.com/kwea123/nerf_Unity/releases)
2.  Follow the below image to add the mesh to scene:
    *  Select gameobject
    *  Drag mesh into the missing parts
![image](https://user-images.githubusercontent.com/11364490/82660456-5df21e80-9c65-11ea-95c2-732fa4fed936.png)

## MixedReality

Render a real scene with correct depth values, where you can add virtual objects and get accurate occlusion effect (visual effect only).
![image](https://user-images.githubusercontent.com/11364490/82661303-b8d84580-9c66-11ea-8477-4e9f49192a08.png)

### Data preparation
None

## VolumeRender
Volume render rays of a virtual object.

![image](https://user-images.githubusercontent.com/11364490/82661894-d954cf80-9c67-11ea-916f-d441b522ecc1.png)

### Data preparation

1.  Download the volume files (`*.vol`) from [here](https://github.com/kwea123/nerf_Unity/releases)
2.  Follow the below image to add the volume to scene:
    *  Select gameobject
    *  Drag vol into the missing part
![image](https://user-images.githubusercontent.com/11364490/82661695-72371b00-9c67-11ea-96cd-4f1972fdf48b.png)

# TODO
- [ ] Render the volume from inside
