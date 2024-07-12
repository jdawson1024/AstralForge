#pragma once
#include <glad/glad.h>
#include <GLFW/glfw3.h>

class Renderer {
public:
    Renderer();
    ~Renderer();

    bool Initialize(GLFWwindow* window);
    void Shutdown();
    void Clear();
    void SwapBuffers();

private:
    GLFWwindow* m_Window;
};