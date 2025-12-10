#include <GL/glew.h>
#include <SFML/Window.hpp>
#include <SFML/OpenGL.hpp>
#include <SFML/Graphics.hpp>
#include <iostream>
#include <vector>
#include <cmath>
#include <fstream>
#include <sstream>
#include <filesystem>
#include <algorithm>
#include <string>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include "Model.h"
#include "Camera.h"
#include "Textures.h"
#include "Shaders.h"

Camera camera;
Model currentModel;
GLuint shaderProgram;
float deltaTime = 0.0f;
sf::Clock gameClock;

float modelRotationX = 0.0f;
float modelRotationY = 0.0f;
bool rotatingCamera = false;
float lastMouseX = 0.0f;
float lastMouseY = 0.0f;

std::vector<std::string> modelFiles;
std::string currentModelName = "teapot.obj";

GLuint currentTextureID = 0;
std::string currentTextureName = "";
std::vector<std::string> textureFiles;

std::vector<Planet> planets;

void ScanModelsFolder()
{
    modelFiles.clear();
    try {
        if (!std::filesystem::exists("models")) {
            std::cout << "Warning: 'models/' folder not found. Creating it..." << std::endl;
            std::filesystem::create_directory("models");
            return;
        }

        int count = 0;
        for (const auto& entry : std::filesystem::directory_iterator("models")) {
            if (entry.path().extension() == ".obj") {
                std::string filename = entry.path().filename().string();
                modelFiles.push_back(filename);
                count++;
            }
        }

        std::sort(modelFiles.begin(), modelFiles.end());

        if (count == 0) {
            std::cout << "No OBJ files found in 'models/' folder." << std::endl;
        }
        else {
            std::cout << "Found " << count << " OBJ file(s)." << std::endl;
        }
    }
    catch (const std::exception& e) {
        std::cerr << "Error scanning models folder: " << e.what() << std::endl;
    }
}

bool LoadModel(const std::string& filename)
{
    std::string fullPath = "models/" + filename;
    std::cout << "\nLoading model: " << filename << std::endl;

    Model newModel;
    if (!newModel.LoadFromOBJ(fullPath)) {
        std::cout << "Failed to load: " << filename << std::endl;
        return false;
    }

    currentModel = std::move(newModel);
    currentModelName = filename;

    std::cout << "Successfully loaded: " << filename << std::endl;

    currentModel.InitPlanets();

    return true;
}

void ShowModelSelectionMenu() {
    ScanModelsFolder();

    if (modelFiles.empty()) {
        std::cout << "\nNo models available in 'models/' folder." << std::endl;
        std::cout << "Please add OBJ files to the 'models/' folder." << std::endl;
        return;
    }

    std::cout << "\nSelect model to load:" << std::endl;
    std::cout << "Available models in 'models/' folder:" << std::endl;

    for (size_t i = 0; i < modelFiles.size(); i++) {
        std::cout << "  " << i + 1 << ". " << modelFiles[i];
        if (modelFiles[i] == currentModelName) {
            std::cout << " [CURRENT]";
        }
        std::cout << std::endl;
    }

    std::cout << "\nEnter model number (1-" << modelFiles.size() << "), or 0 to cancel: ";

    int choice;
    if (!(std::cin >> choice)) {
        std::cin.clear();
        std::cin.ignore(10000, '\n');
        std::cout << "Invalid input. Please enter a number." << std::endl;
        return;
    }

    if (choice == 0) {
        std::cout << "Selection cancelled." << std::endl;
        return;
    }

    if (choice < 1 || choice > static_cast<int>(modelFiles.size())) {
        std::cout << "Invalid choice. Please enter a number between 1 and "
            << modelFiles.size() << "." << std::endl;
        return;
    }

    std::string selectedModel = modelFiles[choice - 1];
    if (LoadModel(selectedModel)) {
        std::cout << "Model switched to: " << selectedModel << std::endl;
    }
}

int main() {
    sf::Window window(sf::VideoMode({ 1000, 800 }), "3D Model Viewer");
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

    currentTextureID = LoadTextureFromFile("textures/texture2.png");
    if (!currentTextureID) currentTextureID = CreateSimpleTexture();

    ScanModelsFolder();
    bool modelLoaded = false;

    if (std::find(modelFiles.begin(), modelFiles.end(), "teapot.obj") != modelFiles.end())
        LoadModel("teapot.obj");
    else if (!modelFiles.empty()) LoadModel(modelFiles[0]);

    if (!modelLoaded) {
        std::cout << "No models found. Please add OBJ files to the 'models/' folder." << std::endl;
        std::cout << "Press L to load a model after adding files." << std::endl;
    }

    std::cout << "CAMERA MOVEMENT:" << std::endl;
    std::cout << "  Space - Move forward" << std::endl;
    std::cout << "  LShift - Move backward" << std::endl;
    std::cout << "  D - Move left" << std::endl;
    std::cout << "  A - Move right" << std::endl;
    std::cout << "  S - Move up" << std::endl;
    std::cout << "  W - Move down" << std::endl;
    std::cout << "  Mouse Wheel - Zoom in/out" << std::endl;
    std::cout << "  L - Load new model from list" << std::endl;
    std::cout << "  T - Load texture from list" << std::endl;
    std::cout << "  R - Reset camera and model rotation" << std::endl;
    std::cout << "  Escape - Exit program" << std::endl;
    std::cout << "\nCurrent model: " << currentModelName << std::endl;
    std::cout << "\nPress L to select a different model!" << std::endl;

    camera.SetPosition(glm::vec3(0.0f, 0.0f, 5.0f));
    camera.LookAt(glm::vec3(0.0f, 0.0f, 0.0f));

    float mouseSensitivity = 0.3f;
    float zoomSpeed = 2.0f;

    while (window.isOpen()) {
        deltaTime = gameClock.restart().asSeconds();

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
                        modelRotationX = 0.0f;
                        modelRotationY = 0.0f;
                        std::cout << "Camera and model rotation reset" << std::endl;
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::L) {
                        ShowModelSelectionMenu();
                    }
                    else if (keyEvent->scancode == sf::Keyboard::Scancode::T) {
                        ShowTextureSelectionMenu();
                    }
                }
            }
        }

        for (auto& planet : planets) {
            planet.rotation += planet.selfRotationSpeed * deltaTime;
            if (planet.orbitRadius > 0.0f) {
                planet.orbitAngle += planet.orbitSpeed * deltaTime;

                if (planet.orbitAngle > 360.0f) {
                    planet.orbitAngle -= 360.0f;
                }

                float rad = glm::radians(planet.orbitAngle);
                planet.position.x = planet.orbitRadius * cos(rad);
                planet.position.z = planet.orbitRadius * sin(rad);
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

        glm::mat4 model = glm::mat4(1.0f);
        model = glm::rotate(model, glm::radians(modelRotationY), glm::vec3(0.0f, 1.0f, 0.0f));
        model = glm::rotate(model, glm::radians(modelRotationX), glm::vec3(1.0f, 0.0f, 0.0f));

        glm::mat4 view = camera.GetViewMatrix();
        glm::mat4 projection = camera.GetProjectionMatrix(camera.GetAspectRatio());

        glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "model"), 1, GL_FALSE, glm::value_ptr(model));
        glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "view"), 1, GL_FALSE, glm::value_ptr(view));
        glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "projection"), 1, GL_FALSE, glm::value_ptr(projection));

        glm::vec3 lightPos = glm::vec3(3.0f, 3.0f, 3.0f);
        glm::vec3 viewPos = camera.GetPosition();
        glUniform3f(glGetUniformLocation(shaderProgram, "lightPos"), lightPos.x, lightPos.y, lightPos.z);
        glUniform3f(glGetUniformLocation(shaderProgram, "viewPos"), viewPos.x, viewPos.y, viewPos.z);

        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, currentTextureID);
        glUniform1i(glGetUniformLocation(shaderProgram, "texture1"), 0);

        if (currentModel.IsInitialized()) {
            for (const auto& planet : planets) {
                glm::mat4 modelMat = glm::mat4(1.0f);
                modelMat = glm::translate(modelMat, planet.position);
                modelMat = glm::rotate(modelMat, glm::radians(planet.rotation), glm::vec3(0.0f, 1.0f, 0.0f));
                modelMat = glm::scale(modelMat, glm::vec3(planet.scale));

                glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "model"), 1, GL_FALSE, glm::value_ptr(modelMat));
                currentModel.Draw();
            }
        }

        window.display();
    }

    return 0;
}