#include <GL/glew.h>
#include <SFML/Window.hpp>
#include <SFML/OpenGL.hpp>
#include <iostream>
#include <vector>
#include <cmath>
#include <fstream>
#include <sstream>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include "Model.h"
#include "Camera.h"

Camera camera;
Model teapotModel;
GLuint shaderProgram;
GLuint textureID = 0;
float deltaTime = 0.0f;
sf::Clock gameClock;

const char* vertexShaderSource = R"(
    #version 330 core
    layout(location = 0) in vec3 aPos;
    layout(location = 1) in vec2 aTexCoord;
    layout(location = 2) in vec3 aNormal;
    
    out vec2 TexCoord;
    out vec3 Normal;
    out vec3 FragPos;
    
    uniform mat4 model;
    uniform mat4 view;
    uniform mat4 projection;
    
    void main() {
        gl_Position = projection * view * model * vec4(aPos, 1.0);
        FragPos = vec3(model * vec4(aPos, 1.0));
        TexCoord = aTexCoord;
        Normal = mat3(transpose(inverse(model))) * aNormal;
    }
)";

const char* fragmentShaderSource = R"(
    #version 330 core
    in vec2 TexCoord;
    in vec3 Normal;
    in vec3 FragPos;
    
    out vec4 FragColor;
    
    uniform sampler2D texture1;
    uniform vec3 lightPos;
    uniform vec3 viewPos;
    
    void main() {
        vec3 lightColor = vec3(1.0, 1.0, 1.0);
        float ambientStrength = 0.3;
        vec3 ambient = ambientStrength * lightColor;
        
        vec3 norm = normalize(Normal);
        vec3 lightDir = normalize(lightPos - FragPos);
        float diff = max(dot(norm, lightDir), 0.0);
        vec3 diffuse = diff * lightColor;
        
        vec4 texColor = texture(texture1, TexCoord);
        vec3 result = (ambient + diffuse) * texColor.rgb;
        FragColor = vec4(result, texColor.a);
    }
)";

bool InitShader() {
    GLuint vertexShader = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vertexShader, 1, &vertexShaderSource, nullptr);
    glCompileShader(vertexShader);

    GLint success;
    char infoLog[512];
    glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success);
    if (!success) {
        glGetShaderInfoLog(vertexShader, 512, nullptr, infoLog);
        return false;
    }

    GLuint fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fragmentShader, 1, &fragmentShaderSource, nullptr);
    glCompileShader(fragmentShader);

    glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &success);
    if (!success) {
        glGetShaderInfoLog(fragmentShader, 512, nullptr, infoLog);
        return false;
    }

    shaderProgram = glCreateProgram();
    glAttachShader(shaderProgram, vertexShader);
    glAttachShader(shaderProgram, fragmentShader);
    glLinkProgram(shaderProgram);

    glGetProgramiv(shaderProgram, GL_LINK_STATUS, &success);
    if (!success) {
        glGetProgramInfoLog(shaderProgram, 512, nullptr, infoLog);
        return false;
    }

    glDeleteShader(vertexShader);
    glDeleteShader(fragmentShader);

    return true;
}

GLuint CreateSimpleTexture() {
    const int width = 256;
    const int height = 256;
    std::vector<unsigned char> imageData(width * height * 4);

    unsigned char r = 200;
    unsigned char g = 150;
    unsigned char b = 100;

    for (int y = 0; y < height; ++y) {
        for (int x = 0; x < width; ++x) {
            int index = (y * width + x) * 4;
            imageData[index] = r;
            imageData[index + 1] = g;
            imageData[index + 2] = b;
            imageData[index + 3] = 255;
        }
    }

    GLuint texID;
    glGenTextures(1, &texID);
    glBindTexture(GL_TEXTURE_2D, texID);

    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0,
        GL_RGBA, GL_UNSIGNED_BYTE, imageData.data());

    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

    return texID;
}

int main() {
    sf::Window window(sf::VideoMode({ 1000, 800 }), "3D Teapot with Camera");
    window.setVerticalSyncEnabled(true);
    window.setActive(true);

    glewInit();
    glEnable(GL_DEPTH_TEST);
    glClearColor(0.1f, 0.1f, 0.15f, 1.0f);
    InitShader();
    teapotModel.LoadFromOBJ("models/teapot.obj");
    textureID = CreateSimpleTexture();

    std::cout << "CAMERA CONTROLERS" << std::endl;
    std::cout << "Space - Move forward" << std::endl;
    std::cout << "LShift - Move backward" << std::endl;
    std::cout << "D - Move left" << std::endl;
    std::cout << "A - Move right" << std::endl;
    std::cout << "S - Move up" << std::endl;
    std::cout << "W - Move down" << std::endl;
    std::cout << "Mouse - Rotate camera (click and drag)" << std::endl;
    std::cout << "Mouse Wheel - Zoom in/out" << std::endl;
    std::cout << "R - Reset camera to initial position" << std::endl;
    std::cout << "Escape - Exit program" << std::endl;

    camera.SetPosition(glm::vec3(0.0f, 0.0f, 5.0f));

    bool firstMouse = true;
    float lastX = 500.0f, lastY = 400.0f;

    while (window.isOpen()) {
        deltaTime = gameClock.restart().asSeconds();

        while (auto event = window.pollEvent()) {
            if (event->is<sf::Event::Closed>()) {
                window.close();
            }

            if (event->is<sf::Event::KeyPressed>()) {
                const auto& keyEvent = event->getIf<sf::Event::KeyPressed>();
                if (keyEvent) {
                    if (keyEvent->scancode == sf::Keyboard::Scancode::Escape) {
                        window.close();
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::R) {
                        camera = Camera();
                        camera.SetPosition(glm::vec3(0.0f, 0.0f, 5.0f));
                        firstMouse = true;
                        std::cout << "Camera reset" << std::endl;
                    }
                }
            }

            if (event->is<sf::Event::MouseMoved>()) {
                const auto& mouseEvent = event->getIf<sf::Event::MouseMoved>();
                if (mouseEvent) {
                    float xpos = static_cast<float>(mouseEvent->position.x);
                    float ypos = static_cast<float>(mouseEvent->position.y);

                    if (firstMouse) {
                        lastX = xpos;
                        lastY = ypos;
                        firstMouse = false;
                    }

                    float xoffset = xpos - lastX;
                    float yoffset = lastY - ypos;

                    lastX = xpos;
                    lastY = ypos;

                    camera.ProcessMouseMovement(xoffset, yoffset);
                }
            }

            if (event->is<sf::Event::MouseWheelScrolled>()) {
                const auto& wheelEvent = event->getIf<sf::Event::MouseWheelScrolled>();
                if (wheelEvent && wheelEvent->wheel == sf::Mouse::Wheel::Vertical) {
                    float zoom = wheelEvent->delta * 0.3f;
                    if (zoom > 0) camera.MoveForward(0.5f);
                    else camera.MoveBackward(0.5f);
                }
            }
        }

        float moveSpeed = 3.0f * deltaTime;
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::Space)) {
            camera.MoveForward(moveSpeed);
        }
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::LShift)) {
            camera.MoveBackward(moveSpeed);
        }
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::D)) {
            camera.MoveLeft(moveSpeed);
        }
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::A)) {
            camera.MoveRight(moveSpeed);
        }
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::S)) {
            camera.MoveUp(moveSpeed);
        }
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::W)) {
            camera.MoveDown(moveSpeed);
        }

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        glUseProgram(shaderProgram);

        static float rotationAngle = 0.0f;
        rotationAngle += deltaTime * 30.0f;

        glm::mat4 model = glm::mat4(1.0f);
        model = glm::rotate(model, glm::radians(rotationAngle), glm::vec3(0.0f, 1.0f, 0.0f));

        glm::mat4 view = camera.GetViewMatrix();
        glm::mat4 projection = camera.GetProjectionMatrix(1000.0f / 800.0f);

        glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "model"), 1, GL_FALSE, glm::value_ptr(model));
        glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "view"), 1, GL_FALSE, glm::value_ptr(view));
        glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "projection"), 1, GL_FALSE, glm::value_ptr(projection));

        glm::vec3 lightPos = glm::vec3(3.0f, 3.0f, 3.0f);
        glm::vec3 viewPos = camera.GetPosition();
        glUniform3f(glGetUniformLocation(shaderProgram, "lightPos"), lightPos.x, lightPos.y, lightPos.z);
        glUniform3f(glGetUniformLocation(shaderProgram, "viewPos"), viewPos.x, viewPos.y, viewPos.z);

        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, textureID);
        glUniform1i(glGetUniformLocation(shaderProgram, "texture1"), 0);

        teapotModel.Draw();
        window.display();
    }

    return 0;
}