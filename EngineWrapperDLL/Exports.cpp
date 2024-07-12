#include "EngineWrapper.h"

ENGINEWRAPPER_API EngineWrapper::ManagedEngine* CreateEngine() {
    return new EngineWrapper::ManagedEngine();
}

ENGINEWRAPPER_API void DestroyEngine(EngineWrapper::ManagedEngine* engine) {
    delete engine;
}

ENGINEWRAPPER_API bool InitializeEngine(EngineWrapper::ManagedEngine* engine, HWND windowHandle, int width, int height) {
    return engine->Initialize(windowHandle, width, height);
}

ENGINEWRAPPER_API void RenderEngine(EngineWrapper::ManagedEngine* engine) {
    engine->Render();
}