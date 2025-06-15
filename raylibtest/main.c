#include <X11/Xlib.h>
#include <X11/Xutil.h>
#include <X11/keysym.h>
#include <stdio.h>

#define Font RayFont  // Font çakışmasını engelle
#include "raylib.h"
#undef Font

bool IsKeyPressedGlobal(Display *display, KeyCode keycode) {
    char keys_return[32];
    XQueryKeymap(display, keys_return);
    return (keys_return[keycode / 8] & (1 << (keycode % 8)));
}

int main() {
    InitWindow(800, 600, "Global Input on X11");
    SetTargetFPS(60);

    Display *display = XOpenDisplay(NULL);
    if (!display) {
        printf("X display cannot be opened\n");
        return 1;
    }

    KeyCode key_w = XKeysymToKeycode(display, XStringToKeysym("w"));
    KeyCode key_a = XKeysymToKeycode(display, XStringToKeysym("a"));

    while (!WindowShouldClose()) {
        BeginDrawing();
        ClearBackground(RAYWHITE);

        if (IsKeyPressedGlobal(display, key_w)) {
            DrawText("W BASILI (global)", 10, 10, 20, RED);
        }

        if (IsKeyPressedGlobal(display, key_a)) {
            DrawText("A BASILI (global)", 10, 40, 20, BLUE);
        }

        EndDrawing();
    }

    XCloseDisplay(display);
    CloseWindow();
    return 0;
}
