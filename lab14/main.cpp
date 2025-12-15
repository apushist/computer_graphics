#include <GL/glew.h>
#include <SFML/Window.hpp>
#include <SFML/OpenGL.hpp>
#include <SFML/Graphics.hpp>
#include <iostream>
#include <vector>
#include <cmath>
#include <filesystem>
#include <string>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include "Model.h"
#include "Camera.h"
#include "Shaders.h"
#include "Textures.h"

struct StaticObject {
    std::unique_ptr<Model> model;
    GLuint textureID;
    glm::vec3 position;
    glm::vec3 rotation;
    glm::vec3 scale;
    std::string name;

    StaticObject(const StaticObject&) = delete;
    StaticObject& operator=(const StaticObject&) = delete;

    StaticObject() = default;
    StaticObject(StaticObject&&) = default;
    StaticObject& operator=(StaticObject&&) = default;
};

struct PointLight {
    glm::vec3 position;
    glm::vec3 ambient;
    glm::vec3 diffuse;
    glm::vec3 specular;
    bool enabled;
    float constant;
    float linear;
    float quadratic;

    PointLight() :
        position(glm::vec3(5.0f, 5.0f, 5.0f)),
        ambient(glm::vec3(1.5f, 1.5f, 1.5f)),
        diffuse(glm::vec3(1.8f, 1.8f, 1.8f)),
        specular(glm::vec3(2.0f, 2.0f, 2.0f)),
        enabled(true),
        constant(1.0f),
        linear(0.09f),
        quadratic(0.032f) {
    }
};

Camera camera;
GLuint shaderProgram;
float deltaTime = 0.0f;
sf::Clock gameClock;

std::vector<StaticObject> staticObjects;
PointLight pointLight;  
std::unique_ptr<Model> lightSphereModel;  // Модель для визуализации источника света

bool LoadStaticScene() {
    staticObjects.clear();

    lightSphereModel = std::make_unique<Model>();
    if (!lightSphereModel->LoadFromOBJ("models/sphere.obj")) {
        std::cerr << "Failed to load sphere.obj for light visualization" << std::endl;
        return false;
    }

    // Объект 1
    StaticObject obj1;
    /*obj1.name = "Metal_table";
    obj1.model = std::make_unique<Model>();
    if (!obj1.model->LoadFromOBJ("models/Metal_table.obj")) {
        std::cerr << "Failed to load Metal_table.obj" << std::endl;
        return false;
    }
    obj1.textureID = LoadTextureFromFile("textures/metal.png");

    obj1.position = glm::vec3(3.0f, -5.6f, 0.0f);
    obj1.rotation = glm::vec3(-90.0f, 0.0f, 0.0f);
    obj1.scale = glm::vec3(0.02f, 0.02f, 0.02f);*/

    obj1.name = "Cube";
    obj1.model = std::make_unique<Model>();
    if (!obj1.model->LoadFromOBJ("models/cube.obj")) {
        std::cerr << "Failed to load cube.obj" << std::endl;
        return false;
    }
    obj1.position = glm::vec3(0.0f, -11.0f, 0.0f);
    obj1.rotation = glm::vec3(0.0f, 0.0f, 0.0f);
    obj1.scale = glm::vec3(10.0f, 10.0f, 10.0f);

    obj1.textureID = LoadTextureFromFile("textures/ceramic.jpg");
    staticObjects.push_back(std::move(obj1));

    // Сфера
    StaticObject obj2;
    obj2.name = "Sphere";
    obj2.model = std::make_unique<Model>();
    if (!obj2.model->LoadFromOBJ("models/sphere.obj")) {
        std::cerr << "Failed to load sphere.obj" << std::endl;
        return false;
    }
    obj2.textureID = LoadTextureFromFile("textures/texture1.png");
    obj2.position = glm::vec3(0.0f, 0.0f, 0.0f);
    obj2.rotation = glm::vec3(0.0f, 0.0f, 0.0f);
    obj2.scale = glm::vec3(1.0f, 1.0f, 1.0f);
    staticObjects.push_back(std::move(obj2));

    // Чайник
    StaticObject obj3;
    obj3.name = "Teapot";
    obj3.model = std::make_unique<Model>();
    if (!obj3.model->LoadFromOBJ("models/teapot.obj")) {
        std::cerr << "Failed to load teapot.obj" << std::endl;
        return false;
    }
    obj3.textureID = LoadTextureFromFile("textures/flowers.jpg");
    obj3.position = glm::vec3(3.0f, 0.0f, 0.0f);
    obj3.rotation = glm::vec3(0.0f, -45.0f, 0.0f);
    obj3.scale = glm::vec3(2.5f, 2.5f, 2.5f);
    staticObjects.push_back(std::move(obj3));

    //Чашка 
    StaticObject obj4;
    obj4.name = "Cup";
    obj4.model = std::make_unique<Model>();
    if (!obj4.model->LoadFromOBJ("models/cup.obj")) {
        std::cerr << "Failed to load cup.obj" << std::endl;
        return false;
    }
    obj4.textureID = LoadTextureFromFile("textures/flowers2.png");
    obj4.position = glm::vec3(4.0f, -0.87f, 2.0f);
    obj4.rotation = glm::vec3(-90.0f, 0.0f, 0.0f);
    obj4.scale = glm::vec3(0.008f, 0.008f, 0.008f);
    staticObjects.push_back(std::move(obj4));
    
    //
    //StaticObject obj5;
    //obj5.name = "Elephant";
    //obj5.model = std::make_unique<Model>();
    //if (!obj5.model->LoadFromOBJ("models/Elephant.obj")) {
    //    std::cerr << "Failed to load Elephant.obj" << std::endl;
    //    return false;
    //}
    //obj5.textureID = LoadTextureFromFile("textures/Elephant.png");
    //obj5.position = glm::vec3(5.0f, -0.65f, 0.0f);
    //obj5.rotation = glm::vec3(0.0f, 0.0f, 0.0f);
    //obj5.scale = glm::vec3(0.005f, 0.005f, 0.005f);
    //staticObjects.push_back(std::move(obj5));

    return true;
}

// Функция для настройки параметров точечного источника света в шейдере
void SetPointLightUniforms(GLuint shaderProgram, const PointLight& light) {
    glUniform3f(glGetUniformLocation(shaderProgram, "pointLight.position"),
        light.position.x, light.position.y, light.position.z);
    glUniform3f(glGetUniformLocation(shaderProgram, "pointLight.ambient"),
        light.ambient.x, light.ambient.y, light.ambient.z);
    glUniform3f(glGetUniformLocation(shaderProgram, "pointLight.diffuse"),
        light.diffuse.x, light.diffuse.y, light.diffuse.z);
    glUniform3f(glGetUniformLocation(shaderProgram, "pointLight.specular"),
        light.specular.x, light.specular.y, light.specular.z);
    glUniform1i(glGetUniformLocation(shaderProgram, "pointLight.enabled"), light.enabled);
    glUniform1f(glGetUniformLocation(shaderProgram, "pointLight.constant"), light.constant);
    glUniform1f(glGetUniformLocation(shaderProgram, "pointLight.linear"), light.linear);
    glUniform1f(glGetUniformLocation(shaderProgram, "pointLight.quadratic"), light.quadratic);
}


int main() {
    sf::Window window(sf::VideoMode({ 1000, 800 }), "3D Scene");
    window.setVerticalSyncEnabled(true);
    window.setActive(true);

    GLenum err = glewInit();
    if (err != GLEW_OK) {
        std::cerr << "Failed to initialize GLEW: " << glewGetErrorString(err) << std::endl;
        return -1;
    }
    glEnable(GL_DEPTH_TEST);
    glClearColor(0.1f, 0.1f, 0.15f, 1.0f);

    if (!InitShader(shaderProgram)) {
        std::cerr << "Failed to initialize shader!" << std::endl;
        return -1;
    }

    if (!LoadStaticScene()) {
        std::cerr << "Failed to load static scene!" << std::endl;
        return -1;
    }

    std::cout << "Static scene loaded successfully!" << std::endl;
    std::cout << "Objects in scene: " << staticObjects.size() << std::endl;
    for (const auto& obj : staticObjects) {
        std::cout << "  - " << obj.name << std::endl;
    }

    std::cout << "\nCAMERA MOVEMENT:" << std::endl;
    std::cout << "  Space - Move forward" << std::endl;
    std::cout << "  LShift - Move backward" << std::endl;
    std::cout << "  D - Move left" << std::endl;
    std::cout << "  A - Move right" << std::endl;
    std::cout << "  S - Move up" << std::endl;
    std::cout << "  W - Move down" << std::endl;
    std::cout << "  Mouse Wheel - Zoom in/out" << std::endl;
    std::cout << "  R - Reset camera" << std::endl;

    std::cout << "\nLIGHT CONTROLS:" << std::endl;
    std::cout << "  1 - Toggle point light" << std::endl;
    std::cout << "  I/K - Move light up/down" << std::endl;
    std::cout << "  J/L - Move light left/right" << std::endl;
    std::cout << "  U/O - Move light forward/backward" << std::endl;
    std::cout << "  N/M - Increase/Decrease light intensity" << std::endl;

    std::cout << "\n  Escape - Exit program" << std::endl;

    camera.SetPosition(glm::vec3(0.0f, 0.0f, 10.0f));
    camera.LookAt(glm::vec3(0.0f, 0.0f, 0.0f));

    float mouseSensitivity = 0.3f;
    float zoomSpeed = 2.0f;
    bool rotatingCamera = false;
    float lastMouseX = 0.0f;
    float lastMouseY = 0.0f;

    GLuint lightTextureID = LoadTextureFromFile("textures/light.png");

    while (window.isOpen()) {
        deltaTime = gameClock.restart().asSeconds();
        float moveSpeed = 3.0f * deltaTime;

        while (auto event = window.pollEvent()) {
            if (event->is<sf::Event::Closed>()) {
                window.close();
            }

            if (event->is<sf::Event::Resized>()) {
                const auto& sizeEvent = event->getIf<sf::Event::Resized>();
                if (sizeEvent) {
                    int width = sizeEvent->size.x;
                    int height = sizeEvent->size.y;

                    glViewport(0, 0, width, height);
                    camera.SetAspectRatio(static_cast<float>(width) / height);
                }
            }

            if (event->is<sf::Event::MouseButtonPressed>()) {
                const auto& btn = event->getIf<sf::Event::MouseButtonPressed>();
                if (btn && btn->button == sf::Mouse::Button::Left) {
                    rotatingCamera = true;
                    lastMouseX = static_cast<float>(btn->position.x);
                    lastMouseY = static_cast<float>(btn->position.y);
                }
            }

            if (event->is<sf::Event::MouseButtonReleased>()) {
                const auto& buttonEvent = event->getIf<sf::Event::MouseButtonReleased>();
                if (buttonEvent) {
                    if (buttonEvent->button == sf::Mouse::Button::Left) {
                        rotatingCamera = false;
                    }
                }
            }

            if (event->is<sf::Event::MouseMoved>()) {
                const auto& mouse = event->getIf<sf::Event::MouseMoved>();
                if (mouse && rotatingCamera) {
                    float xoffset = mouse->position.x - lastMouseX;
                    float yoffset = lastMouseY - mouse->position.y;
                    lastMouseX = static_cast<float>(mouse->position.x);
                    lastMouseY = static_cast<float>(mouse->position.y);

                    camera.Rotate(xoffset * mouseSensitivity, yoffset * mouseSensitivity);
                }
            }

            if (event->is<sf::Event::MouseWheelScrolled>()) {
                const auto& wheel = event->getIf<sf::Event::MouseWheelScrolled>();
                if (wheel && wheel->wheel == sf::Mouse::Wheel::Vertical) {
                    if (wheel->delta > 0) {
                        camera.MoveForward(zoomSpeed);
                    }
                    else {
                        camera.MoveBackward(zoomSpeed);
                    }
                }
            }

            if (event->is<sf::Event::KeyPressed>()) {
                const auto& keyEvent = event->getIf<sf::Event::KeyPressed>();
                if (keyEvent) {
                    if (keyEvent->scancode == sf::Keyboard::Scancode::Escape) {
                        window.close();
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::R) {
                        camera = Camera();
                        camera.SetPosition(glm::vec3(0.0f, 0.0f, 15.0f));
                        camera.LookAt(glm::vec3(0.0f, 0.0f, 0.0f));
                        std::cout << "Camera reset" << std::endl;
                    }
                    // Управление источником света
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Num1) {
                        pointLight.enabled = !pointLight.enabled;
                        std::cout << "Point light " << (pointLight.enabled ? "ENABLED" : "DISABLED") << std::endl;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::N) {
                        pointLight.diffuse *= 1.2f;
                        pointLight.ambient *= 1.2f;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::M) {
                        pointLight.diffuse *= 0.8f;
                        pointLight.ambient *= 0.8f;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::I) {
                        pointLight.position.y += moveSpeed;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::K) {
                        pointLight.position.y -= moveSpeed;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::J) {
                        pointLight.position.x -= moveSpeed;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::L) {
                        pointLight.position.x += moveSpeed;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::U) {
                        pointLight.position.z += moveSpeed;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::O) {
                        pointLight.position.z -= moveSpeed;
                    }
                
                }
            }

        }

        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::Space)) {
            camera.MoveForward(moveSpeed);
        }
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::LShift)) {
            camera.MoveBackward(moveSpeed);
        }
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::D)) {
            camera.MoveRight(moveSpeed);
        }
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::A)) {
            camera.MoveLeft(moveSpeed);
        }
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::S)) {
            camera.MoveDown(moveSpeed);
        }
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::W)) {
            camera.MoveUp(moveSpeed);
        }

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        glUseProgram(shaderProgram);

        glm::mat4 view = camera.GetViewMatrix();
        glm::mat4 projection = camera.GetProjectionMatrix(camera.GetAspectRatio());

        glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "view"), 1, GL_FALSE, glm::value_ptr(view));
        glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "projection"), 1, GL_FALSE, glm::value_ptr(projection));

        glm::vec3 viewPos = camera.GetPosition();
        glUniform3f(glGetUniformLocation(shaderProgram, "viewPos"), viewPos.x, viewPos.y, viewPos.z);


        SetPointLightUniforms(shaderProgram, pointLight);

        // Отрисовка всех статических объектов
        for (const auto& obj : staticObjects) {
            glm::mat4 model = glm::mat4(1.0f);
            model = glm::translate(model, obj.position);
            model = glm::rotate(model, glm::radians(obj.rotation.x), glm::vec3(1.0f, 0.0f, 0.0f));
            model = glm::rotate(model, glm::radians(obj.rotation.y), glm::vec3(0.0f, 1.0f, 0.0f));
            model = glm::rotate(model, glm::radians(obj.rotation.z), glm::vec3(0.0f, 0.0f, 1.0f));
            model = glm::scale(model, obj.scale);

            glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "model"), 1, GL_FALSE, glm::value_ptr(model));

            glActiveTexture(GL_TEXTURE0);
            glBindTexture(GL_TEXTURE_2D, obj.textureID);
            glUniform1i(glGetUniformLocation(shaderProgram, "texture1"), 0);

            obj.model->Draw();
        }

        if (lightSphereModel && lightSphereModel->IsInitialized()) {
            glm::mat4 lightModel = glm::mat4(1.0f);
            lightModel = glm::translate(lightModel, pointLight.position);
            lightModel = glm::scale(lightModel, glm::vec3(0.2f, 0.2f, 0.2f)); // Маленький размер

            glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "model"), 1, GL_FALSE, glm::value_ptr(lightModel));

            glActiveTexture(GL_TEXTURE0);
            glBindTexture(GL_TEXTURE_2D, lightTextureID);
            glUniform1i(glGetUniformLocation(shaderProgram, "texture1"), 0);

            lightSphereModel->Draw();
        }

        window.display();
    }

    return 0;
}