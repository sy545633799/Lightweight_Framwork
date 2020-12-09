功能: 
==
    将Animator/Animation组件内动画数据烘培至贴图，并在运行时由shader顶点函数执行动画播放，以此节省资源
用法: 
==
    Window->GPUAnimBakeWindow，拖入带动画物体，选择烘培类型，点击"Bake"会在选择的路径下生成同名文件夹，内含prefab、材质球动画信息及贴图各一份，将prefab拖入场景运行可预览效果

BakeType: 
==
1.SkeletonAnim 保存骨骼动画，适用顶点数量较多带骨骼的动画物体，贴图中以4x4矩阵形式保存所有骨骼的位移、旋转、缩放信息，格式为从左至右每列像素代表动画的每帧，从下至上每四个像素分别存一个矩阵的四行，代表该骨骼在这一帧的运动情况(实际已乘bindpose作为蒙皮矩阵使用)，shader中暂定使用tangent通道存两根骨骼的索引和权重，运行时按索引采样对应的四个像素合成蒙皮矩阵，即可计算当前顶点的动画位置。
    
2.VertexAnim 则简单粗暴，直接保存每帧每顶点位置信息，适用网格顶点较少的物体，同样从左至右每列像素存一帧，从下至上每像素存一个顶点的position。
    
已知问题：
==
1.骨骼动画烘培后网格偶有塌陷错位，需要改进烘培方式;(clear)
2.烘培后坐标系朝向问题;(clear)
3.使用GPUAnimManager生成并管理prefab，可以不生成游戏实体，使用Graphics.DrawMeshInstanced每帧统一提交至GPU渲染，但合批部分代码可能仍存在性能问题,见InstancingBatch,GPUAnimGroup类;

Reference：
==
https://github.com/chengkehan/GPUSkinning

https://github.com/chenjd/Render-Crowd-Of-Animated-Characters