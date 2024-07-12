#include "EngineWrapper.h"
#include <Core/Engine.h>
#include <Core/Window.h>
#include <Renderer/Renderer.h>

namespace EngineWrapper {
    class ManagedEngine::EngineImpl
    {
    public:
        Engine engine;
        Window window;
        Renderer renderer;
    };

    ManagedEngine::ManagedEngine() : m_pImpl(new EngineImpl()) {}

    ManagedEngine::~ManagedEngine()
    {
        delete m_pImpl;
    }

    bool ManagedEngine::Initialize(HWND windowHandle, int width, int height)
    {
        if (!m_pImpl->window.Create(width, height, "OpenGL ES Window"))
            return false;

        if (!m_pImpl->engine.Initialize(width, height, "OpenGL ES Window"))
            return false;

        if (!m_pImpl->renderer.Initialize(m_pImpl->window.GetNativeWindow()))
            return false;

        return true;
    }

    void ManagedEngine::Render()
    {
        m_pImpl->renderer.Clear();
        // Add any additional rendering calls here
        m_pImpl->window.SwapBuffers();
    }
}