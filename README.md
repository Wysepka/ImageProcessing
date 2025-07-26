# ImageProcessing Unity Project (wysepka-imageprocessing)

## Overview
This Unity project provides a flexible and extensible base for developing image processing algorithms and applications, with a focus on pixelation, color slicing, and visualization. Originally created to assist with custom painting workflows, it is suitable for 2D game development, digital art, and educational purposes.

## Features
- **Pixelation and Color Slicing:** Easily pixelate images and slice them by color using customizable settings.
- **Black & White and Color Processing:** Supports both black & white and color-based image transformations.
- **ScriptableObject-Driven:** All data, settings, and results are managed via Unity ScriptableObjects for easy configuration and persistence.
- **Extensible Pipeline:** Modular step-based processing (e.g., black & white, color, merging) allows for easy extension and customization.
- **Visualization:** Includes a visualizer for exploring processed images, with keyboard navigation and material switching.
- **PNG Export:** Processed images are saved as PNGs to the desktop for further use.
- **Editor Integration:** Custom Unity Editor inspector for streamlined workflow.

## Directory Structure
```
wysepka-imageprocessing/
├── Assets/
│   ├── 2D/ (Materials, Textures, XML for UI)
│   ├── Editor/ (Custom Editor scripts)
│   ├── Scenes/ (Demo scenes)
│   └── Scripts/
│       ├── Extensions/ (Color extensions)
│       ├── ImageColorSlicer/ (Core processing logic)
│       │   ├── Camera/ (Camera control)
│       │   ├── QuadColorSlicer/ (Processing steps, ScriptableObjects)
│       │   ├── QuadSlicerVisualizer/ (Visualization logic)
│       │   └── UI/ (Canvas integration)
│       └── Utility/ (Color, Texture, Camera utilities)
├── Packages/ (Unity package manifests)
├── ProjectSettings/ (Unity project settings)
└── UIElementsSchema/ (UI schema files)
```

## Main Components
### 1. **ScriptableObjects**
- **QuadColorSlicerData:** Holds input textures.
- **QuadColorSlicerSettings:** Controls processing parameters (e.g., color slicing space, dominant colors, thresholds).
- **QuadColorSlicerResult:** Stores output textures and manages saving results.

### 2. **Processing Pipeline**
- **QuadSlicerStepBlackWhite:** Converts images to black & white based on threshold.
- **QuadSlicerStepColor:** Slices images into color regions using dominant colors and comparison methods (RGB, Hue, HSV).
- **QuadSlicerTextureMerger:** Merges processed textures for final output.

### 3. **Visualization**
- **QuadSlicerVisualizerFacade:** Instantiates a grid of quads for each image region, applies materials, and allows navigation.
- **QuadSlicerVisualizerCanvas:** UI for displaying current selection.
- **CameraController:** Smooth camera transitions to focused regions.

### 4. **Editor Integration**
- **QuadColorSlicerFacadeEditor:** Custom inspector for setting up data, settings, and results, and triggering processing from the Unity Editor.

## Usage
1. **Open the Project in Unity (2021.3.25f1 or later).**
2. **Configure Input:**
   - Create and assign `QuadColorSlicerData`, `QuadColorSlicerSettings`, and `QuadColorSlicerResult` ScriptableObjects.
   - Set input textures and processing parameters in the Inspector.
3. **Run Processing:**
   - Use the custom inspector or call `Generate()` on the `QuadColorFacade` to process the image.
   - Processed PNGs are saved to your Desktop under `ImageProcessing/QuadColorSlicer/` with the specified OutputID.
4. **Visualize Results:**
   - Open the `QuadColorSlicer` scene.
   - Use arrow keys to navigate between image regions.
   - Switch between materials (final, black & white, color, color-sliced) using assigned keycodes.

## Extending the Project
- **Add New Processing Steps:** Implement the `IQuadSlicerStep` interface and add your step to the pipeline in `QuadColorFacade`.
- **Customize Visualization:** Modify or extend `QuadSlicerVisualizerFacade` for new visual effects or navigation schemes.
- **Integrate with Other Tools:** Use the exported PNGs in external tools or pipelines.

## Dependencies
- Unity 2021.3.25f1 or later
- Uses built-in Unity packages (TextMeshPro, UGUI, etc.)

## License
MIT License (c) 2024 Marcin Wysocki

## Demo Videos
https://github.com/Wysepka/ImageProcessing/assets/57349483/8df6cbbf-7050-4764-a83a-8ffad19af905

https://github.com/Wysepka/ImageProcessing/assets/57349483/35bce024-5e06-4d03-b2c9-8f0e1af124d5

## Screenshots
![Painting Process 1](https://github.com/Wysepka/ImageProcessing/assets/57349483/6e2a8914-229e-46ed-a751-0182fa79bcc4)
![Painting Process 2](https://github.com/Wysepka/ImageProcessing/assets/57349483/662df11c-564b-42a8-a6fa-7742d9796e42)

## Credits
Created by Marcin Wysocki ([Wysepka](https://github.com/Wysepka))

---
Feel free to fork, contribute, and adapt this project for your own creative or technical needs! 
