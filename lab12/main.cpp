#include <GL/glew.h>
#include <SFML/Window.hpp>
#include <SFML/OpenGL.hpp>

#include "Shaders.h"
#include "Graphics.h"

int currentMode = 0;

int main()
{
    sf::Window window(sf::VideoMode({ 1000, 1000 }), "Lab12");
    glewInit();
    
    glEnable(GL_DEPTH_TEST);;
    glDepthFunc(GL_LESS);
    glClearDepth(1.0f);

    InitShader();
    InitTetrahedron();
    InitTextures();

    float moveSpeed = 0.05f;

    while (window.isOpen())
    {
        while (auto event = window.pollEvent())
        {
            if (event->is<sf::Event::Closed>())
                window.close();

            if (event->is<sf::Event::KeyPressed>())
            {
                auto key = event->getIf<sf::Event::KeyPressed>()->scancode;

                if (key == sf::Keyboard::Scan::Right)
                    MoveFigure(moveSpeed, 0.0f, 0.0f);
                else if (key == sf::Keyboard::Scan::Left)
                    MoveFigure(-moveSpeed, 0.0f, 0.0f);
                else if (key == sf::Keyboard::Scan::Up)
                    MoveFigure(0.0f, moveSpeed, 0.0f);
                else if (key == sf::Keyboard::Scan::Down)
                    MoveFigure(0.0f, -moveSpeed, 0.0f);
                else if (key == sf::Keyboard::Scan::W)
                    MoveFigure(0.0f, 0.0f, moveSpeed);
                else if (key == sf::Keyboard::Scan::S)
                    MoveFigure(0.0f, 0.0f, -moveSpeed);

                else if (key == sf::Keyboard::Scan::Num1)
                {
                    InitTetrahedron();
                    currentMode = 0;
                }
                else if (key == sf::Keyboard::Scan::Num2)
                {
                    InitCube();
                    currentMode = 1;
                }
                else if (key == sf::Keyboard::Scan::Num3)
                {
                    InitCube();
                    currentMode = 2;
                }

                else if (key == sf::Keyboard::Scan::Z)
                {
                    if (currentMode == 1)
                    {
                        colorMix += 0.05f;
                        if (colorMix > 1.0f) colorMix = 1.0f;
                    }
                    else if (currentMode == 2)
                    {
                        textureMix += 0.05f;
                        if (textureMix > 1.0f) textureMix = 1.0f;
                    }
                }
                else if (key == sf::Keyboard::Scan::X)
                {
                    if (currentMode == 1)
                    {
                        colorMix -= 0.05f;
                        if (colorMix > 1.0f) colorMix = 1.0f;
                    }
                    else if (currentMode == 2)
                    {
                        textureMix -= 0.05f;
                        if (textureMix > 1.0f) textureMix = 1.0f;
                    }
                }
            }
        }

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        if (currentMode == 0)
        {
            DrawGradient3D(currentIndexCount);
        }
        else if (currentMode == 1)
        {
            DrawTextureCube1();
        }
        else if (currentMode == 2)
        {
            DrawTextureCube2();
        }

        window.display();
    }


    return 0;
}