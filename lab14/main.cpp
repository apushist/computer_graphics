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

    int shadingModel = 0;

    StaticObject(const StaticObject&) = delete;
    StaticObject& operator=(const StaticObject&) = delete;

    StaticObject() = default;
    StaticObject(StaticObject&&) = default;
    StaticObject& operator=(StaticObject&&) = default;
};

int currentObjectIndex = 0;

struct PointLight {
    glm::vec3 position;
    glm::vec3 diffuse;
    glm::vec3 specular;
    glm::vec3 ambient;
    glm::vec3 attenuation; 
    bool enabled;
    float intensity;

    PointLight() :
        position(glm::vec3(5.0f, 5.0f, 5.0f)),
        ambient(glm::vec3(0.2f, 0.2f, 0.2f)),
        diffuse(glm::vec3(1.0f, 0.9f, 0.8f)),
        specular(glm::vec3(1.0f, 0.9f, 0.8f)),
        attenuation(glm::vec3(1.0f, 0.09f, 0.032f)),
        enabled(true),
        intensity(1.0f) {
    }

    void Load(GLuint program) const {
        std::string prefix = "pointl.";
        glUniform3fv(glGetUniformLocation(program, (prefix + "position").c_str()), 1, glm::value_ptr(position));
        glUniform3fv(glGetUniformLocation(program, (prefix + "ambient").c_str()), 1, glm::value_ptr(ambient * intensity));
        glUniform3fv(glGetUniformLocation(program, (prefix + "diffuse").c_str()), 1, glm::value_ptr(diffuse * intensity));
        glUniform3fv(glGetUniformLocation(program, (prefix + "specular").c_str()), 1, glm::value_ptr(specular * intensity));
        glUniform3fv(glGetUniformLocation(program, (prefix + "attenuation").c_str()), 1, glm::value_ptr(attenuation));
        glUniform1i(glGetUniformLocation(program, (prefix + "enabled").c_str()), enabled ? 1 : 0);
    }
};

struct DirectionalLight {
    glm::vec3 direction;
    glm::vec3 diffuse;
    glm::vec3 specular;
    glm::vec3 ambient;
    bool enabled;
    float intensity;

    DirectionalLight() :
        direction(glm::vec3(-0.2f, -1.0f, -0.3f)),
        ambient(glm::vec3(0.3f, 0.3f, 0.4f)),
        diffuse(glm::vec3(0.8f, 0.8f, 1.0f)),
        specular(glm::vec3(0.8f, 0.8f, 1.0f)),
        enabled(true),
        intensity(1.0f) {
    }

    void Load(GLuint program) const {
        std::string prefix = "dirl.";
        glm::vec3 normalizedDir = glm::normalize(direction);
        glUniform3fv(glGetUniformLocation(program, (prefix + "direction").c_str()), 1, glm::value_ptr(normalizedDir));
        glUniform3fv(glGetUniformLocation(program, (prefix + "ambient").c_str()), 1, glm::value_ptr(ambient * intensity));
        glUniform3fv(glGetUniformLocation(program, (prefix + "diffuse").c_str()), 1, glm::value_ptr(diffuse * intensity));
        glUniform3fv(glGetUniformLocation(program, (prefix + "specular").c_str()), 1, glm::value_ptr(specular * intensity));
        glUniform1i(glGetUniformLocation(program, (prefix + "enabled").c_str()), enabled ? 1 : 0);
    }
};

struct SpotLight {
    glm::vec3 position;
    glm::vec3 direction;
    glm::vec3 diffuse;
    glm::vec3 specular;
    glm::vec3 ambient;
    glm::vec3 attenuation;
    float cutoff;
    float outerCutoff;
    bool enabled;
    float intensity;

    SpotLight() :
        position(glm::vec3(0.0f, 5.0f, 5.0f)),
        direction(glm::vec3(0.0f, -0.5f, -1.0f)),
        ambient(glm::vec3(0.1f, 0.1f, 0.1f)),
        diffuse(glm::vec3(1.0f, 1.0f, 1.0f)),
        specular(glm::vec3(1.0f, 1.0f, 1.0f)),
        attenuation(glm::vec3(1.0f, 0.09f, 0.032f)),
        cutoff(glm::cos(glm::radians(15.0f))),
        outerCutoff(glm::cos(glm::radians(20.0f))),
        enabled(true),
        intensity(1.0f) {
    }

    void Load(GLuint program) const {
        std::string prefix = "spotl.";
        glm::vec3 normalizedDir = glm::normalize(direction);
        glUniform3fv(glGetUniformLocation(program, (prefix + "position").c_str()), 1, glm::value_ptr(position));
        glUniform3fv(glGetUniformLocation(program, (prefix + "direction").c_str()), 1, glm::value_ptr(normalizedDir));
        glUniform3fv(glGetUniformLocation(program, (prefix + "ambient").c_str()), 1, glm::value_ptr(ambient * intensity));
        glUniform3fv(glGetUniformLocation(program, (prefix + "diffuse").c_str()), 1, glm::value_ptr(diffuse * intensity));
        glUniform3fv(glGetUniformLocation(program, (prefix + "specular").c_str()), 1, glm::value_ptr(specular * intensity));
        glUniform3fv(glGetUniformLocation(program, (prefix + "attenuation").c_str()), 1, glm::value_ptr(attenuation));
        glUniform1f(glGetUniformLocation(program, (prefix + "cutoff").c_str()), cutoff);
        glUniform1f(glGetUniformLocation(program, (prefix + "outerCutoff").c_str()), outerCutoff);
        glUniform1i(glGetUniformLocation(program, (prefix + "enabled").c_str()), enabled ? 1 : 0);
    }
};

Camera camera;
GLuint shaderProgram;
GLuint outlineShaderProgram;
float deltaTime = 0.0f;
sf::Clock gameClock;

std::vector<StaticObject> staticObjects;
PointLight pointLight;
DirectionalLight directionalLight;
SpotLight spotLight;
std::unique_ptr<Model> lightSphereModel;

bool LoadStaticScene() {
    staticObjects.clear();

    lightSphereModel = std::make_unique<Model>();
    if (!lightSphereModel->LoadFromOBJ("models/sphere.obj")) {
        std::cerr << "Failed to load sphere.obj for light visualization" << std::endl;
        return false;
    }

    // Объект 1 - Куб (пол)
    StaticObject obj1;
    obj1.name = "Metal_table";
    obj1.model = std::make_unique<Model>();
    if (!obj1.model->LoadFromOBJ("models/table.obj")) {
        std::cerr << "Failed to load Metal_table.obj" << std::endl;
        return false;
    }
    obj1.textureID = LoadTextureFromFile("textures/tabletexture_Albedo.png");

    obj1.position = glm::vec3(-6.0f, -4.0f, -2.0f);
    obj1.rotation = glm::vec3(0.0f, 0.0f, 0.0f);
    obj1.scale = glm::vec3(3.0f, 2.0f, 10.0f);

    /*obj1.name = "Cube";
    obj1.model = std::make_unique<Model>();
    if (!obj1.model->LoadFromOBJ("models/cube.obj")) {
        std::cerr << "Failed to load cube.obj" << std::endl;
        return false;
    }
    obj1.position = glm::vec3(0.0f, -11.0f, 0.0f);
    obj1.rotation = glm::vec3(0.0f, 0.0f, 0.0f);
    obj1.scale = glm::vec3(10.0f, 10.0f, 10.0f);

    obj1.textureID = LoadTextureFromFile("textures/ceramic.jpg");*/
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

    // Чашка-жаба
    StaticObject obj5;
    obj5.name = "Frog_Cup";
    obj5.model = std::make_unique<Model>();
    if (!obj5.model->LoadFromOBJ("models/Frog_Cup.obj")) {
        std::cerr << "Failed to load Frog_Cup.obj" << std::endl;
        return false;
    }
    obj5.textureID = LoadTextureFromFile("textures/Frog_Cup_Material.png");
    obj5.position = glm::vec3(3.0f, -0.9f, 2.5f);
    obj5.rotation = glm::vec3(0.0f, 40.0f, 0.0f);
    obj5.scale = glm::vec3(5.0f, 5.0f, 5.0f);
    staticObjects.push_back(std::move(obj5));

    // Женщина
    StaticObject obj6;
    obj6.name = "Woman";
    obj6.model = std::make_unique<Model>();
    if (!obj6.model->LoadFromOBJ("models/AngelaMartinSprinkles.obj")) {
        std::cerr << "Failed to load AngelaMartinSprinkles.obj" << std::endl;
        return false;
    }
    obj6.textureID = LoadTextureFromFile("textures/AngelaMartinSprinkles_material.tga");
    obj6.position = glm::vec3(-1.0f, -0.9f, 2.0f);
    obj6.rotation = glm::vec3(0.0f, 30.0f, 0.0f);
    obj6.scale = glm::vec3(0.2f, 0.2f, 0.2f);
    staticObjects.push_back(std::move(obj6));

    return true;
}

int main() {
    sf::Window window(sf::VideoMode({ 1000, 800 }), "3D Scene with Lighting");
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

    if (!InitOutlineShader(outlineShaderProgram)) {
        std::cerr << "Failed to initialize outline shader!" << std::endl;
        return -1;
    }

    if (!LoadStaticScene()) {
        std::cerr << "Failed to load static scene!" << std::endl;
        return -1;
    }

    system("cls");
    std::cout << "=== 3D SCENE WITH LIGHTING ===" << std::endl;
    std::cout << "\n=== LIGHT TOGGLE ===" << std::endl;
    std::cout << "1 - Point light (yellow)" << std::endl;
    std::cout << "2 - Directional light (blue)" << std::endl;
    std::cout << "3 - Spot light (white)" << std::endl;

    std::cout << "\n=== INTENSITY CONTROL ===" << std::endl;
    std::cout << "4/5 - Directional light -/+" << std::endl;
    std::cout << "6/7 - Point light -/+ (TFGH)" << std::endl;
    std::cout << "8/9 - Spot light -/+ (IJKL)" << std::endl;

    std::cout << "\n=== POINT LIGHT MOVEMENT (TFGH) ===" << std::endl;
    std::cout << "T - Up" << std::endl;
    std::cout << "G - Down" << std::endl;
    std::cout << "F - Left" << std::endl;
    std::cout << "H - Right" << std::endl;
    std::cout << "U - Forward" << std::endl;
    std::cout << "O - Backward" << std::endl;

    std::cout << "\n=== SPOT LIGHT MOVEMENT (IJKL) ===" << std::endl;
    std::cout << "I - Up" << std::endl;
    std::cout << "K - Down" << std::endl;
    std::cout << "J - Left" << std::endl;
    std::cout << "L - Right" << std::endl;
    std::cout << "Y - Forward" << std::endl;
    std::cout << "H - Backward" << std::endl;

    std::cout << "\n=== SPOT LIGHT DIRECTION ===" << std::endl;
    std::cout << "Q/E - Tilt up/down" << std::endl;
    std::cout << "A/D - Pan left/right" << std::endl;
    std::cout << "Z/X - Narrower/wider cone" << std::endl;

    std::cout << "\n=== DIRECTIONAL LIGHT ===" << std::endl;
    std::cout << "Arrow keys - Change direction" << std::endl;
    std::cout << "C/V - Rotate around Y axis" << std::endl;

    std::cout << "\n=== CAMERA CONTROLS ===" << std::endl;
    std::cout << "WASD + Space/Shift - Move camera" << std::endl;
    std::cout << "Mouse drag - Rotate camera" << std::endl;
    std::cout << "Mouse wheel - Zoom" << std::endl;
    std::cout << "R - Reset camera" << std::endl;
    std::cout << "ESC - Exit" << std::endl;

    std::cout << "\n=== OBJECTS SWITCH ===" << std::endl;
    std::cout << "NUMPAD 4/6 - Switch objects" << std::endl;

    std::cout << "\n=== SHADING MODEL SWITCH ===" << std::endl;
    std::cout << "NUMPAD 1 - Phong shading" << std::endl;
    std::cout << "NUMPAD 2 - Toon shading" << std::endl;
    std::cout << "NUMPAD 3 - Minnaert shading" << std::endl;
    std::cout << "NUMPAD 5 - Ashikhmin–Shirley shading" << std::endl;

    camera.SetPosition(glm::vec3(0.0f, 0.0f, 15.0f));
    camera.LookAt(glm::vec3(0.0f, 0.0f, 0.0f));

    float mouseSensitivity = 0.3f;
    float zoomSpeed = 2.0f;
    bool rotatingCamera = false;
    float lastMouseX = 0.0f;
    float lastMouseY = 0.0f;

    GLuint lightTextureID = LoadTextureFromFile("textures/light.png");

    // Флаг для подавления лишнего вывода
    bool showInstructions = true;

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
                if (buttonEvent && buttonEvent->button == sf::Mouse::Button::Left) {
                    rotatingCamera = false;
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
                    if (wheel->delta > 0) camera.MoveForward(zoomSpeed);
                    else camera.MoveBackward(zoomSpeed);
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
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Num1) {
                        pointLight.enabled = !pointLight.enabled;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Num2) {
                        directionalLight.enabled = !directionalLight.enabled;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Num3) {
                        spotLight.enabled = !spotLight.enabled;
                    }

                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Num4) {
                        directionalLight.intensity *= 0.8f;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Num5) {
                        directionalLight.intensity *= 1.2f;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Num6) {
                        pointLight.intensity *= 0.8f;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Num7) {
                        pointLight.intensity *= 1.2f;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Num8) {
                        spotLight.intensity *= 0.8f;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Num9) {
                        spotLight.intensity *= 1.2f;
                    }

                    else if (keyEvent->scancode == sf::Keyboard::Scancode::T) {
                        pointLight.position.y += moveSpeed;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::G) {
                        pointLight.position.y -= moveSpeed;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::F) {
                        pointLight.position.x -= moveSpeed;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::H) {
                        pointLight.position.x += moveSpeed;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::U) {
                        pointLight.position.z += moveSpeed;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::O) {
                        pointLight.position.z -= moveSpeed;
                    }

                    else if (keyEvent->scancode == sf::Keyboard::Scancode::I) {
                        spotLight.position.y += moveSpeed;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::K) {
                        spotLight.position.y -= moveSpeed;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::J) {
                        spotLight.position.x -= moveSpeed;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::L) {
                        spotLight.position.x += moveSpeed;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Y) {
                        spotLight.position.z += moveSpeed;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::B) {
                        spotLight.position.z -= moveSpeed;
                    }

                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Q) {
                        spotLight.direction.y -= 0.1f;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::E) {
                        spotLight.direction.y += 0.1f;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::A) {
                        spotLight.direction.x -= 0.1f;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::D) {
                        spotLight.direction.x += 0.1f;
                    }

                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Z) {
                        float newAngle = glm::acos(spotLight.cutoff) - glm::radians(2.0f);
                        if (newAngle > glm::radians(5.0f)) {
                            spotLight.cutoff = glm::cos(newAngle);
                            spotLight.outerCutoff = glm::cos(newAngle + glm::radians(5.0f));
                        }
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::X) {
                        float newAngle = glm::acos(spotLight.cutoff) + glm::radians(2.0f);
                        if (newAngle < glm::radians(45.0f)) {
                            spotLight.cutoff = glm::cos(newAngle);
                            spotLight.outerCutoff = glm::cos(newAngle + glm::radians(5.0f));
                        }
                    }

                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Up) {
                        directionalLight.direction.y -= 0.1f;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Down) {
                        directionalLight.direction.y += 0.1f;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Left) {
                        directionalLight.direction.x += 0.1f;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Right) {
                        directionalLight.direction.x -= 0.1f;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::C) {
                        glm::mat4 rotation = glm::rotate(glm::mat4(1.0f), glm::radians(-10.0f), glm::vec3(0.0f, 1.0f, 0.0f));
                        directionalLight.direction = glm::vec3(rotation * glm::vec4(directionalLight.direction, 0.0f));
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::V) {
                        glm::mat4 rotation = glm::rotate(glm::mat4(1.0f), glm::radians(10.0f), glm::vec3(0.0f, 1.0f, 0.0f));
                        directionalLight.direction = glm::vec3(rotation * glm::vec4(directionalLight.direction, 0.0f));
                    }

                    // ПЕРЕКЛЮЧЕНИЕ ТЕКУЩЕГО ОБЪЕКТА
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Numpad4)
                    {
                        currentObjectIndex--;
                        if (currentObjectIndex < 0)
                        {
                            currentObjectIndex = staticObjects.size() - 1;
                        }
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Numpad6)
                    {
                        currentObjectIndex++;
                        if (currentObjectIndex >= staticObjects.size())
                        {
                            currentObjectIndex = 0;
                        }
                    }

                    // ПЕРЕКЛЮЧЕНИЕ МОДЕЛЕЙ ОСВЕЩЕНИЯ
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Numpad1)
                    {
                        staticObjects[currentObjectIndex].shadingModel = 0;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Numpad2)
                    {
                        staticObjects[currentObjectIndex].shadingModel = 1;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Numpad3)
                    {
                        staticObjects[currentObjectIndex].shadingModel = 2;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::Numpad5)
                    {
                        staticObjects[currentObjectIndex].shadingModel = 3;
                    }
                }
            }
        }

        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::Space)) camera.MoveForward(moveSpeed);
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::LShift)) camera.MoveBackward(moveSpeed);
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::D)) camera.MoveRight(moveSpeed);
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::A)) camera.MoveLeft(moveSpeed);
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::S)) camera.MoveDown(moveSpeed);
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::W)) camera.MoveUp(moveSpeed);

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        glUseProgram(shaderProgram);

        glm::mat4 view = camera.GetViewMatrix();
        glm::mat4 projection = camera.GetProjectionMatrix(camera.GetAspectRatio());
        glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "view"), 1, GL_FALSE, glm::value_ptr(view));
        glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "projection"), 1, GL_FALSE, glm::value_ptr(projection));

        glm::vec3 viewPos = camera.GetPosition();
        glUniform3f(glGetUniformLocation(shaderProgram, "viewPos"), viewPos.x, viewPos.y, viewPos.z);

        pointLight.Load(shaderProgram);
        directionalLight.Load(shaderProgram);
        spotLight.Load(shaderProgram);

        for (const auto& obj : staticObjects) {
            glm::mat4 model = glm::mat4(1.0f);
            model = glm::translate(model, obj.position);
            model = glm::rotate(model, glm::radians(obj.rotation.x), glm::vec3(1.0f, 0.0f, 0.0f));
            model = glm::rotate(model, glm::radians(obj.rotation.y), glm::vec3(0.0f, 1.0f, 0.0f));
            model = glm::rotate(model, glm::radians(obj.rotation.z), glm::vec3(0.0f, 0.0f, 1.0f));
            model = glm::scale(model, obj.scale);

            glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "model"), 1, GL_FALSE, glm::value_ptr(model));

            glUniform1i(
                glGetUniformLocation(shaderProgram, "uShadingModel"),
                obj.shadingModel
            );

            glActiveTexture(GL_TEXTURE0);
            glBindTexture(GL_TEXTURE_2D, obj.textureID);
            glUniform1i(glGetUniformLocation(shaderProgram, "texture1"), 0);

            obj.model->Draw();
        }

        // Контур для текущего объекта
        const auto& selected = staticObjects[currentObjectIndex];

        glUseProgram(outlineShaderProgram);

        glEnable(GL_CULL_FACE);
        glCullFace(GL_FRONT);
        glEnable(GL_DEPTH_TEST);
        glDepthFunc(GL_LEQUAL);

        glm::mat4 model = glm::mat4(1.0f);
        model = glm::translate(model, selected.position);
        model = glm::rotate(model, glm::radians(selected.rotation.x), glm::vec3(1, 0, 0));
        model = glm::rotate(model, glm::radians(selected.rotation.y), glm::vec3(0, 1, 0));
        model = glm::rotate(model, glm::radians(selected.rotation.z), glm::vec3(0, 0, 1));
        model = glm::scale(model, selected.scale);

        glUniformMatrix4fv(glGetUniformLocation(outlineShaderProgram, "model"), 1, GL_FALSE, glm::value_ptr(model));
        glUniformMatrix4fv(glGetUniformLocation(outlineShaderProgram, "view"), 1, GL_FALSE, glm::value_ptr(view));
        glUniformMatrix4fv(glGetUniformLocation(outlineShaderProgram, "projection"), 1, GL_FALSE, glm::value_ptr(projection));

        glUniform1f(glGetUniformLocation(outlineShaderProgram, "outlineWidth"), 0.03f);
        glUniform3f(glGetUniformLocation(outlineShaderProgram, "outlineColor"), 1.0f, 0.2f, 0.2f);

        selected.model->Draw();

        glCullFace(GL_BACK);
        glDisable(GL_CULL_FACE);
        glDepthFunc(GL_LESS);


        if (lightSphereModel && lightSphereModel->IsInitialized() && pointLight.enabled) {
            glm::mat4 lightModel = glm::mat4(1.0f);
            lightModel = glm::translate(lightModel, pointLight.position);
            lightModel = glm::scale(lightModel, glm::vec3(0.2f, 0.2f, 0.2f));
            glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "model"), 1, GL_FALSE, glm::value_ptr(lightModel));
            glActiveTexture(GL_TEXTURE0);
            glBindTexture(GL_TEXTURE_2D, lightTextureID);
            glUniform1i(glGetUniformLocation(shaderProgram, "texture1"), 0);
            lightSphereModel->Draw();
        }

        if (lightSphereModel && lightSphereModel->IsInitialized() && spotLight.enabled) {
            glm::mat4 spotModel = glm::mat4(1.0f);
            spotModel = glm::translate(spotModel, spotLight.position);
            spotModel = glm::scale(spotModel, glm::vec3(0.15f, 0.15f, 0.15f));
            glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "model"), 1, GL_FALSE, glm::value_ptr(spotModel));
            glActiveTexture(GL_TEXTURE0);
            glBindTexture(GL_TEXTURE_2D, lightTextureID);
            glUniform1i(glGetUniformLocation(shaderProgram, "texture1"), 0);
            lightSphereModel->Draw();
        }

        window.display();
    }

    return 0;
}