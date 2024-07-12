#pragma once

#ifdef ENGINEWRAPPERDLL_EXPORTS
#define ENGINEWRAPPER_API __declspec(dllexport)
#else
#define ENGINEWRAPPER_API __declspec(dllimport)
#endif

#include <Windows.h>
#include "Engine/Core/Engine.h"
#include "Engine/Core/Window.h"
#include "Engine/Renderer/Renderer.h"

namespace EngineWrapper {
    class ENGINEWRAPPER_API ManagedEngine
    {
    public:
        ManagedEngine();
        ~ManagedEngine();

        bool Initialize(HWND windowHandle, int width, int height);
        void Render();

    private:
        class EngineImpl;
        EngineImpl* m_pImpl;
    };
}

extern "C" {
    ENGINEWRAPPER_API EngineWrapper::ManagedEngine* CreateEngine();
    ENGINEWRAPPER_API void DestroyEngine(EngineWrapper::ManagedEngine* engine);
    ENGINEWRAPPER_API bool InitializeEngine(EngineWrapper::ManagedEngine* engine, HWND windowHandle, int width, int height);
    ENGINEWRAPPER_API void RenderEngine(EngineWrapper::ManagedEngine* engine);
}