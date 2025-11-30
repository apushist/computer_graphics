#include <GL/glew.h>
#include <SFML/Window.hpp>
#include <SFML/OpenGL.hpp>

#include "Shaders.h"
#include "Graphics.h"

int main()
{
    sf::Window window(sf::VideoMode({ 1000, 1000 }), "Lab12");
    glewInit();
    
    glEnable(GL_DEPTH_TEST);;
    glDepthFunc(GL_LESS);
    glClearDepth(1.0f);

    InitShader();
    InitTetrahedron();

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
                    InitTetrahedron();
                else if (key == sf::Keyboard::Scan::Num2)
                    InitCube();
            }
        }

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        DrawGradient3D(currentIndexCount);
        window.display();
    }


    return 0;
}