#pragma once
#include <glad/glad.h>
#include <GLFW/glfw3.h>

class Window {
public:
    Window();
    ~Window();

    bool Create(int width, int height, const char* title);
    void Destroy();
    void PollEvents();
    bool ShouldClose();
    GLFWwindow* GetNativeWindow() const { return m_Window; }
    void SwapBuffers();

private:
    GLFWwindow* m_Window;
};