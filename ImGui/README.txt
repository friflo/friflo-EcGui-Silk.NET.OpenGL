Adapted from the OpenGL 3 reference implementation of ImGui under the provisions of the MIT license at:
https://github.com/ocornut/imgui/blob/master/backends/imgui_impl_glfw.cpp
https://github.com/ocornut/imgui/blob/master/backends/imgui_impl_opengl3.cpp
https://github.com/ocornut/imgui/blob/master/examples/example_glfw_opengl3/main.cpp

2025-04-28 friflo
- added sources from: https://github.com/dotnet/Silk.NET/tree/v2.22.0/src/OpenGL/Extensions/Silk.NET.OpenGL.Extensions.ImGui
- support non US keyboard keys like: Key.World1 (key: < in German layout) Key.World2
- fixed compiler warnings: Warning CS9192 : Argument ... should be passed with 'ref' or 'in' keyword