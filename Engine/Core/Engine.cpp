#include "pch.h"
#include "Engine.h"


Engine::Engine() {}

Engine::~Engine() {}

bool Engine::Initialize(int width, int height, const char* title) {
    if (!m_Window.Create(width, height, title)) {
        return false;
    }

    if (!m_Renderer.Initialize(m_Window.GetNativeWindow())) {
        return false;
    }

    return true;
}

void Engine::Run() {
    while (!m_Window.ShouldClose()) {
        m_Window.PollEvents();
        m_Renderer.Clear();
        // Add scene rendering here
        m_Renderer.SwapBuffers();
    }
}

void Engine::Shutdown() {
    m_Renderer.Shutdown();
    m_Window.Destroy();
}