#pragma once
#include "Window.h"
#include "../Renderer/Renderer.h"

class Engine {
public:
    Engine();
    ~Engine();

    bool Initialize(int width, int height, const char* title);
    void Run();
    void Shutdown();

private:
    Window m_Window;
    Renderer m_Renderer;
};