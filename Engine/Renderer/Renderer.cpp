#include "pch.h"
#include "Renderer.h"
#include <iostream>

Renderer::Renderer() : m_Window(nullptr) {}

Renderer::~Renderer() {}

bool Renderer::Initialize(GLFWwindow* window) {
    m_Window = window;
    glfwMakeContextCurrent(m_Window);

    if (!gladLoadGLES2Loader((GLADloadproc)glfwGetProcAddress)) {
        std::cerr << "Failed to initialize GLAD" << std::endl;
        return false;
    }

    glViewport(0, 0, 800, 600); // Set this based on your window size
    return true;
}

void Renderer::Shutdown() {
    // Add any necessary cleanup here
}

void Renderer::Clear() {
    glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
    glClear(GL_COLOR_BUFFER_BIT);
}

void Renderer::SwapBuffers() {
    glfwSwapBuffers(m_Window);
}